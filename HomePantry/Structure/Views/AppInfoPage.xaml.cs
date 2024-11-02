using Plugin.Maui.Audio;
using System;
namespace HomePantry.Structure.Views;

public partial class AppInfoPage : ContentPage
{

    private readonly IAudioManager _audioManager;
    public AppInfoPage()
	{
        InitializeComponent();
        _audioManager = AudioManager.Current;
    }


    private bool _isPlayingSound;

    private async void ToggleLight(object sender, EventArgs e)
    {
        if (_isPlayingSound) return;

        _isPlayingSound = true;

        if (ClickableImage.Source.ToString().Contains("appinfologo_on.png"))
        {
            ClickableImage.Source = "appinfologo_off.png";
            PlaySound("lightChange.mp3");
        }
        else
        {
            ClickableImage.Source = "appinfologo_on.png";
            PlaySound("lightChange.mp3");
        }

        await Task.Delay(500);
        _isPlayingSound = false;
    }

    private void PlaySound(string fileName)
    {
        var player = _audioManager.CreatePlayer(fileName);
        player.Play();
    }

}