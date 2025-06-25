using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Application.Interfaces.Commands;

public class HelpCommandHandler : CommandHandler
{
    public override string Command => "/help";

    public HelpCommandHandler(IMessageSender sender) : base(sender) { }

    public override Task HandleAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        var helpText = "**I can help you with the following commands:**\n" +
                       "/start - Start the bot\n" +
                       "/help - Show this help message\n";

        return _messageSender.SendTextAsync(chatId, helpText, cancellationToken, ParseMode.Markdown);
    }
}
