using System;
using MathKit;

namespace Broad.Graphics
{
    public struct ColorF: IEquatable<ColorF>
    {
        private Float4 rgba;


        /// <summary>
        /// 赤
        /// </summary>
        public float R {
            get => rgba.x;
            set => rgba.x = value;
        }

        /// <summary>
        /// 緑
        /// </summary>
        public float G {
            get => rgba.y;
            set => rgba.y = value;
        }

        /// <summary>
        /// 青
        /// </summary>
        public float B {
            get => rgba.z;
            set => rgba.z = value;
        }

        /// <summary>
        /// 透明度
        /// </summary>
        public float A {
            get => rgba.w;
            set => rgba.w = value;
        }


        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="a">透明度</param>
        public ColorF(byte r, byte g, byte b, byte a)
        {
            rgba = new Float4(r / 255f, g / 255f, b / 255f, a / 255f);
        }


        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="a">透明度</param>
        public ColorF(float r, float g, float b, float a)
        {
            rgba = new Float4(r, g, b, a);
        }


        /// <param name="color">RGBAを表す4次元ベクトル</param>
        public ColorF(in Float4 color)
        {
            rgba = color;
        }


        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        public ColorF(float r, float g, float b): this(r, g, b, 1f) {}


        /// <summary>
        /// RGB成分
        /// </summary>
        /// <returns>RGBを表す3次元ベクトル</returns>
        public Float3 Rgb => rgba.xyz;


        /// <summary>
        /// 正規化 0~1の範囲にクランプ
        /// </summary>
        public void Normalize()
        {
            R = MathUtil.Clamp01(R);
            G = MathUtil.Clamp01(G);
            B = MathUtil.Clamp01(B);
            A = MathUtil.Clamp01(A);
        }


        public static ColorF Lerp(in ColorF a, in ColorF b, float t)
        {
            return (ColorF)Float4.Lerp(a.rgba, b.rgba, t);
        }


        /// <summary>
        /// <see cref="Byte4"/>に変換
        /// </summary>
        /// <returns>生成した<see cref="Byte4"/></returns>
        public Byte4 ToByte4() => new Byte4((byte)(R * 255f), (byte)(G * 255f), (byte)(B * 255f), (byte)(A * 255f)); 


        /// <summary>
        /// 加算
        /// </summary>
        public static ColorF operator +(in ColorF a, in ColorF b) => new(a.rgba + b.rgba);

        /// <summary>
        /// 減算
        /// </summary>
        public static ColorF operator -(in ColorF a, in ColorF b) => new(a.rgba - b.rgba);

        /// <summary>
        /// 乗算
        /// </summary>
        public static ColorF operator *(in ColorF a, in ColorF b) => new(a.rgba * b.rgba);


        public static bool operator ==(in ColorF a, in ColorF b) 
            => (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A);

        public static bool operator !=(in ColorF a, in ColorF b) => !(a == b); 


        public static implicit operator Float4(ColorF color) => color.rgba;

        public static implicit operator ColorF(Float4 color) => new ColorF(color);


        public override int GetHashCode() => HashCode.Combine(R, G, B, A);


        public override string ToString() => $"(R: {R}, G: {G}, B: {B}, A: {A})";


        public bool Equals(ColorF color)
        {
            return rgba == color.rgba;
        }


        public override bool Equals(object obj) => obj is ColorF col && Equals(col);


        public static readonly ColorF White  = new(255, 255, 255, 255);
        public static readonly ColorF Black  = new(  0,   0,   0, 255);
        public static readonly ColorF Red    = new(255,   0,   0, 255);
        public static readonly ColorF Green  = new(  0, 255,   0, 255);
        public static readonly ColorF Blue   = new(  0,   0, 255, 255); 
        public static readonly ColorF Aqua   = new(  0, 255, 255, 255); 
        public static readonly ColorF Brown  = new(165,  42,  42, 255); 
        public static readonly ColorF Gold   = new(155, 215,   0, 255); 
        public static readonly ColorF Gray   = new(128, 128, 128, 255); 
        public static readonly ColorF Orange = new(255, 165,   0, 255); 
        public static readonly ColorF Yellow = new(255, 255,   0, 255); 
        public static readonly ColorF Transparent = new(255, 255, 255, 0);
        public static readonly ColorF Zero = new(0, 0, 0, 0);
    }


    /// <summary>
    /// カラーのブレンド方法
    /// </summary>
    public enum ColorBlendMode
    {
        /// <summary>
        /// 掛け算
        /// </summary>
        Multiple,
   
        /// <summary>
        /// 足し算
        /// </summary>
        Addaptive,

        /// <summary>
        /// 引き算
        /// </summary>
        Substractive,

        /// <summary>
        /// 元の色を上書き
        /// </summary>
        Override
    }
}