using NVorbis;
using OpenTK.Audio.OpenAL;

namespace Broad.Audio
{
    /// <summary>
    /// オーディオデータ
    /// </summary>
    public class Sound: Asset
    {
        /// <summary>
        /// OpenALのバッファハンドル
        /// </summary>
        public readonly int Handle;

        /// <summary>
        /// トータルの秒数
        /// </summary>
        public readonly float Seconds;

        bool disposed = false;

        /// <summary>
        /// アセット情報
        /// </summary>
        public override AssetInfo Info { get; protected set; }


        Sound(float[] waveData, float totalSeconds, int sampleRate, AssetInfo info)
        {
            Seconds = totalSeconds;
            AL.GetError();
            Handle = AL.GenBuffer();
            AL.BufferData(Handle, ALFormat.MonoFloat32Ext, waveData, sampleRate);
            Info = info;
        }


        /// <summary>
        /// <see cref="Speaker"/>を作成
        /// </summary>
        /// <returns>作成した<see cref="Speaker"/></returns>
        public Speaker CreateSpeaker() => new Speaker(this);


        internal static Sound Load(AudioInfo info)
        {
            using var stream = info.CreateStream();
            using var reader = new VorbisReader(stream);
            float[] waveData = new float[reader.TotalSamples * reader.Channels];
            reader.ReadSamples(waveData);
            if (reader.Channels == 2) {
                waveData = ToMonoral(waveData);
            }
            return new Sound(waveData, (float)reader.TotalTime.TotalSeconds, reader.SampleRate, info);
        }


        static  float[] ToMonoral(float[] waveData)
        {
            int len = waveData.Length;
            float[] clippedWaveData = new float[len / 2];
            for (int i = 0; i < len; i += 2) {
                clippedWaveData[i / 2] = (waveData[i] + waveData[i + 1]) / 2;
            }
            return clippedWaveData;
        }


        /// <summary>
        /// リソースを破棄
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;
            AL.DeleteBuffer(Handle);  
            disposed = true;
        } 
    }
}