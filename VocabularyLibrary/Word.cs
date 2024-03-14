
namespace VocabularyLibrary
{
    public enum Level { A1, A2, B1, B2, U }
    public class Word
    {
        public int Id { get; set; }
        private string english;
        private string transcription;
        private string ukrainian;
        public bool Status { get; set; }  
        public Level Level { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Word()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Id = 0;
            English = "";
            Transcription = "";
            Ukrainian = "";
            Status = false;
            Level = Level.U;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Word(int id, string english, string transcription, string ukrainian, bool status, Level level)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Id = id;
            English = english;
            Transcription = transcription;
            Ukrainian = ukrainian;
            Status = status;
            Level = level;
        }

        public string English 
        {
            get { return english; }
            set { english = Change(value); }
        }

        public string Transcription
        {
            get { return transcription; }
            set
            {
                if (value.Contains(" "))
                    value = value.Trim();
                if (value.Contains("\t"))
                    value = value.Replace("\t", "");
                transcription = value;
            }
        }

        public string Ukrainian
        {
            get { return ukrainian; }
            set { ukrainian = Change(value); }
        }

        private string Change(string str)
        {
            if (str.Contains(" "))
                str = str.Trim();
            if (str.Contains("\t"))
                str = str.Replace("\t", "");
            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1).ToLower();
            else return str.ToUpper();
        }

    }
}
