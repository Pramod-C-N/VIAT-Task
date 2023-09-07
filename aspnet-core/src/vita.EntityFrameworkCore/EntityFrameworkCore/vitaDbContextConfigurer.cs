using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace vita.EntityFrameworkCore
{
    public static class vitaDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<vitaDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<vitaDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}