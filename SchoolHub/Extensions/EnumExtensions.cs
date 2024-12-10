using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SchoolHub.Mvc.Extensions;

public static class EnumExtensions
{
    private static Dictionary<Type, dynamic> Values { get; set; } = new Dictionary<Type, dynamic>();

    public static string GetDisplayName<T>(this T enumerador) where T : struct, IConvertible
    {
        var values = GetEnumNames<T>();
        return values.ContainsKey(enumerador) ? values[enumerador] : string.Empty;
    }

    public static Dictionary<T, string> GetEnumNames<T>() where T : struct, IConvertible
    {
        var type = typeof(T);
        if (!Values.ContainsKey(type))
        {
            var values = new Dictionary<T, string>();
            var enumeradores = Enum.GetValues(type).Cast<T>();
            foreach (var enumerador in enumeradores)
            {
                var name = Enum.GetName(typeof(T), enumerador);
                var display = type.GetMember(name).First().GetCustomAttribute<DisplayAttribute>();
                if (display != null)
                {
                    name = display.Name;
                }
                values.Add(enumerador, name);
            }
            Values.Add(type, values);
        }
        return Values[type] as Dictionary<T, string>;
    }
}
