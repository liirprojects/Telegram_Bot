namespace TelegramBot.Application.Interfaces.Commands;

public abstract class CommandHandler : ICommandHandler
{
    public abstract string Command { get; }

    public abstract Task HandleAsync(long chatId, string message, CancellationToken cancellationToken);

    protected readonly IMessageService _messageService;

    protected CommandHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }
}