using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedisSample.Services;
using StackExchange.Redis;

namespace RedisSample.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly StackExchangeRedisService _redisService;
        private readonly IDatabase db;
        private readonly string hashKey = "hashKey";

        public HashTypeController(StackExchangeRedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(4);
        }
        public IActionResult Index()
        {
            if (!db.KeyExists(hashKey))
            {
                db.KeyExpire(hashKey, DateTime.Now.AddMinutes(1));
            }

            db.HashSet(hashKey, "KeyS", "Selahattin");
            db.HashSet(hashKey, 2, "Ahmet");
            db.HashSet(hashKey, "KeyMehmet", "Mehmet"); 
            db.HashSet(hashKey, "KeyMehmet", "Mehmet"); 
            return View();
        }

        public IActionResult Show()
        {
            if (!db.KeyExists(hashKey)) return Content("hash not found");

            Dictionary<string, string> list = new Dictionary<string, string>();

            db.HashGetAll(hashKey).ToList().ForEach(item => { list.Add(item.Name, item.Value); });


            return Content(JsonConvert.SerializeObject(list));
        }

        public IActionResult DeleteItem()
        {
            if (!db.KeyExists(hashKey)) return Content("hash not found");

            string item = "KeyS";
            db.HashDelete(hashKey, item);

            return Content("hash deleted");
        }
    }
}
