using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocabulary
{
    public class Word
    {
        public string english { get; set; }
        public string ukrainian { get; set; }
        public bool status { get; set; }

        public Word(string english, string ukrainian, bool status)
        {
            this.english = english;
            this.ukrainian = ukrainian;
            this.status = status;
        }
    }
}
