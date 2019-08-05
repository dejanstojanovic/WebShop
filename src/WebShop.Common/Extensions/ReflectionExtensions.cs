using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WebShop.Common.Extensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Sets the property value through backing field insted through setter method
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void ForceSetValue(this PropertyInfo propertyInfo, object obj, object value)
        {
            var backingFieldInfo = obj.GetType().GetField($"<{propertyInfo.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            if (backingFieldInfo != null)
            {
                backingFieldInfo.SetValue(obj, value);
            }
        }
    }
}
