using nFury.Utils.Core;
using nFury.Utils.Diagnostics;

using System;
using System.Text;
namespace nFury.Utils
{
	public static class StringUtils
	{
		private const string NUMERIC_CHARACTERS = "0123456789";
		public static T ParseEnum<T>(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return default(T);
			}
			string value = StringUtils.ToPascalCase(name);
			T result;
			try
			{
				result = (T)((object)Enum.Parse(typeof(T), value));
			}
			catch
			{
				Service.Get<Logger>().Error(string.Format("Enum value '{0}' not found in {1}", name, typeof(T)));
				result = default(T);
			}
			return result;
		}
		public static string ToLowerCaseUnderscoreSeperated(string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			int length = s.Length;
			while (i < length)
			{
				char c = s[i];
				bool flag = char.IsUpper(c);
				bool flag2 = false;
				if (i < s.Length - 1)
				{
					flag2 = char.IsUpper(s[i + 1]);
				}
				if (flag2 && !flag)
				{
					stringBuilder.Append(char.ToLower(c));
					stringBuilder.Append('_');
				}
				else
				{
					if ((flag && flag2) || (flag && i == s.Length - 1))
					{
						stringBuilder.Append(c);
					}
					else
					{
						stringBuilder.Append(char.ToLower(c));
					}
				}
				i++;
			}
			return stringBuilder.ToString();
		}
		public static string ToPascalCase(string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			int i = 0;
			int length = s.Length;
			while (i < length)
			{
				char c = s[i];
				bool flag2 = char.IsLetter(c);
				if (flag2)
				{
					if (!flag)
					{
						stringBuilder.Append(char.ToUpper(c));
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				else
				{
					if (char.IsNumber(c))
					{
						stringBuilder.Append(c);
					}
				}
				flag = flag2;
				i++;
			}
			return stringBuilder.ToString();
		}
		public static string GetRomanNumeral(int number)
		{
			if (number <= 0)
			{
				return string.Empty;
			}
			if (number > 39)
			{
				return number.ToString();
			}
			string text = string.Empty;
			switch (number % 10)
			{
			case 1:
				text = "I";
				break;
			case 2:
				text = "II";
				break;
			case 3:
				text = "III";
				break;
			case 4:
				text = "IV";
				break;
			case 5:
				text = "V";
				break;
			case 6:
				text = "VI";
				break;
			case 7:
				text = "VII";
				break;
			case 8:
				text = "VIII";
				break;
			case 9:
				text = "IX";
				break;
			}
			if (number >= 10)
			{
				string str = new string('X', number / 10);
				text = str + text;
			}
			return text;
		}
		public static int GetIndexOfFirstNumericCharacter(string s)
		{
            return s.IndexOfAny(NUMERIC_CHARACTERS.ToCharArray());
		}
		public static string Substring(string s, int startIndex, int endIndex)
		{
			return s.Substring(startIndex, endIndex - startIndex);
		}
		public static string Substring(string s, int startIndex)
		{
			return s.Substring(startIndex);
		}
		public static bool IsBlank(string s)
		{
			return string.IsNullOrEmpty(s) || string.IsNullOrEmpty(s.Trim());
		}
		public static bool IsNotBlank(string s)
		{
			return !StringUtils.IsBlank(s);
		}
		public static bool IsEmpty(string s)
		{
			return string.IsNullOrEmpty(s);
		}
		public static bool IsNotEmpty(string s)
		{
			return !StringUtils.IsEmpty(s);
		}
		public static bool IsNull(string s)
		{
			return s == null;
		}
		public static bool IsNotNull(string s)
		{
			return !StringUtils.IsNull(s);
		}
		public static string GenerateRandom(uint length)
		{
			string text = "abcdefghijklmnopqrstuvwxyz0123456789";
			StringBuilder stringBuilder = new StringBuilder();
			Rand rand = Service.Get<Rand>();
			int num = 0;
			while ((long)num < (long)((ulong)length))
			{
				int index = rand.ViewRangeInt(0, text.Length);
				stringBuilder.Append(text[index]);
				num++;
			}
			return stringBuilder.ToString();
		}
	}
}
