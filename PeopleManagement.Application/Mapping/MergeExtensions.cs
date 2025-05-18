using System.Reflection;

namespace PeopleManagement.Application.Mapping;

public static class MergeExtensions
{
    public static void MergeInto<TTarget>(this object source, TTarget target)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (target == null) throw new ArgumentNullException(nameof(target));

        var sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var targetProperties = typeof(TTarget).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var targetProp in targetProperties)
        {
            if (!targetProp.CanWrite)
                continue;

            var sourceProp = sourceProperties.FirstOrDefault(p => p.Name == targetProp.Name
                                                               && p.PropertyType == targetProp.PropertyType);
            if (sourceProp == null)
                continue;

            var sourceValue = sourceProp.GetValue(source);
            var isDefault = IsDefaultValue(sourceValue, sourceProp.PropertyType);

            if (sourceValue != null && !isDefault)
            {
                targetProp.SetValue(target, sourceValue);
            }
        }
    }

    private static bool IsDefaultValue(object? value, Type type)
    {
        if (value == null)
            return true;

        if (type.IsValueType)
            return value.Equals(Activator.CreateInstance(type));

        return false;
    }
}
