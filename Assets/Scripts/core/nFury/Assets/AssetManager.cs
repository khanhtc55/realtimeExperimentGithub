using nFury.Externals.FileManagement;
using nFury.Main.Configs;
using nFury.Utils;
using nFury.Utils.Core;
using nFury.Utils.Diagnostics;
using nFury.Utils.Scheduling;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
namespace nFury.Assets
{
    public class AssetManager : IViewFrameTimeObserver
    {
        private const string ASSETBUNDLE_DIR = "assetbundles/";
#if UNITY_ANDROID
		private const string ASSETBUNDLE_TARGET = "android";
		private const string ASSETBUNDLE_SUBDIR = "android/";
		private const string LOCALBUNDLE_EXT = ".local.android.assetbundle";
		private const string ASSETBUNDLE_PATH_PREFIX = "assetbundles/android/";
		
#else
        private const string ASSETBUNDLE_TARGET = "ios";
        private const string ASSETBUNDLE_SUBDIR = "ios/";
        private const string LOCALBUNDLE_EXT = ".local.ios.assetbundle";
        private const string ASSETBUNDLE_PATH_PREFIX = "assetbundles/ios/";
#endif

        private const string BATTLES_DIR = "battles/";

        private const string ASSETBUNDLE_EXT = ".assetbundle";

        private const string PNG_EXT = ".png";
        public const string JSON_EXT = ".json";
        public const string JSON_COMPRESSED_EXT = ".json.zip";


        private const float MAX_CALLBACKS_TIME_PER_FRAME = 0.011f;
        private const int MAX_CALLBACKS_PER_FRAME = 10;
        private const float LAZY_LOAD_START_DELAY = 3f;
        private const float UNLOAD_PRELOADABLES_DELAY = 5f;

        private Dictionary<string, ManifestEntry> manifest;
        private Dictionary<string, AssetInfo> assetInfos;
        private Dictionary<string, HashSet<string>> bundleContents;
        private AssetHandle nextRequestHandle;
        private Dictionary<AssetHandle, string> requestedAssets;
        private List<AssetRequest> callbackQueue;
        private MutableIterator callbackQueueIter;
        private LoadingPhase phase;
        private Dictionary<string, AssetHandle> preloadables;
        private HashSet<string> customPreloadables;
        private Dictionary<string, AssetHandle> lazyloadables;
        private bool unloadedPreloadables;
	    private bool loadLocal = false;
        public GameShaders Shaders
        {
            get;
            private set;
        }
        public AssetProfiler Profiler
        {
            get;
            private set;
        }
	    protected void Init(bool loadLocal)
	    {
			this.phase = LoadingPhase.Initialized;
			this.preloadables = new Dictionary<string, AssetHandle>();
			this.customPreloadables = new HashSet<string>();
			this.lazyloadables = new Dictionary<string, AssetHandle>();
			this.unloadedPreloadables = false;
			this.bundleContents = new Dictionary<string, HashSet<string>>();
			this.manifest = new Dictionary<string, ManifestEntry>();

			this.assetInfos = new Dictionary<string, AssetInfo>();
			this.callbackQueue = new List<AssetRequest>();
			this.callbackQueueIter = new MutableIterator();
			this.nextRequestHandle = AssetHandle.FirstAvailable;
			this.requestedAssets = new Dictionary<AssetHandle, string>();
			this.Shaders = null;
			this.Profiler = new AssetProfiler();
			this.loadLocal = loadLocal;
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
	    }
        public void AddToManifest(AssetType assetType, string assetName, string assetPath)
        {
            if (string.IsNullOrEmpty(assetName) || string.IsNullOrEmpty(assetPath) || this.manifest.ContainsKey(assetName))
            {
                return;
            }
//#if UNITY_IOS
//			if (assetPath.EndsWith(".local.ios.assetbundle") && !this.customPreloadables.Contains(assetName))
//#elif UNITY_ANDROID
//			if (assetPath.EndsWith(".local.android.assetbundle") && !this.customPreloadables.Contains(assetName))
//#endif
//            {
//                this.customPreloadables.Add(assetName);
//            }

            this.manifest.Add(assetName, new ManifestEntry(assetType, assetPath));
            if (this.InBundle(assetType, assetPath))
            {
                HashSet<string> hashSet;
                if (!this.manifest.ContainsKey(assetPath))
                {
                    this.manifest.Add(assetPath, new ManifestEntry(AssetType.Bundle, assetPath));
                    hashSet = new HashSet<string>();
                    this.bundleContents.Add(assetPath, hashSet);
                }
                else
                {
                    hashSet = this.bundleContents[assetPath];
                }
                hashSet.Add(assetName);

            }
        }
        public void AddJsonFileToManifest(string assetName, string assetPath)
        {
            string text = assetName;
            if (!text.EndsWith(".json"))
            {
                text += ".json";
            }
            if (!string.IsNullOrEmpty(assetPath))
            {
                text = assetPath + "/" + text;
            }
            this.AddToManifest(AssetType.Text, assetName, text);
        }
        public void AddZipFileToManifest(string assetName, string assetPath)
        {
            string text = assetName;
            if (!text.EndsWith(".json.zip"))
            {
                text += ".json.zip";
            }
            if (!string.IsNullOrEmpty(assetPath))
            {
                text = assetPath + "/" + text;
            }
            this.AddToManifest(AssetType.Bytes, assetName, text);
        }
        public void AddPNGFileToManifest(string assetName, string assetPath)
        {
            this.AddToManifest(AssetType.Texture, assetName, assetPath + "/" + assetName + ".png");
        }
		public void AddModelToManifest(string assetName, string bundleName)
		{
			this.AddToManifest(AssetType.UXGameObject, assetName, bundleName);
		}
	    public void AddToPreload(string assetName, AssetHandle assetHandle)
	    {
            if(!this.preloadables.ContainsKey(assetName))
		    this.preloadables.Add(assetName,assetHandle);
	    }

        /// <summary>
        /// return true if the assetType isnt AssetType.Bundle and the extension part of the the assetPath is .assetbundle
        /// </summary>
        /// <param name="assetType"></param>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        private bool InBundle(AssetType assetType, string assetPath)
        {
            return true;
#if UNITY_IOS
			return assetType != AssetType.Bundle && (assetPath.EndsWith(".assetbundle") || assetPath.EndsWith(".local.ios.assetbundle"));
#elif UNITY_ANDROID
			return assetType != AssetType.Bundle && (assetPath.EndsWith(".assetbundle") || assetPath.EndsWith(".local.android.assetbundle"));
#endif
            return assetType != AssetType.Bundle && (assetPath.EndsWith(".assetbundle"));
        }
        public void SetupManifest(bool inFue)
        {
           // this.AddToManifest(AssetType.Texture, current16.AssetName, this.DeduceAssetPath(current16.BundleName));
        }
        private string DeduceAssetPath(string bundleName)
        {
//#if UNITY_IOS
//			return (!AssetConstants.LOCAL_BUNDLE_NAMES.Contains(bundleName)) ? ("assetbundles/ios/" + bundleName + ".assetbundle") : (bundleName + ".local.ios.assetbundle");
//#elif UNITY_ANDROID
//			return (!AssetConstants.LOCAL_BUNDLE_NAMES.Contains(bundleName)) ? ("assetbundles/android/" + bundleName + ".assetbundle") : (bundleName + ".local.android.assetbundle");
//			
//#endif
            return bundleName ;
        }
        public void LoadGameShaders(AssetsCompleteDelegate onCompleteCallback, object onCompleteCookie)
        {
            this.Shaders = new GameShaders(onCompleteCallback, onCompleteCookie);
        }
        public void ReleaseAll()
        {
            this.Shaders = null;
            this.Profiler = null;
            List<AssetBundle> list = new List<AssetBundle>();
            foreach (string current in this.assetInfos.Keys)
            {
                AssetInfo assetInfo = this.assetInfos[current];
                if (assetInfo.AssetType == AssetType.Bundle)
                {
                    AssetBundle assetBundle = assetInfo.AssetObject as AssetBundle;
                    if (assetBundle != null)
                    {
                        list.Add(assetBundle);
                    }
                }
                else
                {
                    ManifestEntry manifestEntry = this.manifest[assetInfo.AssetName];
                    if (!this.InBundle(assetInfo.AssetType, manifestEntry.AssetPath) && assetInfo.AssetObject is UnityEngine.Object)
                    {
                        UnityEngine.Object obj = (UnityEngine.Object)assetInfo.AssetObject;
                        UnityEngine.Object.Destroy(obj);
                    }
                }
            }
            this.assetInfos.Clear();
            this.callbackQueue.Clear();
            this.callbackQueueIter.Reset();
            int i = 0;
            int count = list.Count;
            while (i < count)
            {
                list[i].Unload(true);
                i++;
            }
            this.manifest.Clear();
            this.bundleContents.Clear();
        }
        private AssetInfo GetContainingBundle(AssetInfo assetInfo, string assetPath)
        {
            if (!this.InBundle(assetInfo.AssetType, assetPath))
            {
                return null;
            }
            if (!this.assetInfos.ContainsKey(assetPath))
            {
                return null;
            }
            return this.assetInfos[assetPath];
        }
        private void ReferenceAssetRecursively(AssetInfo assetInfo, string assetPath)
        {
            assetInfo.LoadCount++;
            AssetInfo containingBundle = this.GetContainingBundle(assetInfo, assetPath);
            if (containingBundle != null)
            {
                containingBundle.LoadCount++;
            }
            string assetName = assetInfo.AssetName;
            if (this.preloadables.ContainsKey(assetName) && assetInfo.LoadCount > 1)
            {
                this.Unload(this.preloadables[assetName]);
                this.preloadables.Remove(assetName);
            }
        }
        public void Load(ref AssetHandle handle, string assetName, AssetSuccessDelegate onSuccess, AssetFailureDelegate onFailure, object cookie)
        {

            Service.Get<Logger>().DebugFormat("Load handle {0} : assetName ---> {1}", handle, assetName);
            if (handle != AssetHandle.Invalid)
            {
                throw new Exception("AssetManager: load requres invalid input handle");
            }
            handle = this.nextRequestHandle++;

            this.requestedAssets.Add(handle, assetName);
            if (string.IsNullOrEmpty(assetName))
            {
                Service.Get<Logger>().Error("Asset name cannot be null or empty");
                if (onFailure != null)
                {
                    this.callbackQueue.Add(new AssetRequest(handle, assetName, null, onFailure, cookie));
                }
                return;
            }

            if (!this.manifest.ContainsKey(assetName))
            {
                //DebugCenter.Instance.Log("checkpoint 4.1");//////////////////////
                Service.Get<Logger>().Error("Asset not found in the manifest: " + assetName);
                if (onFailure != null)
                {
                    this.callbackQueue.Add(new AssetRequest(handle, assetName, null, onFailure, cookie));
                }
                return;
            }
            //make sure that the all of the paramenters are valid, call onnFailure and return if invalid

            ManifestEntry manifestEntry = this.manifest[assetName];
            AssetType assetType = manifestEntry.AssetType;
            string assetPath = manifestEntry.AssetPath;
            bool flag = false;
            AssetInfo assetInfo = null;


            //DebugCenter.Instance.Log("checkpoint 6?,check assetType: " + assetType + ",," + assetPath);/////////khong di vao day
            if (this.assetInfos.ContainsKey(assetName))
            {

                assetInfo = this.assetInfos[assetName];
                //DebugCenter.Instance.Log("checkpoint 6.1?");//////////////////////
                if (assetInfo.AssetRequests == null)
                {
                    if (assetInfo.AssetObject != null)
                    {
                        //DebugCenter.Instance.Log("checkpoint 6.1.1?");//////////////////////success case
                        if (onSuccess != null)
                        {
                            //DebugCenter.Instance.Log("checkpoint 6.1.1.1");//////////////////////
                            this.callbackQueue.Add(new AssetRequest(handle, assetName, onSuccess, null, cookie));
                        }
                    }
                    else
                    {
                        //DebugCenter.Instance.Log("checkpoint 6.1.2");//////////////////////failure case
                        if (assetInfo.AllContentsExtracted)
                        {
                            assetInfo.AllContentsExtracted = false;
                            flag = true;
                        }
                        else
                        {
                            if (onFailure != null)
                            {
                                this.callbackQueue.Add(new AssetRequest(handle, assetName, null, onFailure, cookie));
                            }
                        }
                    }
                    if (!flag)
                    {
                        //DebugCenter.Instance.Log("checkpoint 6.1.3");//////////////////////
                        this.ReferenceAssetRecursively(assetInfo, assetPath);
                        return;
                    }
                }
            }


            AssetRequest item = new AssetRequest(handle, assetName, onSuccess, onFailure, cookie);
            //DebugCenter.Instance.Log("checkpoint 7? check handle also: " + handle);//////////////////////khong di vao day
            if (!flag && assetInfo != null)
            {
                //DebugCenter.Instance.Log("checkpoint 7.1");//////////////////////
                assetInfo.AssetRequests.Add(item);
                this.ReferenceAssetRecursively(assetInfo, assetPath);
                return;
            }

            //DebugCenter.Instance.Log("checkpoint 8?");////////////////////// 
            if (!flag)
            {
                //DebugCenter.Instance.Log("checkpoint 8.1");//////////////////////
                assetInfo = new AssetInfo(assetName, assetType);//<--------<<<<<<<<<<<<<<<<<------------------------
                assetInfo.LoadCount++;
                this.assetInfos.Add(assetName, assetInfo);
            }
            assetInfo.AssetRequests = new List<AssetRequest>();
            assetInfo.AssetRequests.Add(item);
            bool flag2 = this.InBundle(assetType, assetPath);

            //DebugCenter.Instance.Log("checkpoint 9?");//////////////////////
            if (flag2)
            {
                //DebugCenter.Instance.Log("checkpoint 9.1");//////////////////////khong di vao day
                if (this.phase == LoadingPhase.PreLoading && !this.preloadables.ContainsKey(assetName) && !this.customPreloadables.Contains(assetName))
                {
                    Service.Get<Logger>().Warn("Asset not flagged for preload: " + assetName);
                }
                AssetHandle bundleHandle = AssetHandle.Invalid;
                this.Load(ref bundleHandle, assetPath, new AssetSuccessDelegate(this.OnBundleSuccess), new AssetFailureDelegate(this.OnBundleFailure), assetName);
                assetInfo.BundleHandle = bundleHandle;
            }
            else
            {
                //DebugCenter.Instance.Log("checkpoint 9.2");////////////////////// co di vao nhanh nay
                RotCoroutiner.StartCoroutine(this.Fetch(assetPath, assetInfo));//<- chua onSuccess
            }
            //DebugCenter.Instance.Log("checkpoint 10");//////////////////////
        }
        public void Unload(AssetHandle handle)
        {
            if (!this.requestedAssets.ContainsKey(handle))
            {
                Service.Get<Logger>().Error("Unload: invalid request handle: " + (uint)handle);
                return;
            }
            string text = this.requestedAssets[handle];
            if (string.IsNullOrEmpty(text))
            {
                Service.Get<Logger>().Error("Unload: asset name cannot be null or empty");
                return;
            }
            if (!this.assetInfos.ContainsKey(text))
            {
                Service.Get<Logger>().Error("Unload: not loaded: " + text);
                return;
            }
            if (!this.manifest.ContainsKey(text))
            {
                Service.Get<Logger>().Error("Unload: asset not in the manifest: " + text);
                return;
            }
            AssetInfo assetInfo = this.assetInfos[text];
            ManifestEntry manifestEntry = this.manifest[text];
            string assetPath = manifestEntry.AssetPath;
            int i = 0;
            int count = this.callbackQueue.Count;
            while (i < count)
            {
                if (this.callbackQueue[i].Handle == handle)
                {
                    this.RemoveFromCallbackQueue(i);
                    break;
                }
                i++;
            }
            if (assetInfo.AssetRequests != null)
            {
                if (assetInfo.UnloadHandles == null)
                {
                    assetInfo.UnloadHandles = new List<AssetHandle>();
                }
                assetInfo.UnloadHandles.Add(handle);
                int j = 0;
                int count2 = assetInfo.AssetRequests.Count;
                while (j < count2)
                {
                    if (assetInfo.AssetRequests[j].Handle == handle)
                    {
                        assetInfo.AssetRequests.RemoveAt(j);
                        break;
                    }
                    j++;
                }
                return;
            }
            if (assetInfo.LoadCount == 0)
            {
                Service.Get<Logger>().ErrorFormat("Asset {0} has negative loadCount={1}", new object[]
				{
					text,
					assetInfo.LoadCount
				});
                return;
            }
            assetInfo.LoadCount--;
            if (assetInfo.AssetType != AssetType.Bundle || assetInfo.LoadCount == 0)
            {
                this.requestedAssets.Remove(handle);
            }
            bool flag = this.InBundle(assetInfo.AssetType, assetPath);
            if (assetInfo.LoadCount <= 0)
            {
                object assetObject = assetInfo.AssetObject;
                assetInfo.AssetObject = null;
                this.assetInfos.Remove(text);
                if (assetInfo.AssetType == AssetType.Bundle)
                {
                    AssetBundle assetBundle = assetObject as AssetBundle;
                    if (assetBundle != null)
                    {
                        assetBundle.Unload(true);
                    }
                }
                else
                {
                    if (!flag)
                    {
                        UnityEngine.Object @object = assetObject as UnityEngine.Object;
                        if (@object != null)
                        {
                            UnityEngine.Object.Destroy(@object);
                        }
                    }
                }
            }
            if (flag)
            {
                this.Unload(assetInfo.BundleHandle);
                if (assetInfo.LoadCount == 0 && !this.preloadables.ContainsKey(text))
                {
                    HashSet<string> hashSet = this.bundleContents[assetPath];
                    hashSet.Add(text);
                }
            }
        }
        private void AllContentsExtracted(string bundleName)
        {
            AssetInfo assetInfo = this.assetInfos[bundleName];
            AssetBundle assetBundle = assetInfo.AssetObject as AssetBundle;
            if (assetBundle != null)
            {
                assetInfo.AssetObject = null;
                assetInfo.AllContentsExtracted = true;
                assetBundle.Unload(false);
            }
        }
        public void MultiLoad(List<AssetHandle> handles, List<string> assetNames, AssetSuccessDelegate onSuccess, AssetFailureDelegate onFailure, List<object> cookies, AssetsCompleteDelegate onComplete, object completeCookie)
        {
            if (assetNames == null || cookies == null || handles == null || assetNames.Count == 0 || assetNames.Count != cookies.Count || assetNames.Count != handles.Count)
            {
                throw new Exception("AssetManager: multi-load requires matching input lists");
            }
            int count = assetNames.Count;
            RefCount refCount = new RefCount(count);
            for (int i = 0; i < count; i++)
            {
                MultiAssetInfo cookie = new MultiAssetInfo(assetNames[i], onSuccess, onFailure, cookies[i], refCount, onComplete, completeCookie);
                AssetHandle value = handles[i];
                this.Load(ref value, assetNames[i], new AssetSuccessDelegate(this.OnMultiLoadSuccess), new AssetFailureDelegate(this.OnMultiLoadFailure), cookie);
                handles[i] = value;
            }
        }
        public void RegisterPreloadableAsset(string assetName)
        {
            LoadingPhase loadingPhase = this.phase;
            if (loadingPhase == LoadingPhase.Initialized || loadingPhase == LoadingPhase.PreLoading)
            {
                if (!this.customPreloadables.Contains(assetName))
                {
                    this.customPreloadables.Add(assetName);
                }
            }
        }
        public void PreloadAssets(AssetsCompleteDelegate onComplete, object completeCookie)
        {
            if (this.phase == LoadingPhase.Initialized)
            {
                this.phase = LoadingPhase.PreLoading;
                List<string> list = new List<string>();
                List<object> list2 = new List<object>();
                List<AssetHandle> list3 = new List<AssetHandle>();
                foreach (string current in this.preloadables.Keys)
                {
                    list.Add(current);
                    list2.Add(new InternalLoadCookie(current));
                    list3.Add(AssetHandle.Invalid);
                }
                if (list.Count == 0)
                {
                    if (onComplete != null)
                    {
                        onComplete(completeCookie);
                    }
                }
                else
                {
                    this.MultiLoad(list3, list, null, null, list2, onComplete, completeCookie);
                    int i = 0;
                    int count = list3.Count;
                    while (i < count)
                    {
                        this.preloadables[list[i]] = list3[i];
                        i++;
                    }
                }
            }
        }
        public void DonePreloading()
        {
            if (this.phase == LoadingPhase.PreLoading)
            {
                this.phase = LoadingPhase.OnDemand;
                this.customPreloadables.Clear();
                Service.Get<ViewTimerManager>().CreateViewTimer(3f, false, new TimerDelegate(this.OnLazyLoadStartTimer), null);
            }
        }
        private void OnLazyLoadStartTimer(uint id, object cookie)
        {
            this.phase = LoadingPhase.LazyLoading;
            List<string> list = new List<string>();
            List<object> list2 = new List<object>();
            List<AssetHandle> list3 = new List<AssetHandle>();
            foreach (string current in this.lazyloadables.Keys)
            {
                list.Add(current);
                list2.Add(new InternalLoadCookie(current));
                list3.Add(AssetHandle.Invalid);
            }
            if (list.Count == 0)
            {
                this.OnLazyLoadComplete(null);
            }
            else
            {
                this.MultiLoad(list3, list, new AssetSuccessDelegate(this.OnLazyLoadSuccess), new AssetFailureDelegate(this.OnLazyLoadFailure), list2, new AssetsCompleteDelegate(this.OnLazyLoadComplete), null);
                int i = 0;
                int count = list3.Count;
                while (i < count)
                {
                    this.lazyloadables[list[i]] = list3[i];
                    i++;
                }
            }
        }
        public void UnloadPreloadables()
        {
            if (this.unloadedPreloadables)
            {
                return;
            }
            this.unloadedPreloadables = true;
            Service.Get<ViewTimerManager>().CreateViewTimer(5f, false, new TimerDelegate(this.OnUnloadPreloadablesTimer), null);
        }
        private void OnUnloadPreloadablesTimer(uint id, object cookie)
        {
            foreach (AssetHandle current in this.preloadables.Values)
            {
                this.Unload(current);
            }
            this.preloadables.Clear();
        }
        private void OnLazyLoadSuccess(object asset, object cookie)
        {
            string assetName = ((InternalLoadCookie)cookie).AssetName;
            this.Unload(this.lazyloadables[assetName]);
            this.lazyloadables.Remove(assetName);
        }
        private void OnLazyLoadFailure(object cookie)
        {
            this.OnLazyLoadSuccess(null, cookie);
        }
        private void OnLazyLoadComplete(object cookie)
        {
            this.phase = LoadingPhase.OnDemand;
        }

        /// <summary>
        /// return [Application.streamingAssetsPath + assetPath], WARNING: there is unreachable code
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected virtual string GetBundleUrl(string assetPath, out int version)
        {
            if (assetPath.EndsWith(LOCALBUNDLE_EXT) || this.loadLocal)  // for all true at Dev Mod
            {
                version = 0;
                string str = Application.streamingAssetsPath + "/" + assetPath; //Path.Combine(Application.streamingAssetsPath, assetPath);
                if (!str.StartsWith("file://"))
                {
                    return ("file://" + str);
                }
                return str;
            }
            version = Service.Get<FMS>().GetFileVersion(assetPath);
            return Service.Get<FMS>().GetFileUrl(assetPath);
        }


        private IEnumerator Fetch(string assetPath, AssetInfo assetInfo)
        {


            int version = 0;
            bool ignoreCache = this.Profiler.Enabled;
            WWW www;
            String error = null;

            AssetBundle bundle;

            object asset = null;

            //DebugCenter.Instance.Log("Fetch_checkpoint 0, assetPath:" + assetPath);//////////////////////
            String url = GetBundleUrl(assetPath, out version);
            //DebugCenter.Instance.Log("Fetch_checkpoint 1, url:" + url);//////////////////////

            if (!string.IsNullOrEmpty(url))
            {
                //DebugCenter.Instance.Log("Fetch_checkpoint 2?");////////////////////////////////////
                if (!ignoreCache && (assetInfo.AssetType == AssetType.Bundle||assetInfo.AssetType==AssetType.UXGameObject))
                {
                    //DebugCenter.Instance.Log("Fetch_checkpoint 2.1");//////////////khong di vao cai nay
                  
                    www = WWW.LoadFromCacheOrDownload(url, version);
                }
                else
                {
                    //DebugCenter.Instance.Log("Fetch_checkpoint 2.2");////////////////////////////////////<--<<<
                    this.Profiler.RecordFetchEvent(assetPath, 0, false, false);
                    www = new WWW(url);
                }

                WWWManager.Add(www);
	            if (!www.isDone)
	            {
		            yield return null;
	            }
	            //DebugCenter.Instance.Log("Fetch_checkpoint 3?");////////////////////////////////////
                if (WWWManager.Remove(www))
                {
                    error = www.error;
                    //DebugCenter.Instance.Log("Fetch_checkpoint 3.1? check error: " + error);////////////////////////////////////
                    if (string.IsNullOrEmpty(error))
                    {
                        //DebugCenter.Instance.Log("Fetch_checkpoint 3.1.1");///////khong vao day vi co error. Can phai vao
                        if (ignoreCache)
                        {
                            this.Profiler.RecordFetchEvent(assetPath, www.bytesDownloaded, false, true);
                        }
                        switch (assetInfo.AssetType)
                        {
                            case AssetType.Bundle:
                                bundle = www.assetBundle;
                                if (bundle != null)
                                {
                                    asset = bundle;
                                }
                                break;

                            case AssetType.Text:
                                asset = www.text;
                                break;
                            case AssetType.Bytes:
                                asset = www.bytes;
                                break;
                            case AssetType.AudioClip:
                                asset = www.audioClip;
                                break;
                            case AssetType.Texture:
                                asset = www.texture;
                                break;
                            case AssetType.UXGameObject:
                                bundle = www.assetBundle;
                                UnityEngine.Debug.Log(assetPath+"  "+assetInfo.AssetName);
                                asset = bundle.Load(assetInfo.AssetName, typeof(GameObject)) as object;
                                bundle.Unload(false);
                                //DebugCenter.Instance.Log("Fetch_checkpoint 3.1.1.1, test asset: " + asset);////////////////////////////////////
                                break;
                        }
                    }
                }

                www.Dispose();
            }
            else
            {
                error = string.Format("Unable to map '{0}' to a valid url", assetPath);
            }

            //DebugCenter.Instance.Log("Fetch_checkpoint 4? is asset null:" + (asset == null));//////////////
            if (asset == null)
            {
                //DebugCenter.Instance.Log("Fetch_checkpoint 4.1, haiz assetBundle null chac roi");/////////////////////////////////////////
                object[] args = new object[] { url, error };
                Service.Get<Logger>().ErrorFormat("Failed to fetch asset {0} ({1})", args);
            }

            this.OnAssetLoaded(asset, assetInfo);
        }


        private void OnAssetLoaded(object asset, AssetInfo assetInfo)
        {

            assetInfo.AssetObject = asset;
            List<AssetRequest> assetRequests = assetInfo.AssetRequests;
            assetInfo.AssetRequests = null;
            //DebugCenter.Instance.Log("OnAssetLoaded_cpoint 1?");/////////////////////////////////////
            if (assetInfo.UnloadHandles != null)
            {
                //DebugCenter.Instance.Log("OnAssetLoaded_cpoint 1.1");/////////////khong vao day
                int i = 0;
                int count = assetInfo.UnloadHandles.Count;
                while (i < count)
                {
                    this.Unload(assetInfo.UnloadHandles[i]);
                    i++;
                }
                assetInfo.UnloadHandles = null;
            }

            //DebugCenter.Instance.Log("OnAssetLoaded_cpoint 2?");/////////////////////////////////////
            if (asset != null)
            {
                //DebugCenter.Instance.Log("OnAssetLoaded_cpoint 2.1, kkk, thanh cong loading roi");////////////////////why not go in here?
                int j = 0;
                int count2 = assetRequests.Count;
                while (j < count2)
                {
                    AssetRequest assetRequest = assetRequests[j];
                    if (assetRequest.OnSuccess != null)
                    {
                        this.callbackQueue.Add(new AssetRequest(assetRequest.Handle, assetRequest.AssetName, assetRequest.OnSuccess, null, assetRequest.Cookie));
                    }
                    j++;
                }
            }
            else
            {
                //DebugCenter.Instance.Log("OnAssetLoaded_cpoint 2.2(failure case), check assetRequest.count:" + assetRequests.Count);/////////////////////////////////////
                int k = 0;
                int count3 = assetRequests.Count;
                while (k < count3)
                {
                    AssetRequest assetRequest2 = assetRequests[k];
                    if (assetRequest2.OnFailure != null)
                    {
                        //DebugCenter.Instance.Log("OnAssetLoaded_cpoint 2.2.loop_in");///////////////////////////
                        this.callbackQueue.Add(new AssetRequest(assetRequest2.Handle, assetRequest2.AssetName, null, assetRequest2.OnFailure, assetRequest2.Cookie));
                    }
                    k++;
                }
            }
        }


        private object Prepare(AssetInfo assetInfo)
        {
            object obj = assetInfo.AssetObject;
            if (this.IsAssetCloned(assetInfo))
            {
                GameObject gameObject = obj as GameObject;
                obj = null;
                if (gameObject != null)
                {
                    gameObject = this.CloneGameObject(gameObject);
                    UnityUtils.RemoveColliders(gameObject);
                    obj = gameObject;
                }
                else
                {
                    if (assetInfo.AssetObject != null)
                    {
                        Service.Get<Logger>().Error("Loaded asset of unexpected type " + assetInfo.AssetObject.GetType().Name);
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// instantiate the gameObject and set it the same name
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public GameObject CloneGameObject(GameObject gameObject)
        {
            string name = gameObject.name;
            gameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
            gameObject.name = name;
            return gameObject;
        }
        private void OnBundleSuccess(object asset, object cookie)
        {
            string text = cookie as string;
            AssetInfo assetInfo = this.assetInfos[text];
            AssetBundle assetBundle = asset as AssetBundle;
            ManifestEntry manifestEntry = this.manifest[text];
            string assetPath = manifestEntry.AssetPath;
            asset = null;
            if (assetBundle == null)
            {
                Service.Get<Logger>().ErrorFormat("Unable to load bundle {0} for asset {1}", new object[]
				{
					assetPath,
					text
				});
            }
            else
            {
                bool flag = true;
                if (this.phase == LoadingPhase.LazyLoading && this.lazyloadables.ContainsKey(text))
                {
                    flag = false;
                    List<AssetRequest> assetRequests = assetInfo.AssetRequests;
                    int i = 0;
                    int count = assetRequests.Count;
                    while (i < count)
                    {
                        AssetRequest request = assetRequests[i];
                        if (!this.IsForInternalLoad(request))
                        {
                            flag = true;
                            break;
                        }
                        i++;
                    }
                }
                if (flag)
                {
                    asset = assetBundle.Load(text);
                    if (asset == null)
                    {
                        Service.Get<Logger>().ErrorFormat("Unable to load asset {0} from bundle {1} (main asset {2})", new object[]
						{
							text,
							assetPath,
							assetBundle.mainAsset
						});
                    }
                    else
                    {
                        HashSet<string> hashSet = this.bundleContents[assetPath];
                        hashSet.Remove(text);
//                        if (hashSet.Count != 0 || assetPath != this.manifest["gui_shared"].AssetPath)
//                        {
//                        }
                    }
                }
            }
            this.OnAssetLoaded(asset, assetInfo);
        }
        private void OnBundleFailure(object cookie)
        {
            this.OnBundleSuccess(null, cookie);
        }
        private void OnMultiLoadSuccess(object asset, object cookie)
        {
            MultiAssetInfo multiAssetInfo = (MultiAssetInfo)cookie;
            if (multiAssetInfo.OnSuccess != null)
            {
                multiAssetInfo.OnSuccess(asset, multiAssetInfo.Cookie);
            }
            if (multiAssetInfo.RefCount.Release() == 0 && multiAssetInfo.OnComplete != null)
            {
                multiAssetInfo.OnComplete(multiAssetInfo.CompleteCookie);
            }
        }
        private void OnMultiLoadFailure(object cookie)
        {
            MultiAssetInfo multiAssetInfo = (MultiAssetInfo)cookie;
            if (multiAssetInfo.OnFailure != null)
            {
                multiAssetInfo.OnFailure(multiAssetInfo.Cookie);
            }
            if (multiAssetInfo.RefCount.Release() == 0 && multiAssetInfo.OnComplete != null)
            {
                multiAssetInfo.OnComplete(multiAssetInfo.CompleteCookie);
            }
        }
        private bool IsForInternalLoad(AssetRequest request)
        {
            return request.Cookie is MultiAssetInfo && ((MultiAssetInfo)request.Cookie).Cookie is InternalLoadCookie;
        }
        private AssetRequest RemoveFromCallbackQueue(int i)
        {
            AssetRequest result = this.callbackQueue[i];
            this.callbackQueue.RemoveAt(i);
            this.callbackQueueIter.OnRemove(i);
            return result;
        }
        public void OnViewFrameTime(float dt)
        {
            int count = this.callbackQueue.Count;
            if (count == 0)
            {
                return;
            }
            float realTimeSinceStartUp = UnityUtils.GetRealTimeSinceStartUp();
            int num = 0;
            this.callbackQueueIter.Init(count);
            while (this.callbackQueueIter.Active())
            {
                //lay ra assetRequest(list cac item(AssetRequest), moi item luu assetname, handle, success, failure,...)
                AssetRequest assetRequest = this.RemoveFromCallbackQueue(this.callbackQueueIter.Index);
                num++;
                string assetName = assetRequest.AssetName;
                if (string.IsNullOrEmpty(assetName))
                {
                    if (assetRequest.OnFailure != null)
                    {
                        assetRequest.OnFailure(assetRequest.Cookie);
                    }
                }
                else
                {
                    if (this.assetInfos.ContainsKey(assetName))
                    {
                        if (assetRequest.OnSuccess != null)
                        {
                            AssetInfo assetInfo = this.assetInfos[assetName];
                            bool flag = true;
                            if (this.phase == LoadingPhase.PreLoading && this.preloadables.ContainsKey(assetInfo.AssetName) && this.IsForInternalLoad(assetRequest))
                            {
                                flag = false;
                            }
                            else
                            {
                                if (this.phase == LoadingPhase.LazyLoading && this.lazyloadables.ContainsKey(assetInfo.AssetName) && this.IsForInternalLoad(assetRequest))
                                {
                                    flag = false;
                                }
                            }
                            object asset = (!flag) ? assetInfo.AssetObject : this.Prepare(assetInfo);
                            assetRequest.OnSuccess(asset, assetRequest.Cookie);
                        }
                        else
                        {
                            assetRequest.OnFailure(assetRequest.Cookie);
                        }
                    }
                }
                float num2 = UnityUtils.GetRealTimeSinceStartUp() - realTimeSinceStartUp;
                if (num2 == 0f)
                {
                    if (num >= 10)
                    {
                        this.callbackQueueIter.Next();
                        break;
                    }
                }
                else
                {
                    if (num2 >= 0.011f)
                    {
                        this.callbackQueueIter.Next();
                        break;
                    }
                }
                this.callbackQueueIter.Next();
            }
            this.callbackQueueIter.Reset();
        }
        public bool IsAssetCloned(string assetName)
        {
            bool result = false;
            if (this.assetInfos.ContainsKey(assetName))
            {
                result = this.IsAssetCloned(this.assetInfos[assetName]);
            }
            return result;
        }
        private bool IsAssetCloned(AssetInfo assetInfo)
        {
            return assetInfo.AssetType == AssetType.ClonedGameObject;
        }
    }
}
