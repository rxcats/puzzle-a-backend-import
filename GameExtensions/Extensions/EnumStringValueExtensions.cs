using System;

namespace GameExtensions.Extensions
{
    public static class EnumStringValueExtensions
    {
        public static string GetStringAttr(this Enum value)
        {
            var type = value.GetType();

            var fieldInfo = type.GetField(value.ToString());

            if (fieldInfo == null) return string.Empty;

            var attributes = fieldInfo.GetCustomAttributes(typeof(StringAttr), false) as StringAttr[];

            return attributes?.Length > 0 ? attributes[0].Value : string.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    [Serializable]
    public class StringAttr : Attribute
    {
        public StringAttr(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}