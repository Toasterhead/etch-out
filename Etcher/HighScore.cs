using System;
using System.IO;

namespace Etcher
{
    public static class HighScore
    {
        public const int LIST_LENGTH = 10;

        private const int MAX_ENTRY_LENGTH = 20;
        private const string FILE_NAME = "high_score.dat";
        private const uint FINISHED_DURATION = 2 * Game1.FRAME_RATE;

        public struct FieldEntry
        {
            public string name;
            public int score;

            public FieldEntry(string name, int score)
            {
                this.name = name;
                this.score = score;
            }
        }

        public const string PROMPT = "Enter name: ";
        public const string VIRTUAL_KEYPAD = "ABCDEFGHIJKLMNOPQRSTUVWXYZ <";

        private static char markerCommand = '$';
        private static char[] delimiters = { '$', ':' };
        private static FieldEntry[] topScore = new FieldEntry[LIST_LENGTH];

        public static string NameEntry { get; private set; } = "";
        public static int VirtualKeypadIndex { get; private set; } = 0;
        public static uint? Finished { get; private set; } = null;

        public static FieldEntry[] TopScore
        {
            get
            {
                FieldEntry[] temp = new FieldEntry[topScore.Length];

                for (int i = 0; i < topScore.Length; i++)
                    temp[i] = new FieldEntry(topScore[i].name, topScore[i].score);

                return temp;
            }
        }

        public static string[] TopScoresAsString()
        {
            string[] temp = new string[topScore.Length];
            for (int i = 0; i < topScore.Length; i++)
                temp[i] = 
                    (i + 1).ToString() + 
                    (i + 1 >= 10 ? ". " : ".  ") + 
                    FormattedName(topScore[i].name, 20, i) + " " + 
                    topScore[i].score.ToString();

            return temp;
        }

        public static void ResetFields()
        {
            Finished = null;
            NameEntry = "";
            VirtualKeypadIndex = 0;
        }

        public static void MoveCursorLeft()
        {
            if (--VirtualKeypadIndex < 0)
                VirtualKeypadIndex = VIRTUAL_KEYPAD.Length;
        }

        public static void MoveCursorRight()
        {
            if (++VirtualKeypadIndex > VIRTUAL_KEYPAD.Length)
                VirtualKeypadIndex = 0;
        }

        public static void ExecuteCommand()
        {
            if (VirtualKeypadIndex == VIRTUAL_KEYPAD.Length)
            { 
                Finished = FINISHED_DURATION;
                Game1.StopMusic();
                Game1.PlaySound(Sounds.SELECT);
            }
            else if (VIRTUAL_KEYPAD[VirtualKeypadIndex] == '<')
            {
                if (NameEntry.Length > 0)
                {
                    NameEntry = NameEntry.Substring(0, NameEntry.Length - 1);
                    Game1.StopMusic();
                    Game1.PlaySound(Sounds.NAME_INPUT); 
                }
                else Game1.PlaySound(Sounds.NAME_INPUT); 
            }
            else if (VIRTUAL_KEYPAD[VirtualKeypadIndex] == '_')
            {
                NameEntry += " ";
                Game1.PlaySound(Sounds.NAME_INPUT); 
            }
            else
            {
                if (NameEntry.Length <= MAX_ENTRY_LENGTH)
                {
                    NameEntry += VIRTUAL_KEYPAD[VirtualKeypadIndex];
                    Game1.PlaySound(Sounds.NAME_INPUT); 
                }
                else Game1.PlaySound(Sounds.NAME_INPUT); 
            }
        }

        public static bool CheckFinished()
        {
            if (Finished != null)

                return --Finished == 0;

            return false;
        }

        private static string FormattedName(string name, int maxLength, int ranking)
        {
            string s = name == null ? "NULL" : name;

            if (s.Length < maxLength)
                for (int i = s.Length; i < maxLength; i++)
                    s += "@";

            return s;
        }

        public static void SubmitScore(FieldEntry entry)
        {
            for (int i = 0; i < LIST_LENGTH; i++)
            {
                if (entry.score > topScore[i].score)
                {
                    for (int j = LIST_LENGTH - 1; j > i; j--)
                        topScore[j] = topScore[j - 1];
                    topScore[i] = entry;

                    break;
                }
            }
        }

        public static int? ListPosition(int score)
        {
            for (int i = 0; i < LIST_LENGTH; i++)
                if (score > topScore[i].score)

                    return i;

            return null;
        }

        public static async void LoadFromFileAsync()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            
            if (!File.Exists(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + FILE_NAME))
            {
                Windows.Storage.StorageFile file = await storageFolder.CreateFileAsync(
                    FILE_NAME,
                    Windows.Storage.CreationCollisionOption.OpenIfExists);

                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                using (var outputStream = stream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream))
                    {
                        for (int i = 0; i < LIST_LENGTH; i++)
                            dataWriter.WriteString("$Player" + i + ":" + (30000 - (i * 3000)) + Environment.NewLine);

                        await dataWriter.StoreAsync();
                        await outputStream.FlushAsync();
                    }
                }
                stream.Dispose();
            }

            Windows.Storage.StorageFile fileRead = await storageFolder.GetFileAsync(FILE_NAME);

            string text = await Windows.Storage.FileIO.ReadTextAsync(fileRead);

            string[] lines = text.Split(Environment.NewLine);

            int count = 0;

            foreach (string line in lines)
            { 
                bool isValidLine = line != null && line != "" && line[0] == markerCommand;

                if (isValidLine)
                {
                    string[] terms = line.Split(delimiters);

                    if (terms.Length == 3 && count < topScore.Length)
                    {
                        topScore[count] = new FieldEntry(terms[1], Convert.ToInt32(terms[2]));
                        count++;
                    }
                }
                else if (count >= topScore.Length)
                    break;
            }

            MenuManager.UpdateHighScore(HighScore.TopScoresAsString());
        }

        public static async void WriteToFileAsync()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            
            if (File.Exists(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + FILE_NAME))
            {
                Windows.Storage.StorageFile file = await storageFolder.GetFileAsync(FILE_NAME);

                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                using (var outputStream = stream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream))
                    {
                        for (int i = 0; i < LIST_LENGTH; i++)
                            dataWriter.WriteString("$" + topScore[i].name + ":" + topScore[i].score + Environment.NewLine);

                        await dataWriter.StoreAsync();
                        await outputStream.FlushAsync();
                    }
                }
                stream.Dispose();
            }
        }
    }
}
