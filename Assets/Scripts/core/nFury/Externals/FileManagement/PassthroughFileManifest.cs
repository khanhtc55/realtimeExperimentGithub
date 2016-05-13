using nFury.Utils.Core;
using nFury.Utils.Diagnostics;

using System;
namespace nFury.Externals.FileManagement
{
	public class PassthroughFileManifest : IFileManifest
	{
		private FmsOptions options;
		public void Prepare(FmsOptions options, string json)
		{
			this.options = options;
			Service.Get<Logger>().Debug("Passthrough manifest is ready.");
		}
		public string TranslateFileUrl(string relativePath)
		{
			return this.options.RootUrl + relativePath;
		}
		public int GetVersionFromFileUrl(string relativePath)
		{
			return 0;
		}
		public string GetManifestVersion()
		{
			return "0";
		}
	}
}
