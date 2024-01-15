using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CatalogOnline2.Models;

namespace CatalogOnline2.Data
{
    public class CatalogOnline2Context : DbContext
    {
        public CatalogOnline2Context (DbContextOptions<CatalogOnline2Context> options)
            : base(options)
        {
        }

        public DbSet<CatalogOnline2.Models.User> User { get; set; } = default!;
    }
}
