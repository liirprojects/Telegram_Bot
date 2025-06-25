namespace TelegramBot.Application.Interfaces.Commands;

public abstract class CommandHandler : ICommandHandler
{
    public abstract string Command { get; }

    public abstract Task HandleAsync(long chatId, string message, CancellationToken cancellationToken);

    protected readonly IMessageSender _messageSender;

    protected CommandHandler(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }
}