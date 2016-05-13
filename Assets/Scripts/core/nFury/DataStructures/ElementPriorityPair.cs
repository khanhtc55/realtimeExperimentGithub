using System;
namespace nFury.DataStructures
{
	public class ElementPriorityPair<T>
	{
		public T Element
		{
			get;
			set;
		}
		public int Priority
		{
			get;
			set;
		}
		public ElementPriorityPair(T element, int priority)
		{
			this.Element = element;
			this.Priority = priority;
		}
	}
}
