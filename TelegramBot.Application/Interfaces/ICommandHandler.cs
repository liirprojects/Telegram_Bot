namespace TelegramBot.Application.Interfaces;


public interface ICommandHandler
{
    string Command { get; }
    Task HandleAsync(long chatId, string message, CancellationToken cancellationToken);
}