using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetTopologySuite.IO
{
    public class ShapefileVisitorBase
    {
        public virtual ValueTask VisitMainFileHeaderAsync(ShapefileHeader header, CancellationToken cancellationToken = default) => default;

        public virtual ValueTask VisitMainFileRecordHeaderAsync(ShapefileMainFileRecordHeader header, CancellationToken cancellationToken = default) => default;

        public async ValueTask VisitMainFileRecordAsync(ShapeType shapeType, Memory<byte> rawRecordData, CancellationToken cancellationToken = default)
        {
            (shapeType, rawRecordData) = await this.OnVisitRawMainFileRecordAsync(shapeType, rawRecordData, cancellationToken).ConfigureAwait(false);

            if (rawRecordData.IsEmpty)
            {
                // subclass fully processed this at the raw level.
                // we can move on.
                return;
            }

            // TODO: interpret the record and add "friendly" methods for subclasses to override.
        }

        protected virtual ValueTask<(ShapeType shapeType, Memory<byte> rawRecordData)> OnVisitRawMainFileRecordAsync(ShapeType shapeType, Memory<byte> rawRecordData, CancellationToken cancellationToken) => new ValueTask<(ShapeType shapeType, Memory<byte> rawRecordDate)>((shapeType, rawRecordData));
    }
}
