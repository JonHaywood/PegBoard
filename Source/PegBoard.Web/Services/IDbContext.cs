using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using PegBoard.Web.Models;

namespace PegBoard.Web.Services
{
    public interface IDbContext : IDisposable
    {
        IDbSet<Score> Scores { get; set; }        
    }
}
