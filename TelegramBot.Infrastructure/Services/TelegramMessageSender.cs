using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using TelegramBot.Application.Interfaces;

namespace TelegramBot.Infrastructure.Services;
public class TelegramMessageSender : IMessageSender
{
    private readonly ITelegramBotClient _botClient;

    public TelegramMessageSender(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public Task SendTextAsync(long chatId, string text, CancellationToken token)
    {
        return _botClient.SendMessage(
            chatId,
            text,
            parseMode: ParseMode.Markdown,
            cancellationToken: token);
    }
}