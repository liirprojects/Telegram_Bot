using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Interfaces;
using TelegramBot.Application.Interfaces.Commands;
using TelegramBot.Infrastructure.Data;
using TelegramBot.Infrastructure.Services;
using TelegramBot.Presentation.Telegram;


var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
    {
        // Add Entity Framework Core with SQL Server support
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        // tocken for a telegram bot 
        var botToken = context.Configuration["Telegram:BotToken"]
                ?? throw new InvalidOperationException("Bot token is missing in environment variables!");

        // Telegram-client registration
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

        // Add services for the application
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IUpdateHandler, BotUpdateHandler>();

        // Register TelegramMessageSender as an implementation of IMessageSender.
        // This allows teams to send messages without knowing about the Telegram API directly.
        services.AddScoped<IMessageSender, TelegramMessageSender>();

        // Register new command for a command list
        services.AddScoped<ICommandHandler, StartCommandHandler>();
        services.AddScoped<ICommandHandler, HelpCommandHandler>();
    });

    var app = builder.Build();

    var botClient = app.Services.GetRequiredService<ITelegramBotClient>();
    var updateHandler = app.Services.GetRequiredService<IUpdateHandler>();
    var cancellationToken = new CancellationTokenSource().Token;

    botClient.StartReceiving(
        updateHandler: updateHandler,
        receiverOptions: new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // слушаем все типы обновлений
        },
        cancellationToken: cancellationToken
    );

    Console.WriteLine("✅ The bot is running. Press Ctrl+C to stop.");
    await Task.Delay(-1);