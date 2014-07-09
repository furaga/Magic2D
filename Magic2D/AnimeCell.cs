using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic2D
{
    public class AnimeCell
    {
        public string key = "";
        public int durationMilliseconds = 33;
        public AnimeCell(string key, int durationMilliseconds = 33)
        {
            this.key = key;
            this.durationMilliseconds = durationMilliseconds;
        }
    }
}
