public class SettingsVM : ISettingsVM
{
    public bool Notifications { get; set; }
    public bool DarkTheme { get; set; }

    public Task GetSettings()
    {
        throw new NotImplementedException();
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }
}