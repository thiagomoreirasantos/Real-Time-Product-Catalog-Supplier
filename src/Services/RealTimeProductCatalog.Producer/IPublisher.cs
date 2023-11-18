namespace RealTimeProductCatalog.Producer
{
    public interface IPublisher
    {
        Task<bool> StartSendingMessages(string content);
    }
}