using ReactiveUI;

namespace LinDrive.Desktop.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private string _seed;
    
    public string Seed
    {
        get { return _seed; }
        set { this.RaiseAndSetIfChanged(ref _seed, value); }
    }
    
    private string _message = string.Empty;
    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }

    public void Login()
    {
        
    }
}