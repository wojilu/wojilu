using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Represents the collection of <c>ParamPair</c> .
    /// </summary>
    public class ParamCollection : Collection<ParamPair>
    {
        /// <summary>
        ///   Initializes a new instance of <c>ParamCollection</c> .
        /// </summary>
        public ParamCollection()
        {
        }

        /// <summary>
        ///   Initializes a new instance of <c>ParamCollection</c> .
        /// </summary>
        /// <param name="items"> The items to add. </param>
        public ParamCollection(IEnumerable<ParamPair> items)
        {
            foreach (ParamPair item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        ///   Adds a name and value pari into the collection.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="value"> The value. </param>
        public void Add(string name, string value)
        {
            base.Add(new ParamPair(name, value));
        }
    }
}