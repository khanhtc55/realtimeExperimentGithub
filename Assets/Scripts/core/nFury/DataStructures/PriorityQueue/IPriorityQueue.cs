using System;
using System.Collections;
using System.Collections.Generic;
namespace nFury.DataStructures.PriorityQueue
{
	public interface IPriorityQueue<T> : IEnumerable, IEnumerable<T> where T : PriorityQueueNode
	{
		T First
		{
			get;
		}
		int Count
		{
			get;
		}
		int MaxSize
		{
			get;
		}
		void Remove(T node);
		void UpdatePriority(T node, int priority);
		void Enqueue(T node, int priority);
		T Dequeue();
		void Clear();
		bool Contains(T node);
	}
}
