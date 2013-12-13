using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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