using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetTopologySuite.IO
{
    public class ShapefileVisitorBase
    {
        public virtual ValueTask<ShapefileHeader> VisitMainFileHeaderAsync(ShapefileHeader header, CancellationToken cancellationToken = default) => new ValueTask<ShapefileHeader>(header);

        public virtual ValueTask<ShapefileMainFileRecordHeader> VisitMainFileRecordHeaderAsync(ShapefileMainFileRecordHeader header, CancellationToken cancellationToken = default) => new ValueTask<ShapefileMainFileRecordHeader>(header);

        public async ValueTask<Memory<byte>> VisitMainFileRecordAsync(Memory<byte> rawRecordData, CancellationToken cancellationToken = default)
        {
            rawRecordData = await this.OnVisitRawMainFileRecordAsync(rawRecordData, cancellationToken).ConfigureAwait(false);

            // TODO: interpret the record and add "friendly" methods for subclasses to override.
            return rawRecordData;
        }

        protected virtual ValueTask<Memory<byte>> OnVisitRawMainFileRecordAsync(Memory<byte> rawRecordData, CancellationToken cancellationToken) => new ValueTask<Memory<byte>>(rawRecordData);
    }
}
