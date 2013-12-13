namespace PegBoard
{
    public interface ICloneable<out T> where T : class 
    {
        T Clone();
    }
}