using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace NetTopologySuite.IO
{
    public static class ShapefileReader
    {
        public static async Task ReadShapefileAsync(ShapefileRequiredFileContainer streamContainer, ShapefileVisitorBase visitor, CancellationToken cancellationToken = default)
        {
            if (streamContainer is null)
            {
                throw new ArgumentNullException(nameof(streamContainer));
            }

            if (visitor is null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            using (var oneHundredByteBufferOwner = MemoryPool<byte>.Shared.Rent(100))
            {
                var oneHundredByteBuffer = oneHundredByteBufferOwner.Memory;
                var (mainFile, indexFile, attributeFile) = (streamContainer.MainFileReader, streamContainer.IndexFileReader, streamContainer.AttributeFileReader);

                var mainFileHeaderBuf = oneHundredByteBuffer.Slice(0, Unsafe.SizeOf<ShapefileHeader>());
                if (!await FillBufferFromPipeAsync(mainFile, mainFileHeaderBuf, cancellationToken).ConfigureAwait(false))
                {
                    return;
                }

                var mainFileHeader = Unsafe.ReadUnaligned<ShapefileHeader>(ref oneHundredByteBuffer.Span[0]);
                await visitor.VisitMainFileHeaderAsync(mainFileHeader, cancellationToken).ConfigureAwait(false);

                var shapeType = mainFileHeader.ShapeTypeForAllRecords;

                while (true)
                {
                    var nextRecordHeaderBuf = oneHundredByteBuffer.Slice(0, Unsafe.SizeOf<ShapefileMainFileRecordHeader>());
                    if (!await FillBufferFromPipeAsync(mainFile, nextRecordHeaderBuf, cancellationToken).ConfigureAwait(false))
                    {
                        break;
                    }

                    var nextRecordHeader = Unsafe.ReadUnaligned<ShapefileMainFileRecordHeader>(ref nextRecordHeaderBuf.Span[0]);
                    await visitor.VisitMainFileRecordHeaderAsync(nextRecordHeader, cancellationToken).ConfigureAwait(false);

                    uint nextRecordContentLengthInBytes = nextRecordHeader.ContentLengthInBytes;
                    if (nextRecordContentLengthInBytes > int.MaxValue)
                    {
                        throw new NotSupportedException("Each individual shapefile record must be smaller than 2 GiB, for now.");
                    }

                    int recordLength = unchecked((int)nextRecordContentLengthInBytes);
                    var recordBufOwner = recordLength <= oneHundredByteBuffer.Length
                        ? oneHundredByteBufferOwner
                        : MemoryPool<byte>.Shared.Rent(recordLength);
                    try
                    {
                        var recordBuf = recordBufOwner.Memory.Slice(0, recordLength);
                        if (!await FillBufferFromPipeAsync(mainFile, recordBuf, cancellationToken).ConfigureAwait(false))
                        {
                            break;
                        }

                        await visitor.VisitMainFileRecordAsync(shapeType, recordBuf, cancellationToken).ConfigureAwait(false);
                    }
                    finally
                    {
                        if (recordBufOwner != oneHundredByteBufferOwner)
                        {
                            recordBufOwner.Dispose();
                        }
                    }
                }
            }
        }

        private static async ValueTask<bool> FillBufferFromPipeAsync(PipeReader pipe, Memory<byte> rem, CancellationToken cancellationToken)
        {
            while (rem.Length != 0)
            {
                var read = await pipe.ReadAsync(cancellationToken).ConfigureAwait(false);
                if (read.IsCanceled)
                {
                    return false;
                }

                var buffer = read.Buffer;
                if (buffer.IsEmpty && read.IsCompleted)
                {
                    return false;
                }

                int processed = 0;
                foreach (var segment in buffer)
                {
                    int curProcessed;
                    if (segment.Length >= rem.Length)
                    {
                        segment.Slice(0, curProcessed = rem.Length).CopyTo(rem);
                    }
                    else
                    {
                        segment.CopyTo(rem.Slice(0, curProcessed = segment.Length));
                    }

                    rem = rem.Slice(curProcessed);
                    processed += curProcessed;

                    if (rem.Length == 0)
                    {
                        break;
                    }
                }

                pipe.AdvanceTo(buffer.GetPosition(processed));
            }

            return true;
        }
    }
}
