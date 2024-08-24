using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Configuration;
using DotNetEnv;
namespace habi_bot {
    class Program {
        private readonly DiscordSocketConfig config = new DiscordSocketConfig {
            GatewayIntents = GatewayIntents.DirectMessages | GatewayIntents.GuildMessages | GatewayIntents.Guilds | GatewayIntents.MessageContent
        };
        private DiscordSocketClient client; 
        private Timer timer;
        
        public async Task RunningBotAsync() {
            client = new DiscordSocketClient(config);
            client.Log += botLog;
            client.MessageReceived += messageReceived;
            client.Ready += async () => {
                createTimer();
            };
            
            Env.Load();
            string token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private Task botLog(LogMessage logMessage) {
            Console.Write(logMessage.ToString());
            return Task.CompletedTask;
        }

        private async Task readyAsync() {
            Console.Write("bot da san sang !");
            ulong channelId = 1116338091075051646;
            var channel = client.GetChannel(channelId) as IMessageChannel;

            if (channel != null) {
                await channel.SendMessageAsync("chu server da chet");
            } else {
                Console.Write("id kenh bi sai hoac khong tim thay");
            }
        }
        
        private void createTimer() {
            TimeSpan timeToSend = new TimeSpan(16, 0, 0);
            scheduleDailyMessage(timeToSend);
        }
        
        private async void dailyMessage(object state) {
            ulong channelId = 1116338091075051646;
            var channel = client.GetChannel(channelId) as IMessageChannel;
            
            if (channel != null) {
                await channel.SendMessageAsync("chu acc da chet <@576948115140247555>");
            } else {
                Console.Write("khong tim thay id channel");
            }
        }

        private void scheduleDailyMessage(TimeSpan timeToSend) {
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, timeToSend.Hours, timeToSend.Minutes, timeToSend.Seconds);

            if (now > firstRun) {
                firstRun.AddDays(1);
            }
            TimeSpan timeUntilTheFirstRun = firstRun - now;
            timer = new Timer(dailyMessage, null, timeUntilTheFirstRun, TimeSpan.FromDays(1));
        }
        
        private async Task messageReceived(SocketMessage message) {
            if(message.Author.IsBot) return;
            Console.WriteLine($"Received a message: {message.Content} from {message.Author.Username}");
            
            if (message.Author.IsBot) return;
            if (message.Channel is IDMChannel) {
                if (message.Content == "hello bot") {
                await message.Channel.SendMessageAsync("hello i'm your bot");
                }
            }
            if (message is not IUserMessage userMessage || message.Author.IsBot) return;
            if (message.Content == "hello") {
                await message.Channel.SendMessageAsync("lo con cac");
            }
            
        }

        static async Task Main(string[] arg) {
            var Program = new Program();
            await Program.RunningBotAsync();
        }
    }
}
