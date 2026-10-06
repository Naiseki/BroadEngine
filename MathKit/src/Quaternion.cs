using System;

namespace MathKit
{
    public struct Quaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;


        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }


        public void Set(float newX, float newY, float newZ, float newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }


        public static float Dot(in Quaternion a, in Quaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }


        public static Quaternion operator *(in Quaternion a, in Quaternion b)
        {
            return new Quaternion(
                a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
                a.w * b.y + a.y * b.w + a.z * b.x - a.x * b.z,
                a.w * b.z + a.z * b.w + a.x * b.y - a.y * b.x,
                a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
            );
        }



        public static Float3 operator *(Quaternion rot, Float3 point)
        {
            float x = rot.x * 2f;
            float y = rot.y * 2f;
            float z = rot.z * 2f;
            float xx = rot.x * x;
            float yy = rot.y * y;
            float zz = rot.z * z;
            float xy = rot.x * y;
            float xz = rot.x * z;
            float yz = rot.y * z;
            float wx = rot.w * x;
            float wy = rot.w * y;
            float wz = rot.w * z;

            Float3 res;
            res.x = (1f - (yy + zz)) * point.x + (xy - wz) * point.y + (xz + wy) * point.z;
            res.y = (xy + wz) * point.x + (1f - (xx + zz)) * point.y + (yz - wx) * point.z;
            res.z = (xz - wy) * point.x + (yz + wx) * point.y + (1f - (xx + yy)) * point.z;
            return res;
        }



    }
}
