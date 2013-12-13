using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PegBoard
{
    /// <summary>
    /// Represents a collection of assignments that we can use IEnumerable with and is clonable.
    /// </summary>
    public interface IAssignmentCollection : ICollection<Assignment>, ICloneable<IAssignmentCollection>
    {        
    }

    public class AssignmentCollection : Collection<Assignment>, IAssignmentCollection
    {
        public AssignmentCollection() : base() { }
        public AssignmentCollection(IEnumerable<Assignment> assignments) : base(EnsureValidCollection(assignments)) { }        

        /// <summary>
        /// Ensures that the collection is not null and converts it to an IList.
        /// </summary>        
        private static IList<Assignment> EnsureValidCollection(IEnumerable<Assignment> assignments)
        {
            Check.Require(assignments != null, "assignments is a required argument.");
            return assignments.ToList();
        }

        public IAssignmentCollection Clone()
        {
            return new AssignmentCollection(Items);
        }
    }
}
