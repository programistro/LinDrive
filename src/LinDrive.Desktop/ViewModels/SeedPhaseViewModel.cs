using System.Security.Cryptography;
using CommunityToolkit.Mvvm.ComponentModel;
using NBitcoin;

namespace LinDrive.Desktop.ViewModels;

public partial class SeedPhaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string seedPhrase;
    
    public SeedPhaseViewModel()
    {
        using var rng = RandomNumberGenerator.Create();
        var entropy = new byte[16];
        rng.GetBytes(entropy);
        
        Mnemonic mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
        seedPhrase = string.Join(" ", mnemonic.Words);
    }
}