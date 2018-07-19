using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;

namespace NetTopologySuite.IO
{
    public static class ShapefileReader
    {
        public static void ReadShapefile(string mainFilePath, ShapefileVisitorBase visitor)
        {
            if (visitor is null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            FileStream mainFileStream = null;
            FileStream indexFileStream = null;
            FileStream attributeFileStream = null;
            try
            {
                ReadShapefile(
                    new ShapefileRequiredStreamContainer(
                        mainFileStream = new FileStream(mainFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan),
                        indexFileStream = new FileStream(Path.ChangeExtension(mainFilePath, "shx"), FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan),
                        attributeFileStream = new FileStream(Path.ChangeExtension(mainFilePath, "dbf"), FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan)),
                        visitor);
            }
            finally
            {
                mainFileStream?.Dispose();
                indexFileStream?.Dispose();
                attributeFileStream?.Dispose();
            }
        }

        public static void ReadShapefile(ShapefileRequiredStreamContainer streamContainer, ShapefileVisitorBase visitor)
        {
            if (streamContainer is null)
            {
                throw new ArgumentNullException(nameof(streamContainer));
            }

            if (visitor is null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            byte[] oneHundredByteBuffer = ArrayPool<byte>.Shared.Rent(100);
            try
            {
                var (mainFileStream, indexFileStream, attributeFileStream) = (streamContainer.MainFileStream, streamContainer.IndexFileStream, streamContainer.AttributeFileStream);
                var mainFileHeader = Read<ShapefileHeader>(mainFileStream, oneHundredByteBuffer);
                visitor.VisitMainFileHeader(ref mainFileHeader);

                long bytesReadSoFar = Unsafe.SizeOf<ShapefileHeader>();
                long endOfFile = mainFileHeader.FileLengthInBytes;
                long recordHeaderLength = Unsafe.SizeOf<ShapefileMainFileRecordHeader>();
                while (bytesReadSoFar + recordHeaderLength <= endOfFile)
                {
                    var nextRecordHeader = Read<ShapefileMainFileRecordHeader>(mainFileStream, oneHundredByteBuffer);
                    bytesReadSoFar += recordHeaderLength;
                    visitor.VisitMainFileRecordHeader(ref nextRecordHeader);

                    long nextRecordContentLengthInBytes = nextRecordHeader.ContentLengthInBytes;
                    if (nextRecordContentLengthInBytes > int.MaxValue)
                    {
                        throw new NotSupportedException("Each individual shapefile record must be smaller than 2 GiB, for now.");
                    }

                    bytesReadSoFar += nextRecordContentLengthInBytes;
                    if (bytesReadSoFar > endOfFile)
                    {
                        break;
                    }

                    int recordLength = unchecked((int)nextRecordContentLengthInBytes);
                    if (recordLength <= oneHundredByteBuffer.Length)
                    {
                        Read(mainFileStream, oneHundredByteBuffer, recordLength);
                    }
                    else
                    {
                        byte[] recordBuf = ArrayPool<byte>.Shared.Rent(recordLength);
                        try
                        {
                            Read(mainFileStream, recordBuf, recordLength);
                        }
                        finally
                        {
                            ArrayPool<byte>.Shared.Return(recordBuf);
                        }
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(oneHundredByteBuffer);
            }
        }

        private static void Read(Stream stream, byte[] buf, int length)
        {
            int cur = 0;
            while (length > 0)
            {
                int nxt = stream.Read(buf, cur, length);
                if (nxt == 0)
                {
                    ThrowEndOfStreamException();
                }

                cur += nxt;
                length -= nxt;
            }
        }

        private static T Read<T>(Stream stream, byte[] buf)
        {
            Read(stream, buf, Unsafe.SizeOf<T>());
            return Unsafe.ReadUnaligned<T>(ref buf[0]);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowEndOfStreamException() => throw new EndOfStreamException();
    }
}
