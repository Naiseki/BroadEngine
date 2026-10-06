using System;
using OpenTK.Audio.OpenAL;
using OpenTK.Audio.OpenAL.Extensions.Creative.EFX;
using MathKit;
using OpenTK.Mathematics;

namespace Broad.Audio
{
    static class ALProgram
    {
        static ALContext context;
        static ALDevice device;

        //初期化処理
        internal static void Init()
        {
            device = ALC.OpenDevice(null);
            context = ALC.CreateContext(device, (int[])null);
            ALC.MakeContextCurrent(context);
            AL.DistanceModel(ALDistanceModel.LinearDistanceClamped);
        }


        internal static void Deinit()
        {
            ALC.DestroyContext(context);
            ALC.CloseDevice(device);
        }
    }


    public static class AudioListener
    {
        public static Float2 Position {
            get { 
                AL.GetListener(ALListener3f.Position, out Vector3 pos);
                return new Float2(pos.X, pos.Y);
            }
            set { 
                AL.Listener(ALListener3f.Position, value.x, value.y, 0f);
            }
        }

        public static void SetPosition(Float2 position)
        {
            AL.Listener(ALListener3f.Position, position.x, position.y, 0f);
        }
    }
}