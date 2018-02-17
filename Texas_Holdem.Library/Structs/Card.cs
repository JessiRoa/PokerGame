using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texas_Holdem.Library.Enums;

namespace Texas_Holdem.Library.Structs
{


    public struct Card : IComparable //Gör att du kan jämföra båda obj
    //5. Implementing the Playing Card 
    {

        public Values Value { get; }
        public Suits Suit { get; set; }

        public string Output 
        {
            get
            {   
                var value = (int)Value <= 10 ? ((int)Value).ToString() : Value.ToString().Substring(0, 1);
                return $"{value}\n{(char)Suit}";
            }
        }

        //Constractor
        public Card(Values values, Suits suits)
        {
            Value = values;
            Suit = suits;
        }

        public int CompareTo(object Card)
        {
            if (Value > ((Card)Card).Value) return 1;
            else if (Value == ((Card)Card).Value) return 0; 
            else return -1;
        }
    }
}
