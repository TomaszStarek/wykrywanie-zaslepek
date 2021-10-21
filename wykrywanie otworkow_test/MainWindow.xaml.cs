using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Windows.Threading;
using System.Windows.Interop;
using Emgu.CV.CvEnum;
using System.Threading;
using Emgu.CV.Util;
using System.Timers;

namespace wykrywanie_otworkow_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        VideoCapture _capture;
        private Mat _frame;
        string barcode = "";
        bool[] circle_detected = new bool[2];
        bool manual_log_enabled;

        private static System.Timers.Timer aTimer;

        //private void ProcessFrame(object sender, EventArgs e)
        //{

        //}



        public MainWindow()
        {
            InitializeComponent();
            SetTimer();
            aTimer.Stop();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
          //  _streaming = false;
            _capture = new VideoCapture();
        }

        private void SetTimer()
        {
            // Create a timer with a ONE second interval.
            aTimer = new System.Timers.Timer(5000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }
        // bool flaga; 
        TimeSpan result;
        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            manual_log_enabled = true;
            Dispatcher.Invoke(new Action(() => label_state.Content = "  2.Zamontuj SECURITY CAP,\r\n wykrywanie trwa, sprawdź ich obecność! \n Aby nadać ręcznie loga naciśnij Enter"));
            Dispatcher.Invoke(new Action(() => label_state.Background = System.Windows.Media.Brushes.Red));
        }




        private void button_Click(object sender, RoutedEventArgs e)
        {
          //  image_capture.Source = image_stream.Source;
            save_image.SaveToJpeg(image_capture, barcode + ".jpg"); //C://tars//image.jpg
        }


        //////public void SaveToJpeg(FrameworkElement visual, string fileName)
        //////{
        //////    var encoder = new JpegBitmapEncoder();
        //////    //     SaveUsingEncoder(visual, fileName, encoder);
        //////    SaveUsingEncoder(visual, fileName, encoder);

        //////}

        //////private static void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        //////{
        //////    RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
        //////    System.Windows.Size visualSize = new System.Windows.Size(visual.ActualWidth, visual.ActualHeight);
        //////    visual.Measure(visualSize);
        //////    visual.Arrange(new Rect(visualSize));
        //////    bitmap.Render(visual);
        //////    BitmapFrame frame = BitmapFrame.Create(bitmap);
        //////    encoder.Frames.Add(frame);

        //////    using (var stream = File.Create(fileName))
        //////    {
        //////        encoder.Save(stream);
        //////    }
        //////}



 //       bool _streaming;
        private void btn_stream_Click(object sender, RoutedEventArgs e)
        {

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Window1))
                {
                    window.Close();
                }
            }

            Window1 win1 = new Window1();
            win1.Show();

            MessageBox.Show(win1.label_PIS.Content.ToString());
            

            ////////////////if (!_streaming)
            ////////////////{
            ////////////////    ComponentDispatcher.ThreadIdle += streaming;
            ////////////////    //System.Windows.Interop.ComponentDispatcher.ThreadIdle += (_, __) =>
            ////////////////    //{
            ////////////////    //    streaming(this, EventArgs.Empty);
            ////////////////    //};
            ////////////////}
            ////////////////else
            ////////////////{
            ////////////////    ComponentDispatcher.ThreadIdle -= streaming;
            ////////////////    //System.Windows.Interop.ComponentDispatcher.ThreadIdle -= (_, __) =>
            ////////////////    //{
            ////////////////    //    streaming(this, EventArgs.Empty);
            ////////////////    //};
            ////////////////}
            ////////////////_streaming = !_streaming;
        }


        private void run_streaming()
        {
                ComponentDispatcher.ThreadIdle += streaming;
        }
        private void stop_streaming()
        {
                ComponentDispatcher.ThreadIdle -= streaming;
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if(manual_log_enabled)
                {
                    manual_log_enabled = false;
                    circle_detected[0] = circle_detected[1] = false;
                    save_image.SaveToJpeg(image_capture, barcode);
                    plik.tworzeniepliku(@barcode);

                    Dispatcher.Invoke(new Action(() => label_state.Content = "Nadano ręcznie loga, zeskanuj kolejną płytę"));
                    Dispatcher.Invoke(new Action(() => label_state.Background = System.Windows.Media.Brushes.LawnGreen));
                    Dispatcher.Invoke(new Action(() => textBox.Text = ""));
                    Dispatcher.Invoke(new Action(() => textBox.IsReadOnly = false));
                    Dispatcher.Invoke(new Action(() => textBox.Focus()));
                    Dispatcher.Invoke(new Action(() => textBox.SelectAll()));
                    Dispatcher.Invoke(new Action(() => Podaj_barkode.Content = "Zeskanuj kolejną płytę"));
                    Dispatcher.Invoke(new Action(() => l_n_wykryto_lewa.Visibility = Visibility.Hidden));
                    Dispatcher.Invoke(new Action(() => l_n_wykryto_prawa.Visibility = Visibility.Hidden));
                    return;
                }

                Dispatcher.Invoke(new Action(() => label_state.Background = System.Windows.Media.Brushes.White));
                barcode = textBox.Text;

                if (barcode.Length == 27 && barcode[0] == '1')
                {
                    manual_log_enabled = false;
                    aTimer.Start();
                    Dispatcher.Invoke(new Action(() => label_state.Content = "  2.Zamontuj SECURITY CAP,\r\n     czekam na ich wykrycie"));
                    Dispatcher.Invoke(new Action(() => Podaj_barkode.Content = "Zeskanowany numer seryjny:"));
                    textBox.IsReadOnly = true;
                    run_streaming();
                }
                else
                {
                    Dispatcher.Invoke(new Action(() => label_state.Content = "1.Podaj poprawny barkode!"));
                    barcode = textBox.Text = string.Empty;

                }
                    
            }
        }


        private void streaming(object sender, System.EventArgs e)
        {

            using (Image<Hsv, byte> frame = _capture.QueryFrame().ToImage<Hsv, byte>())
            using (UMat gray = new UMat())
            using (Mat triangleRectangleImage = new Mat(frame.Size, DepthType.Cv8U, 3)) //image to draw triangles and rectangles on
            using (Mat circleImage = new Mat(frame.Size, DepthType.Cv8U, 3)) //image to draw circles on
            using (Mat lineImage = new Mat(frame.Size, DepthType.Cv8U, 3)) //image to drtaw lines on
            {

                Hsv lowerLimit = new Hsv(display.lower_H, display.lower_S, display.lower_V);
                Hsv upperLimit = new Hsv(display.high_H, display.high_S, display.high_V);



                Image<Gray, byte> imageHSVDest = frame.InRange(lowerLimit, upperLimit);
                CvInvoke.GaussianBlur(imageHSVDest, imageHSVDest, new System.Drawing.Size(7, 7), 2, 2);

                Image<Gray, byte> imageHSVDest_revert = imageHSVDest.Not();




                #region circle detection

                CircleF[] circles = CvInvoke.HoughCircles(imageHSVDest, HoughModes.Gradient, display.dp, display.minDist, display.param1, display.param2, display.minRadius, display.maxRadius);
                #endregion

                #region draw circles
                circleImage.SetTo(new MCvScalar(0));
                foreach (CircleF circle in circles)
                {
                    CvInvoke.Circle(frame, System.Drawing.Point.Round(circle.Center), (int)circle.Radius + 20,
                    new Bgr(System.Drawing.Color.White).MCvScalar, 10);
                    // MessageBox.Show("znaleziono okrag");
                    CvInvoke.PutText(frame, System.Drawing.Point.Round(circle.Center).ToString() + "radius: " + circle.Radius.ToString(), System.Drawing.Point.Round(circle.Center), FontFace.HersheyDuplex, 0.5,
                                    new MCvScalar(255, 0, 0));

                    if (circle.Center.X > 138 && circle.Center.X < 148)
                    {
                        PointF lewa_zaslepka = new PointF(40.5F, 450F);

                        CvInvoke.PutText(frame, "Lewa wykryta", System.Drawing.Point.Round(lewa_zaslepka),
                        FontFace.HersheyDuplex, 0.8, new MCvScalar(255, 255, 255));

                        circle_detected[0] = true;
                        Dispatcher.Invoke(new Action(() => l_n_wykryto_lewa.Visibility = Visibility.Visible));
                    }


                    if (circle.Center.X > 380 && circle.Center.X < 392)
                    {
                        PointF prawa_zaslepka = new PointF(360.5F, 450F);

                        CvInvoke.PutText(frame, "Prawa wykryta", System.Drawing.Point.Round(prawa_zaslepka),
                        FontFace.HersheyDuplex, 0.8, new MCvScalar(255, 255, 255));

                        circle_detected[1] = true;

                        Dispatcher.Invoke(new Action(() => l_n_wykryto_prawa.Visibility = Visibility.Visible));
                    }


                }

                if (circle_detected[0] && circle_detected[1])
                {
                    aTimer.Stop();
                    circle_detected[0] = circle_detected[1] = false;
                    //   MessageBox.Show("Wykryto zaslepki");
                    manual_log_enabled = false;

                    save_image.SaveToJpeg(image_capture, barcode);
                    plik.tworzeniepliku(@barcode);

                    Dispatcher.Invoke(new Action(() => label_state.Content = "TEST OK, zeskanuj kolejną płytę"));
                    Dispatcher.Invoke(new Action(() => label_state.Background = System.Windows.Media.Brushes.LawnGreen));
                    Dispatcher.Invoke(new Action(() => textBox.Text = ""));
                    Dispatcher.Invoke(new Action(() => textBox.IsReadOnly = false));
                    Dispatcher.Invoke(new Action(() => textBox.Focus()));
                    Dispatcher.Invoke(new Action(() => textBox.SelectAll()));
                    Dispatcher.Invoke(new Action(() => Podaj_barkode.Content = "Zeskanuj kolejną płytę"));
                    Dispatcher.Invoke(new Action(() => l_n_wykryto_lewa.Visibility = Visibility.Hidden));
                    Dispatcher.Invoke(new Action(() => l_n_wykryto_prawa.Visibility = Visibility.Hidden));
                    //  textBox.Focus();


                    stop_streaming();
                }




                //Drawing a light gray frame around the image
                CvInvoke.Rectangle(frame,
                        new System.Drawing.Rectangle(System.Drawing.Point.Empty, new System.Drawing.Size(circleImage.Width - 1, circleImage.Height - 1)),
                        new MCvScalar(120, 120, 120));
                //Draw the labels
                CvInvoke.PutText(frame, "Circles", new System.Drawing.Point(20, 20), FontFace.HersheyDuplex, 0.5,
                    new MCvScalar(120, 120, 120));
                #endregion



                if (frame != null)
                {

                    using (var stream = new MemoryStream())
                    {
                        // My way to display frame
                        imageHSVDest.ToBitmap().Save(stream, ImageFormat.Bmp);
                        //    frame.AsBitmap().Save(stream, ImageFormat.Bmp);

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = new MemoryStream(stream.ToArray());
                        bitmap.EndInit();

                        image_stream.Source = bitmap;
                    }


                }


                if (circleImage != null)
                {

                    using (var stream2 = new MemoryStream())
                    {
                        // My way to display frame
                        frame.ToBitmap().Save(stream2, ImageFormat.Bmp);
                        //    frame.AsBitmap().Save(stream, ImageFormat.Bmp);

                        BitmapImage bitmap2 = new BitmapImage();
                        bitmap2.BeginInit();
                        bitmap2.StreamSource = new MemoryStream(stream2.ToArray());
                        bitmap2.EndInit();

                        image_capture.Source = bitmap2;
                        // Thread.Sleep(5000);
                    }


                }
            }
        }

    }
}