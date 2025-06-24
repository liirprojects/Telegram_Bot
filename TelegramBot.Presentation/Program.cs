using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Interfaces;
using TelegramBot.Infrastructure.Services;
using TelegramBot.Presentation.Telegram;

    var builder = Host.CreateDefaultBuilder(args);

    builder.ConfigureServices((context, services) =>
    {
        // токен на телеграм бота
        string botToken = "7880450189:AAEryAUFjGPAB3-n_xVE2JswtDP4V_aqIwA";

        // Регистрация Telegram-клиента
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

        // Подключаем бизнес-логику
        services.AddSingleton<IMessageService, MessageService>();

        // Подключаем обработчик обновлений
        services.AddSingleton<IUpdateHandler, BotUpdateHandler>();
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

    Console.WriteLine("✅ Бот запущен. Нажми Ctrl+C чтобы остановить.");
    await Task.Delay(-1);
