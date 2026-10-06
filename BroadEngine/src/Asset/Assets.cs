using System.Reflection;
using System.IO;
using System;
using System.Collections.Generic;

#pragma warning disable 8603

namespace Broad
{
    public static class Assets
    {
        readonly static Dictionary<string, AssetInfo> assetInfos = new Dictionary<string, AssetInfo>();


        /// <summary>
        /// アセットを読み込む
        /// </summary>
        /// <param name="address">アセットのアドレス</param>
        /// <returns>読み込んだアセット</returns>
        public static T Load<T>(string address) where T: Asset
        {
            if (assetInfos.TryGetValue(address, out var info)) {
                info.Use();
                if (!info.IsLoaded) {
                    var asset = info.Load();
                    info.Asset = asset;
                    return asset as T;
                }
                return info.Asset as T;
            }
            throw new Exception($"アセット{address}は登録されていません");
        }


        /// <summary>
        /// アセットを登録
        /// </summary>
        /// <param name="infos">アセット情報</param>
        public static void RegisterAssets(params AssetInfo[] infos)
        {
            foreach (var info in infos) {
                assetInfos.Add(info.Name, info);
            }
        }


        /// <summary>
        /// 未使用のアセットを破棄
        /// </summary>
        public static void UnloadUnusedAssets()
        {
            foreach (var info in assetInfos.Values) {
                info.TryUnload();
            }
        }
    }
}
