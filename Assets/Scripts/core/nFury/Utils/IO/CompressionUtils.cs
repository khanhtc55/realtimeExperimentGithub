
using ICSharpCode.SharpZipLib.Zip;
using nFury.Utils.Core;
using nFury.Utils.Diagnostics;

using System.IO;
using System.Text;

namespace nFury.Utils.IO
{
  public static class CompressionUtils
  {
    public static string GetDecompressedString(byte[] compressedData)
    {
      Stream stream = (Stream) new MemoryStream(compressedData);
      ZipInputStream zipInputStream = new ZipInputStream(stream);
      ZipEntry nextEntry = zipInputStream.GetNextEntry();
      byte[] numArray = new byte[nextEntry.Size];
      if ((long) zipInputStream.Read(numArray, 0, numArray.Length) != nextEntry.Size)
        Service.Get<Logger>().ErrorFormat("Read mismatch on decompressed zip: {0}", new object[0]);
      zipInputStream.Close();
      stream.Close();
      ((Stream) zipInputStream).Dispose();
      stream.Dispose();
      return Encoding.UTF8.GetString(numArray);
    }
  }
}
