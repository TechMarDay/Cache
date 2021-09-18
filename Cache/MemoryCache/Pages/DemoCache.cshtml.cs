using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryCache.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCache.Pages
{
    public class DemoCacheModel : PageModel
    {
        private readonly IMemoryCache cache;
        public string TimeCache { get; set; }

        public List<User> Users { get; set; }

        public DemoCacheModel(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void OnGet()
        {
            TimeCache = cache.Get<string>("timeCache");

            Users = cache.Get<List<User>>("ListUser");
        }
    }
}
