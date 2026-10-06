using System;
using System.Collections.Generic;
using System.IO;
using MathKit;


namespace Broad
{
    public static class CsvUtil
    {
        /// <summary>
        /// csvファイルを読み込む
        /// </summary>
        /// <param name="path">ファイルのパス</param>
        /// <returns>読み込んだCSV</returns>
        public static CSV<int> LoadInt(string path)
        {
            using var reader = new StreamReader(path);
            var data = new List<int>();
            var length = Int2.Zero;

            while (!reader.EndOfStream) {
                var line = reader.ReadLine().Split(",");
                length.x = line.Length;
                foreach (var d in line) {
                    data.Add(int.Parse(d));
                }
                length.y++;
            }

            return new CSV<int>(data.ToArray(), length);
        }
    }
}