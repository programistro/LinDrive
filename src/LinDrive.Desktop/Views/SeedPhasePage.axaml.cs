using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LinDrive.Desktop.ViewModels;

namespace LinDrive.Desktop.Views;

public partial class SeedPhasePage : Window
{
    private SeedPhaseViewModel viewModel;
    
    public SeedPhasePage()
    {
        InitializeComponent();

        viewModel = new SeedPhaseViewModel();
        DataContext = viewModel;
    }
}