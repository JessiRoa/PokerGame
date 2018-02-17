using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Structs;

namespace Texas_Holdem.Library.Classes
{
    //8 å 9.Creating a Table / Add Deck Class
    public class Deck
    {
        List<Card> _cards = new List<Card>(); //c.Sparar 25 kort i Listan
        private void NewDeck()
        {
            _cards.Clear();
            foreach (Suits suit in Enum.GetValues(typeof(Suits))) //a.Hämtar iconer av 5 kort å skickar till den andra foreach.
            {
                if (suit.Equals(Suits.Unknown)) continue;
                foreach (Values value in Enum.GetValues(typeof(Values))) //b.Plusar 5 + 14 kort, adderar siffrorna och loopar den till Listan.
                {
                    _cards.Add(new Card(value, suit));
                    //Deck.Add(_cards);
                }
            }
        }

        public void ShuffleDeck(int shuffles)
        {
            NewDeck();
            var rnd = new Random();
            for (int i = 0; i < shuffles; i++)
            {
                List<Card> tmpDeck = new List<Card>();

                while (_cards.Count > 0)
                {
                    var index = rnd.Next(_cards.Count);

                    var card = _cards[index];
                    _cards.RemoveAt(index);
                    tmpDeck.Add(card);
                }
                _cards = tmpDeck;
            }
        }

        public Card DrawCard()
        {
            var card = _cards[0];
            _cards.Remove(card);
            return card;

        }
    }
}
