using System;

namespace Broad
{
    public abstract class Asset: IDisposable
    {
        public abstract AssetInfo Info { get; protected set; }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected abstract void Dispose(bool disposing);


        public void Release()
        {
            Info.Release();
        }


        ~Asset() => Dispose(false);
    }
}