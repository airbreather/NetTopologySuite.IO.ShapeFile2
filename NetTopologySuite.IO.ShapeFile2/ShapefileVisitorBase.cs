using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetTopologySuite.IO
{
    public class ShapefileVisitorBase
    {
        public virtual ValueTask VisitMainFileHeaderAsync(ShapefileHeader header, CancellationToken cancellationToken = default) => default;

        public virtual ValueTask VisitMainFileRecordHeaderAsync(ShapefileMainFileRecordHeader header, CancellationToken cancellationToken = default) => default;

        public async ValueTask VisitMainFileRecordAsync(Memory<byte> rawRecordData, CancellationToken cancellationToken = default)
        {
            rawRecordData = await this.OnVisitRawMainFileRecordAsync(rawRecordData, cancellationToken).ConfigureAwait(false);

            // TODO: interpret the record and add "friendly" methods for subclasses to override.
        }

        protected virtual ValueTask<Memory<byte>> OnVisitRawMainFileRecordAsync(Memory<byte> rawRecordData, CancellationToken cancellationToken) => new ValueTask<Memory<byte>>(rawRecordData);
    }
}
