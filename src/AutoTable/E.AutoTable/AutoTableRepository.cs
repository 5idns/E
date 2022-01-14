using E.Repository;

namespace E.AutoTable
{
    public class AutoTableRepository : PageTableRepository
    {
        public AutoTableRepository(FreeSqlDbContext context) : base(context)
        {
        }
    }
}
