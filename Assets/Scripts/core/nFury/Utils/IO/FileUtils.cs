
using System;
using System.IO;

namespace nFury.Utils.IO
{
  public static class FileUtils
  {
    public static string Read(string absFilePath)
    {
      try
      {
        return File.ReadAllText(absFilePath);
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to read file: " + absFilePath);
      }
    }

    public static void Write(string absFilePath, string text)
    {
      StreamWriter streamWriter = (StreamWriter) null;
      try
      {
        streamWriter = File.CreateText(absFilePath);
        streamWriter.WriteLine(text);
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to write file: " + absFilePath);
      }
      finally
      {
        if (streamWriter != null)
        {
          streamWriter.Close();
          streamWriter.Dispose();
        }
      }
    }

    public static string GetAbsFilePathInMyDocuments(string fileName, string dir)
    {
      return FileUtils.GetAbsDirPathInMyDocuments(dir) + "/" + fileName;
    }

    public static string GetAbsDirPathInMyDocuments(string dir)
    {
      dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + dir;
      return dir;
    }
  }
}
