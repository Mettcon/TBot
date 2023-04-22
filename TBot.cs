using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

// should this be a singleton?
namespace TBot;
sealed class Bot
{
    // Should I read this via config File?
    // probably runtime? maybe compile time?

    // I stick to unencrypted until I figure out why the cert gets rejected
    // ssl Port is 6697

    const int PORT = 6667;
    const string HOST = "irc.twitch.tv";
    StreamReader? reader;
    StreamWriter? writer;
    readonly string nick;
    readonly string password;
    TaskCompletionSource<int> connected = new();

    public Bot(string nick, string password)
    {
        this.nick = nick;
        this.password = password;
    }

    async internal Task Connect()
    {
        //? Maybe put the connection in the constructor?
        TcpClient client = new();
        await client.ConnectAsync(HOST, PORT);
        var stream = client.GetStream();
        reader = new(stream);
        writer = new(stream) { NewLine = "\r\n", AutoFlush = true };

        await writer.WriteLineAsync("CAP REQ :twitch.tv/commands twitch.tv/tags twitch.tv/membership");

        var CapResponse = await reader.ReadLineAsync();

        if (CapResponse != ":tmi.twitch.tv CAP * ACK :twitch.tv/commands twitch.tv/tags twitch.tv/membership")
            throw new Exception("Could not request capabilites");

        //Send the authentication message
        await writer.WriteLineAsync($"PASS {password}");
        await writer.WriteLineAsync($"NICK {nick}");
        writer.Flush();
    }

    async internal Task Start()
    {
        while (true)
        {
            string? message = await reader.ReadLineAsync();
            foreach (var entry in message.Split("\r\n"))
            {
                Console.WriteLine(entry);

                ParseMessage(entry);

            }
        }
    }

    internal async void ParseMessage(string line)
    {
        string[] message = line.Split(' ');

        if (message[0].StartsWith("PING"))
        {
            await writer.WriteLineAsync("PONG " + message[1]);
        }

        else if (message[0].StartsWith('@'))
        {
            switch (message[2])
            {
                case "PRIVMSG":
                    await writer.WriteLineAsync("PRIVMSG #mettcon :thx").ConfigureAwait(false);
                    break;
            }
        }
        
    }
    internal void Join(string channel)
    {
        writer.WriteLine($"JOIN #{channel}");
        writer.Flush();
    }
}
