using MathKit;

namespace Broad
{
    public readonly struct CSV<T>
    {
        public readonly T[] Data;
        public readonly Int2 Length;


        public CSV(T[] data, Int2 length)
        {
            Data = data;
            Length = length;
        }
    }
}