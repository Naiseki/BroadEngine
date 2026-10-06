using System;
using MathKit;
using OpenTK.Mathematics;
using Broad.Graphics;

namespace Broad
{
    public struct Transform: IEquatable<Transform>
    {
        /// <summary>
        /// ワールド座標
        /// </summary>
        public Float2 Position;

        /// <summary>
        /// 大きさ
        /// </summary>     
        public Float2 Scale;

        /// <summary>
        /// 回転(ラジアン)
        /// </summary>        
        public float Rotation;

        /// <summary>
        /// 回転(度数)
        /// </summary>   
        public float RotationDegrees {
            get => Rotation * MathUtil.RadToDeg;
            set => Rotation = value * MathUtil.DegToRad;
        }

        /// <summary>
        /// 半分の大きさ
        /// </summary>
        public Float2 HalfScale {
            get => Scale * 0.5f;
            set => Scale = value * 2f;
        }

        /// <summary>
        /// 回転している方向のベクトルを取得
        /// </summary>
        public Float2 Direction => Float2.Rotate(Float2.UnitX, -Rotation);


        /// <summary>
        /// トランスフォームを初期化
        /// </summary>
        /// <param name="position">ワールド座標</param>
        /// <param name="scale">大きさ</param>
        /// <param name="rotation">ラジアンの回転</param>
        public Transform(Float2 position, Float2 scale, float rotation)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }


        public Float2x2 ToFloat2x2()
        {
            var scaMat = Float2x2.CreateScale(Scale.ToScreenScale());
            var rotMat = Float2x2.CreateRotation(-Rotation);
            
            return scaMat * rotMat * Window.AspectMatrix;
        }


        public Transform Translated(Float2 translation)
        {
            var res = this;
            res.Position += translation;
            return res;
        }


        public Transform Scaled(Float2 scale)
        {
            var res = this;
            res.Scale *= scale;
            return res;
        }


        public Transform Rotated(float radians)
        {
            var res = this;
            res.Rotation += radians;
            return res;
        }

        [Obsolete("Don't use")]
        internal Matrix4 GetTranslationMatrix() 
        {
            Float2 pos = Position.ToScreenCoord();
            return Matrix4.CreateTranslation(pos.x, pos.y, 0f);
        }

        [Obsolete("Don't use")]
        internal Matrix4 GetScaleMatrix()
        {
            Float2 sca = Scale.ToScreenScale();
            return Matrix4.CreateScale(sca.x, sca.y, 0f);
        }

        [Obsolete("Don't use")]
        internal Matrix4 GetRotationMatrix() => Matrix4.CreateRotationZ(Rotation);




        public static bool operator ==(in Transform a, in Transform b)
        {
            return  a.Position == b.Position &&
                    a.Scale == b.Scale &&
                    MathUtil.Nearly(a.Rotation, b.Rotation);
        }


        public static bool operator !=(in Transform a, in Transform b) => !(a == b);


        public bool Equals(Transform other) => this == other;


        public override bool Equals(object obj) => (obj is Transform converted && Equals(converted));


        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Scale, Rotation);
        }


        /// <summary>
        /// Position = (0, 0)
        /// Scale = (1, 1)
        /// Rotation = 0
        /// </summary>
        public static readonly Transform Identity = new Transform(Float2.Zero, Float2.One, 0f);

        /// <summary>
        /// Position = (0, 0)
        /// Scale = (0, 0)
        /// Rotation = 0
        /// </summary>
        public static readonly Transform Zero = new Transform(Float2.Zero, Float2.Zero, 0f);
    }
}