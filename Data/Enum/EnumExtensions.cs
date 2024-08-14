using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        if (value == null)
            return string.Empty;

        FieldInfo field = value.GetType().GetField(value.ToString())!;

        if (field != null)
        {
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            if (attribute != null)
                return attribute.Description;
        }

        return value.ToString(); // Fallback to the enum name if no description is found
    }
}
