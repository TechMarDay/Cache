using MemoryCache.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MemoryCache.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IMemoryCache cache;

        public IndexModel(ILogger<IndexModel> logger, IMemoryCache cache)
        {
            _logger = logger;
            this.cache = cache;
        }

        public void OnGet()
        {
            cache.Set<string>("timeCache", DateTime.Now.ToString());

            if (!cache.TryGetValue<string>("timeCache", out string timeCache))
            {
                ViewData["ExistingCache"] = "Cache với key: timeCache không tồn tại";
                cache.Set<string>("timeCache", DateTime.Now.ToString());
            }
            else
            {
                ViewData["ExistingCache"] = $"Cache với key: timeCache đã tồn tại với value: {timeCache}";
            }

            //Set cache for complex data
            var listUserCached = new List<User>();
            if (!cache.TryGetValue<List<User>>("ListUser", out listUserCached))
            {
                var listUser = new List<User>()
                {
                    new User
                    {
                        Id = 1,
                        Name = "Iron man"
                    },
                    new User
                    {
                        Id = 2,
                        Name = "Spider man"
                    }
                };
                cache.Set<List<User>>("ListUser", listUser);
            }


            var timeCacheGetOrCreate = cache.GetOrCreate<string>("timeCacheGetOrCreate", entry =>
            {
                return DateTime.Now.ToString();
            });

            ViewData["timeCacheGetOrCreate"] = timeCacheGetOrCreate;

            ViewData["expiredCache"] = cache.Get<string>("expiredCache");

            ViewData["callbackMessage"] = cache.Get<string>("callbackMessage");

            ViewData["parentKey"] = cache.Get<string>("parentKey");
            ViewData["ChildKey1"] = cache.Get<string>("ChildKey1");
            ViewData["ChildKey2"] = cache.Get<string>("ChildKey2");
        }

        //Remove dependences cache
        public void OnPostRemoveDependencesCache()
        {
            CancellationTokenSource cts =
                cache.Get<CancellationTokenSource>("cts");
            cts.Cancel();
        }
    }
}