using Microsoft.EntityFrameworkCore;

namespace MyFileManager
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<MyData> MyDatas { get; set; }
    }
}
