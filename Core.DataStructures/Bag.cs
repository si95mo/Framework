using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Core.DataStructures
{
    /// <summary>
    /// Handle the <see cref="Bag{T}"/> collection change
    /// </summary>
    /// <typeparam name="T">The type of the items in the <see cref="Bag{T}"/></typeparam>
    public class BagChangedEventArgs<T> : EventArgs where T : IProperty
    {
        /// <summary>
        /// The item
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
            Item = item;
        }
    }

    /// <summary>
    /// Handle the <see cref="Bag{T}"/> collection is cleared
    /// </summary>
    /// <typeparam name="T">The type of the items in the <see cref="Bag{T}"/></typeparam>
    public class BagClearedEventArgs<T> : EventArgs where T : IProperty
    {
        /// <summary>
        /// The <see cref="Bag{T}"/>
        /// </summary>
        public Bag<T> Bag
        {
            get;
            set;
        }

        /// <summary>
        /// Create a new instance of <see cref="BagClearedEventArgs{T}"/>
        /// </summary>
        /// <param name="bag">The <see cref="Bag{T}"/></param>
        public BagClearedEventArgs(Bag<T> bag)
        {
            Bag = bag;
        }
    }

    /// <summary>
    /// Class that represent a mathematical set of item,
    /// i.e. a collection of distinct items
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="Bag{T}"/></typeparam>
    [Serializable]
    public class Bag<T> : IEnumerable<T>, ISerializable where T : IProperty
    {
        #region Event handlers

        /// <summary>
        /// <see cref="EventHandler"/> invoked when an item is added to the <see cref="Bag{T}"/>
        /// </summary>
        public event EventHandler<BagChangedEventArgs<T>> Added;

        /// <summary>
        /// <see cref="EventHandler"/> invoked when an item is removed from the <see cref="Bag{T}"/>
        /// </summary>
        public event EventHandler<BagChangedEventArgs<T>> Removed;

        /// <summary>
        /// <see cref="EventHandler"/> invoked when the <see cref="Bag{T}"/> is cleared (see <see cref="Clear"/>)
        /// </summary>
        public event EventHandler<BagClearedEventArgs<T>> Cleared;

        #endregion Event handlers

        #region Public properties

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
        public List<T> Values => bag.Values.OfType<T>().ToList();

        #endregion Public properties

        private readonly Dictionary<string, T> bag;

        /// <summary>
        /// Create a new instance of <see cref="Bag{T}"/>
        /// </summary>
        public Bag()
        {
            bag = new Dictionary<string, T>();
        }

        /// <summary>
        /// Constructor method use d in the serialization
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/></param>
        /// <param name="context">The <see cref="StreamingContext"/></param>
        protected Bag(SerializationInfo info, StreamingContext context)
        {
            bag = (Dictionary<string, T>)info.GetValue(nameof(bag), typeof(Dictionary<string, T>));
        }

        #region Handlers

        /// <summary>
        /// On item added event
        /// </summary>
        /// <param name="e">The <see cref="BagChangedEventArgs{T}"/></param>
        protected virtual void OnItemAdded(BagChangedEventArgs<T> e)
        {
            Added?.Invoke(this, e);
        }

        /// <summary>
        /// On item removed event
        /// </summary>
        /// <param name="e">The <see cref="BagChangedEventArgs{T}"/></param>
        protected virtual void OnItemRemoved(BagChangedEventArgs<T> e)
        {
            Removed?.Invoke(this, e);
        }

        #endregion Handlers

        /// <summary>
        /// Add an item to the <see cref="Bag{T}"/>
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <returns><see langword="true"/> if the item is added,
        /// <see langword="false"/> otherwise</returns>
        public bool Add(T item)
        {
            bool added = false;

            if (item != null)
            {
                if (!bag.ContainsKey(item.Code))
                {
                    bag.Add(item.Code, item);
                    added = true;

                    OnItemAdded(new BagChangedEventArgs<T>(item));
                }
            }

            return added;
        }

        /// <summary>
        /// Remove an item to the <see cref="Bag{T}"/>. See <see cref="Remove(T)"/> and also <see cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <returns><see langword="true"/> if the item is removed,
        /// <see langword="false"/> otherwise</returns>
        public bool Remove(T item)
            => Remove(item.Code);

        /// <summary>
        /// Remove an item to the <see cref="Bag{T}"/> given its code. See <see cref="Remove(T)"/> and also <see cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
        /// </summary>
        /// <param name="code">The item code to be removed</param>
        /// <returns><see langword="true"/> if the item is removed,
        /// <see langword="false"/> otherwise</returns>
        public bool Remove(string code)
        {
            T itemRemoved = default;

            if (bag.ContainsKey(code))
            {
                itemRemoved = bag[code];
            }

            bool removed = bag.Remove(code);

            if (removed)
            {
                OnItemRemoved(new BagChangedEventArgs<T>(itemRemoved));
            }

            return removed;
        }

        /// <summary>
        /// Clear the <see cref="Bag{T}"/>, thus removing all of the stored items
        /// </summary>
        public void Clear()
        {
            Cleared?.Invoke(this, new BagClearedEventArgs<T>(this));
            bag.Clear();
        }

        /// <summary>
        /// Retrieve an item from the <see cref="Bag{T}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <returns>The object if the code is found in the <see cref="Bag{T}"/>, or <see langword="default"/> otherwise</returns>
        public T Get(string code)
        {
            T item = bag.ContainsKey(code) ? (T)bag[code] : default;
            return item;
        }

        /// <summary>
        /// Convert the <see cref="Bag{T}"/>
        /// into a <see cref="List{T}"/>
        /// </summary>
        /// <returns>The converted <see cref="List{T}"/></returns>
        public List<T> ToList()
        {
            return bag.Values.ToList();
        }

        public override string ToString()
        {
            string description = $"Count = {Count}";
            return description;
        }

        /// <summary>
        /// Check if the <see cref="Bag{T}"/> contains a specific <paramref name="key"/>
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns><see langword="true"/> if <paramref name="key"/> is found, <see langword="false"/> otherwise</returns>
        public bool ContainsKey(string key)
            => bag.ContainsKey(key);

        #region Interfaces implementation

        /// <summary>
        /// Get the <see cref="IEnumerator{T}"/> used to
        /// iterate through the <see cref="Bag{T}"/>
        /// </summary>
        /// <returns>The <see cref="IEnumerator{T}"/></returns>
        public IEnumerator<T> GetEnumerator()
            => bag.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <summary>
        /// Serialize the instance of <see cref="Bag{T}"/>
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/></param>
        /// <param name="context">The <see cref="StreamingContext"/></param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(bag), bag);
        }

        #endregion Interfaces implementation
    }
}