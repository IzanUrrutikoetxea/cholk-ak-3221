namespace cholk_ak_3221.Services;

public class AppStateService
{
    public string AccentColor { get; private set; } = "#8b7bb3";
    public bool PrivacyMode { get; private set; } = false;
    public string MoneyFilter => PrivacyMode ? "blur(9px)" : "none";

    public event Action? OnChange;

    public void SetAccent(string color)
    {
        AccentColor = color;
        OnChange?.Invoke();
    }

    public void TogglePrivacy()
    {
        PrivacyMode = !PrivacyMode;
        OnChange?.Invoke();
    }
}
