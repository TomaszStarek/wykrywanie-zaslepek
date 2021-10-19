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

namespace wykrywanie_otworkow_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
     //   VideoCapture capture = new VideoCapture(0);

        VideoCapture _capture;
        private Mat _frame;


        private void ProcessFrame(object sender, EventArgs e)
        {

        }



        public MainWindow()
        {
            InitializeComponent();

        }



        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            _streaming = false;
            _capture = new VideoCapture();
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            //  VideoCapture capture = new VideoCapture(0);
            //var img = capture.QueryFrame().ToImage< Bgr, byte>();
            //var bmp = img.ToBitmap();

            //BitmapImage bitmap = new BitmapImage();
            //bitmap.BeginInit();
            //bitmap.StreamSource = new stre(selectedFileName);
            //bitmap.EndInit();


            //działa ale chyba niepotrzebne
            //////////using (Image<Bgr, byte> frame = _capture.QueryFrame().ToImage<Bgr, byte>() )
            //////////{
            //////////    if (frame != null)
            //////////    {

            //////////            using (var stream = new MemoryStream())
            //////////            {
            //////////                // My way to display frame 
            //////////                frame.AsBitmap().Save(stream, ImageFormat.Bmp);

            //////////                BitmapImage bitmap = new BitmapImage();
            //////////                bitmap.BeginInit();
            //////////                bitmap.StreamSource = new MemoryStream(stream.ToArray());
            //////////                bitmap.EndInit();

            //////////            image_capture.Source = bitmap;
            //////////            }


            //////////    }
            //////////}
            ///
            image_capture.Source = image_stream.Source;
            SaveToJpeg(image_stream, "image.jpg");
        }


        public void SaveToJpeg(FrameworkElement visual, string fileName)
        {
            var encoder = new JpegBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        private static void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
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
        //void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        //{
        //    RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
        //    bitmap.Render(visual);
        //    BitmapFrame frame = BitmapFrame.Create(bitmap);
        //    encoder.Frames.Add(frame);

        //    using (var stream = File.Create(fileName))
        //    {
        //        encoder.Save(stream);
        //    }
        //}




        bool _streaming;
        private void btn_stream_Click(object sender, RoutedEventArgs e)
        {


            if (!_streaming)
            {
                ComponentDispatcher.ThreadIdle += streaming;
                //System.Windows.Interop.ComponentDispatcher.ThreadIdle += (_, __) =>
                //{
                //    streaming(this, EventArgs.Empty);
                //};
            }
            else
            {
                ComponentDispatcher.ThreadIdle -= streaming;
                //System.Windows.Interop.ComponentDispatcher.ThreadIdle -= (_, __) =>
                //{
                //    streaming(this, EventArgs.Empty);
                //};
            }
            _streaming = !_streaming;
        }

        private void streaming(object sender, System.EventArgs e)
        {
            
            //var img = capture.QueryFrame().ToImage< Bgr, byte>();
            //var bmp = img.ToBitmap();

            //BitmapImage bitmap = new BitmapImage();
            //bitmap.BeginInit();
            //bitmap.StreamSource = new stre(selectedFileName);
            //bitmap.EndInit();

            using (Image<Xyz, byte> frame = _capture.QueryFrame().ToImage<Xyz, byte>())
            {
                if (frame != null)
                {

                    using (var stream = new MemoryStream())
                    {
                        // My way to display frame 
                        frame.AsBitmap().Save(stream, ImageFormat.Bmp);

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = new MemoryStream(stream.ToArray());
                        bitmap.EndInit();

                        image_stream.Source = bitmap;
                    }


                }
            }
        }


    }
}
