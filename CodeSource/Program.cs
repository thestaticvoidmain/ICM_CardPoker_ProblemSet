using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PokerGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Game Start");

            if (args.Length != 1)
            {
                Console.WriteLine("Please provide the file path.");
                return;
            }

            //Pick up the file, and load data to object.
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]);
            PokerPlayer p1 = new PokerPlayer("Player 1");
            PokerPlayer p2 = new PokerPlayer("Player 2");
            TextImporter.Import(filePath, out List<CardSet> s1, out List<CardSet> s2);
            p1.PlayerDeck = s1;
            p2.PlayerDeck = s2;

            //Determine the winner based on the poker rules.
            PokerRuleEngine re = new PokerRuleEngine();
            int p1Points = 0;
            int p2Points = 0;

            //Stream to file for confirming each round.
            string resultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "result_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
            StreamWriter sw = new StreamWriter(resultPath, true);

            for (int i = 0; i < p1.PlayerDeck.Count; i++)
            {
                //Debugging the problem set for each round.
                StringBuilder p1Cards = new StringBuilder();
                foreach (Card c in p1.PlayerDeck[i].Cards)
                {
                    p1Cards.Append(c.Value + c.Suit + " ");
                }

                StringBuilder p2Cards = new StringBuilder();
                foreach (Card c in p2.PlayerDeck[i].Cards)
                {
                    p2Cards.Append(c.Value + c.Suit + " ");
                }

                //Check result for each round.
                int result = re.DetermineWinner(p1.PlayerDeck[i], p2.PlayerDeck[i]);
                var x = result == 1 ? p1Points++ : p2Points++;

                sw.WriteLine(p1Cards.ToString() + p2Cards.ToString() + "= W" + result);
                
            }

            sw.Close();

            //Output
            Console.WriteLine("{0}: {1}", p1.PlayerName, p1Points);
            Console.WriteLine("{0}: {1}", p2.PlayerName, p2Points);

        }
    }
}
