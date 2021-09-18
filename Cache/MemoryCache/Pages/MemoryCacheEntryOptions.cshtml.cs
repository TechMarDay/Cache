using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace MemoryCache.Pages
{
    public class MemoryCacheEntryOptionsModel : PageModel
    {
        private readonly IMemoryCache cache;

        public MemoryCacheEntryOptionsModel(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void OnGet()
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            //cache se het han sau 2 phut tu thoi diem hien tai
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(2);

            //cache se het han sau 1 phut neu khong duoc truy cap, ke ca con thoi han cua AbsoluteExpiration
            options.SlidingExpiration = TimeSpan.FromMinutes(1);

            //Low, Normal, High và NeverRemove => cache sẽ xóa dựa vào độ ưu tiên từ thấp đến cao. Nerver remove không bao h xóa
            options.Priority = CacheItemPriority.Normal;

            // Callback when cached item is removed
            options.RegisterPostEvictionCallback(EvictionCallback, this);

            cache.Set<string>("expiredCache", DateTime.Now.ToString(), options);
        }

        private static void EvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            var message = $"Entry was evicted. Reason: {reason}.";
            ((MemoryCacheEntryOptionsModel)state).cache.Set("callbackMessage", message);
        }
    }
}