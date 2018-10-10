using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Wpf_UI
{
    public class SelectedItemToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
               // bool boolValue = (bool)value;
               // if (boolValue)
               // {
               //     return Visibility.Collapsed;
               // }
               // else
               // {
               //     return Visibility.Visible;
               // }

                     BE.Payment payment = (BE.Payment)Enum.Parse(typeof(BE.Payment), (string)value);
                    
                     if (payment == BE.Payment.HOURLY)
                     {
                         return true;
                     }
                     else
                         return false;

                    //BE.Payment payment = (BE.Payment)Enum.Parse(typeof(BE.Payment), (string)value);
                    //switch (payment)
                    //{
                    //    case BE.Payment.HOURLY:
                    //        return Visibility.Visible;                  //"true";
                    //    case BE.Payment.MONTHLY:
                    //        return Visibility.Hidden;                                //"false";
                    //    default:
                    //        return "false";
                    //}
                    // BE.Child child = value as BE.Child;
                    // return ((value as BE.Child).Id).ToString();
                    //
                    // if (child != null)
                    // {
                    //
                    //     return child.IdAndName;//        ToString();
                    // }
                    // else
                    // {
                    //     return "";
                    // }
                }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
