using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace wykrywanie_otworkow_test
{
    class save_image
    {
        void SaveToBmp(FrameworkElement visual, string fileName)
        {
            var encoder = new BmpBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }
        public static void SaveToJpeg(FrameworkElement visual, string fileName)
        {
            DateTime data = DateTime.Now;
            string path = @"C://images" + @"//" + @data.Day + @"-" + @data.Month + @"-" + @data.Year + @"//";
            string footer = @"-" + @"(" + data.Day + @"-" + data.Month + @"-" + @data.Year + @" " + @data.Hour + @"-" + @data.Minute + @"-" + @data.Second + @").jpg";

            if (Directory.Exists(path))       //sprawdzanie czy sciezka istnieje
            {
                ;
            }
            else
                System.IO.Directory.CreateDirectory(path); //jeśli nie to ją tworzy


            var encoder = new JpegBitmapEncoder();
            SaveUsingEncoder(visual, path + fileName + footer, encoder);
        }

        public static void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            try
            {
            if ((int)visual.ActualWidth > 0 && (int)visual.ActualHeight > 0)
            {
                RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                System.Windows.Size visualSize = new System.Windows.Size(visual.ActualWidth, visual.ActualHeight);
                visual.Measure(visualSize);
                visual.Arrange(new Rect(visualSize));
                bitmap.Render(visual);
                BitmapFrame frame = BitmapFrame.Create(bitmap);
                encoder.Frames.Add(frame);

                using (var stream = File.Create(fileName))
                {
                    encoder.Save(stream);
                }
            }
            }
            catch
            {
            //    MessageBox.Show("Nie udało się zapisać zdjęcia");
            }
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

            using (StreamWriter sw = new StreamWriter("C:/tars/" + serial + "-" + "(" + stop.Day + "-" + stop.Month + "-" + stop.Year + " " + stop.Hour + "-" + stop.Minute + "-" + stop.Second + ")" + ".Tars"))
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
