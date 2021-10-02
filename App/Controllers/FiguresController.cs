using BlockingResourcesInConcurrentAccess.Core.UserStories.PurchaceFigure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlockingResourcesInConcurrentAccess.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FiguresController : ControllerBase
    {
        private readonly ILogger<FiguresController> _logger;
        private readonly PurchaseFigure _purchaseFigure;

        public FiguresController(ILogger<FiguresController> logger, PurchaseFigure purchaseFigure)
        {
            _logger = logger;
            _purchaseFigure = purchaseFigure ?? throw new ArgumentNullException(nameof(purchaseFigure));


        }

        [HttpPost]
        public async Task<ActionResult> Order(Order order)
        {
            var resultPurchace = await _purchaseFigure.Purchace(order);

            return new OkObjectResult(resultPurchace);
        }


    }




}