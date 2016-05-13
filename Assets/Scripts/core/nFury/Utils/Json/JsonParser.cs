
using System.Collections.Generic;
using System.Text;

namespace nFury.Utils.Json
{
  public class JsonParser
  {
    private static readonly JsonTokens[] jsonTokenMap = new JsonTokens[256];
    private readonly StringBuilder sb = new StringBuilder();
    private int jsonLength = -1;
    private int nextBackslash = -1;
    private readonly string json;
    private int index;
    private JsonTokens lookAheadToken;
    private bool internStrings;
    private readonly List<Dictionary<string, object>> pool;

    static JsonParser()
    {
      for (int index = 0; index < 256; ++index)
        JsonParser.jsonTokenMap[index] = JsonTokens.None;
      JsonParser.jsonTokenMap[123] = JsonTokens.ObjectOpen;
      JsonParser.jsonTokenMap[125] = JsonTokens.ObjectClose;
      JsonParser.jsonTokenMap[91] = JsonTokens.ArrayOpen;
      JsonParser.jsonTokenMap[93] = JsonTokens.ArrayClose;
      JsonParser.jsonTokenMap[44] = JsonTokens.Comma;
      JsonParser.jsonTokenMap[34] = JsonTokens.String;
      JsonParser.jsonTokenMap[48] = JsonTokens.Number;
      JsonParser.jsonTokenMap[49] = JsonTokens.Number;
      JsonParser.jsonTokenMap[50] = JsonTokens.Number;
      JsonParser.jsonTokenMap[51] = JsonTokens.Number;
      JsonParser.jsonTokenMap[52] = JsonTokens.Number;
      JsonParser.jsonTokenMap[53] = JsonTokens.Number;
      JsonParser.jsonTokenMap[54] = JsonTokens.Number;
      JsonParser.jsonTokenMap[55] = JsonTokens.Number;
      JsonParser.jsonTokenMap[56] = JsonTokens.Number;
      JsonParser.jsonTokenMap[57] = JsonTokens.Number;
      JsonParser.jsonTokenMap[45] = JsonTokens.Number;
      JsonParser.jsonTokenMap[43] = JsonTokens.Number;
      JsonParser.jsonTokenMap[46] = JsonTokens.Number;
      JsonParser.jsonTokenMap[58] = JsonTokens.Colon;
      JsonParser.jsonTokenMap[102] = JsonTokens.False;
      JsonParser.jsonTokenMap[116] = JsonTokens.WordFirst;
      JsonParser.jsonTokenMap[110] = JsonTokens.Null;
    }

    public JsonParser(string json)
      : this(json, 0, false)
    {
    }

    public JsonParser(string json, int startFrom)
      : this(json, startFrom, false)
    {
    }

    public JsonParser(string json, int startFrom, bool internStrings)
    {
      this.json = json;
      this.jsonLength = json.Length;
      this.index = startFrom;
      this.internStrings = internStrings;
      this.pool = new List<Dictionary<string, object>>();
      this.nextBackslash = 0;
      this.FindNextBackslash();
    }

    public object Parse()
    {
      if (this.jsonLength == 0)
        return (object) null;
      else
        return this.ParseValue();
    }

    private Dictionary<string, object> ParseObject()
    {
      Dictionary<string, object> dictionary;
      if (this.pool.Count == 0)
      {
        dictionary = new Dictionary<string, object>();
      }
      else
      {
        int index = this.pool.Count - 1;
        dictionary = this.pool[index];
        this.pool.RemoveAt(index);
      }
      this.lookAheadToken = JsonTokens.None;
      while (true)
      {
        switch (this.LookAhead())
        {
          case JsonTokens.Comma:
            this.lookAheadToken = JsonTokens.None;
            continue;
          case JsonTokens.ObjectClose:
           break;
          default:
            string index1 = this.ParseString();
            JsonTokens jsonTokens = this.lookAheadToken == JsonTokens.None ? this.NextTokenCore() : this.lookAheadToken;
            this.lookAheadToken = JsonTokens.None;
            if (jsonTokens == JsonTokens.Colon)
            {
              object obj = this.ParseValue();
              dictionary[index1] = obj;
              continue;
            }
            else
                dictionary.Clear();
                break;
        }
      }

      this.lookAheadToken = JsonTokens.None;

      if (dictionary.Count == 0)
      {
        this.pool.Add(dictionary);
        dictionary = (Dictionary<string, object>) null;
      }
      return dictionary;
    }

    private List<object> ParseArray()
    {
      List<object> list = new List<object>();
      this.lookAheadToken = JsonTokens.None;
      while (true)
      {
        switch (this.LookAhead())
        {
          case JsonTokens.Comma:
            this.lookAheadToken = JsonTokens.None;
            continue;
          case JsonTokens.ArrayClose:
           break;
          default:
            list.Add(this.ParseValue());
            continue;
        }
      }

      this.lookAheadToken = JsonTokens.None;
      list.Capacity = list.Count;
      return list;
    }

    private object ParseValue()
    {
      JsonTokens jsonTokens = this.LookAhead();
      switch (jsonTokens)
      {
        case JsonTokens.ObjectOpen:
          return (object) this.ParseObject();
        case JsonTokens.ArrayOpen:
          return (object) this.ParseArray();
        case JsonTokens.String:
          return (object) this.ParseString();
        case JsonTokens.Number:
          return (object) this.ParseNumber();
        default:
          switch (jsonTokens - 100)
          {
            case JsonTokens.None:
              this.lookAheadToken = JsonTokens.None;
              return (object) true;
            case JsonTokens.ObjectOpen:
              this.lookAheadToken = JsonTokens.None;
              return (object) false;
            case JsonTokens.ObjectClose:
              this.lookAheadToken = JsonTokens.None;
              return (object) null;
            default:
              return (object) null;
          }
      }
    }

    private void FindNextBackslash()
    {
      if (this.nextBackslash < 0)
        return;
      this.nextBackslash = this.json.IndexOf('\\', this.index);
    }

    private string NewString(string s)
    {
      if (this.internStrings)
        return string.Intern(s);
      else
        return s;
    }

    private string ParseString()
    {
      this.lookAheadToken = JsonTokens.None;
      int num = this.json.IndexOf('"', this.index);
      if (num < 0)
        return (string) null;
      if (this.nextBackslash < 0 || this.nextBackslash > num)
      {
        string s = this.json.Substring(this.index, num - this.index);
        this.index = num + 1;
        return this.NewString(s);
      }
      else
      {
        this.sb.Length = 0;
        int startIndex = -1;
        while (this.index < this.jsonLength)
        {
          switch (this.json[this.index++])
          {
            case '"':
              if (startIndex != -1)
              {
                if (this.sb.Length == 0)
                {
                  this.FindNextBackslash();
                  return this.NewString(this.json.Substring(startIndex, this.index - startIndex - 1));
                }
                else
                  this.sb.Append(this.json.Substring(startIndex, this.index - startIndex - 1));
              }
              this.FindNextBackslash();
              return this.NewString(this.sb.ToString());
            case '\\':
              if (this.index != this.jsonLength)
              {
                if (startIndex != -1)
                {
                  this.sb.Append(this.json.Substring(startIndex, this.index - startIndex - 1));
                  startIndex = -1;
                }
                char ch = this.json[this.index++];
                switch (ch)
                {
                  case 'n':
                    this.sb.Append('\n');
                    continue;
                  case 'r':
                    this.sb.Append('\r');
                    continue;
                  case 't':
                    this.sb.Append('\t');
                    continue;
                  case 'u':
                    if (this.jsonLength - this.index >= 4)
                    {
                      this.sb.Append((char) this.ParseUnicode(this.json[this.index], this.json[this.index + 1], this.json[this.index + 2], this.json[this.index + 3]));
                      this.index += 4;
                      continue;
                    }
                    else
                      continue;
                  default:
                    if ((int) ch != 34)
                    {
                      if ((int) ch != 47)
                      {
                        if ((int) ch != 92)
                        {
                          if ((int) ch != 98)
                          {
                            if ((int) ch == 102)
                            {
                              this.sb.Append('\f');
                              continue;
                            }
                            else
                              continue;
                          }
                          else
                          {
                            this.sb.Append('\b');
                            continue;
                          }
                        }
                        else
                        {
                          this.sb.Append('\\');
                          continue;
                        }
                      }
                      else
                      {
                        this.sb.Append('/');
                        continue;
                      }
                    }
                    else
                    {
                      this.sb.Append('"');
                      continue;
                    }
                }
              }
              else
                break;
            default:
              if (startIndex == -1)
              {
                startIndex = this.index - 1;
                continue;
              }
              else
                continue;
          }
        }

        this.FindNextBackslash();
        return (string) null;
      }
    }

    private uint ParseSingleChar(char c1, uint multipliyer)
    {
      uint num = 0U;
      if ((int) c1 >= 48 && (int) c1 <= 57)
        num = ((uint) c1 - 48U) * multipliyer;
      else if ((int) c1 >= 65 && (int) c1 <= 70)
        num = (uint) ((int) c1 - 65 + 10) * multipliyer;
      else if ((int) c1 >= 97 && (int) c1 <= 102)
        num = (uint) ((int) c1 - 97 + 10) * multipliyer;
      return num;
    }

    private uint ParseUnicode(char c1, char c2, char c3, char c4)
    {
      return this.ParseSingleChar(c1, 4096U) + this.ParseSingleChar(c2, 256U) + this.ParseSingleChar(c3, 16U) + this.ParseSingleChar(c4, 1U);
    }

    private string ParseNumber()
    {
      this.lookAheadToken = JsonTokens.None;
      int startIndex = this.index - 1;
      for (char ch = this.json[this.index]; (int) ch >= 48 && (int) ch <= 57 || ((int) ch == 46 || (int) ch == 45) || ((int) ch == 43 || (int) ch == 101 || (int) ch == 69); ch = this.json[this.index])
      {
        if (++this.index == this.jsonLength)
          return (string) null;
      }
      return this.NewString(this.json.Substring(startIndex, this.index - startIndex));
    }

    private JsonTokens LookAhead()
    {
      if (this.lookAheadToken != JsonTokens.None)
        return this.lookAheadToken;
      else
        return this.lookAheadToken = this.NextTokenCore();
    }

    private JsonTokens NextTokenCore()
    {
      char ch1;
      for (ch1 = this.json[this.index]; (int) ch1 <= 32; ch1 = this.json[this.index])
      {
        if (++this.index == this.jsonLength)
          return JsonTokens.None;
      }
      ++this.index;
      if ((int) ch1 >= 256)
        return JsonTokens.None;
      JsonTokens jsonTokens = JsonParser.jsonTokenMap[(int) ch1];
      if (jsonTokens >= JsonTokens.WordFirst)
      {
        char ch2;
        do
        {
          ch2 = this.json[this.index];
        }
        while ((int) ch2 >= 97 && (int) ch2 <= 122 && ++this.index < this.jsonLength);
      }
      return jsonTokens;
    }
  }
}
