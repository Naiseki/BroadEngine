using System;
using OpenTK.Audio.OpenAL;
using MathKit;

namespace Broad.Audio
{
    /// <summary>
    /// 音を鳴らす
    /// </summary>
    public class Speaker: Component
    {
        readonly Sound audio;
        readonly int sourceHandle;
        Float2 pos;
        bool isSpatial = true;
        bool disposed = false;
        

        /// <summary>
        /// 位置
        /// </summary>
        public Float2 Position {
            get => pos;
            set {
                pos = value; 
                if (isSpatial) 
                    AL.Source(sourceHandle, ALSource3f.Position, value.x, value.y, 0f);   
            }
        }

        /// <summary>
        /// 速度
        /// </summary>
        public Float2 Velocity {
            get {
                 AL.GetSource(sourceHandle, ALSource3f.Velocity, out var val);
                 return new Float2(val.X, val.Y);
            }
            set {
                AL.Source(sourceHandle, ALSource3f.Velocity, value.x, value.y, 0f);    
            }
        }

        /// <summary>
        /// 音量
        /// </summary>
        public float Volume {
            get {
                AL.GetSource(sourceHandle, ALSourcef.Gain, out float val);
                return val;
            }
            set {
                AL.Source(sourceHandle, ALSourcef.Gain, value);
            }
        }

        /// <summary>
        /// 現在の再生位置
        /// </summary>
        public float CurrentTime {
            get {
                 AL.GetSource(sourceHandle, ALSourcef.SecOffset, out float val);
                 return val;
            }
            set {
                AL.Source(sourceHandle, ALSourcef.SecOffset, Math.Clamp(value, 0f, audio.Seconds));
            }
        }

        /// <summary>
        /// ループさせるかどうか
        /// </summary>
        public bool Looping {
            get {
                AL.GetSource(sourceHandle, ALSourceb.Looping, out bool val);
                return val;
            }
            set {
                AL.Source(sourceHandle, ALSourceb.Looping, value);
            }
        }

        /// <summary>
        /// 立体音響にするかどうか
        /// </summary>
        public bool IsSpatial {
            get => isSpatial;
            set => SetSpatial(value);
        }

        public float MaxDistance {
            get {
                AL.GetSource(sourceHandle, ALSourcef.MaxDistance, out float val);
                return val;
            }
            set => AL.Source(sourceHandle, ALSourcef.MaxDistance, value);
        }

        /// <summary>
        /// ピッチ
        /// </summary>
        public float Pitch {
            get {
                AL.GetSource(sourceHandle, ALSourcef.Pitch, out float val);
                return val;
            }
            set => AL.Source(sourceHandle, ALSourcef.Pitch, value);
        }

        public float RolloffFactor {
            get {
                AL.GetSource(sourceHandle, ALSourcef.RolloffFactor, out float val);
                return val;
            }
            set => AL.Source(sourceHandle, ALSourcef.RolloffFactor, value);
        }

        public float RefDistance {
            get {
                AL.GetSource(sourceHandle, ALSourcef.ReferenceDistance, out float val);
                return val;
            }
            set => AL.Source(sourceHandle, ALSourcef.ReferenceDistance, value);
        }

        public float RoomRolloffFactor {
            get {
                AL.GetSource(sourceHandle, ALSourcef.EfxRoomRolloffFactor, out float val);
                return val;
            }
            set => AL.Source(sourceHandle, ALSourcef.EfxRoomRolloffFactor, value);
        }

        public float ConeOuterGainHighFrequency {
            get {
                AL.GetSource(sourceHandle, ALSourcef.EfxConeOuterGainHighFrequency, out float val);
                return val;
            }
            set => AL.Source(sourceHandle, ALSourcef.EfxConeOuterGainHighFrequency, value);
        }

        public float AirAbsorptionFactor {
            get {
                AL.GetSource(sourceHandle, ALSourcef.EfxAirAbsorptionFactor, out float val);
                return val;
            }
            set => AL.Source(sourceHandle, ALSourcef.EfxAirAbsorptionFactor, value);
        }

        public bool IsPlaying => (AL.GetSourceState(sourceHandle) == ALSourceState.Playing); 


        /// <param name="audio">オーディオ</param>
        public Speaker(Sound audio)
        {
            this.audio = audio;
            sourceHandle = AL.GenSource();
            AL.Source(sourceHandle, ALSourcei.Buffer, audio.Handle);
        }


        /// <param name="audioName">オーディオ名</param>
        public Speaker(string audioName):
            this(Assets.Load<Sound>(audioName)) {}



        /// <summary>
        /// オーディオを再生
        /// </summary>
        public void Play()
        {
            AL.SourcePlay(sourceHandle);
        }


        /// <summary>
        /// オーディオを停止
        /// </summary>
        public void Stop()
        {
            AL.SourceStop(sourceHandle);
        }


        /// <summary>
        /// オーディオを一時停止
        /// </summary>
        public void Pause()
        {
            AL.SourcePause(sourceHandle);
        }

        /// <summary>
        /// リソースを破棄
        /// </summary>
        public void Dispose()
        {
            if (disposed)
                return;
            Stop();
            audio.Release();
            AL.DeleteSource(sourceHandle);
            disposed = true;
        }


        void SetSpatial(bool spatial)
        {
            if (spatial) {
                AL.Source(sourceHandle, ALSourceb.SourceRelative, false);
                isSpatial = true;
                Position = pos;
            }
            else {
                Position = Float2.Zero;
                AL.Source(sourceHandle, ALSourceb.SourceRelative, true);
                isSpatial = false;
            }
        }


        protected internal override void OnUpdate(Actor actor)
        {
            Position = actor.Form.Position;
        }


        protected internal override void OnDestroy(Actor actor)
        {
            Dispose();
        }


        ~Speaker() => Dispose();
    }
}
