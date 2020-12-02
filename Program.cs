using System;
using System.Threading;
using System.CommandLine;
using Windows.UI.Notifications;
using System.CommandLine.Invocation;
using System.IO;

namespace notify
{
    class Program
    {
        static int Main(string[] args)
        {
           var rootCommand = new RootCommand("Notify")
            {
                new Option<string>(
                    new string[] { "--app-id", "-a" },
                    description: "Application identifier"
                ) {
                    IsRequired = true
                },
                new Option<string>(
                    new string[] { "--title", "-t" },
                    "Notification title"
                ) {
                    IsRequired = true
                },
                new Option<string>(
                    new string[] { "--body", "-b" },
                    "Notification body"
                ) {
                    IsRequired = true
                },
                new Option<FileInfo>(
                    new string[] { "--image", "-i" },
                    "Notification image path"
                ) {
                    Argument = new Argument<FileInfo>().ExistingOnly()
                }
            };

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create(
                (string appId,string title,string body, FileInfo image) =>
            {
                Show(appId, title, body, image?.FullName);
                Thread.Sleep(300);
            });

            return rootCommand.InvokeAsync(args).Result;
        }

        private static void Show(string appId, string title, string body, string imagePath)
        {
            
            var templateType = (imagePath == null) ? ToastTemplateType.ToastText02 : ToastTemplateType.ToastImageAndText02;
            var template = ToastNotificationManager.GetTemplateContent(templateType);

            if (imagePath != null) {
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
