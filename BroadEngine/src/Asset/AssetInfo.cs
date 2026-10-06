using System.IO;
using System.Reflection;
using MathKit;
using Broad.Graphics;
using Broad.Audio;

#pragma warning disable 8602

namespace Broad
{
    public abstract class AssetInfo   
    {
        public readonly string Path;

        public readonly string Name;

        public bool IsLoaded => (Asset != null);

        internal Asset? Asset;

        public int RefCount => refCount;

        int refCount = 0;

        static readonly Assembly? assembly = Assembly.GetEntryAssembly();


        protected AssetInfo(string path, string name) 
        {
            Path = path;
            Name = name;
        }

        internal Stream? CreateStream()
        {
            try {
                return assembly.GetManifestResourceStream(Path);
            }
            catch(System.Exception e) {
                System.Console.WriteLine(e);
                return default;
            }
        }

        internal void Use() => refCount++;
    
        internal void Release() => refCount--;


        public bool TryUnload()
        {
            if (refCount < 1 && Asset != null) {
                Unload();
                return true;
            }
            return false;
        }


        public void Unload()
        {
            Asset?.Dispose();
            Asset = null;
        }
        

        internal abstract Asset Load();
    }



    public class SpriteInfo: AssetInfo
    {
        public readonly Int2 SliceValue;
        public readonly SliceMode SliceMode;


        public SpriteInfo(string path, string name, Int2 sliceSize, SliceMode mode = SliceMode.Size): base(path, name)
        {
            SliceValue = sliceSize;
            SliceMode = mode;
        } 


        internal override Asset Load()
        {
            return Sprite.Load(this);
        }
    }



    public class AudioInfo: AssetInfo
    {

        public AudioInfo(string path, string name): base(path, name) {} 


        internal override Asset Load()
        {
            return Sound.Load(this);
        }
    }


    /// <summary>
    /// ローディングの状態を表す
    /// </summary>
    public enum LoadState: byte
    {
        /// <summary>
        /// ロードされていない
        /// </summary>
        NotLoaded = 0,

        /// <summary>
        /// ローディング中
        /// </summary>
        Loading = 1,

        /// <summary>
        /// ロード済み
        /// </summary>
        Loaded = 2
    }
}