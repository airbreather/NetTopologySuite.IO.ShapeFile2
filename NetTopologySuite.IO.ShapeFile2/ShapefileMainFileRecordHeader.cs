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
            get => ToOrFromBigEndian(this.recordNumber);
            set => this.recordNumber = ToOrFromBigEndian(value);
        }

        public int ContentLengthInWords
        {
            get => ToOrFromBigEndian(this.contentLengthInWords);
            set => this.contentLengthInWords = ToOrFromBigEndian(value);
        }

        public long ContentLengthInBytes
        {
            get => WordsToBytes(this.ContentLengthInWords);
            set => this.ContentLengthInWords = BytesToWords(value);
        }
    }
}
