using BlockingResourcesInConcurrentAccess.Core.Contract;
using System.Collections.Generic;

namespace BlockingResourcesInConcurrentAccess.Core.UserStories.PurchaceFigure
{
    public class Order
    {
        public List<Position> Positions { get; set; }
    }
}
