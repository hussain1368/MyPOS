using POS.DAL.Domain;

namespace POS.DAL.Repository
{
    public abstract class BaseRepository
    {
        protected readonly POSContext dbContext;

        public BaseRepository(POSContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
