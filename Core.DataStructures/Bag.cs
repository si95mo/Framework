using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

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

    [Serializable]
    public class Bag<T> : Dictionary<string, IProperty>, ISerializable
    {
        // public delegate void BagChanged(object sender, BagChangedEventArgs<T> e);

        public event EventHandler<BagChangedEventArgs<IProperty>> ItemAdded;

        public event EventHandler<BagChangedEventArgs<IProperty>> ItemRemoved;

        private Dictionary<string, IProperty> set;

        /// <summary>
        /// The number of items in the <see cref="Bag{IProperty}"/>
        /// </summary>
        public new int Count => set.Count;

        /// <summary>
        /// The <see cref="Bag{T}"/> collection of keys
        /// </summary>
        public new KeyCollection Keys => set.Keys;

        /// <summary>
        /// Create a new instance of <see cref="Bag{IProperty}"/>
        /// </summary>
        public Bag()
        {
            set = new Dictionary<string, IProperty>();
        }

        protected Bag(SerializationInfo info, StreamingContext context)
        {
            set = (Dictionary<string, IProperty>)info.GetValue(
                nameof(set), 
                typeof(Dictionary<string, IProperty>)
            );
        }

        // The following method serializes the instance.
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info,
           StreamingContext context)
        {
            info.AddValue(nameof(set), set);
        }

        /// <summary>
        /// On item added event
        /// </summary>
        /// <param name="e">The <see cref="BagChangedEventArgs{T}"/></param>
        protected virtual void OnItemAdded(BagChangedEventArgs<IProperty> e)
        {
            if (ItemAdded != null)
                ItemAdded(this, e);
        }

        /// <summary>
        /// On item removed event
        /// </summary>
        /// <param name="e">The <see cref="BagChangedEventArgs{T}"/></param>
        protected virtual void OnItemRemoved(BagChangedEventArgs<IProperty> e)
        {
            if (ItemRemoved != null)
                ItemRemoved(this, e);
        }

        /// <summary>
        /// Add an item to the <see cref="Bag"/>.
        /// See also <see cref="HashSet{T}.Add(IProperty)"/>
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <returns><see langword="true"/> if the item is added,
        /// <see langword="false"/> otherwise</returns>
        public bool Add(IProperty item)
        {
            bool added = false;

            if (!set.ContainsKey(item.Code))
            {
                set.Add(item.Code, item);
                added = true;
            }

            OnItemAdded(new BagChangedEventArgs<IProperty>(item));

            return added;
        }

        /// <summary>
        /// Remove an item to the <see cref="Bag"/>.
        /// See <see cref="Remove(IProperty)"/> and also
        /// <see cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <returns><see langword="true"/> if the item is removed,
        /// <see langword="false"/> otherwise</returns>
        public bool Remove(IProperty item) => Remove(item.Code);

        /// <summary>
        /// Remove an item to the <see cref="Bag"/> given its code.
        /// See <see cref="Remove(IProperty)"/> and also
        /// <see cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
        /// </summary>
        /// <param name="item">The item code to be removed</param>
        /// <returns><see langword="true"/> if the item is removed,
        /// <see langword="false"/> otherwise</returns>
        public new bool Remove(string code)
        {
            IProperty itemRemoved = null;

            if (set.ContainsKey(code))
                itemRemoved = set[code];

            bool removed = set.Remove(code);

            if (removed)
                OnItemRemoved(new BagChangedEventArgs<IProperty>(itemRemoved));

            return removed;
        }

        /// <summary>
        /// Retrieve an item from the <see cref="Bag{IProperty}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <returns>The <see cref="Core.IProperty"/> if the code is found
        ///  n the <see cref="Bag{IProperty}"/>, <see langword="null"/> otherwise</returns>
        public IProperty Get(string code)
        {
            IProperty item = set.ContainsKey(code) ? set[code] : null;

            return item;
        }

        /// <summary>
        /// Convert the <see cref="Bag{T}"/>
        /// into a <see cref="List{IProperty}"/>
        /// </summary>
        /// <returns>The converted <see cref="List{IProperty}"/></returns>
        public List<IProperty> ToList()
        {
            return set.Values.ToList();
        }

        public override string ToString()
        {
            string description = $"Count = {Count}";
            return description;
        }
    }
}