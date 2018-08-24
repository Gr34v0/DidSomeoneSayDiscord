using System;
using System.IO;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace DidSomeoneSayDiscord
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("+++++ Meme online +++++");

            MenuHelper();

            Console.ReadLine();
        }

        static void MenuHelper()
        {
            Console.WriteLine("Enter twitch channel name: ");

            string twitch_target = Console.ReadLine();

            Bot bot = new Bot(twitch_target);

            Console.WriteLine($"Enter 1 to connect to {twitch_target}.\nEnter 0 to enter a new channel");
            string connect_var = Console.ReadLine();

            if (connect_var == "1")
            {
                
                bot.ConnectClient();
            }
            if (connect_var == "0")
            {
                MenuHelper(bot);
            }
        }

        static void MenuHelper(Bot bot)
        {
            Console.WriteLine("Enter twitch channel name: ");

            string twitch_target = Console.ReadLine();

            Console.WriteLine($"Enter 1 to connect to {twitch_target}.\nEnter 0 to enter a new channel");
            string connect_var = Console.ReadLine();

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

    class Bot
    {

        static readonly string cred_path = Path.Combine(Directory.GetCurrentDirectory(), "bot.creds"); //Used for deployment
        //static readonly string cred_path = Path.Combine(Directory.GetCurrentDirectory(), "../../../bot.creds"); //Used for running in IDE

        readonly string[] file_lines = System.IO.File.ReadAllLines(cred_path);

        string twitch_username{ get;set;}
        string access_token { get;set;}

        TwitchClient client;

        public Bot(string twitch_target)
        {
            foreach (string line in file_lines)
            {
                if (line.Contains("twitch_username"))
                {
                    string[] line_split = line.Split(":");
                    twitch_username = line_split[1];
                }
                else if (line.Contains("access_token"))
                {
                    string[] line_split = line.Split(":");
                    access_token = line_split[1];
                }
            }


            ConnectionCredentials credentials = new ConnectionCredentials(twitch_username, access_token);

            client = new TwitchClient();
            client.Initialize(credentials, twitch_target );

            client.OnJoinedChannel += onJoinedChannel;
            client.OnMessageReceived += onMessageReceived;
            client.OnConnected += Client_OnConnected;
            client.OnDisconnected += Client_OnDisconnect;
        }

        public void ConnectClient()
        {
            Console.WriteLine("+++++ Meme connected +++++");
            client.Connect();
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"+++++ Connected to {e.AutoJoinChannel} +++++");
        }

        private void Client_OnDisconnect(object sender, OnDisconnectedArgs e)
        {
            Console.WriteLine($"+++++ Disconnected fron Channel +++++");
        }

        private void onJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Joined chat channel");
            client.SendMessage(e.Channel, "+++++ Meme Activated: If you say it, I will come +++++");
        }

        private void onMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            string message = "Did someone say Discord?? Come join our community here - https://discord.gg/jm8nw7R";

            if (e.ChatMessage.Username.Contains("quackbot28"))
            {
                Console.WriteLine($"Injested invalid message from {e.ChatMessage.Username}: {e.ChatMessage.Message}");
            }
            else if ((e.ChatMessage.Message.ToLower().Contains("discord")) && !e.ChatMessage.Message.Contains("!discord") && !e.ChatMessage.Message.ToLower().Contains("didsomeonesaydiscord"))
            {
                Console.WriteLine($"+++++ Sent message: {message} +++++");
                client.SendMessage(e.ChatMessage.Channel, message);
            }
            else
            {
                Console.WriteLine($"Injested invalid message from {e.ChatMessage.Username}: {e.ChatMessage.Message}");
            }
        }
    }
}
