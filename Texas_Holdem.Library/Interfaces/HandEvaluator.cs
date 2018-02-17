using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Structs;
using Texas_Holdem.Library.Classes;
namespace Texas_Holdem.Library.Interfaces
{
    public class HandEvaluator : IHandEvaluator
    {
        private List<Card> BestCards { get; set; } = new List<Card>();
        private Hands HandValue { get; set; }
        private Suits Suit { get; set; }

        private bool IsFlush(List<Card> cards)
        {
            Suit = Suits.Unknown;
            if (cards.Count(card => card.Suit.Equals(Suits.Spades)) >= 5) Suit = Suits.Spades;
            if (cards.Count(card => card.Suit.Equals(Suits.Hearts)) >= 5) Suit = Suits.Hearts;
            if (cards.Count(card => card.Suit.Equals(Suits.Clubs)) >= 5) Suit = Suits.Clubs;
            if (cards.Count(card => card.Suit.Equals(Suits.Diamonds)) >= 5) Suit = Suits.Diamonds;
            return Suit != Suits.Unknown;
        }

        private void FlushCards(List<Card> cards)
        {
            if (IsFlush(cards))
            {
                #region Flush
                var flush = cards.Where(c => c.Suit.Equals(Suit)).Reverse().Take(5).Reverse();

                if (flush != null)
                {
                    HandValue = Hands.Flush;
                    BestCards = flush.ToList();
                }
                #endregion
            }
        }

        private void IsStraight(List<Card> cards)
        {
            int value1 = (int)cards.ElementAt(0).Value;
            int value2 = (int)cards.ElementAt(1).Value;
            int value3 = (int)cards.ElementAt(2).Value;
            int value4 = (int)cards.ElementAt(3).Value;
            int value5 = (int)cards.ElementAt(4).Value;

            var isStraight = value2.Equals(value1 + 1) && value3.Equals(value2 + 1) && value4.Equals(value3 + 1) && value5.Equals(value4 + 1);
            var isLowStraight = value1.Equals(2) && value5.Equals(14) && value2.Equals(value1 + 1) && value3.Equals(value2 + 1) && value4.Equals(value3 + 1); // A,2,3,4,5
            var isHighStraight = isStraight && value5.Equals(14);
            isStraight = isStraight ? isStraight : isLowStraight || isHighStraight;

            var isFlush = IsFlush(cards);

            #region Royal Straight Flush
            if (isHighStraight && isFlush && Suit.Equals(Suits.Hearts))
            {
                HandValue = Hands.RoyalStraightFlush;
                BestCards = cards;
                return;
            }
            #endregion

            #region Straight Flush
            if (isStraight && isFlush && HandValue != Hands.RoyalStraightFlush)
            {
                HandValue = Hands.StraightFlush;
                BestCards = cards;
                return;
            }
            #endregion

            #region Straight
            if (isStraight && HandValue != Hands.StraightFlush && HandValue != Hands.RoyalStraightFlush)
            {
                HandValue = Hands.Straight;
                BestCards = cards;
                return;
            }
            #endregion
        }

        private void StraightCards(List<Card> cards)
        {
            var hands = new List<List<Card>>();

            if (cards.Count() >= 5)
                hands.Add(cards.Take(5).ToList());                     // [1,2,3,4,5],6,7

            if (cards.Count() >= 6)
            {
                hands.Add(cards.Skip(1).Take(5).ToList());             // 1,[2,3,4,5,6],7

                var tmpCards = cards.Take(4).ToList();
                tmpCards.AddRange(cards.Skip(5).Take(1));
                hands.Add(tmpCards.ToList());                          // [1,2,3,4],5,[6],7
            }
            if (cards.Count() == 7)
            {
                hands.Add(cards.Skip(2).Take(5).ToList());            // 1,2,[3,4,5,6,7]

                var tmpCards = cards.Take(4).ToList();
                tmpCards.AddRange(cards.Skip(6).Take(1));
                hands.Add(tmpCards.ToList());                         // [1,2,3,4],5,6,[7]

                var tmpCards1 = cards.Skip(1).Take(4).ToList();
                tmpCards1.AddRange(cards.Skip(6).Take(1));
                hands.Add(tmpCards1.ToList());                        // 1,[2,3,4,5],6,[7]
            }

            foreach (var hand in hands)
            {
                if (HandValue.Equals(Hands.RoyalStraightFlush)) break;
                IsStraight(hand);
            }
        }

        private void IsFourOfAKind(List<Card> cards)
        {
            var fourOfAKind = cards.GroupBy(c => c.Value)
                .Select(c => new { Value = c.Key, Count = c.Count() })
                .FirstOrDefault(d => d.Count.Equals(4));

            #region Four of a Kind
            if (fourOfAKind != null)
            {
                HandValue = Hands.FourOfAKind;
                BestCards = cards.Where(c => c.Value.Equals(fourOfAKind.Value)).ToList();

                var kicker = cards.Last(c => !c.Value.Equals(fourOfAKind.Value));
                BestCards.Add(kicker);
                return;
            }
            #endregion
        }

        private void IsFullHouse(List<Card> cards)
        {
            var threeOfAKind = cards.GroupBy(c => c.Value)
                .Select(c => new { Value = c.Key, Count = c.Count() })
                .Where(d => d.Count.Equals(3));

            #region Full House (With Two Three Of A Kind)
            if (threeOfAKind.Count().Equals(2))
            {
                HandValue = Hands.FullHouse;
                var value = threeOfAKind.Last().Value;
                var fullHouse = cards.Where(c => c.Value.Equals(value)).ToList();
                value = threeOfAKind.First().Value;
                fullHouse.AddRange(cards.Where(c => c.Value.Equals(value)).Take(2));
                BestCards = fullHouse;
                return;
            }
            #endregion

            var pairs = cards.GroupBy(c => c.Value)
                .Select(c => new { Value = c.Key, Count = c.Count() })
                .Where(d => d.Count.Equals(2));

            #region Full House (With Three Of A Kind and a pair)
            if (threeOfAKind.Count().Equals(1) && pairs.Count() > 0)
            {
                HandValue = Hands.FullHouse;
                var value = pairs.Last().Value;
                var fullHouse = cards.Where(c => c.Value.Equals(value)).ToList();
                value = threeOfAKind.First().Value;
                fullHouse.AddRange(cards.Where(c => c.Value.Equals(value)));
                BestCards = fullHouse;
                return;
            }
            #endregion
        }

        private void IsThreeOfAKind(List<Card> cards)
        {
            var threeOfAKind = cards.GroupBy(c => c.Value)
                .Select(c => new { Value = c.Key, Count = c.Count() })
                .Where(d => d.Count.Equals(3));

            #region Three Of A Kind
            if (threeOfAKind.Count() > 0)
            {
                HandValue = Hands.ThreeOfAKind;
                var value = threeOfAKind.Last().Value;
                var result = cards.Where(c => c.Value.Equals(value)).ToList();

                var kickers = cards.Where(c => !c.Value.Equals(value)).Reverse().Take(2).Reverse();
                result.AddRange(kickers);
                BestCards = result;
                return;
            }
            #endregion
        }

        private void IsTwoPair(List<Card> cards)
        {
            var pairs = cards.GroupBy(c => c.Value)
                .Select(c => new { Value = c.Key, Count = c.Count() })
                .Where(d => d.Count.Equals(2));

            #region Two Pairs
            if (pairs.Count() >= 2)
            {
                HandValue = Hands.TwoPair;
                // First Pair
                var first = pairs.Last().Value;
                var result = cards.Where(c => c.Value.Equals(first)).ToList();

                // Second Pair
                var last = pairs.First().Value;
                if (pairs.Count().Equals(2)) first = pairs.First().Value;
                else if (pairs.Count().Equals(3)) first = pairs.Skip(1).First().Value;

                result.AddRange(cards.Where(c => c.Value.Equals(first)));

                var kickers = cards.Where(c => !c.Value.Equals(first) && !c.Value.Equals(last)).Reverse().Take(1);
                result.AddRange(kickers);
                BestCards = result;
                return;
            }
            #endregion
        }

        private void IsPair(List<Card> cards)
        {
            var pairs = cards.GroupBy(c => c.Value)
                .Select(c => new { Value = c.Key, Count = c.Count() })
                .Where(d => d.Count.Equals(2));

            #region Pairs
            if (pairs.Count().Equals(1))
            {
                HandValue = Hands.Pair;
                // First Pair
                var value = pairs.First().Value;
                var result = cards.Where(c => c.Value.Equals(value)).ToList();

                var kickers = cards.Where(c => !c.Value.Equals(value)).Reverse().Take(3).Reverse();
                result.AddRange(kickers);
                BestCards = result;
                return;
            }
            #endregion
        }

        private void HighCard(List<Card> cards)
        {
            HandValue = Hands.Nothing;
            var kickers = cards.OrderByDescending(c => c.Value).Take(5).Reverse();
            BestCards = kickers.ToList();
        }

        //Sid 17: Returning tuples reqiuires the System.ValueTuple NuGet package
        public (List<Card> Cards, Hands HandValue, Suits Suit) EvaluateHand(List<Card> cards)
        {
            HandValue = Hands.Nothing;
            Suit = 0;
            BestCards.Clear();

            if (cards.Count() >= 2)
            {
                #region Sort the cards
                cards.Sort();
                #endregion

                #region Royal Straight Flush &  Straight Flush & Straight
                StraightCards(cards);
                if (HandValue.Equals(Hands.RoyalStraightFlush) || HandValue.Equals(Hands.StraightFlush)) return (Cards: BestCards, HandValue: HandValue, Suit: Suit);
                #endregion

                #region Four of a kind
                IsFourOfAKind(cards);
                if (HandValue.Equals(Hands.FourOfAKind)) return (Cards: BestCards, HandValue: HandValue, Suit: Suit);
                #endregion

                #region Full House
                IsFullHouse(cards);
                if (HandValue.Equals(Hands.FullHouse)) return (Cards: BestCards, HandValue: HandValue, Suit: Suit);
                #endregion

                #region Flush
                FlushCards(cards);
                if (HandValue.Equals(Hands.Flush)) return (Cards: BestCards, HandValue: HandValue, Suit: Suit);
                #endregion

                #region Straight
                // Already implemented above
                if (HandValue.Equals(Hands.Straight)) return (Cards: BestCards, HandValue: HandValue, Suit: Suit);
                #endregion

                #region Three of a Kind
                IsThreeOfAKind(cards);
                if (HandValue.Equals(Hands.ThreeOfAKind)) return (Cards: BestCards, HandValue: HandValue, Suit: Suit);
                #endregion

                #region Two Pair
                IsTwoPair(cards);
                if (HandValue.Equals(Hands.TwoPair)) return (Cards: BestCards, HandValue: HandValue, Suit: Suit);
                #endregion

                #region Pair
                IsPair(cards);
                if (HandValue.Equals(Hands.Pair)) return (Cards: BestCards, HandValue: HandValue, Suit: Suit);
                #endregion

                #region High Card
                HighCard(cards);
                #endregion
            }

            return (Cards: BestCards, HandValue: HandValue, Suit: Suit);
        }
    }
}