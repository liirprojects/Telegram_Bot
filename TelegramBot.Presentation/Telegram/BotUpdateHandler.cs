using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Interfaces;

namespace TelegramBot.Presentation.Telegram;

public class BotUpdateHandler : IUpdateHandler
{
    private readonly IMessageService _messageService;

    public BotUpdateHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message!.Text is not null)
        {
            var message = update.Message.Text;
            var chatId = update.Message.Chat.Id;

            await _messageService.HandleAsync(message, chatId, cancellationToken);
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Telegram polling error: {exception.Message}");
        return Task.CompletedTask;
    }
}