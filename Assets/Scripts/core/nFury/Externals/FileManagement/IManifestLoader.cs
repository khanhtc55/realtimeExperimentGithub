using System;
namespace nFury.Externals.FileManagement
{
	public interface IManifestLoader
	{
		void Load(FmsOptions options, string manifestUrl, FmsCallback onComplete, FmsCallback onError);
		bool IsLoaded();
		IFileManifest GetManifest();
	}
}
