namespace PegBoard
{
    /// <summary>
    /// Represents an assignment possible in the solution space. This is
    /// a board paired with a jump.
    /// </summary>
    public class Assignment : BaseObject
    {
        public Assignment(Board board, Jump jump)
        {
            Check.Require(board != null, "board is a required argument.");
            Check.Require(jump != null, "jump is a required argument.");

            Board = board;
            Jump = jump;
        }

        [DomainSignature]
        public Board Board { get; private set; }
        [DomainSignature]
        public Jump Jump { get; private set; }
    }
}
