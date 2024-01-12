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
        public string russian { get; set; }
        public bool status { get; set; }

        public Word(string english, string russian, bool status)
        {
            this.english = english;
            this.russian = russian;
            this.status = status;
        }
    }
}
