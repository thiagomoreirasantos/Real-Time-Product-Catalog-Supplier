namespace RealTimeProductCatalog.Application.Interfaces
{
    public interface IPublisher
    {
        Task<bool> StartSendingMessages(string content);
    }
}