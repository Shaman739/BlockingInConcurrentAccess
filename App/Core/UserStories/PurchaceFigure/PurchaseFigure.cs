using BlockingResourcesInConcurrentAccess.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockingResourcesInConcurrentAccess.Core.UserStories.PurchaceFigure
{
    public class PurchaseFigure
    {
        private readonly IOrderStorage _orderStorage;
        private readonly IFiguresStorage _figuresStorage;
        public PurchaseFigure(IOrderStorage orderStorage, IFiguresStorage figuresStorage)
        {
            _orderStorage = orderStorage;
            _figuresStorage = figuresStorage ?? throw new ArgumentNullException(nameof(figuresStorage));
        }

        /// <summary>
        /// Purchace figure
        /// </summary>
        /// <param name="order"></param>
        /// <returns>1 - success; 0 - fail</returns>
        public async ValueTask<int> Purchace(Order order)
        {
            List<(string type, string guidBlocking)> listBlocking = new List<(string type, string guidBlocking)>();
            foreach (var position in order.Positions)
            {
                var resultCheck = await _figuresStorage.Get(position.Figure.GetType().Name, position.Count);
                if (!resultCheck.Item1)
                {
                    return 0;
                }
                else
                    listBlocking.Add((position.Figure.GetType().Name, resultCheck.Item2));
            }

            foreach (var position in order.Positions)
            {
                await _figuresStorage.Set(position.Figure.GetType().Name, position.Count, listBlocking.FirstOrDefault(x => x.type == position.Figure.GetType().Name).guidBlocking);
            }

            var result = await _orderStorage.Save(order);
            return result;
        }
    }
}
