using POS.DAL.Domain;

namespace POS.DAL.Repository.DatabaseRepository
{
    public abstract class BaseDatabaseRepository
    {
        protected readonly POSContext dbContext;

        public BaseDatabaseRepository(POSContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
