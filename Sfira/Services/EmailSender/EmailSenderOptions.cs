namespace MroczekDotDev.Sfira.Services.EmailSender
{
    public class EmailSenderOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SenderName { get; set; }
    }
}
