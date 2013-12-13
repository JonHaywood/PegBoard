using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PegBoard.Web.Models;
using PegBoard.Web.Services;

namespace PegBoard.Web.Controllers
{
    public class ScoresController : ApiController
    {
        private readonly IDatabaseContextFactory databaseContextFactory;

        public ScoresController(IDatabaseContextFactory databaseContextFactory)
        {
            this.databaseContextFactory = databaseContextFactory;
        }

        public IEnumerable<Score> Get()
        {
            using (var context = databaseContextFactory.GetDbContext())
            {
                return context.Scores.ToList();
            }
        }

        public Score Get(string nickname)
        {
            using (var context = databaseContextFactory.GetDbContext())
            {
                // returns this player's high score
                return context.Scores
                    .Where(score => score.Nickname == nickname)
                    .OrderBy(score => score.PegCount)
                    .OrderBy(score => score.TotalTimeInSeconds)
                    .FirstOrDefault();
            }
        }

        public void Post(Score score)
        {
            using (var context = databaseContextFactory.GetDbContext())
            {
                context.Scores.Add(score);                
            }
        }
    }
}
