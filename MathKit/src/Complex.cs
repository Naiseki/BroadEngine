using System;

namespace MathKit
{
    public struct Complex: IEquatable<Complex>
    {
        /// <summary>
        /// 実部
        /// </summary>
        public float real;

        /// <summary>
        /// 虚部
        /// </summary>
        public float i;


        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="real">実部</param>
        /// <param name="imaginary">虚部</param>
        public Complex(float real, float imaginary)
        {
            this.real = real;
            i = imaginary;
        }


        public Complex(Float2 v): this(v.x, v.y) {}


        public static Complex operator +(Complex a, Complex b) => new(a.real + b.real, a.i + b.i);
        public static Complex operator -(Complex a, Complex b) => new(a.real - b.real, a.i - b.i);
        public static Complex operator -(Complex value) => new(-value.real, -value.i);
        public static Complex operator *(Complex a, Complex b) => new(a.real * b.real - a.i * b.i, a.real * b.i + b.real * a.i);

        public static Complex operator /(Complex a, Complex b)
        {
            float ar = a.real;
            float br = a.i;
            float ai = b.real;
            float bi = b.i;

            if (bi * bi < ai * ai) {
                float div = bi / ai;
                return new Complex((ar + br * div) / (ai + bi * div), (br - ar * div) / (ai + bi * div));
            } 
            else {
                float div = ai / bi;
                return new Complex((br + ar * div) / (bi + ai * div), (-ar + br * div) / (bi + ai * div));
            }
        }

        public static bool operator ==(Complex a, Complex b) => a.Equals(b);

        public static bool operator !=(Complex a, Complex b) => !a.Equals(b);

        


        public override bool Equals(object obj) => obj is Complex complex && Equals(complex);
        

        public override int GetHashCode() => HashCode.Combine(real, i);


        public bool Equals(Complex other)
        {
            float diffR = real - other.real;
            float diffI = i - other.i;

            return (diffR * diffR + diffI * diffI < MathUtil.EpsilonSqr);
        }


        public override string ToString()
        {
            return $"{real}+{i}i";
        }


        private static readonly Complex identity = new Complex(1, 0);


        public static Complex Identity => identity;
    }
}
