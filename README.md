Notify - Show notification from command line
====

.NET 5.0 + C#/WinRT

![Screenshot](https://github.com/anod/notify/raw/master/screenshot.png)

Usage
====

`dotnet .\bin\Release\net5.0-windows10.0.19041.0\win10-x64\notify.dll`

```
notify:
  Notify

Usage:
  notify [options]

Options:
  -a, --app-id <app-id> (REQUIRED)    Application identifier
  -t, --title <title> (REQUIRED)      Notification title
  -b, --body <body> (REQUIRED)        Notification body
  -i, --image <image>                 Notification image path
  --version                           Show version information
  -?, -h, --help                      Show help and usage information
  ```
