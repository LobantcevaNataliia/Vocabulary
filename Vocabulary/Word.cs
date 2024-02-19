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
        public int id { get; set; }
        public string english { get; set; }
        public string transcription { get; set; }
        public string ukrainian { get; set; }
        public bool status { get; set; }  
        public Level level { get; set; }

        public Word(int id, string english, string transcription, string ukrainian, bool status, Level level)
        {
            this.id = id;
            this.english = english;
            this.transcription = transcription;
            this.ukrainian = ukrainian;
            this.status = status;
            this.level = level;
        }

    }
}
