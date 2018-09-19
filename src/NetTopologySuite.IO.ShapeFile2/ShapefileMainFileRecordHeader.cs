using System.Runtime.InteropServices;

using static NetTopologySuite.IO.BitTwiddlers;

namespace NetTopologySuite.IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShapefileMainFileRecordHeader
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
    }
}
