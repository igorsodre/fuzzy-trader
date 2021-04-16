using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
    }
}