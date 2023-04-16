using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

sealed class Bot
{
    // Should I read this @ compile Team via File?
    // 
    const int PORT = 6697;
    const string HOST = "irc.twitch.tv";
    StreamReader? reader;
    StreamWriter? writer;
    readonly string nick;
    readonly string password;
    readonly TaskCompletionSource<int> connected = new();

    public Bot(string nick, string password)
    {
        this.nick = nick;
        this.password = password;
    }

    async internal void Start()
    {
        //? Maybe put the connection in the constructor?
        TcpClient client = new();
        await client.ConnectAsync(HOST, PORT);

        SslStream stream = new(
            client.GetStream(),
            false,
            ValidateServerCertificate,
            null);

        await stream.AuthenticateAsClientAsync(HOST);
        reader = new(stream);
        writer = new(stream) { NewLine = "\r\n", AutoFlush = true };

        await writer.WriteLineAsync("CAP REQ :twitch.tv/commands twitch.tv/tags twitch.tv/membership");

        //Send the authentication message
        // send login information
        await writer.WriteLineAsync($"PASS {password}");
        await writer.WriteLineAsync($"NICK {nick}");
        writer.Flush();

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

        async internal void Join(string channel)
        {
            await writer?.WriteLineAsync($"JOIN #{channel}");
            writer.Flush();
        }

        //Outside of start we need to define ValidateServerCertificate
        bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors
            )
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }
    }
}
