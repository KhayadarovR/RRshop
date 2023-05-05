using Microsoft.EntityFrameworkCore;
using RRshop.Data;
using RRshop.Models;
using System.Security.AccessControl;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;

namespace RRshop.Services
{
    public class TgBot: INotifyService
    {
        private readonly string token = "5814717472:AAG-PMenYnjT4QnmeMc0l2SDBm9IZ7CPb6D";
        
        readonly TelegramBotClient botClient;
        private List<long> salesListener = new();
        public string SecretKey { get; set; }

        public TgBot()
        {
            botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("BOT_TOKEN"));
            botClient.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);
            Console.WriteLine("TG BOT STARTED: " + botClient.BotId);
        }


        public async Task Notify(string text)
        {
            foreach (var chatId in salesListener)
            {
                await botClient.SendTextMessageAsync(chatId: chatId, text: 
                    "Новый заказ: " + DateTime.Now.ToString() +"\n" + text);
            }
            await botClient.SendTextMessageAsync(chatId: 1381378405, text: 
                "Новый заказ " + DateTime.Now.ToString() +"\n\n" + text);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;
            var chatId = message.Chat.Id;
            if (message.Text is not { } messageText)
                return;
            if (message.Text.Contains("/start"))
            {
                await botClient.SendTextMessageAsync(chatId: chatId, text: "Для получения информации о заказах -> \n" +
                                                                           "Введите ключ из админ панели в виде:\n" +
                                                                           "key xxxx");
            }
            if (message.Text == "key " + SecretKey)
            {
                AddListener(message.Chat.Id);
                Message sentMessage = await botClient.SendTextMessageAsync(chatId: chatId, 
                    text: "Отправка информации о заказах настроен на этот чат - " + chatId, cancellationToken: cancellationToken);
                return;
            }
            if (message.Text.Contains("key"))
            {
                await botClient.SendTextMessageAsync(chatId: chatId,
                    text: "Обновите админ панель и введите правильный ключ");
            }

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private void AddListener(long chatId)
        {
            if (!salesListener.Any(e => e == chatId))
            {
                salesListener.Add(chatId);
            }
        }
    }
}
