using System;
using System.Runtime.InteropServices;

using static NetTopologySuite.IO.BitTwiddlers;

namespace NetTopologySuite.IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShapefileMainFileRecordHeader : IEquatable<ShapefileMainFileRecordHeader>
    {
        private int recordNumber;

        private int contentLengthInWords;

        public int RecordNumber
        {
            get => ReverseEndianness(this.recordNumber);
            set => this.recordNumber = ReverseEndianness(value);
        }

        public int ContentLengthInWords
        {
            get => ReverseEndianness(this.contentLengthInWords);
            set => this.contentLengthInWords = ReverseEndianness(value);
        }

        public uint ContentLengthInBytes
        {
            get => WordsToBytes(this.ContentLengthInWords);
            set => this.ContentLengthInWords = BytesToWords(value);
        }

        public static bool operator ==(ShapefileMainFileRecordHeader first, ShapefileMainFileRecordHeader second) => first.Equals(second);

        public static bool operator !=(ShapefileMainFileRecordHeader first, ShapefileMainFileRecordHeader second) => !(first == second);

        public override bool Equals(object obj) => obj is ShapefileMainFileRecordHeader other && this.Equals(other);

        public bool Equals(ShapefileMainFileRecordHeader other) => this.recordNumber == other.recordNumber && this.contentLengthInWords == other.contentLengthInWords;

        public override int GetHashCode() => (this.recordNumber, this.contentLengthInWords).GetHashCode();
    }
}
