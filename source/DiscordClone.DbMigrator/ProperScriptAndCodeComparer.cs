using System.ComponentModel;
using System.Reflection;

namespace DiscordClone.DbMigrator;

/// <summary>
/// CSharp classes are not allowed to start with a number. This messes up the ordering of DbUp's
/// migration scripts. This custom sorter fixes that.
/// </summary>
public class ProperScriptAndCodeComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        var xx = x!.EndsWith(".cs") ? GetFullScriptName(x) : x;
        var yy = y!.EndsWith(".cs") ? GetFullScriptName(y) : y;
        return StringComparer.Ordinal.Compare(xx, yy);
    }

    private static string GetFullScriptName(string name)
    {
        var assembly = Assembly.GetAssembly(typeof(ProperScriptAndCodeComparer))!;
        var type = assembly.GetType(name.Replace(".cs", string.Empty))!;
        var attribute = type.GetCustomAttribute<DescriptionAttribute>()!;
        return $"{type.Namespace}.{attribute.Description}";
    }
}
