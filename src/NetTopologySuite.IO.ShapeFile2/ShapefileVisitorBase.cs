using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetTopologySuite.IO
{
    public abstract class ShapefileVisitorBase
    {
        public virtual ValueTask VisitMainFileHeaderAsync(ShapefileHeader header, CancellationToken cancellationToken = default) => default;

        public virtual ValueTask VisitMainFileRecordHeaderAsync(ShapefileMainFileRecordHeader header, CancellationToken cancellationToken = default) => default;

        public virtual ValueTask VisitMainFileRecordAsync(ReadOnlyMemory<byte> rawRecordData, CancellationToken cancellationToken = default) => default;
    }
}
