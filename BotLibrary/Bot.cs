using System.Text.RegularExpressions;
using BotLibrary;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using File = Telegram.Bot.Types.File;
using Microsoft.Extensions.Logging;

namespace Telegram_Bot;

/// <summary>
/// Класс, содержащий всю логику бота.
/// </summary>
public class Bot
{
    public static ILogger logger;

    private CancellationTokenSource _cts;

    public UpdateHandler Updater { get; init; }
    public Dictionary<long, UserInfo> Chats { get; }

    public CancellationToken Token => _cts.Token;

    public TelegramBotClient BotClient { get; }

    private int _off = 0;

    /// <summary>
    /// Инициализирует новый экземпляр класса Bot.
    /// </summary>
    public Bot()
    {
        BotClient = new TelegramBotClient("6952478553:AAHnN7LAy3clRd-WakY0qjTvB7hirOcKQXg");
        _cts = new CancellationTokenSource();
        Chats = new Dictionary<long, UserInfo>();
        Updater = new UpdateHandler();
    }

    /// <summary>
    /// Получает новые обновления.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Массив новых обновлений.</returns>
    public async Task<Update[]> GetNewUpdatesAsync(CancellationToken cancellationToken)
    {
        Update[] allUpdates = await BotClient.GetUpdatesAsync(offset: _off, cancellationToken: cancellationToken);
        List<Update> newUpdates = new List<Update>();
        foreach (Update update in allUpdates)
        {
            if (update.Message != null && update.Message.Date > Updater.LastUpdateTime)
            {
                newUpdates.Add(update);
                if (!Chats.Keys.Contains(update.Message.Chat.Id))
                {
                    Chats.Add(update.Message.Chat.Id, new UserInfo());
                }
            }
            else if (update.CallbackQuery != null)
            {
                if (Chats.Keys.Count != 0)
                {
                    newUpdates.Add(update);
                }
            }
        }

        if (allUpdates.Length > 0)
        {
            if (allUpdates[^1].Message != null)
            {
                Updater.LastUpdateTime = allUpdates[^1].Message.Date;
            }
            _off = allUpdates[^1].Id;
        }
        return newUpdates.ToArray();
    }


}