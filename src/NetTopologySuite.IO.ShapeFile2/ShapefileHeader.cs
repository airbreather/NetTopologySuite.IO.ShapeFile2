using System.Runtime.InteropServices;

using static NetTopologySuite.IO.BitTwiddlers;

namespace NetTopologySuite.IO
{
    [StructLayout(LayoutKind.Explicit, Size = 100)]
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

        [FieldOffset(36)]
        private ShapefileBoundingBox boundingBox;

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

        public uint FileLengthInBytes
        {
            get => WordsToBytes(this.FileLengthInWords);
            set => this.FileLengthInWords = BytesToWords(value);
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

        public ShapefileBoundingBox BoundingBox
        {
            get => this.boundingBox;
            set => this.boundingBox = value;
        }

        public override string ToString() => $"ShapefileMainFileHeader[FileCode={this.FileCode}, FileLengthInWords={this.FileLengthInWords}, Version={this.Version}, ShapeTypeForAllRecords={this.ShapeTypeForAllRecords}, BoundingBox={this.BoundingBox}]";
    }
}
