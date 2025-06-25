namespace TelegramBot.Application.Interfaces.Commands;

public class StartCommandHandler : CommandHandler
{
    public override string Command => "/start";

    public StartCommandHandler(IMessageSender sender) : base(sender) { }

    public override Task HandleAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        return _messageSender.SendTextAsync(chatId, "👋 Hello! I'm your bot.", cancellationToken);
    }
}