using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Photoboom.Models;

namespace Photoboom.Data
{
    public class PhotoContext : DbContext
    {
        public PhotoContext (DbContextOptions<PhotoContext> options)
            : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
