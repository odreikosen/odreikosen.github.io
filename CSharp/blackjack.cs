using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp1
{
    public class BlackJackGame
    {
        private static string blank_card= " _________\n" +
                                            "|---------|\n" +
                                            "|---------|\n" +
                                            "|---------|\n" +
                                            "|---------|\n" +
                                            "|---------|\n" +
                                            "|_________|\n";
        private Deck deck;
        private Player p1;
        static void Main(string[] args)
        {
            Console.Clear();
            BlackJackGame app = new BlackJackGame();
            Console.Write("Please enter your name.\n");

            string name = Console.ReadLine();

            app.p1 = new Player(name);

            for (int i=0;i<4;i++)
                //In case the user cannot enter the correct input for the menu for whatever reason the application will exit
            {
                   Console.WriteLine("\nWelcome to Blackjack, "+app.p1.name+". How would you like to play?\n" +
                    "Enter '1' for playing on console\n" +
                    "\n"
                    );
                var game_mode = Console.ReadLine();
                if (game_mode.Equals("1"))
                {
                    app.deck = new Deck();
                    app.SinglePlayerBlackJack();
                    break;
                }
                else if (game_mode.ToLower().Equals("quit"))
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("I'm sorry I could not understand that input. Could you please re-enter either '1' or '2'\n" +
                        "If you would like to quit simply enter 'quit'");

                }
            }
            Environment.Exit(0);



        }

        private void SinglePlayerBlackJack()
        {
            Console.Clear();
            while(true)
            {
                //In case the user cannot enter the input correctly the application will exit
                Console.Write("You have $" + p1.current_money + ". How much would you like to wager\n");
                var wager_input = Console.ReadLine();
                if (!Int32.TryParse(wager_input, out var wager))
                {
                    Console.WriteLine("I'm sorry, I couldn't quite understand that. Could you try reentering only an integer");
                }
                else if (wager > p1.current_money)
                {
                    Console.WriteLine("I'm, sorry you don't have enough money to make that bid. You currently have " + p1.current_money +
                        "dollars.");
                }
                else
                {
                    p1.current_money = p1.current_money - wager;
                    p1.current_wager = wager;
                    break;
                }
            }
            Console.Clear();
            //Checks to see if the deck needs to be reshuffled
            if (deck.plays >= 5||deck.plays==0)
            {
                deck.Shuffle();
            }
            deck.plays++;

            p1.hand.hand.Add(DealCard());
            p1.hand.hand[0].DisplayCard();
            p1.hand.hand.Add(DealCard());
            p1.hand.hand[1].DisplayCard();
            Console.WriteLine("These are your cards. Press Enter when ready to see dealers cards.");
            Console.ReadKey();
            Player dealer = new Player("dealer");
            dealer.hand.hand.Add(DealCard());
            dealer.hand.hand[0].DisplayCard();
            dealer.hand.hand.Add(DealCard());
            Console.Write(blank_card);
            Boolean stay = true;
            //Player Turn
            while (stay)
            {
                var value_list = p1.hand.TotalValue();
                string hand_value = value_list[0].ToString();
                if (value_list.Count > 1)
                {
                    foreach (int value in value_list)
                    {
                        hand_value = hand_value + " or " + value;
                        if (value == 21)
                        {
                            p1.current_money = p1.current_money + (int)Math.Floor((p1.current_wager * 2.5)+.5);
                            Console.WriteLine("Congratulations!! You hit blackjack!");
                            Console.WriteLine("You have won $" + (int)Math.Floor((p1.current_wager*2.4)+.5) + " and now have $" + p1.current_money + ".");
                            PlayerContinueMenu();
                        }
                    }
                }
                else
                {
                    if (value_list[0] == 21)
                    {
                        //Straight blackjack has higher payouts, rounding up for not integers to preserve chip integrity
                        p1.current_money = p1.current_money + (int)Math.Floor((p1.current_wager * 2.5)+.5);
                        Console.WriteLine("Congratulations!! You hit blackjack!");
                        Console.WriteLine("You have won $" + (int)Math.Floor((p1.current_wager * 2.5) + .5) + " and now have $" + p1.current_money + ".");
                        PlayerContinueMenu();
                    }
                }

                Console.WriteLine("------------------");
                Console.WriteLine("You are currently at " + hand_value + ". Would you like to Hit or Stay?");

                var decision = Console.ReadLine();
                if (decision.ToLower().Equals("stay"))
                {
                    stay = false;
                }
                else if (decision.ToLower().Equals("hit"))
                {
                    p1.hand.hand.Add(DealCard());
                    p1.hand.hand.Last().DisplayCard();
                    if (p1.hand.TotalValue().Min() > 21)
                    {
                        Console.WriteLine("\nYou have busted with a total value of " + p1.hand.TotalValue().Min() + " :(");

                        Console.WriteLine("You lost $" + p1.current_wager + " and now have $" + p1.current_money + ".");
                        PlayerContinueMenu();
                    }
                }
                else
                {
                    Console.WriteLine("I'm sorry I couldn't quite catch what you were trying to say. Could you type either 'hit' or 'stay'");
                }
            }
            //Calculate Player Score
            var values = p1.hand.TotalValue();
            var player_max = p1.PlayerMaxScore();
            int dealer_max = dealer.PlayerMaxScore();
            Console.Clear();
            Console.WriteLine("You finished with a score of " + player_max + ". Lets see how the dealer does.");
            Thread.Sleep(3000);
            Console.WriteLine("-------------------------");
            Console.WriteLine("\n The Dealer's Hand");
            dealer.hand.hand[0].DisplayCard();
            dealer.hand.hand[1].DisplayCard();
            while (true)
            {
                var dealer_value = dealer.hand.TotalValue();
                if (dealer_value.Min() > 21)
                {
                    p1.current_money = p1.current_money + (p1.current_wager * 2);
                    Console.WriteLine("The dealer has busted. You have won!!");
                    Console.WriteLine("You have won $" + p1.current_wager + " and now have $" + p1.current_money + ".");
                    PlayerContinueMenu();

                }
                else
                {
                    dealer_max = dealer.PlayerMaxScore();
                    if (dealer_max >=17)
                    {
                        break;
                    }
                    dealer.hand.hand.Add(DealCard());
                    dealer.hand.hand.Last().DisplayCard();
                }
            }

                    Console.WriteLine("----------------------");
                    Console.WriteLine("You scored " + player_max + " versus the dealer's " + dealer_max + ".");
                    if (player_max > dealer_max)
                    {
                        p1.current_money = p1.current_money + (p1.current_wager * 2);
                        Console.WriteLine("You beat the dealer!");
                        Console.WriteLine("You have won $" + p1.current_wager + " and now have $" + p1.current_money + ".");
                    }
                    else if (player_max == dealer_max)
                    {
                        p1.current_money = p1.current_money + p1.current_wager;
                        Console.WriteLine("You tied the dealer.");
                        Console.WriteLine("You gained your initial bid of $" + p1.current_wager + " back and now have $" + p1.current_money + ".");
                    }
                    else
                    {
                        Console.WriteLine("You lost to the dealer :(");
                        Console.WriteLine("You lost $" + p1.current_wager + " and now have $" + p1.current_money);
                    }
                    PlayerContinueMenu();



        }

        private Card DealCard()
        {
            Console.Write("Dealing");
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(750);
                Console.Write(".");
            }
            Console.WriteLine();
            Random random = new Random();
            int random_number = random.Next(0, deck.deck.Count);
            //Using time dependent value for seed
            Card card= deck.deck[random_number];
            deck.deck.RemoveAt(random_number);
            return card;


        }
        public void PlayerContinueMenu()
        {
            if (p1.current_money > 0)
            {
                Console.WriteLine("Would you like to continue?");
                for (int i = 0; i < 3; i++)
                {
                    var continuing = Console.ReadLine();
                    if (continuing.ToLower().Equals("yes"))
                    {
                        p1.PlayerReset();

                        this.SinglePlayerBlackJack();
                    }
                    else if (continuing.ToLower().Equals("no"))
                    {
                        Console.WriteLine("\nThank you for playing. You won $" + p1.current_money + "!");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("I'm sorry I couldn't quite understand that. Could you try typing in 'yes' or 'no'?");
                    }
                }
            }
            else
            {
                Console.WriteLine("You are all out of money. Thanks for playing!");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
        }
        public class Player
        {
            //Player data containing name and List of Cards player is currently holding (hand), current wager, and current amount of moeny
            public string name;
            public int max;
            public int current_wager;
            public int current_money;
            public Hand hand;
            public Player(string name)
            {
                hand = new Hand();
                this.name = name;
                this.current_money = 100;
            }
        public void PlayerReset()
            {
                this.current_wager = 0;
                this.hand = new Hand();
            }
            public int PlayerMaxScore()
            {
                max = 0;
                foreach (int value in hand.TotalValue())
                {
                    if (value < 22 && value > max)
                    {
                        max = value;
                    }
                }
                return max;
            }

        }
        public class Hand
        {
            public IList<Card> hand;
            public Hand()
            {
                this.hand = new List<Card>();
            }


            public List<int> TotalValue()
            {
                BinaryTree valuetree = new BinaryTree(0);

                foreach (Card card in hand)
                {
                    if (card.number == 0)
                    {
                        valuetree.AddAce();
                    }
                    else
                    {

                        if (card.number >= 10)
                        {
                            valuetree.AddAll(10);
                        }
                        else
                        {
                            valuetree.AddAll(card.number + 1);
                        }
                    }

                }
                return valuetree.GetAllValues();
            }
            }

        public class Card
        {
            //Holds suit and number data
            public Suit suit;
            public int number;
            public Card(Suit suit, int number)
            {
                this.suit = suit;
                this.number = number;
            }
            public void DisplayCard()
                //Displays readable card
            {
                Console.WriteLine(suit.GetCards()[number]);
            }
        }
        public class Deck
        {
            //Holds 52 card objects that represent each card in the deck and number of hands to count if it should be reshuffled
            public List<Card> deck;
            public int plays;
            public Deck()
            {
                deck = new List<Card>();

            }
            public void Shuffle()
            {
                plays = 0;
                deck.Clear();
                Suit heart = new Heart();
                Suit diamond = new Diamond();
                Suit spade = new Spade();
                Suit clubs = new Clubs();
                for (int i = 0; i < 13; i++)
                {
                    deck.Add(new Card(heart, i));
                }
                for (int i = 0; i < 13; i++)
                {
                    deck.Add(new Card(diamond, i));
                }
                for (int i = 0; i < 13; i++)
                {
                    deck.Add(new Card(spade, i));
                }
                for (int i = 0; i < 13; i++)
                {
                    deck.Add(new Card(clubs, i));
                }

                Console.Write("Shuffling");
                for (int i = 0; i < 3; i++)
                {
                    Thread.Sleep(750);
                    Console.Write(".");
                }
                Console.WriteLine();
            }
        }
        public class BinaryTree
        {
            public Node root;
            public BinaryTree(int root)
            {
                this.root = new Node(root);
            }
            public void AddAce()
            {
                root.InsertAce();
            }
            public List<int> GetAllValues()
            {
               List<int> values = new List<int>();
                root.TraverseBottomChildren(values);
                return values;
            }
            public void AddAll(int data)
            {
                root.AddAll(data);
                return;
            }



         public class Node
            {
                public int data;
                public Node left;
                public Node right;
                public Node(int data)
                {
                    this.data = data;
                }
                public void InsertAce()
                {
                    if (left != null)
                    {
                        left.InsertAce();
                    }
                    if (right != null)
                    {
                        right.InsertAce();
                    }
                    if (left == null && right == null)
                    {
                        left = new Node(data + 1);
                        right = new Node(data + 11);
                    }
                }
                public void TraverseBottomChildren(List<int> values)
                    //All current possible hand values are stored at the bottom of the BST
                {
                    if (left != null)
                    {
                        left.TraverseBottomChildren(values);
                    }
                    if (right != null)
                    {
                        right.TraverseBottomChildren(values);
                    }
                    if(left==null && right == null)
                    {
                        values.Add(data);
                    }
                }
                public void AddAll(int data)
                {
                    this.data = this.data + data;
                        if(left != null)
                    {
                        left.AddAll(data);
                    }
                    if (right != null)
                    {
                        right.AddAll(data);
                    }
                }
            }
        }
        public interface Suit
        {
             string[] GetCards();
        }
        public class Spade:Suit
        {
            //class to hold all spade cards
            public string[] cards;
            public static string two_spade = " _________\n" +
                                        "|2        |\n" +
                                        "|         |\n" +
                                        "|  Spade  |\n" +
                                        "|         |\n" +
                                        "|        2|\n" +
                                        "|_________|";
            public static string three_spade = " _________\n" +
                                            "|3        |\n" +
                                            "|         |\n" +
                                            "|  Spade  |\n" +
                                            "|         |\n" +
                                            "|        3|\n" +
                                            "|_________|";
            public static string four_spade = " _________\n" +
                                            "|4        |\n" +
                                            "|         |\n" +
                                            "|  Spade  |\n" +
                                            "|         |\n" +
                                            "|        4|\n" +
                                            "|_________|";
            public static string five_spade = " _________\n" +
                                            "|5        |\n" +
                                            "|         |\n" +
                                            "|  Spade  |\n" +
                                            "|         |\n" +
                                            "|        5|\n" +
                                            "|_________|";
            public static string six_spade = " _________\n" +
                                            "|6        |\n" +
                                            "|         |\n" +
                                            "|  Spade  |\n" +
                                            "|         |\n" +
                                            "|        6|\n" +
                                            "|_________|";
            public static string seven_spade = " _________\n" +
                                            "|7        |\n" +
                                            "|         |\n" +
                                            "|  Spade  |\n" +
                                            "|         |\n" +
                                            "|        7|\n" +
                                            "|_________|";

            public static string eight_spade = " _________\n" +
                                             "|8        |\n" +
                                             "|         |\n" +
                                             "|  Spade  |\n" +
                                             "|         |\n" +
                                             "|        8|\n" +
                                             "|_________|";
            public static string nine_spade = " _________\n" +
                                            "|9        |\n" +
                                            "|         |\n" +
                                            "|  Spade  |\n" +
                                            "|         |\n" +
                                            "|        9|\n" +
                                            "|_________|";
            public static string ten_spade = " _________\n" +
                                            "|10       |\n" +
                                            "|         |\n" +
                                            "|  Spade  |\n" +
                                            "|         |\n" +
                                            "|       10|\n" +
                                            "|_________|";
            public static string jack_spade = " _________\n" +
                                            "|J        |\n" +
                                            "|         |\n" +
                                            "|  Spade  |\n" +
                                            "|         |\n" +
                                            "|        J|\n" +
                                            "|_________|";
            public static string queen_spade = " _________\n" +
                                            "|Q        |\n" +
                                            "|         |\n" +
                                            "|  Spade  |\n" +
                                            "|         |\n" +
                                            "|        Q|\n" +
                                            "|_________|";
            public static string king_spade = " _________\n" +
                                            "|K        |\n" +
                                            "|         |\n" +
                                            "|  Spade  |\n" +
                                            "|         |\n" +
                                            "|        K|\n" +
                                            "|_________|";
            public static string ace_spade = " _________\n" +
                                            "|A        |\n" +
                                            "|         |\n" +
                                            "|  Spade  |\n" +
                                            "|         |\n" +
                                            "|        A|\n" +
                                            "|_________|";


            public Spade()
            {
                cards = new string[] { ace_spade,two_spade, three_spade, four_spade, five_spade, six_spade,
                    seven_spade, eight_spade, nine_spade, ten_spade, jack_spade, queen_spade, king_spade };
            }
          public  string[] GetCards()
            {
                return cards;
            }

        }
        public class Diamond :Suit
        {
            //Holds Diamond cards
            public string[] cards;

            public static string two_diamond= " _________\n" +
                                            "|2        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        2|\n" +
                                            "|_________|";
            public static string three_diamond= " _________\n" +
                                            "|3        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        3|\n" +
                                            "|_________|";
            public static string four_diamond= " _________\n" +
                                            "|4        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        4|\n" +
                                            "|_________|";
            public static string five_diamond= " _________\n" +
                                            "|5        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        5|\n" +
                                            "|_________|";
            public static string six_diamond= " _________\n" +
                                            "|6        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        6|\n" +
                                            "|_________|";
            public static string seven_diamond= " _________\n" +
                                            "|7        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        7|\n" +
                                            "|_________|";
            public static string eight_diamond= " _________\n" +
                                            "|8        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        8|\n" +
                                            "|_________|";
            public static string nine_diamond= " _________\n" +
                                            "|9        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        9|\n" +
                                            "|_________|";
            public static string ten_diamond= " _________\n" +
                                            "|10       |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|       10|\n" +
                                            "|_________|";
            public static string jack_diamond= " _________\n" +
                                            "|J        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        J|\n" +
                                            "|_________|";
            public static string queen_diamond= " _________\n" +
                                            "|Q        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        Q|\n" +
                                            "|_________|";
            public static string king_diamond= " _________\n" +
                                            "|K        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        K|\n" +
                                            "|_________|";
            public static string ace_diamond= " _________\n" +
                                            "|A        |\n" +
                                            "|         |\n" +
                                            "| Diamond |\n" +
                                            "|         |\n" +
                                            "|        A|\n" +
                                            "|_________|";





            public Diamond()
            {
                cards = new string[] { ace_diamond,two_diamond, three_diamond, four_diamond, five_diamond, six_diamond,
           seven_diamond, eight_diamond, nine_diamond, ten_diamond, jack_diamond, queen_diamond, king_diamond };

            }
            public string[] GetCards()
            {
                return cards;
            }
        }
        public class Heart: Suit
        {
            //HOlds heart cards
            public string[] cards;
            public static string two_heart= " _________\n" +
                                            "|2        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        2|\n" +
                                            "|_________|";
            public static string three_heart= " _________\n" +
                                            "|3        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        3|\n" +
                                            "|_________|";
            public static string four_heart= " _________\n" +
                                            "|4        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        4|\n" +
                                            "|_________|";
            public static string five_heart= " _________\n" +
                                            "|5        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        5|\n" +
                                            "|_________|";
            public static string six_heart= " _________\n" +
                                            "|6        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        6|\n" +
                                            "|_________|";
            public static string seven_heart= " _________\n" +
                                            "|7        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        7|\n" +
                                            "|_________|";
            public static string eight_heart= " _________\n" +
                                            "|8        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        8|\n" +
                                            "|_________|";
            public static string nine_heart= " _________\n" +
                                            "|9        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        9|\n" +
                                            "|_________|";
            public static string ten_heart= " _________\n" +
                                            "|10       |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|       10|\n" +
                                            "|_________|";
            public static string jack_heart= " _________\n" +
                                            "|J        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        J|\n" +
                                            "|_________|";
            public static string queen_heart= " _________\n" +
                                            "|Q        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        Q|\n" +
                                            "|_________|";
            public static string king_heart= " _________\n" +
                                            "|K        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        K|\n" +
                                            "|_________|";
            public static string ace_heart= " _________\n" +
                                            "|A        |\n" +
                                            "|         |\n" +
                                            "|  Heart  |\n" +
                                            "|         |\n" +
                                            "|        A|\n" +
                                            "|_________|";

            public Heart()
            {
                cards = new string[] { ace_heart,two_heart, three_heart, four_heart, five_heart, six_heart, seven_heart,
                    eight_heart, nine_heart, ten_heart, jack_heart, queen_heart, king_heart };
            }
            public string[] GetCards()
            {
                return cards;
            }

        }
        public class Clubs:Suit
        {
            //Holds clubs cards
            public string[] cards;
        public static string two_clubs=    " _________\n" +
                                            "|2        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        2|\n" +
                                            "|_________|";
            public static string three_clubs= " _________\n" +
                                            "|3        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        3|\n" +
                                            "|_________|";
            public static string four_clubs= " _________\n" +
                                            "|4        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        4|\n" +
                                            "|_________|";
            public static string five_clubs= " _________\n" +
                                            "|5        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        5|\n" +
                                            "|_________|";
            public static string six_clubs= " _________\n" +
                                            "|6        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        6|\n" +
                                            "|_________|";
            public static string seven_clubs= " _________\n" +
                                            "|7        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        7|\n" +
                                            "|_________|";
            public static string eight_clubs= " _________\n" +
                                            "|8        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        8|\n" +
                                            "|_________|";
            public static string nine_clubs= " _________\n" +
                                            "|9        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        9|\n" +
                                            "|_________|";
            public static string ten_clubs= " _________\n" +
                                            "|10       |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|       10|\n" +
                                            "|_________|";
            public static string jack_clubs= " _________\n" +
                                            "|J        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        J|\n" +
                                            "|_________|";
            public static string queen_clubs= " _________\n" +
                                            "|Q        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        Q|\n" +
                                            "|_________|";
            public static string king_clubs= " _________\n" +
                                            "|K        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        K|\n" +
                                            "|_________|";
            public static string ace_clubs= " _________\n" +
                                            "|A        |\n" +
                                            "|         |\n" +
                                            "|  Clubs  |\n" +
                                            "|         |\n" +
                                            "|        A|\n" +
                                            "|_________|";

            public Clubs()
            {
                cards = new string[] { ace_clubs,two_clubs, three_clubs, four_clubs, five_clubs, six_clubs, seven_clubs, eight_clubs,
                    nine_clubs, ten_clubs, jack_clubs, queen_clubs, king_clubs };
            }
            public string[] GetCards()
            {
                return cards;
            }
        }

    }
}
