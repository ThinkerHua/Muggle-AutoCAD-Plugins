using System;
using System.Globalization;
using System.Windows.Data;
using Muggle.AutoCADPlugins.DWGFilesMerger.Model;

namespace Muggle.AutoCADPlugins.DWGFilesMerger.ValueConverters {
    public class TagEnableMultiValueConverter : IMultiValueConverter {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values[0].Equals(WayOfMergerEnum.OriginalPosition) || (int) values[1] <= 0)
                return false;

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
