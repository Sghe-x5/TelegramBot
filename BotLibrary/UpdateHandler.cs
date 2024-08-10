using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using File = Telegram.Bot.Types.File;
using Telegram_Bot;
using Microsoft.Extensions.Logging;

namespace BotLibrary;

public class UpdateHandler
{
    /// <summary>
    /// Массив команд, которые имеет бот.
    /// </summary>
    private string[] _commands = new string[]
    {
        "/start",
        "/help",
        "/upload_file",
        "/restart"
    };

    /// <summary>
    /// Реагирует на команду '/start'. Отправляет приветственное сообщение.
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    private async Task StartCommandHandlerAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
    {
        await botClient.SendStickerAsync(chatId, new InputFileId("CAACAgIAAxkBAAEENhZl_H4icY075vXLCxT790jV--YSZQACCBYAAnoOOEgjOv-ctXaamjQE"),
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(chatId, "Это бот, работающий  с *csv* и *json* файлами," + " Нажмите /help",
            parseMode: ParseMode.MarkdownV2, cancellationToken: cancellationToken);
        Bot.logger.LogInformation("Была принята команда /start");
    }

    /// <summary>
    /// Реагирует на команду '/help'. Отправляет сообщение с инструкцией.
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    private async Task HelpCommandHandlerAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(chatId, "Напишите /upload_file для начала работы с ботом\n" +
                                                     "Используйте /restart, если что-то пошло не по плану",
            cancellationToken: cancellationToken);
        Bot.logger.LogInformation("Была принята команда /help");
    }

    /// <summary>
    /// Реагирует на команду '/upload_file'. Отправляет сообщение с запросом на загрузку файла.
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    private async Task MakeActionCommandHandlerAsync(ITelegramBotClient botClient, ChatId chatId,
        CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(chatId, "Загрузить файл", cancellationToken: cancellationToken);
        Bot.logger.LogInformation("Была принята команда /upload_file");
    }

    /// <summary>
    /// Реагирует на команду '/restart'. Прекращает работу с файлом и переводит пользователя в исходное состояние.
    /// </summary>
    private async Task RestartCommandHandlerAsync(ITelegramBotClient botClient, ChatId chatId,
        CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(chatId,
            "Работа с файлом прекращена. Если хотите начать снова, используйте команду /upload_file",
            cancellationToken: cancellationToken);
        Bot.logger.LogInformation("Была принята команда /restart");
    }

    /// <summary>
    /// Отвечает на выбор пользователя между выборкой и сортировкой. Устанавливает состояние пользователя в зависимости от выбора.
    /// </summary>
    private async Task ActionChoiceAnswerAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, UserInfo user,
        CancellationToken cancellationToken)
    {
        user.State = string.Equals(callbackQuery.Data, "Выборка") ?
            UserInfo.UserStates.ChoosingAttributeForSelection : UserInfo.UserStates.ChoosingAttributeForSorting;
        long chatId = callbackQuery.From.Id;
        if (user.State == UserInfo.UserStates.ChoosingAttributeForSelection)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Выберите поле для выборки",
                cancellationToken: cancellationToken);
            await AskingSelectionParameterAsync(botClient, chatId, user, cancellationToken);
            Bot.logger.LogInformation("Пользователь выбирает поле для выборки");
        }
        else
        {
            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Выберите порядок для сортировки",
                cancellationToken: cancellationToken);
            await AskingSortingParameterAsync(botClient, chatId, user, cancellationToken);
            Bot.logger.LogInformation("Пользователь выбирает порядок для сортировки");
        }

        user.State = UserInfo.UserStates.WaitingForCallback;
    }

    /// <summary>
    /// Отвечает на выбор пользователя атрибута для выборки и запрашивает значение для выборки.
    /// </summary>
    private async Task SelectAttributeAnswerAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        UserInfo user, CancellationToken cancellationToken)
    {
        long chatId = callbackQuery.From.Id;
        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, $"Выборка будет сделана по параметру {callbackQuery.Data}",
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(chatId,
            $"Введите значение {callbackQuery.Data}, по которому будет сделана выборка");
        Bot.logger.LogInformation($"Пользователь выбирает значение {callbackQuery.Data} для выборки");
        user.State = UserInfo.UserStates.EnteringValueForSelection;
        
    }

    /// <summary>
    /// Отвечает на выбор пользователя нескольких атрибутов для выборки и запрашивает значения для выборки.
    /// </summary>
    private async Task SelectSomeAttributeAnswerAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        UserInfo user, CancellationToken cancellationToken)
    {
        long chatId = callbackQuery.From.Id;
        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, $"Выборка будет сделана по парметрам {callbackQuery.Data}",
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(chatId,
            $"Введите значения через *\\|* AdmArea, Latitude, Longitude, по которым будет сделана выборка",
            parseMode: ParseMode.MarkdownV2);
        Bot.logger.LogInformation($"Пользователь выбирает значения для выборки по параметрам {callbackQuery.Data}");
        user.State = UserInfo.UserStates.EnteringValueForSelection;
    }

    /// <summary>
    /// Отвечает на выбор пользователя метода сортировки.
    /// </summary>
    private async Task SortingAnswerAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancellationToken)
    {
        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id,
            $"Сортировка будет выполнена {callbackQuery.Data.ToLower()}",
            showAlert: true, cancellationToken: cancellationToken);
        Bot.logger.LogInformation($"Пользователь выбрал метод сортировки: {callbackQuery.Data.ToLower()}");
    }

    /// <summary>
    /// Запрашивает у пользователя выбор параметра для выборки.
    /// </summary>
    private async Task AskingSelectionParameterAsync(ITelegramBotClient botClient, ChatId chatId, UserInfo user,
        CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(
            new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "1",
                        "AdmArea"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        "2",
                        "District"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        "3",
                        "DifficultAdmArea"
                    ),
                }
            });
        await botClient.SendTextMessageAsync(chatId, "Выберите поле для выборки:\n" +
                                                     "1. Административный округ\n" +
                                                     "2. Район\n" +
                                                     "3. Административный округ и координаты", replyMarkup: keyboard,
            cancellationToken: cancellationToken);
        user.State = UserInfo.UserStates.WaitingForCallback;

        Bot.logger.LogInformation("Запрошен выбор параметра для выборки.");
    }

    /// <summary>
    /// Запрашивает у пользователя выбор параметра для сортировки.
    /// </summary>
    private async Task AskingSortingParameterAsync(ITelegramBotClient botClient, ChatId chatId, UserInfo user,
        CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(
            new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "1",
                        "В алфавитном порядке"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        "2",
                        "В обратном алфавитном порядке"
                    ),
                }
            });
        await botClient.SendTextMessageAsync(chatId, "Выберите как отсортировать административный округ:\n" +
                                                     "1. По алфавиту\n" +
                                                     "2. По алфавиту в обратном порядке\n",
            replyMarkup: keyboard, cancellationToken: cancellationToken);
        user.State = UserInfo.UserStates.WaitingForCallback;
        Bot.logger.LogInformation("Запрошен выбор параметра для сортировки.");
    }

    /// <summary>
    /// Свойство, которое показывает время последнего обновления.
    /// </summary>
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// Устанавливает время последнего обновления.
    /// </summary>
    public UpdateHandler()
    {
        LastUpdateTime = DateTime.UtcNow;
    }

    /// <summary>
    /// Обработчик команд. Отвечает за обработку команд пользователя.
    /// </summary>
    public async Task CommandHandlerAsync(ITelegramBotClient botClient, Update update, UserInfo user, CancellationToken cancellationToken)
    {
        MessageEntity[] entities = update.Message.Entities;
        if (entities is { Length: 1 } && entities[0].Type == MessageEntityType.BotCommand)
        {
            string command = update.Message.Text;
            if (_commands.Contains(command))
            {
                long chatId = update.Message.Chat.Id;
                switch (command)
                {
                    case "/start":
                        await StartCommandHandlerAsync(botClient, chatId, cancellationToken);
                        break;
                    case "/help":
                        await HelpCommandHandlerAsync(botClient, chatId, cancellationToken);
                        break;
                    case "/upload_file":
                        await MakeActionCommandHandlerAsync(botClient, chatId, cancellationToken);
                        user.State = UserInfo.UserStates.UploadingFile;
                        break;
                    case "/restart":
                        await RestartCommandHandlerAsync(botClient, chatId, cancellationToken);
                        user.State = UserInfo.UserStates.None;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Обработчик загрузки файла. Отвечает за обработку файла, загруженного пользователем.
    /// </summary>
    public async Task UploadingFileHandlerAsync(ITelegramBotClient botClient, Update update, UserInfo user,
        CancellationToken cancellationToken)
    {
        string fileName = update.Message.Document.FileName;
        Regex csv = new Regex(".csv$");
        Regex json = new Regex(".json$");
        if (csv.IsMatch(fileName) || json.IsMatch(fileName))
        {
            user.IsCsv = csv.IsMatch(fileName);
            char sep = Path.DirectorySeparatorChar;
            await using (Stream file = System.IO.File.Open($"..{sep}..{sep}..{sep}..{sep}receivedFiles{sep}{fileName}",
                             FileMode.OpenOrCreate))
            {
                await botClient.GetInfoAndDownloadFileAsync(update.Message.Document.FileId, file, cancellationToken);
                user.File = $"..{sep}..{sep}..{sep}..{sep}receivedFiles{sep}{fileName}";
            }
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Файл получен");
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(
                new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData(
                            "1",
                            "Выборка"
                        ),
                        InlineKeyboardButton.WithCallbackData(
                            "2",
                            "Сортировка"
                        ),
                    }
                });
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Выберите, что делать дальше:\n" +
                "1. Провести выборку по одному из полей\n" +
                "2. Отсортировать файл по одному из полей",
                replyMarkup: keyboard);
            user.State = UserInfo.UserStates.WaitingForCallback;
            Bot.logger.LogInformation("Обработчик загрузки файла: Файл успешно получен и обработан.");
            return;

        }
        await botClient.SendTextMessageAsync(update.Message.Chat.Id,
            "Расширение файла должно быть csv или json, попробуйте еще раз");
        Bot.logger.LogWarning("Обработчик загрузки файла: Неверное расширение файла. Ожидались файлы с расширением csv или json.");
    }


    /// <summary>
    /// Обработчик ответа на коллбэк. Отвечает за обработку ответа на коллбэк-запрос от пользователя.
    /// </summary>
    public async Task CallbackAnswerAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, UserInfo user,
        CancellationToken cancellationToken)
    {
        switch (callbackQuery.Data)
        {
            case "Выборка":
            case "Сортировка":
                await ActionChoiceAnswerAsync(botClient, callbackQuery, user, cancellationToken);
                break;
            case "AdmArea":
                user.TypeOfAction = 1;
                await SelectAttributeAnswerAsync(botClient, callbackQuery, user, cancellationToken);
                break;
            case "District":
                user.TypeOfAction = 2;
                await SelectAttributeAnswerAsync(botClient, callbackQuery, user, cancellationToken);
                break;
            case "DifficultAdmArea":
                user.TypeOfAction = 3;
                await SelectSomeAttributeAnswerAsync(botClient, callbackQuery, user, cancellationToken);
                break;
            case "В алфавитном порядке":
                user.TypeOfAction = 4;
                await SortingAnswerAsync(botClient, callbackQuery, cancellationToken);
                break;
            case "В обратном алфавитном порядке":
                user.TypeOfAction = 5;
                await SortingAnswerAsync(botClient, callbackQuery, cancellationToken);
                break;
        }
        Bot.logger.LogInformation($"Обработчик ответа на коллбэк: Получен ответ на коллбэк-запрос от пользователя: {callbackQuery.Data}");
    }

    /// <summary>
    /// Отправляет файл. Отвечает за отправку файла пользователю.
    /// </summary>
    public async Task SendFileAsync(ITelegramBotClient botClient, ChatId chatId, Stream fileToSend,
        CancellationToken cancellationToken, string? name = null)
    {
        InputFile file = new InputFileStream(fileToSend, name);
        await botClient.SendDocumentAsync(chatId, file, cancellationToken: cancellationToken);
        fileToSend.Close();
        Bot.logger.LogInformation($"Отправка файла: Файл успешно отправлен пользователю в чат с ID: {chatId}");
    }
}