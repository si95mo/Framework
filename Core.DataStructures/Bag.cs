using System;
using System.Collections;
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
    public class Bag<T> : IEnumerable<string>, ISerializable
    {
        // public delegate void BagChanged(object sender, BagChangedEventArgs<T> e);

        public event EventHandler<BagChangedEventArgs<IProperty>> ItemAdded;

        public event EventHandler<BagChangedEventArgs<IProperty>> ItemRemoved;

        private readonly Dictionary<string, IProperty> bag;

        /// <summary>
        /// The number of items in the <see cref="Bag{IProperty}"/>
        /// </summary>
        public int Count => bag.Count;

        /// <summary>
        /// The keys of the items in the <see cref="Bag{IProperty}"/>
        /// </summary>
        public List<string> Keys => bag.Keys.ToList();

        /// <summary>
        /// The values of the items in the <see cref="Bag{IProperty}"/>
        /// </summary>
        public List<IProperty> Values => bag.Values.ToList();

        /// <summary>
        /// Create a new instance of <see cref="Bag{IProperty}"/>
        /// </summary>
        public Bag()
        {
            bag = new Dictionary<string, IProperty>();
        }

        protected Bag(SerializationInfo info, StreamingContext context)
        {
            bag = (Dictionary<string, IProperty>)info.GetValue(
                nameof(bag),
                typeof(Dictionary<string, IProperty>)
            );
        }

        // The following method serializes the instance.
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info,
           StreamingContext context)
        {
            info.AddValue(nameof(bag), bag);
        }

        /// <summary>
        /// On item added event
        /// </summary>
        /// <param name="e">The <see cref="BagChangedEventArgs{T}"/></param>
        protected virtual void OnItemAdded(BagChangedEventArgs<IProperty> e)
        {
            ItemAdded?.Invoke(this, e);
        }

        /// <summary>
        /// On item removed event
        /// </summary>
        /// <param name="e">The <see cref="BagChangedEventArgs{T}"/></param>
        protected virtual void OnItemRemoved(BagChangedEventArgs<IProperty> e)
        {
            ItemRemoved?.Invoke(this, e);
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

            if (!bag.ContainsKey(item.Code))
            {
                bag.Add(item.Code, item);
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
        public bool Remove(IProperty item) => bag.Remove(item.Code);

        /// <summary>
        /// Clear the <see cref="Bag{IProperty}"/>, thus
        /// removing all of the stored items
        /// </summary>
        public void Clear() => bag.Clear();

        /// <summary>
        /// Remove an item to the <see cref="Bag"/> given its code.
        /// See <see cref="Remove(IProperty)"/> and also
        /// <see cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
        /// </summary>
        /// <param name="item">The item code to be removed</param>
        /// <returns><see langword="true"/> if the item is removed,
        /// <see langword="false"/> otherwise</returns>
        public bool Remove(string code)
        {
            IProperty itemRemoved = null;

            if (bag.ContainsKey(code))
                itemRemoved = bag[code];

            bool removed = bag.Remove(code);

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
            IProperty item = bag.ContainsKey(code) ? bag[code] : null;

            return item;
        }

        /// <summary>
        /// Convert the <see cref="Bag{T}"/>
        /// into a <see cref="List{IProperty}"/>
        /// </summary>
        /// <returns>The converted <see cref="List{IProperty}"/></returns>
        public List<IProperty> ToList()
        {
            return bag.Values.ToList();
        }

        public override string ToString()
        {
            string description = $"Count = {Count}";
            return description;
        }

        public IEnumerator<string> GetEnumerator()
            => bag.Keys.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}