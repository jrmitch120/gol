using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Server.Classes
{
    [Serializable]
    public class Tunnel<T> : IEnumerable<T>
    {
        private readonly int _capacity;
        private readonly List<T> _items = new List<T>();

        /// <summary>
        /// Collection clases similar to a Stack.  Objects added to the Tunnel that exceed initial capacity are ejected FIFO.
        /// </summary>
        /// <param name="capacity">Maximum cappacity for object before ejecting FIFO</param>
        public Tunnel(int capacity)
        {
            _capacity = capacity;
        }

        public void Push(T item)
        {
            if (_items.Count == _capacity)
                _items.RemoveAt(_items.Count - 1);

            _items.Insert(0,item);
        }

        public T Peek()
        {
            return _items.Count > 0 ? (_items.First()) : default(T);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return (_items.GetEnumerator());
        }

        public IEnumerator GetEnumerator()
        {
            return (_items.GetEnumerator());
        }
    }
}
