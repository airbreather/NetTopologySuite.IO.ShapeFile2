using System;
using System.Runtime.InteropServices;

using static NetTopologySuite.IO.BitTwiddlers;

namespace NetTopologySuite.IO
{
    [StructLayout(LayoutKind.Explicit, Size = 100)]
    public struct ShapefileHeader : IEquatable<ShapefileHeader>
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

        public static bool operator ==(ShapefileHeader first, ShapefileHeader second) => BitTwiddlers.Equals(ref first, ref second);

        public static bool operator !=(ShapefileHeader first, ShapefileHeader second) => !BitTwiddlers.Equals(ref first, ref second);

        public override bool Equals(object obj) => obj is ShapefileHeader other && BitTwiddlers.Equals(ref this, ref other);

        public bool Equals(ShapefileHeader other) => BitTwiddlers.Equals(ref this, ref other);

        public override int GetHashCode() => BitTwiddlers.GetHashCode(ref this);

        public override string ToString() => $"ShapefileMainFileHeader[FileCode={this.FileCode}, FileLengthInWords={this.FileLengthInWords}, Version={this.Version}, ShapeTypeForAllRecords={this.ShapeTypeForAllRecords}, BoundingBox={this.BoundingBox}]";
    }
}
