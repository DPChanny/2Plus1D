using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileManager
{
    private const string ClearedMapFileName = "ClearedMap.txt";

    public static void AddClearedMap(string _mapName)
    {
        if (!IsMapCleared(_mapName))
        {
            StreamWriter streamWriter = new StreamWriter(ClearedMapFileName, true);
            streamWriter.WriteLine(_mapName);
            streamWriter.Close();
        }
    }

    public static List<string> GetClearedMap()
    {
        List<string> clearedMaps = new List<string>();
        StreamWriter streamWriter = new StreamWriter(ClearedMapFileName, true);
        streamWriter.Close();
        StreamReader streamReader = new StreamReader(ClearedMapFileName);
        while (!streamReader.EndOfStream)
        {
            string line = streamReader.ReadLine();
            if (line != "")
            {
                clearedMaps.Add(line);
            }
        }
        streamReader.Close();
        return clearedMaps;
    }

    public static bool IsMapCleared(string _mapName)
    {
        return GetClearedMap().Contains(_mapName);
    }
}
