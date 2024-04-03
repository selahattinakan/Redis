using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedisSample.Services;
using StackExchange.Redis;

namespace RedisSample.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly StackExchangeRedisService _redisService;
        private readonly IDatabase db;
        private readonly string ListKey = "names";

        public ListTypeController(StackExchangeRedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            db.ListRightPush(ListKey, "Selahattin");
            db.ListLeftPush(ListKey, "Ahmet");
            return View();
        }

        public IActionResult Show()
        {
            List<string> names = new List<string>();
            if (!db.KeyExists(ListKey)) return Content("list not found");

            db.ListRange(ListKey).ToList().ForEach(x =>
            {
                names.Add(x.ToString());
            });

            return Content(JsonConvert.SerializeObject(names));
        }

        public IActionResult DeleteItem()
        {
            if (!db.KeyExists(ListKey)) return Content("list not found");

            string item = "Selahattin";
            db.ListRemove(ListKey, item);

            return Content("list item deleted");
        }

        public IActionResult DeleteFirstItem()
        {
            if (!db.KeyExists(ListKey)) return Content("list not found");

            db.ListLeftPop(ListKey);

            return Content("first item deleted");
        }
    }
}
