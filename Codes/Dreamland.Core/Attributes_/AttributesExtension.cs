using System;
using System.Linq;

namespace Dreamland.Core
{
    /// <summary>
    ///     提供特性方法的拓展
    /// </summary>
    public static class AttributesExtension
    {
        /// <summary>
        ///     获取枚举值上的指定特性
        /// </summary>
        /// <typeparam name="TResult">要获取的<see cref="Attribute" />值的<see cref="Type" /></typeparam>
        /// <param name="enumValue">从哪一个<see cref="Enum" />获取<see cref="Attribute" />值</param>
        /// <param name="result">取得的<see cref="Attribute" />值</param>
        /// <returns></returns>
        public static bool TryGetCustomAttribute<TResult>(this Enum enumValue, out TResult result)
            where TResult : Attribute
        {
            result = enumValue.GetCustomAttribute<TResult>();
            return result != null;
        }

        /// <summary>
        ///     获取枚举值上的指定特性
        /// </summary>
        /// <typeparam name="TResult">要获取的<see cref="Attribute" />值的<see cref="Type" /></typeparam>
        /// <param name="enumValue">从哪一个<see cref="Enum" />获取<see cref="Attribute" />值</param>
        /// <returns>取得的<see cref="Attribute" />值</returns>
        public static TResult GetCustomAttribute<TResult>(this Enum enumValue) where TResult : Attribute
        {
            var type = enumValue.GetType();
            var fieldName = Enum.GetName(type, enumValue);
            var attributes = type.GetField(fieldName ?? string.Empty)?.GetCustomAttributes(false);
            return attributes?.FirstOrDefault(obj => obj.GetType() == typeof(TResult)) as TResult;
        }
    }
}