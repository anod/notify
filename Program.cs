using System.Threading;
using System.CommandLine;
using Windows.UI.Notifications;
using System.IO;
using System.Threading.Tasks;

namespace notify
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.WriteLine($"Culture: {System.Threading.Thread.CurrentThread.CurrentCulture}");
            
            var rootCommand = CreateOptionsCommand();
            var configuration = new CommandLineConfiguration(rootCommand);
            configuration.ThrowIfInvalid();

            rootCommand.SetHandler((string appId, string title, string body, FileInfo image) =>
                {
                    Show(appId, title, body, image?.FullName);
                    Thread.Sleep(300);
                },
                rootCommand.Options[0],
                rootCommand.Options[1],
                rootCommand.Options[2],
                rootCommand.Options[3]
            );

            await rootCommand.InvokeAsync(args);
        }

        private static RootCommand CreateOptionsCommand()
        {
            var appId = new Option<string>(
                    new string[] { "--app-id", "-a" },
                    description: "Application identifier"
                )
            {
                IsRequired = true
            };

            var title = new Option<string>(
                    new string[] { "--title", "-t" },
                    "Notification title"
                )
            {
                IsRequired = true
            };

            var body = new Option<string>(
                    new string[] { "--body", "-b" },
                    "Notification body"
                )
            {
                IsRequired = true
            };
            
            var icon = new Option<FileInfo>(
                    new string[] { "--image", "-i" },
                    "Notification image path"
            ).ExistingOnly();
            
            return new RootCommand("Notify")
            {
                appId,
                title,
                body,
                icon
            };
        }

        private static void Show(string appId, string title, string body, string imagePath)
        {
            var templateType = (imagePath == null) ? ToastTemplateType.ToastText02 : ToastTemplateType.ToastImageAndText02;
            var template = ToastNotificationManager.GetTemplateContent(templateType);

            if (imagePath != null)
            {
                var images = template.GetElementsByTagName("image");
                var src = template.CreateAttribute("src");
                src.Value = imagePath;
                images.Item(0).Attributes.SetNamedItem(src);
            }

            var textNodes = template.GetElementsByTagName("text");
            textNodes.Item(0).InnerText = title;
            textNodes.Item(1).InnerText = body;

            var notifier = ToastNotificationManager.CreateToastNotifier(appId);
            var notification = new ToastNotification(template);
            notifier.Show(notification);
        }
    }
}
