using Microsoft.EntityFrameworkCore;

namespace Test_FilestreamSQL.Models.DAL
{
    public class DBContext: DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }
        public DbSet<Attachment> Attachments { get; set; }
    }
}
