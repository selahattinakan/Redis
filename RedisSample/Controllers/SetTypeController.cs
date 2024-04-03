using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedisSample.Services;
using StackExchange.Redis;

namespace RedisSample.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly StackExchangeRedisService _redisService;
        private readonly IDatabase db;
        private readonly string ListKey = "setNames";

        public SetTypeController(StackExchangeRedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(2);
        }
        public IActionResult Index()
        {
            if (!db.KeyExists(ListKey))
            {
                db.KeyExpire(ListKey, DateTime.Now.AddMinutes(5));
            }
            //SetAdd uniq itemleri tutar, aynı item birden fazla eklenemez
            db.SetAdd(ListKey, "Selahattin");
            db.SetAdd(ListKey, "Ahmet");
            bool r1 = db.SetAdd(ListKey, "Mehmet"); //true
            bool r2 = db.SetAdd(ListKey, "Mehmet"); //false
            return View();
        }

        public IActionResult Show()
        {
            HashSet<string> names = new HashSet<string>();
            if (!db.KeyExists(ListKey)) return Content("set list not found");

            db.SetMembers(ListKey).ToList().ForEach(name => names.Add(name));

            return Content(JsonConvert.SerializeObject(names));
        }

        public IActionResult DeleteItem()
        {
            if (!db.KeyExists(ListKey)) return Content("set list not found");

            string item = "Selahattin";
            db.SetRemove(ListKey, item);

            return Content("set list item deleted");
        }
    }
}
