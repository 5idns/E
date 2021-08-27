using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace E.Bots.Telegram
{
    public class HandleUpdateService
    {
        //private readonly ITelegramBotClient _botClient;
        private readonly ILogger<HandleUpdateService> _logger;

        public HandleUpdateService(ILogger<HandleUpdateService> logger)
        {
            //_botClient = botClient;
            _logger = logger;
        }

        public async Task EchoAsync(ITelegramBotClient botClient,Update update)
        {
            Task handler;
            
            if (botClient == null)
            {
                throw new NullReferenceException("ITelegramBotClient 为空");
            }
            if (update == null)
            {
                throw new NullReferenceException("update 消息为空");
            }
            Console.WriteLine($"收到消息{update.Type}");
            switch (update.Type)
            {
                // case UpdateType.Unknown:
                // case UpdateType.ChannelPost:
                // case UpdateType.EditedChannelPost:
                // case UpdateType.ShippingQuery:
                // case UpdateType.PreCheckoutQuery:
                // case UpdateType.Poll:
                case UpdateType.Message:
                    handler = BotOnMessageReceived(botClient,update.Message);
                    break;
                case UpdateType.EditedMessage:
                    handler = BotOnMessageReceived(botClient, update.EditedMessage);
                    break;
                case UpdateType.CallbackQuery:
                    handler = BotOnCallbackQueryReceived(botClient, update.CallbackQuery);
                    break;
                case UpdateType.InlineQuery:
                    handler = BotOnInlineQueryReceived(botClient, update.InlineQuery);
                    break;
                case UpdateType.ChosenInlineResult:
                    handler = BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult);
                    break;
                default:
                    handler = UnknownUpdateHandlerAsync(botClient, update);
                    break;
            }

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        private async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            _logger.LogInformation($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
                return;

            Task<Message> action;
            switch (message.Text.Split(' ').First())
            {
                case "/inline":
                    action = SendInlineKeyboard(botClient, message);
                    break;
                case "/keyboard":
                    action = SendReplyKeyboard(botClient, message);
                    break;
                case "/remove":
                    action = RemoveKeyboard(botClient, message);
                    break;
                case "/photo":
                    action = SendFile(botClient, message);
                    break;
                case "/request":
                    action = RequestContactAndLocation(botClient, message);
                    break;
                default:
                    action = Usage(botClient, message);
                    break;
            }

            var sentMessage = await action;
            _logger.LogInformation($"The message was sent with id: {sentMessage.MessageId}");

            // Send inline keyboard
            // You can process responses in BotOnCallbackQueryReceived handler
            async Task<Message> SendInlineKeyboard(ITelegramBotClient bot, Message msg)
            {
                await bot.SendChatActionAsync(msg.Chat.Id, ChatAction.Typing);

                // Simulate longer running task
                await Task.Delay(500);

                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    },
                });

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Choose",
                                                      replyMarkup: inlineKeyboard);
            }

            async Task<Message> SendReplyKeyboard(ITelegramBotClient bot, Message msg)
            {
                var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new[]
                    {
                        new KeyboardButton[] { "1.1", "1.2" },
                        new KeyboardButton[] { "2.1", "2.2" },
                    })
                {
                    ResizeKeyboard = true

                };

                return await bot.SendTextMessageAsync(chatId: msg.Chat.Id,
                                                      text: "Choose",
                                                      replyMarkup: replyKeyboardMarkup);
            }

            async Task<Message> RemoveKeyboard(ITelegramBotClient bot, Message msg)
            {
                return await bot.SendTextMessageAsync(chatId: msg.Chat.Id,
                                                      text: "Removing keyboard",
                                                      replyMarkup: new ReplyKeyboardRemove());
            }

            async Task<Message> SendFile(ITelegramBotClient bot, Message msg)
            {
                await bot.SendChatActionAsync(msg.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"Files/tux.png";
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

                    return await bot.SendPhotoAsync(msg.Chat.Id,
                        photo: new InputOnlineFile(fileStream, fileName),
                        caption: "Nice Picture");
                }
            }

            async Task<Message> RequestContactAndLocation(ITelegramBotClient bot, Message msg)
            {
                var requestReplyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                });

                return await bot.SendTextMessageAsync(chatId: msg.Chat.Id,
                                                      text: "Who or Where are you?",
                                                      replyMarkup: requestReplyKeyboard);
            }

            async Task<Message> Usage(ITelegramBotClient bot, Message msg)
            {
                const string usage = "Usage:\n" +
                                     "/inline   - send inline keyboard\n" +
                                     "/keyboard - send custom keyboard\n" +
                                     "/remove   - remove custom keyboard\n" +
                                     "/photo    - send a photo\n" +
                                     "/request  - request location or contact";

                return await bot.SendTextMessageAsync(chatId: msg.Chat.Id,
                                                      text: usage,
                                                      replyMarkup: new ReplyKeyboardRemove());
            }
        }

        // Process Inline Keyboard callback data
        private async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }

        #region Inline Mode

        private async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            _logger.LogInformation($"Received inline query from: {inlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
                // displayed result
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };

            await botClient.AnswerInlineQueryAsync(inlineQuery.Id,
                                                    results: results,
                                                    isPersonal: true,
                                                    cacheTime: 0);
        }

        private Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            _logger.LogInformation($"Received inline result: {chosenInlineResult.ResultId}");
            return Task.CompletedTask;
        }

        #endregion

        private Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            _logger.LogInformation($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

        public Task HandleErrorAsync(Exception exception)
        {
            string errorMessage;
            switch (exception)
            {
                case ApiRequestException apiRequestException:
                    errorMessage =
                        $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}";
                    break;
                default:
                    errorMessage = exception.ToString();
                    break;
            }

            _logger.LogInformation(errorMessage);
            return Task.CompletedTask;
        }

    }
}
