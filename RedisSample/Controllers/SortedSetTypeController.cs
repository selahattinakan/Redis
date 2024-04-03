using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedisSample.Services;
using StackExchange.Redis;

namespace RedisSample.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly StackExchangeRedisService _redisService;
        private readonly IDatabase db;
        private readonly string ListKey = "sortedSetNames";

        public SortedSetTypeController(StackExchangeRedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(3);
        }
        public IActionResult Index()
        {
            if (!db.KeyExists(ListKey))
            {
                db.KeyExpire(ListKey, DateTime.Now.AddMinutes(1));
            }

            db.SortedSetAdd(ListKey, "Selahattin", 3);
            db.SortedSetAdd(ListKey, "Ahmet", 2);
            bool r1 = db.SortedSetAdd(ListKey, "Mehmet", 1); //true
            bool r2 = db.SortedSetAdd(ListKey, "Mehmet", 4); //false ama score değerini günceller
            return View();
        }

        public IActionResult Show()
        {
            HashSet<string> names = new HashSet<string>();
            HashSet<string> namesDesc = new HashSet<string>();
            if (!db.KeyExists(ListKey)) return Content("sorted set list not found");

            db.SortedSetScan(ListKey).ToList().ForEach(item => names.Add($"{item.Score}-{item.Element}"));
            db.SortedSetRangeByRank(ListKey, order: Order.Descending).ToList().ForEach(item => namesDesc.Add(item));

            return Content(JsonConvert.SerializeObject(names) + "\n" + JsonConvert.SerializeObject(namesDesc));
        }

        public IActionResult DeleteItem()
        {
            if (!db.KeyExists(ListKey)) return Content("sorted set list not found");

            string item = "Selahattin";
            db.SortedSetRemove(ListKey, item);

            return Content("sorted set list item deleted");
        }
    }
}
