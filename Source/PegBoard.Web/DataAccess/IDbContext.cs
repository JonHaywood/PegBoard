using System;
using System.Data.Entity;
using PegBoard.Web.Models;

namespace PegBoard.Web.DataAccess
{
    public interface IDbContext : IDisposable
    {
        IDbSet<Score> Scores { get; set; }        
    }
}
