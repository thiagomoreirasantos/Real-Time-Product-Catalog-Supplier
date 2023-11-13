namespace RealTimeProductCatalog.Producer
{
    public interface IPublisher
    {
        Task StartSendingMessages(string content);
    }
}