using MathKit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;

namespace Broad.Graphics
{
    static class Fonts
    {
        static FontCollection collection = new FontCollection();


        static Fonts()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("font");
            var family = collection.Install(stream);
        }


        public static FontFamily GetFontFamily(string name)
        {
            return collection.Find(name);
        }
    }
}