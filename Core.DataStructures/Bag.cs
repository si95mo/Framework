using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataStructures
{
    public class BagChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// The <see cref="T"/> item
        /// </summary>
        public T Item
        {
            get;
            set;
        }

        /// <summary>
        /// Create a new instance of <see cref="BagChangedEventArgs{T}"/>
        /// </summary>
        /// <param name="item">The item</param>
        public BagChangedEventArgs(T item)
        {
            this.Item = item;
        }
    }

    public class Bag<IProperty> : Dictionary<string, Core.IProperty>
    {
        // public delegate void BagChanged(object sender, BagChangedEventArgs<T> e);

        public event EventHandler<BagChangedEventArgs<Core.IProperty>> ItemAdded;
        public event EventHandler<BagChangedEventArgs<Core.IProperty>> ItemRemoved;

        private Dictionary<string, Core.IProperty> set;

        /// <summary>
        /// The number of items in the <see cref="Bag{IProperty}"/>
        /// </summary>
        public new int Count => set.Count;

        /// <summary>
        /// Create a new instance of <see cref="Bag{Core.IProperty}"/>
        /// </summary>
        public Bag()
        {
            set = new Dictionary<string, Core.IProperty>();
        }

        /// <summary>
        /// On item added event
        /// </summary>
        /// <param name="e">The <see cref="BagChangedEventArgs{T}"/></param>
        protected virtual void OnItemAdded(BagChangedEventArgs<Core.IProperty> e)
        {
            if (ItemAdded != null)
                ItemAdded(this, e);
        }

        /// <summary>
        /// On item removed event
        /// </summary>
        /// <param name="e">The <see cref="BagChangedEventArgs{T}"/></param>
        protected virtual void OnItemRemoved(BagChangedEventArgs<Core.IProperty> e)
        {
            if (ItemRemoved != null)
                ItemRemoved(this, e);
        }

        /// <summary>
        /// Add an item to the <see cref="Bag"/>.
        /// See also <see cref="HashSet{T}.Add(Core.IProperty)"/>
        /// </summary>
        /// <param name="item">The item to be added</param>
        public void Add(Core.IProperty item)
        {
            set.Add(item.Code, item);

            OnItemAdded(new BagChangedEventArgs<Core.IProperty>(item));
        }

        /// <summary>
        /// Remove an item to the <see cref="Bag"/>.
        /// See <see cref="Remove(Core.IProperty)"/> and also 
        /// <see cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <returns><see langword="true"/> if the item is removed,
        /// <see langword="false"/> otherwise</returns>
        public bool Remove(Core.IProperty item) => Remove(item.Code);

        /// <summary>
        /// Remove an item to the <see cref="Bag"/> given its code.
        /// See <see cref="Remove(Core.IProperty)"/> and also 
        /// <see cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
        /// </summary>
        /// <param name="item">The item code to be removed</param>
        /// <returns><see langword="true"/> if the item is removed,
        /// <see langword="false"/> otherwise</returns>
        public new bool Remove(string code)
        {
            Core.IProperty itemRemoved = null;

            if (set.ContainsKey(code))
                itemRemoved = set[code];

            bool removed = set.Remove(code);

            if (removed)
                OnItemRemoved(new BagChangedEventArgs<Core.IProperty>(itemRemoved));

            return removed;
        }

        /// <summary>
        /// Retrieve an item from the <see cref="Bag{IProperty}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <returns>The <see cref="Core.IProperty"/> if the code is found
        ///  n the <see cref="Bag{IProperty}"/>, <see langword="null"/> otherwise</returns>
        public Core.IProperty Get(string code)
        {
            Core.IProperty item = set.ContainsKey(code) ? set[code] : null;

            return item;
        }
    }
}
