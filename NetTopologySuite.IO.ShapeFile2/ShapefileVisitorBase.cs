using System;

namespace NetTopologySuite.IO
{
    public abstract class ShapefileVisitorBase
    {
        public virtual void VisitMainFileHeader(ref ShapefileHeader header) { }

        public virtual void VisitMainFileRecordHeader(ref ShapefileMainFileRecordHeader header) { }

        public void VisitMainFileRecord(Span<byte> rawRecordData)
        {
            this.OnVisitRawMainFileRecord(ref rawRecordData);

            // TODO: interpret the record and add "friendly" methods for subclasses to override.
        }

        protected virtual void OnVisitRawMainFileRecord(ref Span<byte> rawRecordData) { }
    }
}
