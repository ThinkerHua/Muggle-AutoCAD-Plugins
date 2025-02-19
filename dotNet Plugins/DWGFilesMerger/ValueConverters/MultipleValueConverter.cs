using System;
using System.Globalization;
using System.Windows.Data;

namespace Muggle.AutoCADPlugins.DWGFilesMerger.ValueConverters {
    public class MultipleValueConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((double) value) * ((double) parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (parameter.Equals(0)) return value;

            return ((double) value) / ((double) parameter);
        }
    }
}
