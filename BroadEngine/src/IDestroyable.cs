using System;

namespace Broad
{
    public interface IDestroyable
    {
        bool IsDestroyed { get; }
        void Destroy();
    }
}