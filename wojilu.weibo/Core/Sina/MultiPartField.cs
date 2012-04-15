
namespace wojilu.weibo.Core
{
    /// <summary>
    /// Represents a muit-part field.
    /// </summary>
    public class MultiPartField
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MultiPartField"/>.
        /// </summary>
        public MultiPartField()
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="MultiPartField"/> with the specified <paramref name="name"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        public MultiPartField(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the name of the multi-part field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the multi-part field.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the file path of the multi-part field it it is a file field.
        /// </summary>
        public string FilePath { get; set; }
    }
}
