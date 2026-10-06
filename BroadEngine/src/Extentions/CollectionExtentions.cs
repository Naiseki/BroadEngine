using System.Collections.Generic;


namespace Broad
{
    public static class ListExtentions
    {
        /// <summary>
        /// 要素の削除を試みる
        /// </summary>
        /// <param name="list">リスト</param>
        /// <param name="element">削除したい要素</param>
        /// <typeparam name="T">リスト要素の型</typeparam>
        /// <returns>削除に成功したかどうか</returns>
        public static bool TryRemove<T>(this List<T> list, T element)
        {
            int x = list.IndexOf(element);
            if (x >= 0) {
                list.RemoveAt(x);
                return true;
            }
            return false;
        }
    }



    public static class DictionaryExtentions
    {
        /// <summary>
        /// 要素の削除を試みる
        /// </summary>
        /// <param name="dic">ディクショナリ</param>
        /// <param name="key">削除したい要素のキー</param>
        /// <returns>削除に成功したかどうか</returns>
        public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            if (dic.ContainsKey(key)) {
                dic.Remove(key);
                return true;
            }
            return false;
        }
    }
}
