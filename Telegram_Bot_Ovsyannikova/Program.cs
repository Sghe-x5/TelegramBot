using ChargesLibrary;
using FileOpsLibrary;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram_Bot;
using File = System.IO.File;
using Microsoft.Extensions.Logging;
using System.Reflection;
using BotLibrary;

namespace Telegram_Bot;

internal class Program
{
    //public static ILogger logger;

    static async Task Main(string[] args)
    {
        string projectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string parentDirectory = Directory.GetParent(projectDirectory).FullName;
        for (int i = 0; i < 3; i++)
        {
            parentDirectory = Directory.GetParent(parentDirectory).FullName;
        }
        string desiredDirectory = parentDirectory;

        string logFilePath = Path.Combine(desiredDirectory, "var/log.txt");
        Console.WriteLine(logFilePath);
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddFile(logFilePath).AddConsole());
        Bot.logger = factory.CreateLogger("Program");
        Bot.logger.LogInformation("Программа запущена");

        Bot bot = new Bot();
        Thread.Sleep(200); // Пауза для инициализации бота
        do
        {
            Update[] updates = await Catcher.TryGetNewUpdates(bot, bot.Token);
            if (updates != Array.Empty<Update>())
            {
                foreach (var up in updates)
                {
                    await Catcher.MakeAction(bot, up, bot.Token);
                }
            }
            Thread.Sleep(400); // Пауза перед следующим цикломц
        } while (true);
    }
}