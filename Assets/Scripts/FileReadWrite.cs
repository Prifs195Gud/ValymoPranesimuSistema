using System;
using System.IO;
using UnityEngine;

public static class FileReadWrite
{
    public static void WriteToFile(object obj, string fileName)
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName + ".txt";
        StreamWriter write = new StreamWriter(path);
        string data = JsonUtility.ToJson(obj, true);
        write.Write(data);
        write.Close();
    }

    public static object ReadFromFile(TextAsset txt, System.Type type)
    {
        return JsonUtility.FromJson(txt.text, type);
    }
}
