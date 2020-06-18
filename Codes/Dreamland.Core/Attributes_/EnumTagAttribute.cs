using System;

namespace Dreamland.Core
{
    /// <summary>
    ///     枚举标记特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EnumTagAttribute : Attribute
    {
        /// <summary>
        ///     构造
        /// </summary>
        public EnumTagAttribute()
        {
        }

        /// <summary>
        ///     构造
        /// </summary>
        public EnumTagAttribute(string name, string description)
        {
            Name = name ?? string.Empty;
            Description = description ?? string.Empty;
        }

        /// <summary>
        ///     获取或设置字段或属性名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     获取或设置字段或属性描述。
        /// </summary>
        public string Description { get; set; }
    }
}