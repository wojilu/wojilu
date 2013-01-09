using System;

namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Represents a Name/Value pair.
    /// </summary>
    public class ParamPair : IComparable<ParamPair>
    {
        /// <summary>
        ///   Initializes a new instance of <see cref="ParamPair" /> .
        /// </summary>
        public ParamPair()
        {
        }

        /// <summary>
        ///   Initializes a new instance of <see cref="ParamPair" /> with the name and value.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="value"> The value. </param>
        public ParamPair(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        ///   Gets or sets the name field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the value field.
        /// </summary>
        public string Value { get; set; }

        #region IComparable<ParamPair> Members

        /// <summary>
        ///   Compares current <c>ParamPair</c> object with another one.
        /// </summary>
        /// <param name="other"> The other <c>ParamPair</c> to compare to. </param>
        /// <returns> The compare result. </returns>
        public int CompareTo(ParamPair other)
        {
            if (null == Name)
            {
                if (null == other.Name)
                {
                    if (null == Value)
                        return null == other.Value ? 0 : -1;
                    else
                        return null == other.Value ? 1 : Value.CompareTo(other.Value);
                }
                else
                    return -1;
            }
            else
            {
                int equal = Name.CompareTo(other.Name);

                if (0 == equal)
                    equal = Value.CompareTo(other.Value);

                return equal;
            }
        }

        #endregion
    }
}