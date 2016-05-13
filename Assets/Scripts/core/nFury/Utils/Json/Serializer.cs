
using System.Collections.Generic;
using System.Text;

namespace nFury.Utils.Json
{
  public class Serializer
  {
    private const string KEY_VALUE_QUOTED = "\"{0}\":\"{1}\"";
    private const string KEY_VALUE_UNQUOTED = "\"{0}\":{1}";
    private const string KEY_ONLY = "\"{0}\":";
    private const string OBJECT_START = "{";
    private const string OBJECT_END = "}";
    private const string ARRAY_START = "[";
    private const string ARRAY_END = "]";
    private const string COMMA = ",";
    private StringBuilder sb;
    private bool first;

    public Serializer()
    {
      this.sb = new StringBuilder();
      this.sb.Append("{");
      this.first = true;
    }

    public static Serializer Start()
    {
      return new Serializer();
    }

    public Serializer End()
    {
      this.sb.Append("}");
      return this;
    }

    public override string ToString()
    {
      return this.sb.ToString();
    }

    public Serializer AddString(string key, string val)
    {
      return this.AddInternal<string>(key, val, "\"{0}\":\"{1}\"");
    }

    public Serializer AddBool(string key, bool val)
    {
      return this.Add<string>(key, val.ToString().ToLower());
    }

    public Serializer Add<T>(string key, T val)
    {
      return this.AddInternal<T>(key, val, "\"{0}\":{1}");
    }

    private Serializer AddInternal<T>(string key, T val, string format)
    {
      this.AppendComma(this.first);
      this.sb.AppendFormat(format, (object) key, (object) val);
      this.first = false;
      return this;
    }

    public Serializer AddObject<T>(string key, T val) where T : ISerializable
    {
      return this.Add<string>(key, val.ToJson());
    }

    public Serializer AddArray<T>(string key, List<T> values) where T : ISerializable
    {
      this.AppendComma(this.first);
      this.sb.AppendFormat("\"{0}\":", (object) key);
      this.sb.Append("[");
      bool first = true;
      using (List<T>.Enumerator enumerator = values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          T current = enumerator.Current;
          this.AppendComma(first);
          this.Add((ISerializable) current);
          first = false;
        }
      }
      this.sb.Append("]");
      this.first = false;
      return this;
    }

    public Serializer AddDictionary<T>(string key, Dictionary<string, T> values)
    {
      this.AppendComma(this.first);
      this.sb.AppendFormat("\"{0}\":", (object) key);
      this.sb.Append("{");
      bool flag = typeof (T) == typeof (string);
      this.first = true;
      using (Dictionary<string, T>.Enumerator enumerator = values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, T> current = enumerator.Current;
          if (flag)
            this.AddString(current.Key, current.Value.ToString());
          else
            this.Add<T>(current.Key, current.Value);
        }
      }
      this.first = false;
      this.sb.Append("}");
      return this;
    }

    public Serializer AddArrayOfPrimitives<T>(string key, List<T> values)
    {
      this.AppendComma(this.first);
      this.sb.AppendFormat("\"{0}\":", (object) key);
      this.sb.Append("[");
      bool first = true;
      bool flag = typeof (T) == typeof (string);
      using (List<T>.Enumerator enumerator = values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          T current = enumerator.Current;
          this.AppendComma(first);
          if (flag)
            this.AddQuoted(current.ToString());
          else
            this.Add(current.ToString());
          first = false;
        }
      }
      this.sb.Append("]");
      this.first = false;
      return this;
    }

    private void Add(ISerializable val)
    {
      this.Add(val.ToJson());
    }

    private void Add(string val)
    {
      this.sb.Append(val);
    }

    private void AddQuoted(string val)
    {
      this.sb.Append('"');
      this.sb.Append(val);
      this.sb.Append('"');
    }

    private void AppendComma(bool first)
    {
      if (first)
        return;
      this.sb.Append(",");
    }
  }
}
