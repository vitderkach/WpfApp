using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    class TextComboBox: ComboBox
    {
        public TextComboBox()
            :base()
        {

        }

        public static readonly DependencyProperty CaptureProperty =
   DependencyProperty.Register("Capture", typeof(string), typeof(TextComboBox), new
      PropertyMetadata("", new PropertyChangedCallback(OnCaptureChanged)));

        public string Capture
        {
            get { return (string)GetValue(CaptureProperty); }
            set { SetValue(CaptureProperty, value); }
        }

        private static void OnCaptureChanged(DependencyObject d,
   DependencyPropertyChangedEventArgs e)
        {
            TextComboBox textComboBox = d as TextComboBox;
            textComboBox.OnCaptureChanged(e);
        }

        private void OnCaptureChanged(DependencyPropertyChangedEventArgs e)
        {
            Capture = e.NewValue.ToString();
        }

    }
}
