using System;
using System.IO;
using System.IO.Pipelines;

namespace NetTopologySuite.IO
{
    /// <summary>
    /// Container for the three <see cref="Stream">Streams</see> that are required to be present in
    /// any valid ESRI shapefile.
    /// </summary>
    public sealed class ShapefileRequiredFileContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileRequiredFileContainer"/> class.
        /// </summary>
        /// <param name="mainFileReader">
        /// The <see cref="PipeReader"/> that holds the shape data.
        /// </param>
        /// <param name="indexFileReader">
        /// The <see cref="PipeReader"/> that holds the spatial index data.
        /// </param>
        /// <param name="attributeFileReader">
        /// The <see cref="PipeReader"/> that holds the shape-by-shape metadata.
        /// </param>
        public ShapefileRequiredFileContainer(PipeReader mainFileReader, PipeReader indexFileReader, PipeReader attributeFileReader)
        {
            this.MainFileReader = mainFileReader ?? throw new ArgumentNullException(nameof(mainFileReader));
            this.IndexFileReader = indexFileReader ?? throw new ArgumentNullException(nameof(indexFileReader));
            this.AttributeFileReader = attributeFileReader ?? throw new ArgumentNullException(nameof(attributeFileReader));
        }

        /// <summary>
        /// Gets the <see cref="PipeReader"/> that holds the shape data.
        /// </summary>
        public PipeReader MainFileReader { get; }

        /// <summary>
        /// Gets the <see cref="PipeReader"/> that holds the spatial index data.
        /// </summary>
        public PipeReader IndexFileReader { get; }

        /// <summary>
        /// Gets the <see cref="PipeReader"/> that holds the shape-by-shape metadata.
        /// </summary>
        public PipeReader AttributeFileReader { get; }
    }
}
