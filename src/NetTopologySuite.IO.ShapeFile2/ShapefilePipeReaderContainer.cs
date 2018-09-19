using System;
using System.IO.Pipelines;

namespace NetTopologySuite.IO
{
    /// <summary>
    /// The input side container for the three file streams that are required to be present in any
    /// fully valid ESRI shapefile.
    /// </summary>
    public sealed class ShapefilePipeReaderContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefilePipeReaderContainer"/> class.
        /// </summary>
        /// <param name="mainFileReader">
        /// The value for <see cref="MainFileReader"/>.
        /// </param>
        /// <param name="indexFileReader">
        /// The value for <see cref="IndexFileReader"/>.
        /// </param>
        /// <param name="attributeFileReader">
        /// The value for <see cref="AttributeFileReader"/>.
        /// </param>
        public ShapefilePipeReaderContainer(PipeReader mainFileReader, PipeReader indexFileReader, PipeReader attributeFileReader)
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
        /// Gets the <see cref="PipeReader"/> that holds the file index data.
        /// </summary>
        public PipeReader IndexFileReader { get; }

        /// <summary>
        /// Gets the <see cref="PipeReader"/> that holds the shape-by-shape metadata.
        /// </summary>
        public PipeReader AttributeFileReader { get; }
    }
}
