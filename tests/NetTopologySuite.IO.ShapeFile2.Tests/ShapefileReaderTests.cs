using NUnit.Framework;
using Pipelines.Sockets.Unofficial;
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
            using (var mainFileStream = new FileStream(@"D:\TIGER-CA\ROADS - NAD83.shp", FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan | FileOptions.Asynchronous))
            using (var indexFileStream = new FileStream(@"D:\TIGER-CA\ROADS - NAD83.shx", FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan | FileOptions.Asynchronous))
            using (var attributeFileStream = new FileStream(@"D:\TIGER-CA\ROADS - NAD83.dbf", FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan | FileOptions.Asynchronous))
            {
                var fileContainer = new ShapefileRequiredFileReaderContainer(
                    StreamConnection.GetReader(mainFileStream, new PipeOptions(useSynchronizationContext: false)),
                    StreamConnection.GetReader(indexFileStream, new PipeOptions(useSynchronizationContext: false)),
                    StreamConnection.GetReader(attributeFileStream, new PipeOptions(useSynchronizationContext: false)));
                var visitor = new ShapefileVisitorBase();
                await ShapefileReader.ReadShapefileAsync(fileContainer, visitor);
            }
        }
    }
}
