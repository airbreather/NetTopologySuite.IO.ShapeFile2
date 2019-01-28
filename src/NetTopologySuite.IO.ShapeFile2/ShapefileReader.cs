using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NetTopologySuite.IO
{
    public static class ShapefileReader
    {
        public static async Task ReadShapefileAsync(ShapefilePipeReaderContainer pipeReaderContainer, ShapefileVisitorBase visitor, CancellationToken cancellationToken = default)
        {
            if (pipeReaderContainer is null)
            {
                throw new ArgumentNullException(nameof(pipeReaderContainer));
            }

            if (visitor is null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            // Shapefiles are really weird, in that they have both big- and little-endian values in
            // them, so both machine types will have to swap byte orders for some values. Since the
            // performance-critical parts are all little-endian, and pretty much everybody who's
            // likely to ever use this is too, we just skip the swapping code paths altogether and
            // just add this check here.  If we have a reason to support big-endian machines later,
            // we can do so at that point, without breaking anyone.
            BitTwiddlers.EnsureLittleEndian();
            using (var oneHundredByteBufferOwner = MemoryPool<byte>.Shared.Rent(100))
            {
                var oneHundredByteBuffer = oneHundredByteBufferOwner.Memory;
                var recordHeaderBuf = oneHundredByteBuffer.Slice(0, Unsafe.SizeOf<ShapefileMainFileRecordHeader>());
                var mainFileHeaderBuf = oneHundredByteBuffer.Slice(0, Unsafe.SizeOf<ShapefileHeader>());

                // TODO: use indexFile, even if it's just to throw if we see something incompatible
                // with our requirements (i.e., fully sequential files without any gaps).
                var (mainFile, indexFile, attributeFile) = (pipeReaderContainer.MainFileReader, pipeReaderContainer.IndexFileReader, pipeReaderContainer.AttributeFileReader);
                if (!await FillBufferFromPipeAsync(mainFile, mainFileHeaderBuf, cancellationToken).ConfigureAwait(false))
                {
                    return;
                }

                var mainFileHeader = MemoryMarshal.Read<ShapefileHeader>(mainFileHeaderBuf.Span);
                await visitor.VisitMainFileHeaderAsync(mainFileHeader, cancellationToken).ConfigureAwait(false);

                if (!await FillBufferFromPipeAsync(indexFile, mainFileHeaderBuf, cancellationToken).ConfigureAwait(false))
                {
                    throw new NotSupportedException("Headers of main and index files must match, for now.");
                }

                var indexFileHeader = MemoryMarshal.Read<ShapefileHeader>(mainFileHeaderBuf.Span);

                // temporarily replace the stored file length so that the two headers should be
                // otherwise identical.
                int indexFileLengthInWords = indexFileHeader.FileLengthInWords;
                indexFileHeader.FileLengthInWords = mainFileHeader.FileLengthInWords;
                if (mainFileHeader != indexFileHeader)
                {
                    throw new NotSupportedException("Headers of main and index files must match, for now.");
                }

                // restore the temporarily replaced file length so that the index file header is
                // back to what's actually stored in the file.
                indexFileHeader.FileLengthInWords = indexFileLengthInWords;

                // TODO: do more with the index file.
                var shapeType = mainFileHeader.ShapeTypeForAllRecords;

                while (true)
                {
                    if (!await FillBufferFromPipeAsync(mainFile, recordHeaderBuf, cancellationToken).ConfigureAwait(false))
                    {
                        break;
                    }

                    var nextRecordHeader = MemoryMarshal.Read<ShapefileMainFileRecordHeader>(recordHeaderBuf.Span);
                    await visitor.VisitMainFileRecordHeaderAsync(nextRecordHeader, cancellationToken).ConfigureAwait(false);

                    uint nextRecordContentLengthInBytes = nextRecordHeader.ContentLengthInBytes;
                    if (nextRecordContentLengthInBytes > int.MaxValue)
                    {
                        throw new NotSupportedException("Each individual shapefile record must be smaller than 2 GiB, for now.");
                    }

                    int recordLength = unchecked((int)nextRecordContentLengthInBytes);
                    IMemoryOwner<byte> shortTermLease;
                    Memory<byte> recordBuf;
                    if (recordLength > oneHundredByteBuffer.Length)
                    {
                        shortTermLease = MemoryPool<byte>.Shared.Rent(recordLength);
                        recordBuf = shortTermLease.Memory.Slice(0, recordLength);
                    }
                    else
                    {
                        shortTermLease = null;
                        recordBuf = oneHundredByteBuffer.Slice(0, recordLength);
                    }

                    using (shortTermLease)
                    {
                        if (!await FillBufferFromPipeAsync(mainFile, recordBuf, cancellationToken).ConfigureAwait(false))
                        {
                            break;
                        }

                        await visitor.VisitMainFileRecordAsync(recordBuf, cancellationToken).ConfigureAwait(false);
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
