using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem_S
{
    public static void SavePlayer(MasterController_S masterController_S)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "player.fun");

        FileStream stream = new FileStream(path, FileMode.Create);
        
        PlayerData_S data = new PlayerData_S(masterController_S);

        formatter.Serialize(stream, data);
        
        stream.Close();
    }


    public static PlayerData_S LoadPlayer()
    {
        string path = Path.Combine(Application.persistentDataPath, "player.fun");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);
            
            PlayerData_S data =  formatter.Deserialize(stream) as PlayerData_S;

            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
