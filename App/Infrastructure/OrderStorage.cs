using BlockingResourcesInConcurrentAccess.Core.Contract;
using BlockingResourcesInConcurrentAccess.Core.UserStories.PurchaceFigure;
using System.Threading.Tasks;

namespace BlockingResourcesInConcurrentAccess.Infrastructure
{
    public class OrderStorage : IOrderStorage
    {
        public OrderStorage()
        {

        }
        public Task<int> Save(Order order)
        {
            return Task.FromResult(1);
        }
    }
}
