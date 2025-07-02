namespace TelegramBot.Application.Entities;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid(); // Уникальный ID

    public long ChatId { get; set; } // ID пользователя Telegram

    public string Title { get; set; } = null!; // Название задачи

    public string? Description { get; set; } // Описание (опционально)

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; } // Дедлайн

    public bool IsCompleted { get; set; } = false; // Статус задачи
}
