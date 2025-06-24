namespace TelegramBot.Application.Interfaces;

public interface IMessageService
{
    Task HandleAsync(string message, long chatID, CancellationToken cancellationToken);
}