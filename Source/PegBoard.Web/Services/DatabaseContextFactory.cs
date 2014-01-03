using PegBoard.Web.DataAccess;

namespace PegBoard.Web.Services
{
    public class DatabaseContextFactory : IDatabaseContextFactory
    {
        public IDbContext GetDbContext()
        {
            return new PegBoardContext();
        }
    }
}