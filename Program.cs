namespace TBot;
sealed class Main
{
    async static void Main(string botUsername, string password)
    {
        Bot bot = new(password, botUsername);
        bot.Start();
        //We could .SafeFireAndForget() these two calls if we want to
        bot.Join("mettcon");
        // await Bot.SendMessage("mettcon", "Hey my bot has started up");
    }
}
