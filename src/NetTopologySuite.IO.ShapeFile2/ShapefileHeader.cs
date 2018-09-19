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

        [field: FieldOffset(28)]
        public int Version { get; set; }

        [field: FieldOffset(32)]
        public ShapeType ShapeTypeForAllRecords { get; set; }

        [FieldOffset(36)]
        public ShapefileBoundingBoxXYZM BoundingBox;

        public int FileCode
        {
            get => ReverseEndianness(this.fileCode);
            set => this.fileCode = ReverseEndianness(value);
        }

        public int FileLengthInWords
        {
            get => ReverseEndianness(this.fileLengthInWords);
            set => this.fileLengthInWords = ReverseEndianness(value);
        }

        public uint FileLengthInBytes
        {
            get => WordsToBytes(this.FileLengthInWords);
            set => this.FileLengthInWords = BytesToWords(value);
        }

        public override string ToString() => $"ShapefileMainFileHeader[FileCode={this.FileCode}, FileLengthInWords={this.FileLengthInWords}, Version={this.Version}, ShapeTypeForAllRecords={this.ShapeTypeForAllRecords}, BoundingBox={this.BoundingBox}]";
    }
}
