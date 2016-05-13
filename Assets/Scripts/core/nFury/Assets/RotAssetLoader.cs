using System;
using System.Collections.Generic;
using Artemis.System;
using nFury.Assets;
using nFury.Externals.FileManagement;
using nFury.Utils.Core;
using Sfs2X.Logging;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class RotAssetLoader
{
    private bool isResoucesLoad = false;
    private bool isLoadLocal = false;
    private Queue<RotAssetRequest> queue = new Queue<RotAssetRequest>();
    private AssetsCompleteDelegate defaultAssetsCompleteDelegate = delegate(object cookie) { };
    private AssetFailureDelegate defaultAssetFailureDelegate = delegate(object cookie) { };
    private bool isInDownloadQueue = false;
    private Dictionary<string, AssetBundle> dic = new Dictionary<string, AssetBundle>();
    public RotAssetLoader()
    {
        Service.Set<RotAssetLoader>(this);
        //isResoucesLoad = (ScriptableObjectData.buildConfigTool.loadAssetsMode == LoadAssetsMode.RESOURCES);
        //isLoadLocal = (ScriptableObjectData.buildConfigTool.loadAssetsMode == LoadAssetsMode.BUNDLE_LOCAL);
    }

    public T Load<T>(string name) where T : Object
    {
        return (T)Resources.Load<T>(name);
    }

    public void Load(RotAssetRequest assetRequest)
    {
        if (isResoucesLoad)// || Service.Get<GameStaticData>().isTestScene)
        {
            ResourcesLoad(assetRequest);
        }
        else
        {
            queue.Enqueue(assetRequest);
            if (!isInDownloadQueue)
            {
                defaultAssetFailureDelegate = delegate(object cookie) { };
                defaultAssetsCompleteDelegate = delegate(object cookie) { };
                RotCoroutiner.StartCoroutine(Fetch());
            }
        }
    }
    private void ResourcesLoad(RotAssetRequest assetRequest)
    {
        string path = assetRequest.assetName;
        Object o = null;
        if (!assetRequest.isPreload)
        {
            switch (assetRequest.assetType)
            {
                    case RotAssetType.GameObject:
                    o = Resources.Load<GameObject>(path);
                    break;
                    case RotAssetType.Texture:
                    o = Resources.Load<Texture>(path);
                    break;
            }
        }
        assetRequest.assetProgressDelegate(1);
        assetRequest.assetSuccessDelegate(o, null);
    }
    public void Load(List<RotAssetRequest> requests, AssetsCompleteDelegate assetsCompleteDelegate,
        AssetFailureDelegate failureDelegate = null)
    {
        if (isResoucesLoad)// || Service.Get<GameStaticData>().isTestScene)
        {
            assetsCompleteDelegate(null);
        }
        else
        {
            defaultAssetFailureDelegate = failureDelegate;
            defaultAssetsCompleteDelegate = assetsCompleteDelegate;
            for (int i = 0; i < requests.Count; i++)
            {
                queue.Enqueue(requests[i]);
            }
            if (!isInDownloadQueue)
                RotCoroutiner.StartCoroutine(Fetch());
        }
    }
    public string GetBundleUrl(string bundleName, out int version)
    {
        //AssetsVersionConfig assetsVersionConfig = Service.Get<ConfigManager>().GetAssetsVersionConfig();
        //version = assetsVersionConfig.GetVersion(bundleName);
        //string url = "";
        //if (isLoadLocal)
        //{
        //    url = "file://" + Application.streamingAssetsPath + "/" + bundleName;
        //}
        //else
        //{
        //    string ex = (Service.Get<GameStaticData>().GetStaticUrl().EndsWith("/")) ? "" : "/";
        //    url = Service.Get<GameStaticData>().GetStaticUrl() + ex + bundleName;
        //}
        //return url;
        version = 0;
        return "";
    }
    private IEnumerator Fetch()
    {
        if (queue.Count > 0)
        {
            RotAssetRequest assetRequest = queue.Peek();
            queue.Dequeue();
            int version = 0;
            string url = GetBundleUrl(assetRequest.bundleName, out version);
            object obj = null;
            AssetBundle bundle = null;
            isInDownloadQueue = true;
            string key = url + version;
            //GameLog.LogDownloadAssets("Load model " + assetRequest.bundleName);
            if (!dic.ContainsKey(key))
            {
                WWW ww = WWW.LoadFromCacheOrDownload(url, version);
                while (!ww.isDone)
                {
                    yield return null;
                    assetRequest.assetProgressDelegate.Invoke(ww.progress);
                }
                assetRequest.assetProgressDelegate.Invoke(1);
                if (string.IsNullOrEmpty(ww.error))
                {
                    bundle = ww.assetBundle;
                    dic.Add(key, bundle);

                }
                else
                {
                    //GameLog.LogError(url + "  " + ww.error + "  ");
                }
            }
            else
            {
                bundle = dic[key];
            }
            if (bundle != null)
            {
                obj = bundle.Load(assetRequest.assetName);
            }
            if (obj != null)
            {
                assetRequest.assetSuccessDelegate.Invoke(obj, null);
            }
            else
            {
                //GameLog.LogError("Not found asset: "+assetRequest.assetName);
                //if (defaultAssetFailureDelegate != null)
                //    assetRequest.assetFailureDelegate.Invoke(null);
                //defaultAssetFailureDelegate.Invoke(null);
            }
            RotCoroutiner.StartCoroutine(Fetch());
        }
        else
        {
            isInDownloadQueue = false;
            defaultAssetsCompleteDelegate.Invoke(null);
        }
    }
    public void ClearQueue()
    {
        queue.Clear();
    }
    public void UnLoadAllBundle()
    {
        foreach (KeyValuePair<string, AssetBundle> keyValuePair in dic)
        {
            keyValuePair.Value.Unload(false);
        }
        dic.Clear();
    }
}

public class RotAssetRequest
{
    public string assetName;
    public string bundleName;
    public AssetHandle assetHandle;
    public AssetSuccessDelegate assetSuccessDelegate;
    public AssetFailureDelegate assetFailureDelegate;
    public AssetProgressDelegate assetProgressDelegate;
    public bool isPreload = false;
    public RotAssetType assetType;
    public RotAssetRequest(string assetName, string bundleName, AssetSuccessDelegate assetSuccessDelegate,
        AssetFailureDelegate assetFailureDelegate,RotAssetType type= RotAssetType.GameObject)
    {
        this.assetName = assetName;
        this.bundleName = bundleName;
        this.assetSuccessDelegate = assetSuccessDelegate;
        this.assetFailureDelegate = assetFailureDelegate;
        assetProgressDelegate = new AssetProgressDelegate((x) =>
        {

        });
        this.assetType = type;
    }
}

public enum RotAssetType
{
    GameObject,Texture
}
