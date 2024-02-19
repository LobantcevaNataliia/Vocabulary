using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vocabulary
{
    public enum Level { A, B, C, U }
    public class Word
    {
        public int Id { get; set; }
        private string english;
        private string transcription;
        private string ukrainian;
        public bool Status { get; set; }  
        public Level Level { get; set; }

        public Word(int id, string english, string transcription, string ukrainian, bool status, Level level)
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
            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1).ToLower();
            else return str.ToUpper();
        }

    }
}
