using System.Runtime.InteropServices;

using static NetTopologySuite.IO.BitTwiddlers;

namespace NetTopologySuite.IO
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ShapefileHeader
    {
        [FieldOffset(0)]
        private int fileCode;

        [FieldOffset(24)]
        private int fileLengthInWords;

        [FieldOffset(28)]
        private int version;

        [FieldOffset(32)]
        private ShapeType shapeTypeForAllRecords;

        public int FileCode
        {
            get => ToOrFromBigEndian(this.fileCode);
            set => this.fileCode = ToOrFromBigEndian(value);
        }

        public int FileLengthInWords
        {
            get => ToOrFromBigEndian(this.fileLengthInWords);
            set => this.fileLengthInWords = ToOrFromBigEndian(value);
        }

        public long FileLengthInBytes
        {
            get => WordsToBytes(this.fileLengthInWords);
            set => this.fileLengthInWords = BytesToWords(value);
        }

        public int Version
        {
            get => ToOrFromLittleEndian(this.version);
            set => this.version = ToOrFromLittleEndian(value);
        }

        public ShapeType ShapeTypeForAllRecords
        {
            get => (ShapeType)ToOrFromLittleEndian((int)this.shapeTypeForAllRecords);
            set => this.shapeTypeForAllRecords = (ShapeType)ToOrFromLittleEndian((int)value);
        }

        [field: FieldOffset(36)]
        public ShapefileBoundingBox BoundingBox { get; set; }

        public override string ToString() => $"ShapefileMainFileHeader[FileCode={this.FileCode}, FileLengthInWords={this.FileLengthInWords}, Version={this.Version}, ShapeTypeForAllRecords={this.ShapeTypeForAllRecords}, BoundingBox={this.BoundingBox}]";
    }
}
