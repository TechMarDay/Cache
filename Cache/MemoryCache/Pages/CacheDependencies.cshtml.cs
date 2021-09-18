using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace MemoryCache.Pages
{
    public class CacheDependenciesModel : PageModel
    {
        private readonly IMemoryCache cache;

        public CacheDependenciesModel(IMemoryCache cache)
        {
            this.cache = cache;
        }    

        public void OnGet()
        {
            var cts = new CancellationTokenSource();
            cache.Set("cts", cts);

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AddExpirationToken(new CancellationChangeToken(cts.Token));

            cache.Set<string>("parentKey", DateTime.Now.ToString(), options);

            cache.Set<string>("ChildKey1", "Child1 value", new CancellationChangeToken(cts.Token));
            cache.Set<string>("ChildKey2", "Child2 value", new CancellationChangeToken(cts.Token));
        }
    }
}
