using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocabulary
{
    public class Word
    {
        public int id { get; set; }
        public string english { get; set; }
        public string transcription { get; set; }
        public string ukrainian { get; set; }
        public bool status { get; set; }

        public Word(int id, string english, string transcription, string ukrainian, bool status)
        {
            this.id = id;
            this.english = english;
            this.transcription = transcription;
            this.ukrainian = ukrainian;
            this.status = status;
        }
    }
}
