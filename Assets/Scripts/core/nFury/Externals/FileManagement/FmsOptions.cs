using System;
using UnityEngine;
namespace nFury.Externals.FileManagement
{
	public class FmsOptions
	{
		public string CodeName
		{
			get;
			set;
		}
		public FmsEnvironment Env
		{
			get;
			set;
		}
		public FmsMode Mode
		{
			get;
			set;
		}
		public string ManifestVersion
		{
			get;
			set;
		}
		public MonoBehaviour Engine
		{
			get;
			set;
		}
		public string RootUrl
		{
			get;
			set;
		}
		public override string ToString()
		{
			return string.Format("CodeName: {0}, Env {1}, Mode {2}, ManifestVersion {3}, RootUrl {4}", new object[]
			{
				this.CodeName,
				this.Env,
				this.Mode,
				this.ManifestVersion,
				this.RootUrl
			});
		}
	}
}
