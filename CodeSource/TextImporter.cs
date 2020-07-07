using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PokerGame
{
    public static class TextImporter
    {
        public static bool Import(string filePath, out List<CardSet> s1, out List<CardSet> s2)
        {
            bool EoF = false;
            string status = String.Empty;
            int lineNum = 0;

            s1 = new List<CardSet>();
            s2 = new List<CardSet>();
            
            try
            {
                FileInfo finfo = new FileInfo(filePath);
                using (StreamReader sr = finfo.OpenText())
                {
                    while (!EoF)
                    {
                        string dataLine = sr.ReadLine();
                        EoF = string.IsNullOrEmpty(dataLine);
                        if (EoF) break;

                        lineNum++;
                        bool ok = Validate(dataLine, out status);
                        if (ok)
                        {
                            //Populate data to the players.
                            CardSet set1 = new CardSet();
                            CardSet set2 = new CardSet();
                            
                            ParseData(dataLine, out set1, out set2);
                            s1.Add(set1);
                            s2.Add(set2);
                        }
                        else
                        {
                            Console.WriteLine(string.Format("Error in file {0} at line {1}.", finfo.Name, lineNum));
                        }
                    }

                    if (EoF)
                    {
                        Console.WriteLine("File {0} loaded.", filePath);
                        Console.WriteLine();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception error when trying to load the file at: {0}.", filePath);
                return false;
            }
        }

        public static bool Validate(string dataline, out string status)
        {
            //No need to validate and categorize error status.
            status = string.Empty;
            return true;
        }
        public static void ParseData(string dataLine, out CardSet set1, out CardSet set2)
        {
            set1 = new CardSet();
            set2 = new CardSet();

            var cards = dataLine.Split(new char[0]).ToList();
            List<string> p1Cards = cards.GetRange(0, 5);
            List<string> p2Cards = cards.GetRange(5, 5);

            for (int i = 0; i < p1Cards.Count; i++)
            {
                //Add the card object to each player.
                string val1 = p1Cards[i].Substring(0, 1);
                string val2 = p2Cards[i].Substring(0, 1);
                set1.Cards.Add(new Card(val1, p1Cards[i].Substring(1, 1), SetCardValue(val1)));
                set2.Cards.Add(new Card(val2, p2Cards[i].Substring(1, 1), SetCardValue(val2)));
            }
        }

        private static int SetCardValue(string card)
        {
            int value;
            bool isNumeric = int.TryParse(card, out value);

            if (!isNumeric)
            {
                switch (card)
                {
                    case "T":
                        value = 10;
                        break;
                    case "J":
                        value = 11;
                        break;
                    case "Q":
                        value = 12;
                        break;
                    case "K":
                        value = 13;
                        break;
                    case "A":
                        value = 14;
                        break;
                }
            }

            return value;
        }
    }
}
