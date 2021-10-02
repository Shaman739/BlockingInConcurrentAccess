using System.Threading.Tasks;

namespace BlockingResourcesInConcurrentAccess.Core.Contract
{
    public interface IFiguresStorage
    {
        Task<(bool isSuccess, string blockingId)> Get(string type, int count);
        Task Set(string type, int current, string guidBlockingData);
    }
}
