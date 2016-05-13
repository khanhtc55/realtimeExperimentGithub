using System;
using System.Collections.Generic;
using UnityEngine;
namespace nFury.Utils.Core
{
	public class WWWManager
	{
		private List<WWW> outstandingWWWs;
		public WWWManager()
		{
			Service.Set<WWWManager>(this);
			this.outstandingWWWs = new List<WWW>();
		}
		public void CancelAll()
		{
			int i = 0;
			int count = this.outstandingWWWs.Count;
			while (i < count)
			{
				this.outstandingWWWs[i].Dispose();
				i++;
			}
			this.outstandingWWWs.Clear();
		}
		public static void Add(WWW www)
		{
			if (Service.IsSet<WWWManager>())
			{
				Service.Get<WWWManager>().outstandingWWWs.Add(www);
			}
		}
		public static bool Remove(WWW www)
		{
			return Service.IsSet<WWWManager>() && Service.Get<WWWManager>().outstandingWWWs.Remove(www);
		}
	}
}
