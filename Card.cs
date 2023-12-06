using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfWar
{
    public class Card
    {
        public CardFace Face { get; set; }
        public CardSuit Suite { get; set; }

        public override string ToString()
        {
            return $"{(int) Enum.Parse(typeof(CardFace), this.Face.ToString())}{(char) this.Suite}";
        }
    }
}
