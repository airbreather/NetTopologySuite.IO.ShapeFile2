using System;
using System.IO.Pipelines;

namespace NetTopologySuite.IO
{
    /// <summary>
    /// The input side container for two of the three file streams that are required to be present
    /// in any fully valid ESRI shapefile.
    /// </summary>
    /// <remarks>
    /// The index file (*.shx) is required by ESRI, but we have no use for it when all we're doing
    /// is reading sequentially.
    /// </remarks>
    public sealed class ShapefilePipeReaderContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefilePipeReaderContainer"/> class.
        /// </summary>
        /// <param name="mainFileReader">
        /// The <see cref="PipeReader"/> that holds the shape data.
        /// </param>
        /// <param name="attributeFileReader">
        /// The <see cref="PipeReader"/> that holds the shape-by-shape metadata.
        /// </param>
        public ShapefilePipeReaderContainer(PipeReader mainFileReader, PipeReader attributeFileReader)
        {
            this.MainFileReader = mainFileReader ?? throw new ArgumentNullException(nameof(mainFileReader));
            this.AttributeFileReader = attributeFileReader ?? throw new ArgumentNullException(nameof(attributeFileReader));
        }

        /// <summary>
        /// Gets the <see cref="PipeReader"/> that holds the shape data.
        /// </summary>
        public PipeReader MainFileReader { get; }

        /// <summary>
        /// Gets the <see cref="PipeReader"/> that holds the shape-by-shape metadata.
        /// </summary>
        public PipeReader AttributeFileReader { get; }
    }
}
