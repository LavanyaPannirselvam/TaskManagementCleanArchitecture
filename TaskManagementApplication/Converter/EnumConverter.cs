using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;
using Windows.UI.Xaml.Data;

namespace TaskManagementCleanArchitecture.Converter
{
    public class EnumConverter : IValueConverter
    {
        static TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        
        public static List<string> EnumToStringConverter(Type enumType)
        {
            List<string> enumList = new List<string>();
            if (enumType.IsEnum)
            {
               var items = enumType.GetEnumValues();
                foreach (var item in items)
                {
                    enumList.Add(ti.ToTitleCase(item.ToString().Replace("_", " ").ToLowerInvariant()));
                }
                return enumList;
            }
            else return null;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Enum enumType)
            {
                var items = enumType.GetType().GetEnumValues();
                foreach (var item in items)
                {
                    if (value.Equals(item))
                    {
                        return ti.ToTitleCase(item.ToString().Replace("_", " ").ToLowerInvariant());
                    }
                }
                return null;
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (targetType.IsEnum)
            {
                var items = targetType.GetEnumValues();
                value = value.ToString().ToUpper().Replace(" ", "");
                foreach (var item in items)
                {
                    if (item.ToString().Replace("_", "").Equals(value))
                        return (Enum)item;
                }
                return null;
            }
            else return null;
        }
    }
}
//_myTI.ToTitleCase(menu.ToString().Replace("_", " ").ToLowerInvariant()))