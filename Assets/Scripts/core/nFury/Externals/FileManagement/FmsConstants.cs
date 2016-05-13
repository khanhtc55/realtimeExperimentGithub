using System;
namespace nFury.Externals.FileManagement
{
	public class FmsConstants
	{
		public const string BASE_URL = "https://starts0-a.akamaihd.net/cloud-cms/";
		public const string NAP7_BASE_URL = "http://vs-n7-starts-cms-api.general.disney.private/";
		public const string STATIC_MANIFEST = "{0}manifest/{1}/{2}/{3}.json";
		public const string STATIC_MANIFEST_NAP7 = "{0}manifest/{1}/{2}?version={3}";
		public const string STATIC_MANIFEST_NAP7_WITHOUT_VERSION = "{0}manifest/{1}/{2}";
		public const string TOKEN_ROOT = "{root}";
		public const string TOKEN_CODENAME = "{codename}";
		public const string TOKEN_ENVIRONMENT = "{environment}";
		public const string TOKEN_RELATIVE_PATH = "{relativePath}";
		public const string TOKEN_HASH = "{hash}";
		public const string TOKEN_FILENAME = "{filename}";
		public const string TOKEN_VERSION = "{version}";
		public const string URL_FORMAT = "{root}{codename}/{environment}/{relativePath}/{hash}.{filename}";
	}
}
