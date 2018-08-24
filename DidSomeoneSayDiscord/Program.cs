

namespace DidSomeoneSayDiscord
{
    class Program
    {
        static void Main(string[] args)
        {

            System.Console.WriteLine("+++++ Meme online +++++");

            MenuHelper();

            System.Console.ReadLine();
        }

        static void MenuHelper()
        {
            System.Console.WriteLine("Enter twitch channel name: ");

            string twitch_target = System.Console.ReadLine();

            MemeBot bot = new MemeBot(twitch_target);

            System.Console.WriteLine($"Enter 1 to connect to {twitch_target}.\nEnter 0 to enter a new channel");
            string connect_var = System.Console.ReadLine();

            if (connect_var == "1")
            {
                
                bot.ConnectClient();
            }
            if (connect_var == "0")
            {
                MenuHelper(bot);
            }
        }

        static void MenuHelper(MemeBot bot)
        {
            System.Console.WriteLine("Enter twitch channel name: ");

            string twitch_target = System.Console.ReadLine();

            System.Console.WriteLine($"Enter 1 to connect to {twitch_target}.\nEnter 0 to enter a new channel");
            string connect_var = System.Console.ReadLine();

            if (connect_var == "1")
            {
                bot.ConnectClient();
            }
            if (connect_var == "0")
            {
                MenuHelper(bot);
            }
        }

    }
}
