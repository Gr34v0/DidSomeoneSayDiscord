using System.IO;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace DidSomeoneSayDiscord
{
    class MemeBot
    {
        static readonly string cred_path = Path.Combine(Directory.GetCurrentDirectory(), "bot.creds"); //Used for deployment
        //static readonly string cred_path = Path.Combine(Directory.GetCurrentDirectory(), "../../../bot.creds"); //Used for running in IDE

        readonly string[] file_lines = System.IO.File.ReadAllLines(cred_path);

        string twitch_username { get; set; }
        string access_token { get; set; }

        TwitchClient client;

        public MemeBot(string twitch_target)
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
            client.Initialize(credentials, twitch_target);

            client.OnJoinedChannel += onJoinedChannel;
            client.OnMessageReceived += onMessageReceived;
            client.OnConnected += Client_OnConnected;
            client.OnDisconnected += Client_OnDisconnect;
        }

        public void ConnectClient()
        {
            System.Console.WriteLine("+++++ Meme Online +++++");
            client.Connect();
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            System.Console.WriteLine($"+++++ Connected to {e.AutoJoinChannel} +++++");
        }

        private void Client_OnDisconnect(object sender, OnDisconnectedArgs e)
        {
            System.Console.WriteLine($"+++++ Disconnected fron Channel +++++");
        }

        private void onJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            System.Console.WriteLine("Joined chat channel");
            client.SendMessage(e.Channel, "+++++ Meme Activated: If you say it, I will come +++++");
        }

        private void onMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            string message = "Did someone say Discord?? Come join our community here - https://discord.gg/jm8nw7R";

            if (e.ChatMessage.Username.Contains("quackbot28"))
            {
                System.Console.WriteLine($"Injested invalid message from {e.ChatMessage.Username}: {e.ChatMessage.Message}");
            }
            else if ((e.ChatMessage.Message.ToLower().Contains("discord")) && !e.ChatMessage.Message.Contains("!discord") && !e.ChatMessage.Message.ToLower().Contains("didsomeonesaydiscord"))
            {
                System.Console.WriteLine($"+++++ Sent message: \"{message}\" in response to {e.ChatMessage.Message} +++++");
                client.SendMessage(e.ChatMessage.Channel, message);
            }
            else
            {
                System.Console.WriteLine($"Injested invalid message from {e.ChatMessage.Username}: {e.ChatMessage.Message}");
            }
        }
    }
}
