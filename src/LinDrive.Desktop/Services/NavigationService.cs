using System;
using Avalonia.Controls;
using LinDrive.Desktop.Interfaces;

namespace LinDrive.Desktop.Services;

public class NavigationService : INavigationSevrice
{
    private readonly Window _mainWindow;

    public NavigationService(Window mainWindow)
    {
        _mainWindow = mainWindow;
    }

    public void NavigateTo<T>() where T : Window
    {
        var newMindow = Activator.CreateInstance<T>() as Window;
        newMindow.Show();
        _mainWindow.Close();
    }
}