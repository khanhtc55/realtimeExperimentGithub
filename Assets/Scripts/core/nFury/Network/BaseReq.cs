using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using System.Collections.Generic;
using Sfs2X.Requests;
namespace core.nFury.Network
{
	public class BaseReq
	{
		private string cmdId;

		public string CmdId {
			get {
				return cmdId;
			}
		}

		public BaseReq(string cmdId)
		{
			this.cmdId = cmdId;
		}

		protected BaseReq(){}

		public virtual ISFSObject GetParams(){
			return new SFSObject();
		}

	}
}


