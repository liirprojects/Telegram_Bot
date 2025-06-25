using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Interfaces;

namespace TelegramBot.Infrastructure.Services; 

public class MessageService : IMessageService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<MessageService> _log;
    private readonly IEnumerable<ICommandHandler> _commandHandlers;

    public MessageService(
        ITelegramBotClient botClient, 
        ILogger<MessageService> log,
        IEnumerable<ICommandHandler> commandHandlers)
    {
        this._botClient = botClient;
        this._log = log;
        _commandHandlers = commandHandlers;
    }
    public async Task HandleAsync(string message, long chatId, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(message))
        {
            _log.LogWarning("Received empty message from chatId: {ChatId}", chatId);
            return;
        }

        // question
        string command = message.Trim().Split(' ', '\n')[0].ToLowerInvariant();

        var handler = _commandHandlers.FirstOrDefault(h => h.Command == command);

        if (handler is not null)
        {
            await handler.HandleAsync(chatId, message, cancellationToken);
        }
        else
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: "❓ Unknown command. Type /help to see available commands.",
                cancellationToken: cancellationToken);

            _log.LogWarning("Unknown command '{Command}' from chatId: {ChatId}", command, chatId);
        }
    }

    #region private handlers

    private Task OnStartAsync(long chatId, CancellationToken cancellationToken) =>
        _botClient.SendMessage(
            chatId: chatId,
            text: "Hello! I'm your bot 🤖. Type /help to find out what I can do!",
            parseMode: ParseMode.None,
            cancellationToken: cancellationToken);


    private Task OnHelpAsync(long chatId, CancellationToken cancellationToken) =>
                _botClient.SendMessage(
            chatId: chatId,
            text: "**I can help you with the following commands:**\n" +
                  "/start - Start the bot\n" +
                  "/help - Show this help message\n",
            parseMode: ParseMode.Markdown,
            cancellationToken: cancellationToken);

    private Task OnEchoAsync(string message, long chatId, CancellationToken cancellationToken)
    {
        var parts = message.Trim().Split(' ', 2);

        // Если пользователь ничего не написал после команды
        if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
        {
            return _botClient.SendMessage(
                chatId,
                "❗ Please, write the text after the command /echo.\nExample: /echo Good morning!",
                cancellationToken: cancellationToken);
        }

        // Извлекаем текст после команды
        var echoText = parts[1];

        return _botClient.SendMessage(
            chatId,
            $"🔁 {echoText}",
            cancellationToken: cancellationToken);
    }
       
    #endregion
}
