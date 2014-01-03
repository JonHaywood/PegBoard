using PegBoard.Web.DataAccess;

namespace PegBoard.Web.Services
{
    public interface IDatabaseContextFactory
    {
        IDbContext GetDbContext();
    }
}
