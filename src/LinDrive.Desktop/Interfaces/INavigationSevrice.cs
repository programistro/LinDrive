using Avalonia.Controls;

namespace LinDrive.Desktop.Interfaces;

public interface INavigationSevrice
{
    void NavigateTo<T>() where T : Window;
}