/*==============================================================================
 *  Muggle AutoCAD-Plugins - tools and plugins for AutoCAD
 *
 *  Copyright © 2024 Huang YongXing.                 
 *
 *  This library is free software, licensed under the terms of the GNU 
 *  General Public License as published by the Free Software Foundation, 
 *  either version 3 of the License, or (at your option) any later version. 
 *  You should have received a copy of the GNU General Public License 
 *  along with this program. If not, see <http://www.gnu.org/licenses/>. 
 *==============================================================================
 *  NumericUpDown.xaml.cs: code behind for NumericUpDown user control
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Muggle.AutoCADPlugins.DWGFilesMerger.Controls {
    /// <summary>
    /// NumericUpDown.xaml 的交互逻辑
    /// </summary>
    public partial class NumericUpDown : UserControl {
        /// <summary>
        /// Initializes a new instance of the NumericUpDownControl.
        /// </summary>
        public NumericUpDown() {
            InitializeComponent();
            MouseWheel += WhenMouseWheel;
            tBox.PreviewKeyDown += WhenArrowKeyDown;
        }

        [Category("Data"), Description("控件的最大值。")]
        public Decimal Maximum {
            get { return (Decimal) GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(Decimal), typeof(NumericUpDown), new PropertyMetadata(100m));


        [Category("Data"), Description("控件的最小值。")]
        public decimal Minimum {
            get { return (decimal) GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimun.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(0m));

        /// <summary>
        /// 每单击一下按钮时增加或减少的数量。
        /// </summary>
        [Category("Data"), Description("每单击一下按钮时增加或减少的数量。")]
        public decimal Increment {
            get { return (decimal) GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Increment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(1m));

        [Category("Appearance"), Description("文本在编辑框中的水平对齐方式。")]
        public TextAlignment TextHorizontalAlignment {
            get { return (TextAlignment) GetValue(TextHorizontalAlignmentProperty); }
            set { SetValue(TextHorizontalAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextAlign.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextHorizontalAlignmentProperty =
            DependencyProperty.Register("TextHorizontalAlignment", typeof(TextAlignment), typeof(NumericUpDown), new PropertyMetadata(TextAlignment.Right));

        [Category("Appearance"), Description("文本在编辑框中的垂直对齐方式。")]
        public VerticalAlignment TextVerticalAlignment {
            get { return (VerticalAlignment) GetValue(TextVerticalAlignmentProperty); }
            set { SetValue(TextVerticalAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextVerticalAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextVerticalAlignmentProperty =
            DependencyProperty.Register("TextVerticalAlignment", typeof(VerticalAlignment), typeof(NumericUpDown), new PropertyMetadata(VerticalAlignment.Center));

        /// <summary>
        /// 控件当前的值
        /// </summary>
        [Category("Data"), Description("控件当前的值。")]
        public decimal Value {
            get { return (decimal) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value", typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(0m, new PropertyChangedCallback(OnValueChanged),
                                              new CoerceValueCallback(CoerceValue)));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
            NumericUpDown control = (NumericUpDown) obj;

            RoutedPropertyChangedEventArgs<decimal> e = new RoutedPropertyChangedEventArgs<decimal>(
                (decimal) args.OldValue, (decimal) args.NewValue, ValueChangedEvent);
            control.OnValueChanged(e);
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> args) {
            RaiseEvent(args);
        }

        private static object CoerceValue(DependencyObject element, object value) {
            decimal newValue = (decimal) value;
            NumericUpDown control = (NumericUpDown) element;

            newValue = Math.Max(control.Minimum, Math.Min(control.Maximum, newValue));

            return newValue;
        }

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }


        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<decimal>), typeof(NumericUpDown));

        private void Increase(object sender, EventArgs e) {
            Value += Increment;
        }

        private void Decrease(object sender, EventArgs e) {
            Value -= Increment;
        }

        private void WhenMouseWheel(object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0)
                Increase(sender, e);
            else if (e.Delta < 0)
                Decrease(sender, e);
        }

        private void WhenArrowKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Up)
                Increase(sender, e);
            else if (e.Key == Key.Down)
                Decrease(sender, e);
        }
    }
}
