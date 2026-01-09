using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LinDrive.Desktop.ViewModels;
using LinDrive.Desktop.Views;
using Microsoft.Extensions.DependencyInjection;
using SQLitePCL;

namespace LinDrive.Desktop;

public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        raw.SetProvider(new SQLite3Provider_e_sqlcipher());
        
        var collection = new ServiceCollection();
        collection.ConfigureServices();
        collection.ConfigureRepositories();

        var services = collection.BuildServiceProvider();

        var vm = services.GetRequiredService<MainWindowViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}