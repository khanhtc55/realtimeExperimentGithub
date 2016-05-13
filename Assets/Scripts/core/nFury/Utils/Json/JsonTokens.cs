﻿
namespace nFury.Utils.Json
{
  public enum JsonTokens
  {
    None = 0,
    ObjectOpen = 1,
    ObjectClose = 2,
    ArrayOpen = 3,
    ArrayClose = 4,
    Colon = 5,
    Comma = 6,
    String = 7,
    Number = 8,
    True = 100,
    WordFirst = 100,
    False = 101,
    Null = 102,
  }
}
