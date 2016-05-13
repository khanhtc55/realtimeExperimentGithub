using nFury.Utils.Core;
using nFury.Utils.Diagnostics;
using nFury.Utils.Json;
using System;
using System.Collections.Generic;
namespace nFury.Externals.FileManagement
{
	public class VersionedFileManifest : IFileManifest
	{
		private const string VERSION_KEY = "version";
		private const string HASHES_KEY = "hashes";
		private Dictionary<string, object> hashes;
		private Dictionary<string, string> translatedUrls;
		private List<string> cdnRoots;
		private FmsOptions options;
		private bool isReady;
		private string version;
		private Logger logger;
		public VersionedFileManifest()
		{
			this.translatedUrls = new Dictionary<string, string>();
			this.logger = Service.Get<Logger>();
		}
		public void Prepare(FmsOptions options, string json)
		{
			this.options = options;
			object obj = new JsonParser(json).Parse();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
            this.hashes = (Dictionary<string, object>)dictionary[HASHES_KEY];
            if (dictionary[VERSION_KEY] != null)
			{
                this.version = dictionary[VERSION_KEY].ToString();
			}
			this.isReady = true;
			this.logger.DebugFormat("Versioned manifest version #{0} is ready with {1} files.", new object[]
			{
				this.version,
				this.hashes.Count
			});
		}
		public string TranslateFileUrl(string relativePath)
		{
			this.AssertReady();
			if (!this.translatedUrls.ContainsKey(relativePath))
			{
				if (!this.hashes.ContainsKey(relativePath))
				{
					this.logger.WarnFormat("Unable to find '{0}' version in the manifest.", new object[]
					{
						relativePath
					});
					return string.Empty;
				}
                string text = FmsConstants.URL_FORMAT;
                text = text.Replace(FmsConstants.TOKEN_ROOT, "https://starts0-a.akamaihd.net/cloud-cms/");
                text = text.Replace(FmsConstants.TOKEN_CODENAME, this.options.CodeName);
                text = text.Replace(FmsConstants.TOKEN_ENVIRONMENT, this.options.Env.ToString().ToLower());
                text = text.Replace(FmsConstants.TOKEN_RELATIVE_PATH, relativePath);
				text = text.Replace(FmsConstants.TOKEN_HASH ,this.hashes[relativePath].ToString());
				string newValue = relativePath.Substring(relativePath.LastIndexOf("/") + 1);
                text = text.Replace(FmsConstants.TOKEN_FILENAME, newValue);
				this.translatedUrls[relativePath] = text;
			}
			return this.translatedUrls[relativePath];
		}
		public int GetVersionFromFileUrl(string relativePath)
		{
			this.AssertReady();
			if (!this.hashes.ContainsKey(relativePath))
			{
				this.logger.WarnFormat("Unable to find '{0}' url in the manifest.", new object[]
				{
					relativePath
				});
				return 0;
			}
			string text = this.hashes[relativePath].ToString();
			return Convert.ToInt32(text.Substring(text.Length - 8), 16);
		}
		public string GetManifestVersion()
		{
			this.AssertReady();
			return this.version;
		}
		private void AssertReady()
		{
			if (!this.isReady)
			{
				throw new Exception("Versioned maniefest is not ready. Call VersionedFileManifest.Prepare first.");
			}
		}
	}
}
