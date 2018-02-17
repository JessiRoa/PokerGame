using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texas_Holdem.Library.Classes;
using Texas_Holdem.Library.Enums;

namespace Texas_Holdem.Library.Classes
{
    public class Table
    {
        private Deck _deck = new Deck();
        public List<Player> Players = new List<Player>();
        //public List<Player> Players = List<Player>();
        public Player Dealer { get; } = new Player("Dealer");

        //Sid 14:
        public void DealPlayersCards()
        {
            foreach (var player in Players)
            {
                player.ClearHand();
                player.ReceiveCard(_deck.DrawCard(), true);
                player.ReceiveCard(_deck.DrawCard(), true);
            }
        }

        public void DealNewHand()
        {
            _deck.ShuffleDeck(5);
            Dealer.ClearHand();
            DealPlayersCards();
        }

        //Constructor
        public Table(string[] playerNames)
        {
            try
            {
                if (playerNames == null || playerNames.Length == 0 || playerNames.Length < 2 || playerNames.Length > 4)
                {

                    throw new ArgumentException();
                }
                else
                {
                    foreach (var participant in playerNames)
                    {
                        //Player newPlayer = new Player(participant);  // new player object
                        //Players.Add(newPlayer);
                        Players.Add(new Player(participant));
                    }
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Incorrect number of players", e);
            }
        }

        //14. Dealer Draws a Card Form the Deck
        public void DealerDrawsCard(int count = 1)
        {
            if (Dealer.CardCount == 5) return;
            for (int i = 1; i <= count; i++)
            {
                var card = _deck.DrawCard();
                Dealer.ReceiveCard(card);
                foreach (var players in Players)
                {
                    players.ReceiveCard(card);

                }
            }
        }

        //Sid 19:
        public void EvaluatePlayerHands()
        {
            foreach(var player in Players)
            {
                player.EvaluateHand();
            }
        }

        //Sid 19: Determining the Winner(s):
public List<Player> DetermineWinner()
{
    var highestHand = Players.Max(m => m.HandValue);
    var bestHands = Players.Where(p => p.HandValue.Equals(highestHand));

    if (bestHands.Count().Equals(1)) return bestHands.ToList();

    List<Player> players;
    switch (highestHand)
    {
        // [E] = Equal
        // [G] = Greater Than
        // [?] = Not used in comparison

        #region High Card
        case Hands.Nothing:
            players = bestHands.Take(1).ToList();
            foreach (var player in bestHands.Skip(1))
            {
                var savedPlayerCards = players.First().BestCards;
                var currentPlayerCards = player.BestCards;

                var card1 = savedPlayerCards[4].CompareTo(currentPlayerCards[4]);
                var card2 = savedPlayerCards[3].CompareTo(currentPlayerCards[3]);
                var card3 = savedPlayerCards[2].CompareTo(currentPlayerCards[2]);
                var card4 = savedPlayerCards[1].CompareTo(currentPlayerCards[1]);
                var card5 = savedPlayerCards[0].CompareTo(currentPlayerCards[0]);

                // [E][E][E][E][E] All cards are equal to saved player's cards
                if (card1.Equals(0) && card2.Equals(0) && card3.Equals(0) && card4.Equals(0) && card5.Equals(0)) players.Add(player);
                // [G][G][G][G][G] At least one of the cards are greater than saved player's cards
                if (card1.Equals(-1) || card2.Equals(-1) || card3.Equals(-1) || card4.Equals(-1) || card5.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
            }
            return players;
        #endregion
        #region Pair
        case Hands.Pair:
            players = bestHands.Take(1).ToList();
            foreach (var player in bestHands.Skip(1))
            {
                var savedPlayerCards = players.First().BestCards;
                var currentPlayerCards = player.BestCards;

                var card1 = savedPlayerCards[0].CompareTo(currentPlayerCards[0]);
                var card2 = savedPlayerCards[1].CompareTo(currentPlayerCards[1]);
                var card3 = savedPlayerCards[2].CompareTo(currentPlayerCards[2]);
                var card4 = savedPlayerCards[3].CompareTo(currentPlayerCards[3]);
                var card5 = savedPlayerCards[4].CompareTo(currentPlayerCards[4]);

                // [E][E][E][E][E] All cards are equal to saved player's cards
                if (card1.Equals(0) && card2.Equals(0) && card3.Equals(0) && card4.Equals(0) && card5.Equals(0)) players.Add(player);
                // [E][E]|[G][G][G] First pair equal, at least one of the remaining cards are greater than saved player's cards
                else if (card1.Equals(0) && card2.Equals(0) && (card3.Equals(-1) || card4.Equals(-1) || card5.Equals(-1)))
                {
                    players.Clear();
                    players.Add(player);
                }
                // [G][G][?][?][?] Pair is greater than saved player's pair
                else if (card1.Equals(-1) && card2.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
            }
            return players;
        #endregion
        #region Two Pair
        case Hands.TwoPair:
            players = bestHands.Take(1).ToList();
            foreach (var player in bestHands.Skip(1))
            {
                var savedPlayerCards = players.First().BestCards;
                var currentPlayerCards = player.BestCards;

                var card1 = savedPlayerCards[0].CompareTo(currentPlayerCards[0]);
                var card2 = savedPlayerCards[1].CompareTo(currentPlayerCards[1]);
                var card3 = savedPlayerCards[2].CompareTo(currentPlayerCards[2]);
                var card4 = savedPlayerCards[3].CompareTo(currentPlayerCards[3]);
                var card5 = savedPlayerCards[4].CompareTo(currentPlayerCards[4]);

                // [E][E][E][E][E] All cards are equal to saved player's cards
                if (card1.Equals(0) && card2.Equals(0) && card3.Equals(0) && card4.Equals(0) && card5.Equals(0)) players.Add(player);
                // [E][E][E][E][G] Kicker card is greater than saved player's cards
                else if (card1.Equals(0) && card2.Equals(0) && card3.Equals(0) && card4.Equals(0) && card5.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
                // [E][E][G][G][?] First pair equal, second pair greater than saved player's cards
                else if (card1.Equals(0) && card2.Equals(0) && card3.Equals(-1) && card4.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
                // [G][G][?][?][?] Highest pair greater than saved player's highest pair
                else if (card1.Equals(-1) && card2.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
            }
            return players;
        #endregion
        #region Three of a kind
        case Hands.ThreeOfAKind:
            players = bestHands.Take(1).ToList();
            foreach (var player in bestHands.Skip(1))
            {
                var savedPlayerCards = players.First().BestCards;
                var currentPlayerCards = player.BestCards;

                var card1 = savedPlayerCards[0].CompareTo(currentPlayerCards[0]);
                var card2 = savedPlayerCards[1].CompareTo(currentPlayerCards[1]);
                var card3 = savedPlayerCards[2].CompareTo(currentPlayerCards[2]);
                var card4 = savedPlayerCards[3].CompareTo(currentPlayerCards[3]);
                var card5 = savedPlayerCards[4].CompareTo(currentPlayerCards[4]);

                // [E][E][E][E][E] All cards are equal to saved player's cards
                if (card1.Equals(0) && card2.Equals(0) && card3.Equals(0) && card4.Equals(0) && card5.Equals(0)) players.Add(player);
                // [E][E][E]|[G][G] One of the kicker cards are greater than saved player's kicker cards
                else if (card1.Equals(0) && card2.Equals(0) && card3.Equals(0) && (card4.Equals(-1) || card5.Equals(-1)))
                {
                    players.Clear();
                    players.Add(player);
                }
                // [G][G][G][?][?] Three of a kind greater than saved player's Three of a kind
                else if (card1.Equals(-1) && card2.Equals(-1) && card3.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
            }
            return players;
        #endregion
        #region Straight
        case Hands.Straight:
            players = bestHands.Take(1).ToList();
            foreach (var player in bestHands.Skip(1))
            {
                var savedPlayerCards = players.First().BestCards;
                var currentPlayerCards = player.BestCards;

                var card1 = savedPlayerCards[4].CompareTo(currentPlayerCards[4]);

                // [?][?][?][?][E] The highest card in the straight is the same as the saved player's highest straight card
                if (card1.Equals(0)) players.Add(player);
                // [?][?][?][?][G] The highest card in the straight is greater than the saved player's highest straight card
                if (card1.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
            }
            return players;
        #endregion
        #region Flush
        case Hands.Flush:
            players = bestHands.Take(1).ToList();
            foreach (var player in bestHands.Skip(1))
            {
                var savedPlayerCards = players.First().BestCards;
                var currentPlayerCards = player.BestCards;

                var card1 = savedPlayerCards[0].CompareTo(currentPlayerCards[0]);
                var card2 = savedPlayerCards[1].CompareTo(currentPlayerCards[1]);
                var card3 = savedPlayerCards[2].CompareTo(currentPlayerCards[2]);
                var card4 = savedPlayerCards[3].CompareTo(currentPlayerCards[3]);
                var card5 = savedPlayerCards[4].CompareTo(currentPlayerCards[4]);

                // There can never be two flushes with different suits at the same time 
                // because three of the cards are from the dealer's cards and two from the player.

                // [E][E][E][E][E] The flush cards are from the dealer's cards only (all players will have the same flush)
                if (card1.Equals(0) && card2.Equals(0) && card3.Equals(0) && card4.Equals(0) && card5.Equals(0)) players.Add(player);
                // [G][G][G][G][G] One of the cards in the flush is higher than the cards in the saves player's cards
                else if (card1.Equals(-1) || card2.Equals(-1) || card3.Equals(-1) || card4.Equals(-1) || card5.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
            }
            return players;
        #endregion
        #region Full House
        case Hands.FullHouse:
            players = bestHands.Take(1).ToList();
            foreach (var player in bestHands.Skip(1))
            {
                var savedPlayerCards = players.First().BestCards;
                var currentPlayerCards = player.BestCards;

                var card1 = savedPlayerCards[0].CompareTo(currentPlayerCards[0]); // Pair
                var card3 = savedPlayerCards[2].CompareTo(currentPlayerCards[2]); // Three of a kind

                // [E][E][E][E][E] The full house is the same as the saved player's full house
                if (card1.Equals(0) && card3.Equals(0)) players.Add(player);
                // [?][?][G][G][G] The Three of a kind is greater than the Three of a kind the saved player's has
                else if (card3.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
                // [G][G][E][E][E] The Three of a kind same and the pair is greater than the pair the saved player's has
                else if (card1.Equals(-1) && card3.Equals(0))
                {
                    players.Clear();
                    players.Add(player);
                }
            }
            return players;
        #endregion
        #region Four of a Kind
        case Hands.FourOfAKind:
            players = bestHands.Take(1).ToList();
            foreach (var player in bestHands.Skip(1))
            {
                var savedPlayerCards = players.First().BestCards;
                var currentPlayerCards = player.BestCards;

                var card1 = savedPlayerCards[0].CompareTo(currentPlayerCards[0]);
                var card5 = savedPlayerCards[4].CompareTo(currentPlayerCards[4]);

                // [E][E][E][E][E] The cards are the same as the saved player's cards
                if (card1.Equals(0) && card5.Equals(0)) players.Add(player);
                // [E][E][E][E][G] The kicker is greater than the saved player's kicker
                else if (card1.Equals(0) && card5.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
                // [G][G][G][G][?] The Four of a kind is greater than the saved player's Four of a kind
                else if (card1.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
            }
            return players;
        #endregion
        #region Straight Flush
        case Hands.StraightFlush:
            players = bestHands.Take(1).ToList();
            foreach (var player in bestHands.Skip(1))
            {
                var savedPlayerCards = players.First().BestCards;
                var currentPlayerCards = player.BestCards;

                var card5 = savedPlayerCards[4].CompareTo(currentPlayerCards[4]);

                // There can never be two straight flushes with different suits at the same time 
                // because three of the cards are from the dealer's cards and two from the player.

                // [E][E][E][E][E] The straight flush cards are from the dealer's cards only (all players will have the same straight flush)
                if (card5.Equals(0)) players.Add(player);
                // [?][?][?][?][G] The highest card in the straight flush is higher than the saved player's highest straight flush card.
                // The current player has a higher straight
                else if (card5.Equals(-1))
                {
                    players.Clear();
                    players.Add(player);
                }
            }
            return players;
        #endregion
        #region Royal Straight Flush
        case Hands.RoyalStraightFlush:
            // There can only be one Royal Straight Flush, 
            // so the bestHands list should only contain that player.
            return bestHands.ToList();
            #endregion
    }

    return null;
        }
    }
}
