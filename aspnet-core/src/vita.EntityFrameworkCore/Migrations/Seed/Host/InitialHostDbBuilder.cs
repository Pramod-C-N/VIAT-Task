using vita.EntityFrameworkCore;

namespace vita.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly vitaDbContext _context;

        public InitialHostDbBuilder(vitaDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
