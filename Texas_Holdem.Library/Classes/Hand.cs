using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texas_Holdem.Library.Structs;
using Texas_Holdem.Library.Classes;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Interfaces;

namespace Texas_Holdem.Library.Classes
{

    public class Hand
    {
        string variable;
        //10. Adding Hand Class:
        public List<Card> Cards { get; } = new List<Card>();
        public List<Card> BestCards { get; } = new List<Card>();
        public List<Card> PlayerCards { get; } = new List<Card>();

        private IHandEvaluator _eval;
        //Constructor IHandEvaluator:
        public Hand(IHandEvaluator eval)
        {
            this._eval = eval;
        }
        public Hands HandValue { get; private set; }
        public Suits Suit { get; private set; }

        public void Clear()
        {
            Cards.Clear();
            BestCards.Clear();
            PlayerCards.Clear();

        }
        //Sid 18: punkt 17:
        public void EvaluateHand()
        {
            if (Cards.Count >= 2)
            {
                var eval = _eval.EvaluateHand(Cards);
                BestCards.AddRange(eval.Cards);
                HandValue = eval.HandValue;
                Suit = eval.Suit;
            }
        }
            public void AddCard(Card card, bool isPlayerCard)
        {
            if (isPlayerCard == true && (PlayerCards.Count < 2)) //Changes: .Count?? 
            {
                PlayerCards.Add(card);
            }
            Cards.Add(card);
        }
    }
}
