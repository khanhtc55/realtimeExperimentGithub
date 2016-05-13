using nFury.Assets;

using System;
using System.Collections.Generic;
using System.Globalization;
using nFury.Utils.Core;
using nFury.Utils.Diagnostics;
using nFury.Utils.Json;
using UnityEngine;
namespace nFury.Utils
{
	public class Lang
	{
		public const string DEFAULT_LANGUAGE = "en";
		public const string DEFAULT_LOCALE = "en_US";
		public const string AUTO_LOCALIZE_ID_PREFIX = "s_";
		private const string KEY_CONTENT = "content";
		private const string KEY_OBJECTS = "objects";
		private const string KEY_LOCALIZEDSTRINGS = "LocalizedStrings";
		private const string KEY_ID = "uid";
		private const string KEY_TEXT = "text";
		private const string LANG_NOT_FOUND = "LANG: {0} missing";
		private const string LANG_BAD_FORMAT = "LANG: {0} bad format";
		public const string JAPANESE_FONT = "IwaNGNewsPro-Md.android";
		public const string JAPAN_LOCALE = "ja_JP";
		private string locale;
		private Dictionary<string, string> strings;
		private Dictionary<string, string> localeToDisplayLanguage;
		private AssetHandle assetHandle;
		private Font japanFontInternal;
		private static Lang staticLang;
		private static StringComparer staticComp;
		public bool Initialized
		{
			get;
			set;
		}
		public Font CustomJapaneseFont
		{
			get
			{
				if (this.japanFontInternal == null)
				{
					this.japanFontInternal = (Font)Resources.Load("IwaNGNewsPro-Md.android");
				}
				return this.japanFontInternal;
			}
		}
		public string Locale
		{
			get
			{
				return this.locale;
			}
			set
			{
				this.locale = value;
				Service.Get<Logger>().Debug("Locale: " + this.locale);
				this.ClearStringData();
			}
		}
		public string DotNetLocale
		{
			get
			{
				return Lang.ToDotNetLocale(this.locale);
			}
		}
		public Lang(string locale)
		{
			Service.Set<Lang>(this);
			this.Initialized = false;
			this.strings = new Dictionary<string, string>();
			this.localeToDisplayLanguage = new Dictionary<string, string>();
			this.Locale = locale;
		}
		public string ExtractLanguageFromLocale()
		{
			int num = this.locale.IndexOf('_');
			return (num < 0) ? this.locale : this.locale.Substring(0, num);
		}
		public static string ToDotNetLocale(string locale)
		{
			return locale.Replace("_", "-");
		}
		public bool IsJapanese()
		{
			return this.locale == "ja_JP";
		}
		public void LoadAsset(string assetName, AssetSuccessDelegate onSuccess, AssetFailureDelegate onFailure, object cookie)
		{
			Service.Get<AssetManager>().Load(ref this.assetHandle, assetName, onSuccess, onFailure, cookie);
		}
		public void UnloadAsset()
		{
			Service.Get<AssetManager>().Unload(this.assetHandle);
			this.assetHandle = AssetHandle.Invalid;
		}
		public void AddStringData(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				return;
			}
			JsonParser jsonParser = new JsonParser(json);
			object obj = jsonParser.Parse();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null || !dictionary.ContainsKey("content"))
			{
				Service.Get<Logger>().Warn("Invalid lang json: no content");
				return;
			}
			dictionary = (dictionary["content"] as Dictionary<string, object>);
			if (dictionary == null || !dictionary.ContainsKey("objects"))
			{
				Service.Get<Logger>().Warn("Invalid lang json: no objects");
				return;
			}
			dictionary = (dictionary["objects"] as Dictionary<string, object>);
			if (dictionary == null || !dictionary.ContainsKey("LocalizedStrings"))
			{
				Service.Get<Logger>().Warn("Invalid lang json: no strings");
				return;
			}
			List<object> list = dictionary["LocalizedStrings"] as List<object>;
			if (list == null)
			{
				Service.Get<Logger>().Warn("Invalid lang json: no entries");
				return;
			}
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
				if (dictionary2 != null && dictionary2.ContainsKey("uid") && dictionary2.ContainsKey("text"))
				{
					string text = dictionary2["uid"] as string;
					string text2 = dictionary2["text"] as string;
					if (this.strings.ContainsKey(text))
					{
						if (this.strings[text] != text2)
						{
							Service.Get<Logger>().Warn("Duplicate id in strings: " + text);
						}
					}
					else
					{
						this.strings.Add(text, text2);
					}
				}
				i++;
			}
		}
		private void ClearStringData()
		{
			this.strings.Clear();
			this.localeToDisplayLanguage.Clear();
			this.Initialized = false;
		}
		public void SetupAvailableLocales(string locales, string displayLanguages)
		{
			this.localeToDisplayLanguage.Clear();
			if (locales == null || displayLanguages == null)
			{
				Service.Get<Logger>().Warn("Invalid locale-language mapping");
				if (locales == null)
				{
					return;
				}
				if (displayLanguages == null)
				{
					displayLanguages = locales;
				}
			}
			string[] array = locales.Split(new char[]
			{
				'|'
			});
			string[] array2 = displayLanguages.Split(new char[]
			{
				'|'
			});
			int num = array.Length;
			if (num != array2.Length)
			{
				Service.Get<Logger>().Warn("Mismatched locale-language mapping");
				array2 = array;
			}
			for (int i = 0; i < num; i++)
			{
				string key = array[i];
				if (this.localeToDisplayLanguage.ContainsKey(key))
				{
					Service.Get<Logger>().Warn("Duplicate locale in locale-language mapping");
				}
				else
				{
					this.localeToDisplayLanguage.Add(key, array2[i]);
				}
			}
		}
		public string GetDisplayLanguage(string locale)
		{
			return (this.localeToDisplayLanguage == null || !this.localeToDisplayLanguage.ContainsKey(locale)) ? null : this.localeToDisplayLanguage[locale];
		}
		public List<string> GetAvailableLocales()
		{
			List<string> list = new List<string>();
			foreach (string current in this.localeToDisplayLanguage.Keys)
			{
				list.Add(current);
			}
			CultureInfo cultureInfo = this.GetCultureInfo();
			if (cultureInfo == null)
			{
				list.Sort();
			}
			else
			{
				Lang.staticLang = this;
				Lang.staticComp = StringComparer.Create(cultureInfo, true);
				try
				{
					list.Sort(new Comparison<string>(Lang.CompareLocaleDisplayLanguage));
				}
				finally
				{
					Lang.staticLang = null;
					Lang.staticComp = null;
				}
			}
			return list;
		}
		private static int CompareLocaleDisplayLanguage(string a, string b)
		{
			return Lang.staticComp.Compare(Lang.staticLang.GetDisplayLanguage(a), Lang.staticLang.GetDisplayLanguage(b));
		}
		private CultureInfo GetCultureInfo()
		{
			CultureInfo cultureInfo = null;
			try
			{
				cultureInfo = CultureInfo.GetCultureInfo(this.DotNetLocale);
			}
			catch
			{
				cultureInfo = null;
			}
			if (cultureInfo == null)
			{
				try
				{
					cultureInfo = CultureInfo.GetCultureInfo("en_US");
				}
				catch
				{
					cultureInfo = null;
				}
			}
			return cultureInfo;
		}
		public string ThousandsSeparated(int value)
		{
			CultureInfo cultureInfo = this.GetCultureInfo();
			return (cultureInfo != null) ? string.Format(cultureInfo, "{0:n0}", new object[]
			{
				value
			}) : string.Format("{0:n0}", value);
		}
		public string GetOptional(string id)
		{
			return (!this.strings.ContainsKey(id)) ? id : this.Get(id, new object[0]);
		}
		public string Get(string id, params object[] args)
		{
			string text;
			if (!this.strings.ContainsKey(id))
			{
				text = ((!id.StartsWith("s_")) ? string.Format("LANG: {0} missing", id) : id);
			}
			else
			{
				text = this.strings[id];
				if (args.Length != 0)
				{
					try
					{
						text = string.Format(text, args);
					}
					catch
					{
						text = string.Format("LANG: {0} bad format", id);
					}
				}
			}
			return text;
		}
	}
}
