using System;
using UnityEngine;
namespace nFury.Externals.FileManagement
{
	public class PassthroughFileManifestLoader : IManifestLoader
	{
		private IFileManifest manifest;
		public void Load(FmsOptions options, string manifestUrl, FmsCallback onComplete, FmsCallback onError)
		{
			this.manifest = new PassthroughFileManifest();
			this.manifest.Prepare(options, string.Empty);
			onComplete();
		}
		public bool IsLoaded()
		{
			return this.manifest != null;
		}
		public IFileManifest GetManifest()
		{
			if (!this.IsLoaded())
			{
				throw new Exception("The passthrough manifest has not been instantiated. Has PassthroughFileManifestLoader.Load been called?");
			}
			return this.manifest;
		}
	}
}
