using System.Net.Sockets;
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
        Console.WriteLine(CapResponse);

        //Send the authentication message
        await writer.WriteLineAsync($"PASS {password}");
        await writer.WriteLineAsync($"NICK {nick}");
        writer.Flush();
    }

    async internal Task Start()
    {
        while (true)
        {
            string? line = await reader.ReadLineAsync();
            Console.WriteLine(line);
            if (line == null)
            {
                break;
            }

            if (line.StartsWith("PING"))
            {
                await writer.WriteLineAsync("PONG");
                writer.Flush();
            }

            if (line.StartsWith("004"))
            {
                connected.TrySetResult(0);
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
