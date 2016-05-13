
using System;

namespace nFury.Utils.Json
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
  public class JsonIgnoreAttribute : Attribute
  {
  }
}
