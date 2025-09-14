using System;
using System.IO;
using System.Text.Json;

namespace ConnectFour
{
    public class SerializableDataField
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public char[,] Grid { get; set; } = default!;
        public int Turn { get; set; }

        //Ordinary, Magnetic, Boring type.
        public string Player1DiscType { get; set; }
        public string Player2DiscType { get; set; }
        public string Player1Symbol { get; set; }
        public string Player2Symbol { get; set; }

    }

    //savig the game.
    public class SaveData
    {
        private string saveFilePath = "save_game.json";


        public void SaveGame(SerializableDataField data)
        {
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(saveFilePath, json);
            Console.WriteLine("Your game has been saved!");
        }

        //load the game.
        public SerializableDataField LoadGame()
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                var data = JsonSerializer.Deserialize<SerializableDataField>(json);
                Console.WriteLine("Your game has been loaded!");
                return data;
            }
            else
            {
                Console.WriteLine("No saved game.");
                return null!;
            }
        }



    }
    
}
   