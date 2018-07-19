using System;
using System.IO;

namespace NetTopologySuite.IO
{
    /// <summary>
    /// Container for the three <see cref="Stream">Streams</see> that are required to be present in
    /// any valid ESRI shapefile.
    /// </summary>
    public sealed class ShapefileRequiredStreamContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileRequiredStreamContainer"/> class.
        /// </summary>
        /// <param name="mainFileStream">
        /// The <see cref="Stream"/> that holds the shape data.
        /// </param>
        /// <param name="indexFileStream">
        /// The <see cref="Stream"/> that holds the spatial index data.
        /// </param>
        /// <param name="attributeFileStream">
        /// The <see cref="Stream"/> that holds the shape-by-shape metadata.
        /// </param>
        public ShapefileRequiredStreamContainer(Stream mainFileStream, Stream indexFileStream, Stream attributeFileStream)
        {
            this.MainFileStream = mainFileStream ?? throw new ArgumentNullException(nameof(mainFileStream));
            this.IndexFileStream = indexFileStream ?? throw new ArgumentNullException(nameof(indexFileStream));
            this.AttributeFileStream = attributeFileStream ?? throw new ArgumentNullException(nameof(attributeFileStream));
        }

        /// <summary>
        /// Gets the <see cref="Stream"/> that holds the shape data.
        /// </summary>
        public Stream MainFileStream { get; }

        /// <summary>
        /// Gets the <see cref="Stream"/> that holds the spatial index data.
        /// </summary>
        public Stream IndexFileStream { get; }

        /// <summary>
        /// Gets the <see cref="Stream"/> that holds the shape-by-shape metadata.
        /// </summary>
        public Stream AttributeFileStream { get; }
    }
}
