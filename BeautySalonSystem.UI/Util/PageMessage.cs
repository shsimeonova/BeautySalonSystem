namespace BeautySalonSystem.UI.Util
{
    public enum PageMessageType
    {
        Warning = 1,
        Danger = 2,
        Success = 3,
        Info = 4
    }
    
    public class PageMessage
    {
        public PageMessageType Type { get; set; }
        public string Text { get; set; }
    }
}