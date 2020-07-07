using System;
using System.Collections.Generic;
using System.Text;

namespace PokerGame
{
    public class PokerPlayer
    {
        public string PlayerName { get; set; }
        public List<CardSet> PlayerDeck { get; set; }

        public PokerPlayer(string name)
        {
            this.PlayerName = name;
        }
    }
    public class CardSet 
    { 
        public List<Card> Cards { set; get; }
        public int Rank { set; get; }
        public int HighCard { set; get; }
        public CardSet()
        {
            this.Cards = new List<Card>();
        }
    }

    public class Card
    { 
        public string Value { set; get; }
        public string Suit { set; get; }
        public int IntValue { set; get; }

        public Card(string value, string suit, int intValue)
        {
            this.Value = value;
            this.Suit = suit;
            this.IntValue = intValue;
        }
    }
}
