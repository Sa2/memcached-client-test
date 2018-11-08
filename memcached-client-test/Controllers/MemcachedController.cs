using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enyim.Caching;
using Microsoft.AspNetCore.Mvc;

namespace memcached_client_test.Controllers
{
    public class MemcachedController : Controller
    {
        private readonly IMemcachedClient _memcachedClient;
        public static readonly string CacheKey = "memcached-test";
        public MemcachedController(IMemcachedClient memcachedClient)
        {
            _memcachedClient = memcachedClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult SetForm()
        //{

        //}

        public async Task<IActionResult> Set([FromQuery] string data)
        {
            string sampleUrl = "https://localhost:5001/Memcached/Set?data=sampleValue";
            int cacheSeconds = 6000;
            var cacheData = data ?? "init value";
            ViewData["Message"] = "Called Memcached Set API.";
            ViewData["SampleUrl"] = sampleUrl;
            ViewData["SetData"] = cacheData;

            for (var i = 0; i < 30; i++) {
                var key = "memcached-key-" + i;
                var value = "value-" + 1;
                int testCacheSeconds = 120;
                await _memcachedClient.SetAsync(key, value, testCacheSeconds);
            }

            await _memcachedClient.SetAsync(CacheKey, cacheData, cacheSeconds);
            return View();
        }
        public async Task<IActionResult> Get()
        {
            ViewData["Message"] = "Called Memcached Get API.";
            var data = await _memcachedClient.GetValueAsync<String>(CacheKey);

            ViewData["GetData"] = data;

            for (var i = 0; i < 30; i++)
            {
                var key = "memcached-key-" + i;
                int testCacheSeconds = 120;
                var result = await _memcachedClient.GetAsync<string>(key);
                if (result.Value != null)
                {
                    Console.WriteLine("Hit!!!! : " + result.Value);
                }
                else
                {
                    Console.WriteLine("No Hit.....");
                }
            }

            return View();
        }

        //public async Task<IActionResult> GetOrCreate()
        //{
        //    int cacheSeconds = 6000;
        //    ViewData["Message"] = "Memcached Stats.";
        //    var stats = _memcachedClient.GetValueOrCreateAsync();

        //    return View();
        //}
    }
}