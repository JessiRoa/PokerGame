using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Texas_Holdem.Library.Structs;
using Texas_Holdem.Library.Classes;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Interfaces;

namespace Texas_Holdem.Library.Classes
{
    //The Player Class
    public class Player
    {
        //Variable _hand
        private Hand _hand; 
        public string Name { get; }
        //{
        //    private AddCard(Value value);
        //}
        //Properties
        public List<Card> Cards { get => _hand.Cards; }
        public List<Card> BestCards { get { return _hand.BestCards; } }
        public List<Card> PlayerCards { get { return _hand.PlayerCards; } }
        public int CardCount { get { return Cards.Count; } }
        //Sid 18:
        public Hands HandValue { get { return _hand.HandValue; } }
        public Suits Suit { get { return _hand.Suit; }}

        //Constructor
        //}
        public Player(string name)
        {
            Name = name;
            _hand = new Hand(new HandEvaluator());
        }

        //Method
        public void ReceiveCard(Card card, bool isPlayerCard = false)
        {
            _hand.AddCard(card, isPlayerCard);
        }
        public void ClearHand()
        {
            _hand.Clear();
        }

        //Sid 18:
        public void EvaluateHand()
        {
                _hand.EvaluateHand();
        }
      
    }
}
