using System.Threading;
using System.Threading.Tasks;

using NetTopologySuite.IO.ShapeWrappers;

namespace NetTopologySuite.IO
{
    internal sealed class PointCountingVisitor : ShapefileRecordVisitorBase
    {
        public long PointCount { get; private set; }

        protected override ValueTask OnVisitPointXYAsync(PointXY point, CancellationToken cancellationToken)
        {
            ++this.PointCount;
            return default;
        }

        protected override ValueTask OnVisitPolygonXYAsync(PolyLineXY polygon, CancellationToken cancellationToken)
        {
            foreach (var line in polygon)
            {
                this.PointCount += line.Length;
            }

            return default;
        }

        protected override ValueTask OnVisitPolyLineXYAsync(PolyLineXY polyLine, CancellationToken cancellationToken)
        {
            foreach (var line in polyLine)
            {
                this.PointCount += line.Length;
            }

            return default;
        }

        protected override ValueTask OnVisitMultiPointXYAsync(MultiPointXY multiPoint, CancellationToken cancellationToken)
        {
            this.PointCount += multiPoint.Points.Length;
            return default;
        }
    }
}
