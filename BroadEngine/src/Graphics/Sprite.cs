using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using MathKit;
using Img32 =  SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>;


namespace Broad.Graphics
{
    /// <summary>
    /// スプライト
    /// </summary>
    public class Sprite: Asset
    {
        readonly Float4[] units;
        bool isDisposed = false;


        /// <summary>
        /// テクスチャ
        /// </summary>
        public readonly Texture Texture;

        /// <summary>
        /// スプライト１枚分のサイズ
        /// </summary>
        public readonly Int2 UnitSize;

        /// <summary>
        /// スプライトのサイズ
        /// </summary>
        public readonly Int2 TotalSize;

        /// <summary>
        /// 縦横のテクスチャユニットの数
        /// </summary>
        public readonly Int2 UnitLength;

        /// <summary>
        /// テクスチャユニットの数
        /// </summary>
        public readonly int UnitCount;

        /// <summary>
        /// アセット情報
        /// </summary>
        public override AssetInfo Info { get; protected set; }


        /// <summary>
        /// テクスチャを取得
        /// </summary>
        /// <param name="index">取得するテクスチャの番号</param>
        /// <returns>取得したテクスチャ</returns>
        public Float4 GetRegion(int index)
        {
            if (index < 0 || units.Length <= index)
                throw new IndexOutOfRangeException("Invalid index!");
            return units[index];
        }


        /// <summary>
        /// <see cref="SpriteRenderer"/>を作成
        /// </summary>
        /// <returns>作成した<see cref="SpriteRenderer"/></returns>
        public SpriteRenderer CreateRenderer() => new SpriteRenderer(this);


        internal static Sprite Load(SpriteInfo info)
        {
            using var stream = info.CreateStream();
            return new Sprite(Image.Load<Rgba32>(stream), info);
        }


        Sprite(Img32 image, SpriteInfo info)
        {
            Texture = new Texture(image);
            TotalSize = new Int2(image.Width, image.Height);
            UnitSize = SliceSize(info.SliceValue, info.SliceMode);
            UnitLength = TotalSize / UnitSize;
            UnitCount = UnitLength.Multiplied;
            Info = info;
            units = new Float4[UnitCount];
            InitSliceData();
        }


        Int2 SliceSize(Int2 sliceValue, SliceMode mode)
        {
            return mode switch {
                SliceMode.Size => sliceValue,
                SliceMode.Count => TotalSize / sliceValue,
                _ => throw new Exception($"{nameof(SliceMode)}が指定されていません")
            };
        }
/*
        Img32[] Slice(Img32 image, Int2 point, SliceMode mode)
        {
            return mode switch {
                SliceMode.Size => SliceBySize(image, point),
                SliceMode.Count => SliceByCount(image, point),
                _ => throw new Exception($"{nameof(SliceMode)}が指定されていません")
            };
        }

        Img32[] SliceBySize(Img32 image, Int2 point)
        {
            var imgSize = new Int2(image.Width, image.Height);
            if (!Int2.InRange(point, Int2.One, imgSize)) { 
                throw new Exception($"Can't slice by {point}!");
            }

            Int2 chunkCnt = imgSize / point;
            var images = new Img32[chunkCnt.Multiplied]; 
            for (int y = 0; y < chunkCnt.y; y++) {
                for (int x = 0; x < chunkCnt.x; x++) {
                    images[y * chunkCnt.x + x] = CopyRgba(image, new RectInt(x * point.x, y * point.y, point.x, point.y));
                }
            }
            return images;
        }


        Img32[] SliceByCount(Img32 image, Int2 count)
        {
            var imgSize = new Int2(image.Width, image.Height);
            if (!(count > Int2.Zero).AllTrue) { 
                throw new Exception($"Count value must be greater than zero");
            }

            var images = new Img32[count.Multiplied]; 
            var size = imgSize / count;
            for (int y = 0; y < count.y; y++) {
                for (int x = 0; x < count.x; x++) {
                    images[y * count.x + x] = CopyRgba(image, new RectInt(x * size.x, y * size.y, size.x, size.y));
                }
            }
            return images;
        }


        Img32 CopyRgba(Img32 image, RectInt rect)
        {
            var outImage = new Img32(rect.Size.x, rect.Size.y);
            for (int y = rect.Top; y < rect.Bottom; y++) {
                for (int x = rect.Left; x < rect.Right; x++) {
                    outImage[x - rect.Left, y - rect.Top] = image[x, y];  
                }
            }
            return outImage;
        }
*/

        void InitSliceData()
        {
            for (int y = 0; y < UnitLength.y; y++) {
                int uy = UnitLength.y - 1 - y;
                for (int x = 0; x < UnitLength.x; x++) {
                    var pos = new Float2(x * UnitSize.x, uy * UnitSize.y);
                    var total = (Float2)TotalSize;
                    units[y * UnitLength.x + x] = new Float4(pos / total, (Float2)UnitSize / total * 0.99f);
                }
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (isDisposed)
                return;
            Texture.Dispose();
            isDisposed = true;
        } 
    }
}