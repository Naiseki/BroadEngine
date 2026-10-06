using System;
using System.Net;
using System.IO;
using System.Text.Json;

namespace Broad
{
    public static class JsonUtil
    {
        static JsonSerializerOptions option = new JsonSerializerOptions { WriteIndented = true };


        /// <summary>
        /// データをシリアライズ
        /// </summary>
        /// <param name="data">シリアライズするデータ</param>
        /// <typeparam name="T">データの型</typeparam>
        /// <returns>シリアライズした文字列</returns>
        public static string Serialeze<T>(T data)
        {
            return JsonSerializer.Serialize(data, option);
        }


        /// <summary>
        /// データをjsonとして保存
        /// </summary>
        /// <param name="path">保存先</param>
        /// <param name="data">保存するデータ</param>
        /// <typeparam name="T">データの型</typeparam>
        public static void Save<T>(string path, T data)
        {
            File.WriteAllText(path, Serialeze(data));
        }


        /// <summary>
        /// データをjsonとして保存
        /// </summary>
        /// <param name="path">保存先</param>
        /// <param name="data">シリアライズ済みのデータ</param>
        public static void Save(string path, string data)
        {
            File.WriteAllText(path, data);
        }


        /// <summary>
        /// JSONをデシリアライズ
        /// </summary>
        /// <param name="jsonString">jsonの文字列</param>
        /// <typeparam name="T">デシリアライズ後の型</typeparam>
        /// <returns>デシリアライズ済みのデータ</returns>
        public static T Deserialze<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }


        /// <summary>
        /// jsonファイルを読み込んでデシリアライズ
        /// </summary>
        /// <param name="path">ファイルのパス</param>
        /// <typeparam name="T">出力の型</typeparam>
        /// <returns>デシリアライズ済みのデータ</returns>
        public static T Load<T>(string path)
        {
            return Deserialze<T>(File.ReadAllText(path));
        }

        /// <summary>
        /// jsonファイルを読み込んでデシリアライズ
        /// </summary>
        /// <param name="stream">ストリーム</param>
        /// <typeparam name="T">出力の型</typeparam>
        /// <returns>デシリアライズ済みのデータ</returns>
        public static T Load<T>(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return Deserialze<T>(reader.ReadToEnd());
        }
    }
}