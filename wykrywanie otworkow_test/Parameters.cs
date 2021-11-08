using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace wykrywanie_otworkow_test
{

    class Parameters
    {

        public static double dp = 1;
        public static double minDist = 50;
        public static double param1 = 39;  //37
        public static double param2 = 31;  //29
        public static int minRadius = 13;
        public static int maxRadius = 19;
        public static double lower_H = 0, lower_S = 158, lower_V = 94;
        public static double high_H = 160, high_S = 255, high_V = 242;
        public static double X_lcap_min = 134;
        public static double X_lcap_max = 148;
        public static double X_Pcap_min = 380;
        public static double X_Pcap_max = 394;

        public static double labelparam_convert(object sender)
        {
            Label control = (Label)sender;
            double result = 0;
            try
            {
                result = Convert.ToDouble(control.Content);
            }
            catch
            {
                ;
            }
            return result;
        }
    }



}
