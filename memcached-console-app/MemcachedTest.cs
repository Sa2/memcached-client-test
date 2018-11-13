using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace memcached_console_app
{
    class MemcachedTest
    {
        private readonly IMemcachedClient _memcachedClient;
        public MemcachedTest(IMemcachedClient memcachedClient)
        {
            _memcachedClient = memcachedClient;
        }
        public async void setValue()
        {

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
        }
    }
}
