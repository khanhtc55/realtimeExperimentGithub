using nFury.Utils.Core;
using nFury.Utils.Diagnostics;

using System;
using System.IO;
using System.Net;
namespace nFury.Externals.FileManagement
{
	public class ServerVersionedFileManifestLoader : IManifestLoader
	{
		private const int MAX_LOAD_ATTEMPTS = 3;
		private const float LOAD_ATTEMPT_INTERVAL = 0.5f;
		private FmsOptions options;
		private FmsCallback onComplete;
		private IFileManifest manifest;
		private string manifestUrl;
		private int loadAttempts;
		public void Load(FmsOptions options, string manifestUrl, FmsCallback onComplete, FmsCallback onError)
		{
			this.options = options;
			this.onComplete = onComplete;
			this.manifestUrl = manifestUrl;
			Service.Get<Logger>().DebugFormat("Setting manifestUrl to {0}", new object[]
			{
				manifestUrl
			});
			this.AttemptManifestRequest(0u, null);
		}
		public bool IsLoaded()
		{
			return this.manifest != null;
		}
		public IFileManifest GetManifest()
		{
			if (!this.IsLoaded())
			{
				throw new Exception("The versioned manifest has not been instantiated yet.");
			}
			return this.manifest;
		}
		private void AttemptManifestRequest(uint id, object cookie)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this.manifestUrl);
			httpWebRequest.KeepAlive = false;
			httpWebRequest.ProtocolVersion = HttpVersion.Version10;
			WebResponse response = httpWebRequest.GetResponse();
			using (Stream responseStream = response.GetResponseStream())
			{
				if (responseStream != null)
				{
					StreamReader streamReader = new StreamReader(responseStream);
					this.PrepareManifest(streamReader.ReadToEnd());
					responseStream.Close();
				}
			}
		}
		private void PrepareManifest(string json)
		{
			this.manifest = new VersionedFileManifest();
			this.manifest.Prepare(this.options, json);
			this.onComplete();
		}
	}
}
