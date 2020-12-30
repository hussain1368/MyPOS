using POS.DAL.Domain;

namespace POS.DAL.Query
{
    public abstract class DbQuery
    {
        protected readonly POSContext dbContext;

        public DbQuery(POSContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
