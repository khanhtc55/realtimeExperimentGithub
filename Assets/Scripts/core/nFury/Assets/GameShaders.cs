using nFury.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace nFury.Assets
{
	public class GameShaders
	{
		public const string SIMPLE_SOLID_COLOR = "SimpleSolidColor";
		public const string UNLIT_TEXTURE_FADE = "UnlitTexture_Fade";
		public const string WIPE_LINEAR = "Wipe_Linear";
		public const string WIPE_ELLIPTICAL = "Wipe_Elliptical";
		public const string OUTLINE_UNLIT = "Outline_Unlit";
		public const string SCROLL_HORIZONTAL = "Scroll_Horizontal";
		public const string TRANSPORT_SHADOW = "TransportShadow";
		public const string SIMPLE_SOLID_COLOR_ATTRIBUTE = "_Pigment";
		private Dictionary<string, Shader> shaders;
		public GameShaders(AssetsCompleteDelegate onCompleteCallback, object onCompleteCookie)
		{
			this.shaders = new Dictionary<string, Shader>();
			List<string> list = new List<string>();
            list.Add(SIMPLE_SOLID_COLOR);
            list.Add(UNLIT_TEXTURE_FADE);
            list.Add(WIPE_LINEAR);
            list.Add(WIPE_ELLIPTICAL);
            list.Add(OUTLINE_UNLIT);
            list.Add(SCROLL_HORIZONTAL);
            list.Add(TRANSPORT_SHADOW);
			List<object> list2 = new List<object>();
			List<AssetHandle> list3 = new List<AssetHandle>();
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				list2.Add(list[i]);
				list3.Add(AssetHandle.Invalid);
				i++;
			}
			Service.Get<AssetManager>().MultiLoad(list3, list, new AssetSuccessDelegate(this.LoadSuccess), null, list2, onCompleteCallback, onCompleteCookie);
		}
		private void LoadSuccess(object asset, object cookie)
		{
			Shader value = asset as Shader;
			string key = cookie as string;
			this.shaders.Add(key, value);
		}
		public Shader GetShader(string shaderName)
		{
			return (string.IsNullOrEmpty(shaderName) || !this.shaders.ContainsKey(shaderName)) ? null : this.shaders[shaderName];
		}
	}
}
