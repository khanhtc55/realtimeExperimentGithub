using System;
namespace nFury.DataStructures.PriorityQueue
{
	public class PriorityQueueNode
	{
		public double Priority
		{
			get;
			set;
		}
		public long InsertionIndex
		{
			get;
			set;
		}
		public int QueueIndex
		{
			get;
			set;
		}
	}
}
