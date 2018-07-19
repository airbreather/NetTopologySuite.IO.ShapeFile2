using Moq;
using NUnit.Framework;

namespace NetTopologySuite.IO
{
    public sealed class ShapefileReaderTests
    {
        [Test]
        public void Test()
        {
            ShapefileReader.ReadShapefile(@"C:\Users\Joe\src\NetTopologySuite\NetTopologySuite.Samples.Shapefiles\CA_Cable_region.shp", Mock.Of<ShapefileVisitorBase>());
        }
    }
}
