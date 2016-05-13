using System;
using System.Collections.Generic;
namespace nFury.DataStructures
{
	public class PriorityList<T>
	{
		private List<ElementPriorityPair<T>> list;
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}
		public PriorityList()
		{
			this.list = new List<ElementPriorityPair<T>>();
		}
		public virtual int Add(T element, int priority)
		{
			if (element == null)
			{
				return -1;
			}
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				ElementPriorityPair<T> elementPriorityPair = this.list[i];
				if (object.ReferenceEquals(elementPriorityPair.Element, element))
				{
					return -1;
				}
				if (priority > elementPriorityPair.Priority)
				{
					this.list.Insert(i, new ElementPriorityPair<T>(element, priority));
					return i;
				}
				i++;
			}
			this.list.Add(new ElementPriorityPair<T>(element, priority));
			return this.list.Count - 1;
		}
		public T GetElement(int i)
		{
			return this.list[i].Element;
		}
		public int GetPriority(int i)
		{
			return this.list[i].Priority;
		}
		public void GetElementPriority(int i, out T element, out int priority)
		{
			ElementPriorityPair<T> elementPriorityPair = this.list[i];
			element = elementPriorityPair.Element;
			priority = elementPriorityPair.Priority;
		}
		public int IndexOf(T element)
		{
			int i = 0;
			int count = this.list.Count;
			while (i < count)
			{
				if (object.ReferenceEquals(this.list[i].Element, element))
				{
					return i;
				}
				i++;
			}
			return -1;
		}
		public void RemoveAt(int i)
		{
			this.list.RemoveAt(i);
		}
	}
}
