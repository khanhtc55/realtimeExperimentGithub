
namespace nFury.Utils.Json
{
  public interface ISerializable
  {
    string ToJson();

    ISerializable FromObject(object obj);
  }
}
