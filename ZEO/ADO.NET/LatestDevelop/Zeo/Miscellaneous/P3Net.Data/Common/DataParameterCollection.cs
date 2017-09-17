using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace P3Net.Data.Common
{
    /// <summary>Provides a collection of <see cref="DataParameter"/> objects.</summary>
    public sealed class DataParameterCollection : KeyedCollection<string, DataParameter>
    {
        #region Construction

        /// <summary>Initializes an instance of the <see cref="DataParameterCollection"/> class.</summary>
        public DataParameterCollection ()
        { /* Do nothing */ }

        /// <summary>Initializes an instance of the <see cref="DataParameterCollection"/> class.</summary>
        /// <param name="items">The initial items to add.</param>
        public DataParameterCollection ( IEnumerable<DataParameter> items )
        {
            if (items != null)
            {
                foreach (var item in items)
                    this.Add(item);
            };
        }
        #endregion

        /// <summary>Inserts an item.</summary>
        /// <param name="index">The index of the new item.</param>
        /// <param name="item">The item to add.</param>
        protected override void InsertItem ( int index, DataParameter item )
        {
            if (item == null)
                throw new ArgumentNullException("item");

            base.InsertItem(index, item);
        }

        /// <summary>Sets an item.</summary>
        /// <param name="index">The index of the new item.</param>
        /// <param name="item">The item to add.</param>
        protected override void SetItem ( int index, DataParameter item )
        {
            if (item == null)
                throw new ArgumentNullException("item");

            base.SetItem(index, item);
        }

        /// <summary>Gets the key of an item.</summary>
        /// <param name="item">The item.</param>
        /// <returns>The key.</returns>
        protected override string GetKeyForItem ( DataParameter item )
        {
            return item.Name;
        }
    }
}
