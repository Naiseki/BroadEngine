using System;
using OpenTK.Audio.OpenAL;
using MathKit;


public enum AudioDistanceModel
{
        None = 0,
        InverseDistance = 53249,
        InverseDistanceClamped = 53250,
        LinearDistance = 53251,
        LinearDistanceClamped = 53252,
        ExponentDistance = 53253,
        ExponentDistanceClamped = 53254
}


public static class AudioUtil
{
    public static void SetDistanceModel(AudioDistanceModel model)
    {
        AL.DistanceModel((ALDistanceModel)model);
    }


    public static AudioDistanceModel GetDistanceModel()
    {
        return (AudioDistanceModel)AL.GetDistanceModel();
    }


    public static void SetListenerPosition(Float2 position)
    {
        AL.Listener(ALListener3f.Position, position.x, position.y, 0f);
    }


    public static Float2 GetListenerPosition()
    {
        AL.GetListener(ALListener3f.Position, out var pos);
        return new Float2(pos.X, pos.Y);
    }


    public static void SetListenerGain(float gain)
    {
        AL.Listener(ALListenerf.Gain, gain);
    }


    public static float GetListenerGain()
    {
        AL.GetListener(ALListenerf.Gain, out float gain);
        return gain;
    }
}