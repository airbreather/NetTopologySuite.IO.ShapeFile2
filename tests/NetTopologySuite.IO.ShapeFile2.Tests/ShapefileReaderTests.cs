using NUnit.Framework;
using Pipelines.Sockets.Unofficial;
using System;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace NetTopologySuite.IO
{
    public sealed class ShapefileReaderTests
    {
        [Test]
        public async Task Test()
        {
            var visitor = new PointCountingVisitor();
            using (var mainFileStream = new FileStream(@"D:\TIGER-CA\ROADS - NAD83.shp", FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan | FileOptions.Asynchronous))
            using (var attributeFileStream = new FileStream(@"D:\TIGER-CA\ROADS - NAD83.dbf", FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan | FileOptions.Asynchronous))
            {
                var fileContainer = new ShapefilePipeReaderContainer(
                    StreamConnection.GetReader(mainFileStream, new PipeOptions(useSynchronizationContext: false)),
                    StreamConnection.GetReader(attributeFileStream, new PipeOptions(useSynchronizationContext: false)));
                await ShapefileReader.ReadShapefileAsync(fileContainer, visitor);
            }

            Console.WriteLine("Shapefile has {0} points.", visitor.PointCount);
        }
    }
}
