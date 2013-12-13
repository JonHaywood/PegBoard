using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PegBoard.Web.Models;

namespace PegBoard.Web.Services
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