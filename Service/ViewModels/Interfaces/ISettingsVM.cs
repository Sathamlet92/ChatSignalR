public interface ISettingsVM
{
    public bool Notifications { get; set; }
    public bool DarkTheme { get; set; }
    public Task Save();
    public Task GetSettings();
}