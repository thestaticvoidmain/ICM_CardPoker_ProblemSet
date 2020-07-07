using System.Collections.Generic;
using System.Linq;

namespace PokerGame
{
    public class PokerRuleEngine
    {
        public int DetermineWinner(CardSet cards1, CardSet cards2)
        {
            int? winner = null;
            while (winner == null)
            {
                cards1 = ComputeRankAndHighCard(cards1.Cards);
                cards2 = ComputeRankAndHighCard(cards2.Cards);

                if (cards1.Rank > cards2.Rank)
                {
                    winner = 1;
                }
                else if (cards1.Rank < cards2.Rank)
                {
                    winner = 2;
                }
                else 
                {
                    //Console.WriteLine("same rank");
                    if (cards1.HighCard > cards2.HighCard)
                    {
                        winner = 1;
                    }
                    else if (cards1.HighCard < cards2.HighCard)
                    {
                        winner = 2;
                    }
                    else
                    {
                        //Console.WriteLine("call recursive");
                        //Console.WriteLine("P1 cards: {0}", string.Join(",", cards1.Cards.Select(c => c.IntValue)));
                        //Console.WriteLine("P2 cards: {0}", string.Join(",", cards2.Cards.Select(c => c.IntValue)));

                        cards1.Cards.RemoveAll(c => c.IntValue == cards1.HighCard);
                        cards2.Cards.RemoveAll(c => c.IntValue == cards2.HighCard);

                        continue;
                    }
                }
            }

            return (int)winner;
        }
        public CardSet ComputeRankAndHighCard(List<Card> cards)
        {
            int hCard;

            CardSet cs = new CardSet();
            cs.Cards = cards;

            if (CheckRoyalFlush(cards))
            {
                cs.Rank = 10;
                cs.HighCard = 0;
                return cs;
            }
            else if (CheckStraightFlush(cards, out hCard))
            {
                cs.Rank = 9;
                cs.HighCard = hCard;
                return cs;
            }
            else if (CheckFourOfKind(cards, out hCard))
            {
                cs.Rank = 8;
                cs.HighCard = hCard;
                return cs;
            }
            else if (CheckFullHouse(cards, out hCard))
            {
                cs.Rank = 7;
                cs.HighCard = hCard;
                return cs;
            }
            else if (CheckFlush(cards, out hCard))
            {
                cs.Rank = 6;
                cs.HighCard = hCard;
                return cs;
            }
            else if (CheckStraight(cards, out hCard))
            {
                cs.Rank = 5;
                cs.HighCard = hCard;
                return cs;
            }
            else if (CheckThreeOfKind(cards, out hCard))
            {
                cs.Rank = 4;
                cs.HighCard = hCard;
                return cs;
            }
            else if (CheckTwoPairs(cards, out hCard))
            {
                cs.Rank = 3;
                cs.HighCard = hCard;
                return cs;
            }
            else if (CheckPair(cards, out hCard))
            {
                cs.Rank = 2;
                cs.HighCard = hCard;
                return cs;
            }
            else 
            {
                cs.Rank = 1;
                cs.HighCard = cards.OrderByDescending(o => o.IntValue).First().IntValue; 
                return cs;
            }
        }


        #region Poker Rules
        private bool CheckPair(List<Card> cards)
        {
            return cards.GroupBy(c => c.IntValue).Count(grp => grp.Count() == 2) == 1;
        }
        private bool CheckPair(List<Card> cards, out int hCard)
        {
            hCard = cards.GroupBy(c => c.IntValue).Where(grp => grp.Count() == 2).Select(c => c.Key).ToList().FirstOrDefault();
            return cards.GroupBy(c => c.IntValue).Count(grp => grp.Count() == 2) == 1;
        }

        private bool CheckTwoPairs(List<Card> cards, out int hCard)
        {
            List<int> pairs = cards.GroupBy(c => c.IntValue).Where(grp => grp.Count() == 2).Select(c => c.Key).ToList();
            hCard = pairs.OrderByDescending(c => c).FirstOrDefault();
            return cards.GroupBy(c => c.IntValue).Count(grp => grp.Count() == 2) == 2;
        }
        private bool CheckThreeOfKind(List<Card> cards, out int hCard)
        {
            hCard = cards.GroupBy(c => c.IntValue).Where(grp => grp.Count() == 3).Select(c => c.Key).ToList().FirstOrDefault();
            return cards.GroupBy(c => c.IntValue).Any(grp => grp.Count() == 3);
        }
        private bool CheckStraight(List<Card> cards, out int hCard)
        {
            Card highCard = cards.OrderByDescending(o => o.IntValue).First();
            Card lowCard = cards.OrderBy(o => o.IntValue).First();

            hCard = highCard.IntValue;
            //hi card - low card should be inclusive
            return (CheckAllUniqueValue(cards) && (highCard.IntValue - lowCard.IntValue == 4));
        }
        private bool CheckFlush(List<Card> cards)
        {
            return cards.GroupBy(c => c.Suit).Count(grp => grp.Count() == 5) == 1;
        }
        private bool CheckFlush(List<Card> cards, out int hCard)
        {
            hCard = cards.OrderByDescending(o => o.IntValue).First().IntValue;
            return cards.GroupBy(c => c.Suit).Count(grp => grp.Count() == 5) == 1;
        }

        private bool CheckFullHouse(List<Card> cards, out int hCard)
        {
            hCard = cards.GroupBy(c => c.IntValue).Where(grp => grp.Count() == 3).Select(c => c.Key).ToList().FirstOrDefault();
            return CheckPair(cards) && CheckThreeOfKind(cards, out hCard);
        }
        private bool CheckFourOfKind(List<Card> cards, out int hCard)
        {
            hCard = cards.GroupBy(c => c.IntValue).Where(grp => grp.Count() == 4).Select(c => c.Key).ToList().FirstOrDefault();
            return cards.GroupBy(c => c.IntValue).Any(grp => grp.Count() == 4);
        }

        private bool CheckStraightFlush(List<Card> cards, out int hCard)
        {
            hCard = cards.OrderByDescending(o => o.IntValue).First().IntValue;
            return (CheckFlush(cards) && CheckStraight(cards, out hCard));
        }

        private bool CheckRoyalFlush(List<Card> cards)
        {
            return (CheckFlush(cards) && CheckRoyal(cards));
        }
        #endregion


        private bool CheckRoyal(List<Card> cards)
        {
            Card highCard = cards.OrderByDescending(o => o.IntValue).First();
            Card lowCard = cards.OrderBy(o => o.IntValue).First();

            return (CheckAllUniqueValue(cards) && lowCard.IntValue == 10 && highCard.IntValue == 14);
        }

        private bool CheckAllUniqueValue(List<Card> cards)
        {
            //Unique value of the object card, and must be five cards.
            return cards.GroupBy(c => c.IntValue).Distinct().Count() == 5;
        }
    }
}
