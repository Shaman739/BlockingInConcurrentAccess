using BlockingResourcesInConcurrentAccess.Core.UserStories.PurchaceFigure;
using System.Threading.Tasks;

namespace BlockingResourcesInConcurrentAccess.Core.Contract
{
    public interface IOrderStorage
    {
        // сохраняет оформленный заказ и возвращает сумму
        Task<int> Save(Order order);
    }
}
