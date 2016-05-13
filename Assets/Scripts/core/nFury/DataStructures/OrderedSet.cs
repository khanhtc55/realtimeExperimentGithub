using System;
using System.Collections;
using System.Collections.Generic;
namespace nFury.DataStructures
{
	public class OrderedSet<T> : IEnumerable, ICollection<T>, IEnumerable<T>
	{
		private readonly IDictionary<T, LinkedListNode<T>> dictionary;
		private readonly LinkedList<T> linkedList;
		public int Count
		{
			get
			{
				return this.dictionary.Count;
			}
		}
		public virtual bool IsReadOnly
		{
			get
			{
				return this.dictionary.IsReadOnly;
			}
		}
		public LinkedListNode<T> First
		{
			get
			{
				return this.linkedList.First;
			}
		}
		public LinkedListNode<T> Last
		{
			get
			{
				return this.linkedList.Last;
			}
		}
		public OrderedSet() : this(EqualityComparer<T>.Default)
		{
		}
		public OrderedSet(IEqualityComparer<T> comparer)
		{
			this.dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
			this.linkedList = new LinkedList<T>();
		}
		void ICollection<T>.Add(T item)
		{
			this.Add(item);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		public void Clear()
		{
			this.linkedList.Clear();
			this.dictionary.Clear();
		}
		public bool Remove(T item)
		{
			LinkedListNode<T> node;
			if (!this.dictionary.TryGetValue(item, out node))
			{
				return false;
			}
			this.dictionary.Remove(item);
			this.linkedList.Remove(node);
			return true;
		}
		public IEnumerator<T> GetEnumerator()
		{
			return this.linkedList.GetEnumerator();
		}
		public bool Contains(T item)
		{
			return this.dictionary.ContainsKey(item);
		}
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.linkedList.CopyTo(array, arrayIndex);
		}
		public bool Add(T item)
		{
			if (this.dictionary.ContainsKey(item))
			{
				return false;
			}
			LinkedListNode<T> value = this.linkedList.AddLast(item);
			this.dictionary.Add(item, value);
			return true;
		}
	}
}
