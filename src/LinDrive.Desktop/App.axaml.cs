using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LinDrive.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace LinDrive.Desktop;

public partial class App : Avalonia.Application
{
    public static IServiceProvider ServiceProvider { get; private set; }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        var services = new ServiceCollection();
        services.AddServices();
        ServiceProvider = services.BuildServiceProvider();

        base.OnFrameworkInitializationCompleted();
    }

    private void ExitClick(object? sender, EventArgs e)
    {
        Environment.Exit(0);
    }
}