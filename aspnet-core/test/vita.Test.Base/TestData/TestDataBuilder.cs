using vita.EntityFrameworkCore;

namespace vita.Test.Base.TestData
{
    public class TestDataBuilder
    {
        private readonly vitaDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(vitaDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            new TestOrganizationUnitsBuilder(_context, _tenantId).Create();
            new TestSubscriptionPaymentBuilder(_context, _tenantId).Create();
            new TestEditionsBuilder(_context).Create();

            _context.SaveChanges();
        }
    }
}
