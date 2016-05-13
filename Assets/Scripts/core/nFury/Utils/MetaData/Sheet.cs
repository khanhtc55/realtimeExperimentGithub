
using System.Collections.Generic;

namespace nFury.Utils.MetaData
{
  public class Sheet
  {
    private const string UID_KEY = "uid";
    private Dictionary<string, Row> dictionary;

    public Sheet()
    {
      this.dictionary = new Dictionary<string, Row>();
    }

    public void PatchRow(Row row)
    {
      string @string = row.GetString("uid");
      if (this.dictionary.ContainsKey(@string))
        this.dictionary[@string].PatchData(row);
      else
        this.dictionary.Add(@string, row);
    }

    public Dictionary<string, Row> GetAllRows()
    {
      return this.dictionary;
    }
  }
}
