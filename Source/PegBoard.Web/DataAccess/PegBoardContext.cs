using System.Data.Entity;
using PegBoard.Web.Models;
using PegBoard.Web.Services;

namespace PegBoard.Web.DataAccess
{
    public class PegBoardContext : DbContext, IDbContext
    {
        public DbContext Context
        {
            get { return this; }
        }

        /// <summary>
        /// All the recorded scores for the game.
        /// </summary>
        public IDbSet<Score> Scores { get; set; }
    }
}