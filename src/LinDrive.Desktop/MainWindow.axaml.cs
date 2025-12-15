using System.Net.Http;
using Avalonia.Controls;
using Avalonia.Interactivity;
using NBitcoin;

namespace LinDrive.Desktop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
        string seedPhrase = mnemo.ToString();
        
        HttpClient httpClient = new  HttpClient();
        var line = TextBox.Text;
    }
}