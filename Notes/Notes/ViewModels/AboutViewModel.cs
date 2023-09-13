using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Notes.ViewModels;

internal class AboutViewModel
{
    public AboutViewModel()
    {
        ShowMoreInfoCommand = new AsyncRelayCommand(ShowMoreInfo);
    }

    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string MoreInfoUrl => "https://aka.ms/maui";
    public string Message => "This app is written in XAML and C# with .NET MAUI.";
    public ICommand ShowMoreInfoCommand { get; }

    private async Task ShowMoreInfo()
    {
        await Launcher.Default.OpenAsync(MoreInfoUrl);
    }
}