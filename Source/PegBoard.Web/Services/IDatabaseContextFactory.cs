using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PegBoard.Web.Services
{
    public interface IDatabaseContextFactory
    {
        IDbContext GetDbContext();
    }
}
