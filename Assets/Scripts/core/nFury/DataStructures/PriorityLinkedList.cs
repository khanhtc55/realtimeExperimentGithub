using System;
using System.Collections.Generic;
namespace nFury.DataStructures
{
	public class PriorityLinkedList<T>
	{
		private LinkedList<ElementPriorityPair<T>> list;
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}
		public PriorityLinkedList()
		{
			this.list = new LinkedList<ElementPriorityPair<T>>();
		}
		public void Add(T element, int priority)
		{
			if (element == null)
			{
				return;
			}
			LinkedListNode<ElementPriorityPair<T>> linkedListNode = this.list.First;
			ElementPriorityPair<T> elementPriorityPair = new ElementPriorityPair<T>(element, priority);
			while (linkedListNode != null)
			{
				if (priority > linkedListNode.Value.Priority)
				{
					this.list.AddBefore(linkedListNode, elementPriorityPair);
					return;
				}
				linkedListNode = linkedListNode.Next;
			}
			this.list.AddLast(elementPriorityPair);
		}
		public void Remove(T element)
		{
			if (element == null)
			{
				return;
			}
			for (LinkedListNode<ElementPriorityPair<T>> linkedListNode = this.list.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				if (object.ReferenceEquals(linkedListNode.Value.Element, element))
				{
					this.list.Remove(linkedListNode);
					return;
				}
			}
		}
		public IEnumerable<ElementPriorityPair<T>> List()
		{
			return this.list;
		}
		public T Pop()
		{
			LinkedListNode<ElementPriorityPair<T>> first = this.list.First;
			this.list.RemoveFirst();
			return first.Value.Element;
		}
		public ElementPriorityPair<T> First()
		{
			if (this.list.First == null)
			{
				return null;
			}
			return this.list.First.Value;
		}
	}
}
