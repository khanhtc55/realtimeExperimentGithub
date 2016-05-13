
using System.Collections.Generic;
using nFury.Utils.Core;
using nFury.Utils.Diagnostics;
using UnityEngine;

namespace nFury.Utils.MetaData
{
  public class Row
  {
    private Dictionary<string, object> dictionary;

    public Row(object obj)
    {
      this.dictionary = obj as Dictionary<string, object>;
    }

    public void PatchData(Row newRow)
    {
      Dictionary<string, object> dictionary = newRow.dictionary;
      using (Dictionary<string, object>.KeyCollection.Enumerator enumerator = dictionary.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          if (this.dictionary.ContainsKey(current))
            this.dictionary[current] = dictionary[current];
          else
            this.dictionary.Add(current, dictionary[current]);
        }
      }
    }

    public string GetString(string key)
    {
      return this.dictionary[key] as string;
    }

    public string[] GetStringArray(string key)
    {
      List<object> list = this.dictionary[key] as List<object>;
      if (list == null)
        return (string[]) null;
      string[] strArray = new string[list.Count];
      int index = 0;
      for (int count = list.Count; index < count; ++index)
        strArray[index] = list[index] as string;
      return strArray;
    }

    public int[] GetIntArray(string key)
    {
      string @string = this.GetString(key);
      if (@string == null)
        return (int[]) null;
      string str = @string;
      char[] chArray = new char[1];
      int index1 = 0;
      int num = 44;
      chArray[index1] = (char) num;
      string[] strArray = str.Split(chArray);
      int length = strArray.Length;
      int[] numArray = new int[length];
      for (int index2 = 0; index2 < length; ++index2)
      {
        int result;
        numArray[index2] = !int.TryParse(strArray[index2], out result) ? 0 : result;
      }
      return numArray;
    }

    public float[] GetFloatArray(string key)
    {
      string @string = this.GetString(key);
      if (@string == null)
        return (float[]) null;
      string str = @string;
      char[] chArray = new char[1];
      int index1 = 0;
      int num = 44;
      chArray[index1] = (char) num;
      string[] strArray = str.Split(chArray);
      int length = strArray.Length;
      float[] numArray = new float[length];
      for (int index2 = 0; index2 < length; ++index2)
      {
        float result;
        numArray[index2] = !float.TryParse(strArray[index2], out result) ? 0.0f : result;
      }
      return numArray;
    }

    public Vector3 GetVector3(string key)
    {
      float[] floatArray = this.GetFloatArray(key);
      if (floatArray == null || floatArray.Length != 3)
        return Vector3.zero;
      else
        return new Vector3(floatArray[0], floatArray[1], floatArray[2]);
    }

    public bool GetBool(string key)
    {
      return bool.Parse(this.GetString(key));
    }

    public int GetInt(string key)
    {
      return int.Parse(this.GetString(key));
    }

    public uint GetUint(string key)
    {
      return uint.Parse(this.GetString(key));
    }

    public long GetLong(string key)
    {
      return long.Parse(this.GetString(key));
    }

    public double GetDouble(string key)
    {
      return double.Parse(this.GetString(key));
    }

    public float GetFloat(string key)
    {
      return float.Parse(this.GetString(key));
    }

    public string TryGetString(string key)
    {
      if (this.dictionary.ContainsKey(key))
        return this.GetString(key);
      else
        return (string) null;
    }

    public string[] TryGetStringArray(string key)
    {
      if (this.dictionary.ContainsKey(key))
        return this.GetStringArray(key);
      else
        return (string[]) null;
    }

    public int[] TryGetIntArray(string key)
    {
      if (this.dictionary.ContainsKey(key))
        return this.GetIntArray(key);
      else
        return (int[]) null;
    }

    public int TryGetInt(string key)
    {
      int num = 0;
      if (this.dictionary.ContainsKey(key))
      {
        string string1 = this.GetString(key);
        int result;
        if (int.TryParse(string1, out result))
          return result;
        string string2 = this.TryGetString("uid");
        Logger logger = Service.Get<Logger>();
        string message = "Failed to parse int {0}.{1}={2}";
        object[] objArray = new object[3];
        int index1 = 0;
        string str1 = string2;
        objArray[index1] = (object) str1;
        int index2 = 1;
        string str2 = key;
        objArray[index2] = (object) str2;
        int index3 = 2;
        string str3 = string1;
        objArray[index3] = (object) str3;
        logger.WarnFormat(message, objArray);
      }
      return num;
    }

    public uint TryGetUint(string key)
    {
      return this.TryGetUint(key, 0U);
    }

    public uint TryGetUint(string key, uint fallback)
    {
      if (this.dictionary.ContainsKey(key))
      {
        string string1 = this.GetString(key);
        uint result;
        if (uint.TryParse(string1, out result))
          return result;
        string string2 = this.TryGetString("uid");
        Logger logger = Service.Get<Logger>();
        string message = "Failed to parse uint {0}.{1}={2}";
        object[] objArray = new object[3];
        int index1 = 0;
        string str1 = string2;
        objArray[index1] = (object) str1;
        int index2 = 1;
        string str2 = key;
        objArray[index2] = (object) str2;
        int index3 = 2;
        string str3 = string1;
        objArray[index3] = (object) str3;
        logger.WarnFormat(message, objArray);
      }
      return fallback;
    }

    public long TryGetLong(string key)
    {
      long num = 0L;
      if (this.dictionary.ContainsKey(key))
      {
        string string1 = this.GetString(key);
        long result;
        if (long.TryParse(string1, out result))
          return result;
        string string2 = this.TryGetString("uid");
        Logger logger = Service.Get<Logger>();
        string message = "Failed to parse long {0}.{1}={2}";
        object[] objArray = new object[3];
        int index1 = 0;
        string str1 = string2;
        objArray[index1] = (object) str1;
        int index2 = 1;
        string str2 = key;
        objArray[index2] = (object) str2;
        int index3 = 2;
        string str3 = string1;
        objArray[index3] = (object) str3;
        logger.WarnFormat(message, objArray);
      }
      return num;
    }

    public double TryGetDouble(string key)
    {
      double num = 0.0;
      if (this.dictionary.ContainsKey(key))
      {
        string string1 = this.GetString(key);
        double result;
        if (double.TryParse(string1, out result))
          return result;
        string string2 = this.TryGetString("uid");
        Logger logger = Service.Get<Logger>();
        string message = "Failed to parse double {0}.{1}={2}";
        object[] objArray = new object[3];
        int index1 = 0;
        string str1 = string2;
        objArray[index1] = (object) str1;
        int index2 = 1;
        string str2 = key;
        objArray[index2] = (object) str2;
        int index3 = 2;
        string str3 = string1;
        objArray[index3] = (object) str3;
        logger.WarnFormat(message, objArray);
      }
      return num;
    }

    public Vector3 TryGetVector3(string key)
    {
      if (this.dictionary.ContainsKey(key))
        return this.GetVector3(key);
      else
        return Vector3.zero;
    }

    public float[] TryGetFloatArray(string key)
    {
      if (this.dictionary.ContainsKey(key))
        return this.GetFloatArray(key);
      else
        return (float[]) null;
    }

    public float TryGetFloat(string key)
    {
      return this.TryGetFloat(key, 0.0f);
    }

    public float TryGetFloat(string key, float fallback)
    {
      if (this.dictionary.ContainsKey(key))
      {
        string string1 = this.GetString(key);
        float result;
        if (float.TryParse(string1, out result))
          return result;
        string string2 = this.TryGetString("uid");
        Logger logger = Service.Get<Logger>();
        string message = "Failed to parse float {0}.{1}={2}";
        object[] objArray = new object[3];
        int index1 = 0;
        string str1 = string2;
        objArray[index1] = (object) str1;
        int index2 = 1;
        string str2 = key;
        objArray[index2] = (object) str2;
        int index3 = 2;
        string str3 = string1;
        objArray[index3] = (object) str3;
        logger.WarnFormat(message, objArray);
      }
      return fallback;
    }

    public bool TryGetBool(string key)
    {
      bool flag = false;
      if (this.dictionary.ContainsKey(key))
      {
        string string1 = this.GetString(key);
        bool result;
        if (bool.TryParse(string1, out result))
          return result;
        string string2 = this.TryGetString("uid");
        Logger logger = Service.Get<Logger>();
        string message = "Failed to parse bool {0}.{1}={2}";
        object[] objArray = new object[3];
        int index1 = 0;
        string str1 = string2;
        objArray[index1] = (object) str1;
        int index2 = 1;
        string str2 = key;
        objArray[index2] = (object) str2;
        int index3 = 2;
        string str3 = string1;
        objArray[index3] = (object) str3;
        logger.WarnFormat(message, objArray);
      }
      return flag;
    }
  }
}
