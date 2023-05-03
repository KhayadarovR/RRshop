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
    public class TgBot
    {
        private readonly string token = "5814717472:AAG-PMenYnjT4QnmeMc0l2SDBm9IZ7CPb6D";
        readonly TelegramBotClient botClient;
        private List<long> salesListener = new();
        public string SecretKey { private get; set; }

        public TgBot()
        {
            botClient = new TelegramBotClient("5814797478:AAG-PMenYnjT4QnmeMc0l2SDBm9IZ7CPb6Q");
            botClient.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);
        }


        public async Task SendNotification(string text)
        {
            foreach (var chatId in salesListener)
            {
                Message sentMessage = await botClient.SendTextMessageAsync(chatId: chatId, text: DateTime.Now.ToString() + 
                    "\nНовый заказ:\n" + text);
            }
            Console.WriteLine("test");
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;
            var chatId = message.Chat.Id;
            if (message.Text is not { } messageText)
                return;
            if (message.Text == "key " + SecretKey)
            {
                AddListener(message.Chat.Id);
                Message sentMessage = await botClient.SendTextMessageAsync(chatId: chatId, 
                    text: "Отправка информации о заказах настроен на этот чат - " + chatId, cancellationToken: cancellationToken);
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
