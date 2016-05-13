

using nFury.Assets;
using nFury.Utils.Core;
using nFury.Utils.IO;

using System;
using System.Collections.Generic;
using nFury.Utils.Json;

namespace nFury.Utils.MetaData
{
  public class Catalog
  {
    private const string CONTENTS_KEY = "content";
    private const string OBJECTS_KEY = "objects";
    private Dictionary<string, Sheet> data;
    private Dictionary<string, AssetHandle> assetHandles;

    public Catalog()
    {
      this.data = new Dictionary<string, Sheet>();
      this.assetHandles = new Dictionary<string, AssetHandle>();
    }

    public Sheet GetSheet(string key)
    {
      return this.data[key];
    }

    public void PatchData(string catalogFile, Action completeCallback)
    {
      catalogFile = catalogFile.Replace(".json", ".json.zip");
      AssetManager assetManager = Service.Get<AssetManager>();
      assetManager.AddZipFileToManifest(catalogFile, string.Empty);
      object cookie = (object) new KeyValuePair<string, Action>(catalogFile, completeCallback);
      AssetHandle handle = AssetHandle.Invalid;
      assetManager.Load(ref handle, catalogFile, new AssetSuccessDelegate(this.AssetSuccess), new AssetFailureDelegate(this.AssetFailure), cookie);
      this.assetHandles.Add(catalogFile, handle);
    }

    private void AssetSuccess(object asset, object cookie)
    {
      this.ProcessJson(CompressionUtils.GetDecompressedString(asset as byte[]), cookie);
    }

    private void AssetFailure(object cookie)
    {
      this.ProcessJson((string) null, cookie);
    }

    public void ProcessJson(string json, object cookie)
    {
      KeyValuePair<string, Action> keyValuePair = (KeyValuePair<string, Action>) cookie;
      string key = keyValuePair.Key;
      Action action = keyValuePair.Value;
      Service.Get<AssetManager>().Unload(this.assetHandles[key]);
      this.assetHandles.Remove(key);
      this.ParseCatalog(json);
      if (action == null)
        return;
      action();
    }

    public void ParseCatalog(string json)
    {
      if (json == null)
        return;
      using (Dictionary<string, object>.Enumerator enumerator = (((new JsonParser(json, 0, true).Parse() as Dictionary<string, object>)["content"] as Dictionary<string, object>)["objects"] as Dictionary<string, object>).GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, object> current = enumerator.Current;
          List<object> list = current.Value as List<object>;
          Sheet sheet;
          if (this.data.ContainsKey(current.Key))
          {
            sheet = this.data[current.Key];
          }
          else
          {
            sheet = new Sheet();
            this.data.Add(current.Key, sheet);
          }
          int index = 0;
          for (int count = list.Count; index < count; ++index)
            sheet.PatchRow(new Row(list[index]));
        }
      }
    }
  }
}
