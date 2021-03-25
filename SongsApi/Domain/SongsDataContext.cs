using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SongsApi.Domain
{
    // Will act as service that allows us to access the database
    public class SongsDataContext : DbContext
    {
        public DbSet<Song> Songs { get; set; }
        
        public SongsDataContext(DbContextOptions<SongsDataContext> options): base(options)
        {

        }
    }
}
