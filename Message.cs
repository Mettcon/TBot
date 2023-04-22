using System.Text.RegularExpressions;
namespace TBot;
sealed partial class RegX
{
    [GeneratedRegex("badge-info=(?<badgeInfo>.*);badges=(?<badges>.*);client-nonce=(?<clientNonce>.*);color=(?<color>.*);display-name=(?<displayName>.*);emotes=(?<emotes>.*);first-msg=(?<firstMsg>.*);flags=(?<flags>.*);id=(?<id>.*);mod=(?<mod>.*);returning-chatter=(?<returningChatter>.*);room-id=(?<roomID>.*);subscriber=(?<subscriber>.*);tmi-sent-ts=(?<timestamp>.*);turbo=(?<turbo>.*);user-id=(?<userID>.*);user-type=(?<userType>.*)", RegexOptions.Compiled)]
    internal static partial Regex messageRegex();
}
class Message
{
    // @badge-info=;;;;;;;;;;;;;;;;
    string badgeInfo = string.Empty;

    // badges=moderator/1,premium/1
    string badges = string.Empty;

    // client-nonce=5a5ceba61f337e1b730d49ed72104070
    string clientNonce = string.Empty;

    // color=#00FF7F
    string color = string.Empty;

    // display-name=0SkillbutMoney
    string displayName = string.Empty;

    // emotes
    string emotes = string.Empty;

    // first-msg=0
    string firstMsg = string.Empty;

    // flags=;
    string flags = string.Empty;

    // id=
    string id = string.Empty;

    //mod=1
    string mod = string.Empty;

    // returning-chatter=0
    string returningChatter = string.Empty;

    // room-id=53567829
    string roomID = string.Empty;

    // subscriber=0
    string subscriber = string.Empty;

    // tmi-sent-ts=1682117300403
    string timestamp = string.Empty;

    // turbo=0
    string turbo = string.Empty;

    // user-id=63352705
    string userID = string.Empty;

    //user-type=mod
    string userType = string.Empty;

    public Message(string message)
    {
        if (message.StartsWith('@'))
        {
            var match = RegX.messageRegex().Match(message);
            if (match.Success)
            {
                badgeInfo = match.Groups["badgeInfo"].Value;
                badges = match.Groups["badges"].Value;
                clientNonce = match.Groups["clientNonce"].Value;
                color = match.Groups["color"].Value;
                displayName = match.Groups["displayName"].Value;
                emotes = match.Groups["emotes"].Value;
                firstMsg = match.Groups["firstMsg"].Value;
                flags = match.Groups["flags"].Value;
                id = match.Groups["id"].Value;
                mod = match.Groups["mod"].Value;
                returningChatter = match.Groups["returningChatter"].Value;
                roomID = match.Groups["roomID"].Value;
                subscriber = match.Groups["subscriber"].Value;
                timestamp = match.Groups["timestamp"].Value;
                turbo = match.Groups["turbo"].Value;
                userID = match.Groups["userID"].Value;
                userType = match.Groups["userType"].Value;
            }
        }
    }
}