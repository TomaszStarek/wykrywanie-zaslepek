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
    class Save
    {
        public void SaveToBmp(FrameworkElement visual, string fileName)
        {
            var encoder = new BmpBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }
        public void SaveToPng(FrameworkElement visual, string fileName)
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

        public static async Task SendLogMesTisAsync(string serial)
        {
            DateTime stop = DateTime.Now;

            wsTis.MES_TISSoapClient ws = new wsTis.MES_TISSoapClient(wsTis.MES_TISSoapClient.EndpointConfiguration.MES_TISSoap);
            try
            {
                
                //var res = await ws.GetVersionAsync();
                //string ver = res.Body.GetVersionResult;

                StringBuilder sb = new StringBuilder();
                sb.Append("S" + serial + "\n");
                sb.Append("CITRON" + "\n");
                sb.Append("NPLKWIM0P25B2SQ1" + "\n");
                sb.Append("PQC_CAP" + "\n");
                sb.Append("Ooperator" + "\n");
                sb.Append("TP" + "\n");
                sb.Append("[" + stop.ToString("yyyy-MM-dd HH:mm:ss") + "\n");
                sb.Append("]" + stop.ToString("yyyy-MM-dd HH:mm:ss") + "\n");

                var res = await ws.ProcessTestDataAsync(sb.ToString(), "Generic");

                if (res != null && res.Body.ProcessTestDataResult.ToString().ToUpper() != "PASS")
                {
                    SaveLog(serial);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                await ws.CloseAsync();
            }

        }

        public static void SaveLog(string serial)
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
                sw.WriteLine("NPLKWIM0P25B2SQ1");
                sw.WriteLine("PQC_CAP");
                sw.WriteLine("Ooperator");
                sw.WriteLine("TP");
                sw.WriteLine("[" + stop.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine("]" + stop.ToString("yyyy-MM-dd HH:mm:ss"));
                //for (int i = 0; i > 15; i++)
                //    result[i] = string.Empty;

            }

            string sourceFile = @"C:/tars/" + serial + @"-" + @"(" + @stop.Day + @"-" + @stop.Month + @"-" + @stop.Year + @" " + @stop.Hour + @"-" + @stop.Minute + @"-" + @stop.Second + @")" + @".Tars";
            string destinationFile = @"C:/copylogi/" + @stop.Day + @"-" + @stop.Month + @"-" + @stop.Year + @"/" + @serial + @"-" + @"(" + @stop.Day + @"-" + @stop.Month + @"-" + @stop.Year + @" " + @stop.Hour + @"-" + @stop.Minute + @"-" + @stop.Second + @")" + @".Tars";

            if (Directory.Exists(@"C:/copylogi/" + @stop.Day + @"-" + @stop.Month + @"-" + @stop.Year + @"/"))       //sprawdzanie czy sciezka istnieje
            {
                ;
            }
            else
                System.IO.Directory.CreateDirectory(@"C:/copylogi/" + @stop.Day + @"-" + @stop.Month + @"-" + @stop.Year + @"/"); //jeśli nie to ją tworzy

            try
            {
                File.Copy(sourceFile, destinationFile, true);
            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message);
            }
        }

        public static void Save_param()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"parametry.txt"))
                {
                    sw.WriteLine(Parameters.dp.ToString());
                    sw.WriteLine(Parameters.minDist.ToString());
                    sw.WriteLine(Parameters.param1.ToString());
                    sw.WriteLine(Parameters.param2.ToString());
                    sw.WriteLine(Parameters.minRadius.ToString());
                    sw.WriteLine(Parameters.maxRadius.ToString());
                    sw.WriteLine(Parameters.lower_H.ToString());
                    sw.WriteLine(Parameters.lower_S.ToString());
                    sw.WriteLine(Parameters.lower_V.ToString());
                    sw.WriteLine(Parameters.high_H.ToString());
                    sw.WriteLine(Parameters.high_S.ToString());
                    sw.WriteLine(Parameters.high_V.ToString());
                    sw.WriteLine(Parameters.X_lcap_min.ToString());
                    sw.WriteLine(Parameters.X_lcap_max.ToString());
                    sw.WriteLine(Parameters.X_Pcap_min.ToString());
                    sw.WriteLine(Parameters.X_Pcap_max.ToString());

                    sw.Close();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("blad zapisu parametrow:" + ex);
            }

        }
        public static void Read_param()
        {
            string sciezka = (@"parametry.txt");



            try
            {
                using (StreamReader sr = new StreamReader(sciezka))
                {
                    int i = 0;
                    while (sr.Peek() >= 0)
                    {
                        switch (i)
                        {
                            case 0:
                                Parameters.dp = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 1:
                                Parameters.minDist = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 2:
                                Parameters.param1 = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 3:
                                Parameters.param2 = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 4:
                                Parameters.minRadius = Convert.ToInt32(sr.ReadLine());
                                break;
                            case 5:
                                Parameters.maxRadius = Convert.ToInt32(sr.ReadLine());
                                break;
                            case 6:
                                Parameters.lower_H = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 7:
                                Parameters.lower_S = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 8:
                                Parameters.lower_V = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 9:
                                Parameters.high_H = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 10:
                                Parameters.high_S = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 11:
                                Parameters.high_V = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 12:
                                Parameters.X_lcap_min = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 13:
                                Parameters.X_lcap_max = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 14:
                                Parameters.X_Pcap_min = Convert.ToDouble(sr.ReadLine());
                                break;
                            case 15:
                                Parameters.X_Pcap_max = Convert.ToDouble(sr.ReadLine());
                                break;

                            default:
                                break;
                        }

                        i++;



                    }
                    sr.Close();
                }
            
            }
            catch (Exception ex)
            {

                MessageBox.Show("blad odczytu parametrow:" + ex);
            }
}
    }
}
