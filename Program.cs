using System.CommandLine;
namespace TBot;
static class Program
{
    static async Task Main(string[] args)
    {
        Option<string> userOption = new(name: "--user", description: "specifies the username of your bot")
        {
            IsRequired = true
        };
        userOption.AddAlias("-u");

        Option<string> tokenOption = new("--token", "specifies the token to authenticate ")
        {
            IsRequired = true
        };
        tokenOption.AddAlias("-t");

        Option<string> channelOption = new("--channel", "specifies the channel to join");
        channelOption.AddAlias("-c");

        RootCommand Command = new("TwitchBot"){
            userOption,
            tokenOption,
            channelOption
        };

        string botUsername = string.Empty;
        string password = string.Empty;
        string channel = string.Empty;

        Command.SetHandler(
            (user, token, channelToJoin) =>
            {
                botUsername = user;
                password = token;
                channel = channelToJoin;
            }, userOption, tokenOption, channelOption
        );

        await Command.InvokeAsync(args);

        // Even when the Options are requiered the code below gets executed.
        // even when you just call "-h" so we have to make sure we break here,
        // when they are not set.
        if (
            string.IsNullOrEmpty(botUsername) &&
            string.IsNullOrEmpty(password)
            ) { return; }

        Bot bot = new(botUsername, password);
        await bot.Connect();
        //We could .SafeFireAndForget() these two calls if we want to
        if (channel is not null)
            bot.Join(channel);
        // await Bot.SendMessage("mettcon", "Hey my bot has started up");
        await bot.Start();
    }
}
