using POS.DAL.Domain;

namespace POS.DAL.Repository.DatabaseRepository
{
    public abstract class BaseDatabaseRepository
    {
        protected readonly POSContext dbContext;
        protected readonly int pageSize = 10;

        public BaseDatabaseRepository(POSContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
