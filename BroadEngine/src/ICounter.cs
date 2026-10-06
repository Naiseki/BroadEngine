namespace Broad
{
    public interface ICounter
    {
        int Value { get; }

        void Count(int x);

        void SetValue(int x);

        void Reset();
    }
}