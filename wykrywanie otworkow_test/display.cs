using System;
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
    class display
    {

        public static double dp = 1;
        public static double minDist = 50;
        public static double param1 = 39;  //37
        public static double param2 = 31;  //29
        public static int minRadius = 13;
        public static int maxRadius = 19;
        public static double lower_H = 0, lower_S = 155, lower_V = 94;
        public static double high_H = 160, high_S = 255, high_V = 242;


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


    class plik
    {
        public static void tworzeniepliku(string serial)
        {
            string sciezka = ("C:/tars/");      //definiowanieścieżki do której zapisywane logi
            DateTime stop = DateTime.Now;
            if (Directory.Exists(sciezka))       //sprawdzanie czy sciezka istnieje
            {
                ;
            }
            else
                System.IO.Directory.CreateDirectory(sciezka); //jeśli nie to ją tworzy
            if (serial != null)
                serial = Regex.Replace(serial, @"\s+", string.Empty);

            using (StreamWriter sw = new StreamWriter("C:/tars/" + serial  + "-" + "(" + stop.Day + "-" + stop.Month + "-" + stop.Year + " " + stop.Hour + "-" + stop.Minute + "-" + stop.Second + ")" + ".Tars"))
            {


                sw.WriteLine("S{0}", serial);
                sw.WriteLine("CITRON");
                sw.WriteLine("NPLKWIM0T");
                sw.WriteLine("PQC_CAPE");
                sw.WriteLine("Ooperator");
                sw.WriteLine("TP");
                sw.WriteLine("[" + stop.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine("]" + stop.ToString("yyyy-MM-dd HH:mm:ss"));
                //for (int i = 0; i > 15; i++)
                //    result[i] = string.Empty;

            }

            string sourceFile = @"C:/tars/" + serial + @"-" + @"(" + @stop.Day + @"-" + @stop.Month + @"-" + @stop.Year + @" " + @stop.Hour + @"-" + @stop.Minute + @"-" + @stop.Second + @")" + @".Tars";
            string destinationFile = @"C:/copylogi/" + @stop.Day + @"-" + @stop.Month + @"-" + @stop.Year + @"/" + @serial + @"/" + @serial + @"-" + @"(" + @stop.Day + @"-" + @stop.Month + @"-" + @stop.Year + @" " + @stop.Hour + @"-" + @stop.Minute + @"-" + @stop.Second + @")" + @".Tars";

            if (Directory.Exists(@"C:/copylogi/" + @stop.Day + @"-" + @stop.Month + @"-" + @stop.Year + @"/" + @serial + @"/"))       //sprawdzanie czy sciezka istnieje
            {
                ;
            }
            else
                System.IO.Directory.CreateDirectory(@"C:/copylogi/" + @stop.Day + @"-" + @stop.Month + @"-" + @stop.Year + @"/" + @serial + @"/"); //jeśli nie to ją tworzy

            try
            {
                File.Copy(sourceFile, destinationFile, true);
            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message);
            }

        }
    }
}
