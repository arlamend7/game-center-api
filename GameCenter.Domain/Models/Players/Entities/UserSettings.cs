namespace GameCenter.Domain.Models.Players.Entities
{
    public class UserPreference
    {
        public string CurrentLanguage { get; set; }
        public string ThemeMode { get; set; }
        public bool MobileRotation { get; set; }
        public bool Sound { get; set; }
        public bool Location { get; set; }
        public bool Notifications { get; set; }
        public bool LargeText { get; set; }
        public bool Vibration { get; set; }
    }
}
