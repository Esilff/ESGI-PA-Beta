using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveScript
{
    static string pathDirectory = Application.persistentDataPath + "/S";
    static string pathFileSelection = "/SeSa.select";
    static string pathFileOptions = "/Op.saOp";
    static BinaryFormatter bf = new BinaryFormatter();

    public static void SaveSelection(int[] indices) 
    {
        if (!Directory.Exists(pathDirectory))
        {
            Directory.CreateDirectory(pathDirectory);
        }

        FileStream stream = new FileStream(pathDirectory + pathFileSelection, FileMode.Create);
        bf.Serialize(stream, new SelectionSave(indices));
        stream.Close();
    }

    public static SelectionSave LoadSelectionSaves()
    {
        if (!Directory.Exists(pathDirectory))
        {
            Directory.CreateDirectory(pathDirectory);
        }


        if (!File.Exists(pathDirectory + pathFileSelection)) SaveSelection(new int[] {0, 1, 2, 0, 1, 0, 1, 2, 0});
        
        FileStream stream = new FileStream(pathDirectory + pathFileSelection, FileMode.Open);
        SelectionSave DataToLoad = (SelectionSave) bf.Deserialize(stream);
        stream.Close();
        return DataToLoad;
    }

    public static void SaveOptions(KeyCode[] keyCodes, float[] vs, float[] svs, int i, bool fs) 
    {
        if (!Directory.Exists(pathDirectory))
        {
            Directory.CreateDirectory(pathDirectory);
        }

        FileStream stream = new FileStream(pathDirectory + pathFileOptions, FileMode.Create);
        bf.Serialize(stream, new OptionsSave(keyCodes, vs, svs, i, fs));
        stream.Close();
    }

    public static OptionsSave LoadOptionsSaves()
    {
        if (!Directory.Exists(pathDirectory))
        {
            Directory.CreateDirectory(pathDirectory);
        }
        if (!File.Exists(pathDirectory + pathFileOptions)) return null;
        
        FileStream stream = new FileStream(pathDirectory + pathFileOptions, FileMode.Open);
        OptionsSave DataToLoad = (OptionsSave) bf.Deserialize(stream);
        stream.Close();
        return DataToLoad;
    }
}