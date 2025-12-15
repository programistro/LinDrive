// using System.IO;
// using Avalonia.Controls;
// using LinDrive.Application.Services.IO.Services;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace LinDrive.Desktop;
//
// public partial class MainWindow : Window
// {
//     private readonly FileWatcherService _watcher;
//     
//     public MainWindow()
//     {
//         _watcher = App.ServiceProvider.GetRequiredService<FileWatcherService>();
//         
//         InitializeComponent();
//     }
// }