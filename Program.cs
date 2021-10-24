using System;
using System.IO;

namespace ArenaFighter
{
    class Program
    {
        public static void Main()
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }
        public static Character player = new();
        private static Character opponent = new();

        public static Character Opponent { get => opponent; set => opponent = value; }

        // Huvudmeny
        public static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("--- Arena Fighter --- \n");
            Console.WriteLine("1) Spela Arena Fighter");
            Console.WriteLine("0) Avsluta");
            Console.Write("\r\nVälj funktion: ");

            switch (Console.ReadLine())
            {
                case "0":
                    Environment.Exit(0);
                    return false;
                case "1":
                    CreateFighter();
                    return true;
                default:
                    return true;
            }
        }

        public static void CreateFighter()
        {
            player.Health = GetRandomNumber();
            player.Strength = GetRandomNumber();
            Console.Clear();
            Console.WriteLine("\n --- Arena Fighter --");
            Console.WriteLine("\nSkriv in namnet på din karaktär och tryck enter");
            player.Name = Console.ReadLine();
            Opponent.Name = CreateOpponent();
            Console.WriteLine($"\nDin karaktär heter {player.Name} \noch besitter följande egenskaper: \nStyrka: {player.Strength} \nHälsa: {player.Health} ");
            Console.WriteLine("\n\nTryck på 'p' för att spela");
            switch (Console.ReadLine())
            {
                case "p":
                    Battle.Play(player.Name, Opponent.Name);
                    break;
                default:
                    MainMenu();
                    break;
            }
        }
        

        // Metod för att generera ett slupmässigt nummer mellan 1-10
        public static int GetRandomNumber()
        {
            Random rand = new();
            int x = rand.Next(1, 10);
            int randomNum = x;
            return randomNum;
        }

        public static string CreateOpponent()
        {
            Opponent.Health = GetRandomNumber();
            Opponent.Strength = GetRandomNumber();

            // Skapar array med namn på motståndare
            string[] opponents = new string[] { "Evil Queen", "Queen of Hearts", "Captain Hook", "---- Maleficent ----", "Cruella de Vil", "--- Scar ---", "Doctor Facilier", "--- Hades ---", "--- Jafar ---" };

            // Skapar ett Random object  
            Random rand = new();

            // Genererar ett random index   
            int index = rand.Next(opponents.Length);
            Opponent.Name = opponents[index];
            Console.WriteLine($"\nDin motståndare heter {Opponent.Name} \noch besitter följande egenskaper: \nStyrka: {Opponent.Strength} \nHälsa: {Opponent.Health} ");
            return Opponent.Name;
        }
    }


    public class Character
    {
        public int Health { get; set; }
        public int Strength { get; set; }
        public string Name { get; set; }

    }

    public class Battle
    {
        private static string player;
        private static string opponent;

        public static string Player { get => player; set => player = value; }
        public static string Opponent { get => opponent; set => opponent = value; }

        public static void Play(string usr, string cpu)
        {
            Player = usr;
            Opponent = cpu;
            Round.Game(Player, Opponent);
        }

        public static void Log(string text)
        {
            string saveToLog = text;

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = Path.Combine(docPath, "log.txt");

            // This text is added when the file is created.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                string createText = "Arena Fighter - Game results" + Environment.NewLine + Environment.NewLine + "Datum:\t\t Spelare:\t Motståndare:\t\t Vinnare:\t Antal ronder:\t\t ";
                File.WriteAllText(path, createText);
            }
            string appendText = saveToLog + Environment.NewLine;
            File.AppendAllText(path, appendText);
        }
    }

    public class Round
    {
        private static int counter;
        private static int reward;

        public static int Counter { get => counter; set => counter = value; }
        public static int Reward { get => reward; set => reward = value; }

        public static int RollDice()
        {
            Random dice = new();
            int x = dice.Next(1, 6);
            int randomDice = x;
            return randomDice;
        }

        public static void Game(string p, string o)
        {
            bool playerStatus;
            Console.Clear();
            string player = p;
            string opponent = o;
            int resultPlayer = RollDice();
            int resultOpponent = RollDice();
            DateTime today = DateTime.Today;
            string strDate = today.ToString("yyyy-MM-dd");
            string winner;
            if (resultPlayer > resultOpponent)
            {
                winner = player;
                playerStatus = true;
                Counter++;
            }
            else if (resultPlayer == resultOpponent)
            {
                winner = "Draw";
                playerStatus = true;
            }
            else
            {
                winner = opponent;
                playerStatus = false;
            }
            Console.WriteLine($"\n{player} slog {resultPlayer} med tärningen\n\n{opponent} slog {resultOpponent} med tärningen\nVinnare är: {winner} ");
            string txtToSave = ($"\n{strDate}\t {player}\t {opponent}\t\t {winner}  \t{Counter}");
            Battle.Log(txtToSave);
            PlayAgain(player, playerStatus);
            Console.ReadKey();
        }

        public static void PlayAgain(string name, bool status)
        {

            bool lifeStatus = status;
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = Path.Combine(docPath, "log.txt");
            Console.WriteLine("\n\nPlay again or retire? y/r");
            string usrInput = Console.ReadLine();
            if (usrInput == "y")
            {
                string newOpponent = Program.CreateOpponent();
                Round.Game(name, newOpponent);
            }
            else if (usrInput == "r" && lifeStatus)
            {
                Console.WriteLine("\n");
                Reward += Counter;
                Console.WriteLine($"\n You have choosed to retire, this earns you: {Reward} gamecoin");

                // Opens file with results.
                string readText = File.ReadAllText(path);
                Console.WriteLine(readText);
                Console.WriteLine("\nWould you like to use the reward to upgrade character? y/n");
                string input = Console.ReadLine();
                if (input == "y")
                {
                    Shop();
                }
                else
                {
                    Console.WriteLine("\nTryck på enter för att återgå till huvudmenyn");
                    Console.ReadKey();
                    Program.MainMenu();
                }
            }
            else
            {
                // Display stats.
                string readText = File.ReadAllText(path);
                Console.WriteLine(readText);
                Console.WriteLine($"\n Du har tjänat in totalt {Reward} gamecoins");
                Console.WriteLine("\nTryck på enter för att återgå till huvudmenyn");
                Console.ReadKey();
                Program.MainMenu();
            }
        }

        private static void Shop()
        {
            bool showShop = true;
            while (showShop)
            {
                showShop = ShopMenu();
            }

            bool ShopMenu()
            {
                Console.Clear();
                Console.WriteLine("--- SHOP --- \n");
                Console.WriteLine("1) Styrka 10 gamecoins");
                Console.WriteLine("2) Hälsa 1 gamecoin");
                Console.WriteLine("0) Avsluta");
                Console.Write("\r\nVälj uppgradering: ");
                Console.WriteLine($"\n Du har {Reward} gamecoins att spendera");

                switch (Console.ReadLine())
                {
                    case "0":
                        Program.MainMenu();
                        return false;
                    case "1":
                        BuyStrength();
                        return true;
                    case "2":
                        BuyHealth();
                        return true;
                    default:
                        return true;
                }
            }
            void BuyStrength()
            {
                int toSpend = Reward;
                if (toSpend >= 10)
                {
                    Reward -= 10;
                    Console.Write("\r\nStyrka är uppgraderad ");
                }
                else
                {
                    Console.WriteLine($"\n--Medges EJ-- \nDu har endast {Reward} gamecoins kvar att spendera, spela mer för o vinna fler");
                }

                Console.WriteLine("\nTryck på enter för att återgå till huvudmenyn");
                Console.ReadKey();
                Program.MainMenu();
            }
            void BuyHealth()
            {
                int budget = Reward;
                if (budget >= 1)
                {
                    Reward = budget - 1;
                    Console.Write("\r\nHälsa är uppgraderad ");
                }
                Console.WriteLine($"\n Du har {Reward} gamecoins kvar att spendera");
                Console.WriteLine("\nTryck på enter för att återgå till huvudmenyn");
                Console.ReadKey();
                Program.MainMenu();

            }
        }

    }
}