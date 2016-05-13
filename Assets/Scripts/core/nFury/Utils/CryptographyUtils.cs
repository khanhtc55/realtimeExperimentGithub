using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
namespace nFury.Utils
{
	public class CryptographyUtils
	{
        private static Dictionary<string,int> mapId;
		public static byte[] ComputeHmacHash(string algorithm, string secret, string plainText)
		{
			if (algorithm != null)
			{
				if (CryptographyUtils.mapId == null)
				{
					CryptographyUtils.mapId = new Dictionary<string, int>(1)
					{

						{
							"HmacSHA256",
							0
						}
					};
				}
				int num;
                if (CryptographyUtils.mapId.TryGetValue(algorithm, out num))
				{
					if (num == 0)
					{
						HMAC hMAC = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
						return hMAC.ComputeHash(Encoding.UTF8.GetBytes(plainText));
					}
				}
			}
			throw new ArgumentException(string.Format("Unknown algorithm {0}", algorithm));
		}
	}
}
