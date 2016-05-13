using nFury.Utils.Core;
using nFury.Utils.Diagnostics;
using nFury.Utils.Scheduling;

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
namespace nFury.Externals.FileManagement
{
	public class VersionedFileManifestLoader : IManifestLoader
	{
		private const int MAX_LOAD_ATTEMPTS = 3;
		private const float LOAD_ATTEMPT_INTERVAL = 0.5f;
		private FmsOptions options;
		private FmsCallback onComplete;
		private FmsCallback onError;
		private MonoBehaviour engine;
		private IFileManifest manifest;
		private string manifestUrl;
		private Logger logger;
		private int loadAttempts;
		public VersionedFileManifestLoader(MonoBehaviour engine)
		{
			this.engine = engine;
			this.logger = Service.Get<Logger>();
		}
		public void Load(FmsOptions options, string manifestUrl, FmsCallback onComplete, FmsCallback onError)
		{
			this.options = options;
			this.onComplete = onComplete;
			this.onError = onError;
			this.manifestUrl = manifestUrl;
			this.logger.DebugFormat("Setting manifestUrl to {0}", new object[]
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
				throw new Exception("The versioned manifest has not been instantiated yet. Has VersionedFileManifestLoader.Load been called?");
			}
			return this.manifest;
		}
		private void AttemptManifestRequest(uint id, object cookie)
		{
			if (++this.loadAttempts > 3)
			{
				this.onError();
			}
			else
			{
				this.engine.StartCoroutine(this.RequestManifestFile());
			}
		}
		private void RetryRequest()
		{
			Service.Get<ViewTimerManager>().CreateViewTimer(0.5f, false, new TimerDelegate(this.AttemptManifestRequest), null);
		}

		private IEnumerator RequestManifestFile()
		{
			yield return null ;
		}
		private void PrepareManifest(string json)
		{
			this.manifest = new VersionedFileManifest();
			this.manifest.Prepare(this.options, json);
			this.onComplete();
		}
	}
}
