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

            using (Image<Hsv, byte> frame = _capture.QueryFrame().ToImage<Hsv, byte>())
            using (UMat gray = new UMat())
            using (UMat cannyEdges = new UMat())
            using (Mat triangleRectangleImage = new Mat(frame.Size, DepthType.Cv8U, 3)) //image to draw triangles and rectangles on
            using (Mat circleImage = new Mat(frame.Size, DepthType.Cv8U, 3)) //image to draw circles on
            using (Mat lineImage = new Mat(frame.Size, DepthType.Cv8U, 3)) //image to drtaw lines on
            {
                //  CvInvoke.CvtColor(frame, gray, ColorConversion.Rgb2Gray);
                //        CvInvoke.GaussianBlur(gray, gray, new System.Drawing.Size(3, 3), 2, 2);
                Hsv lowerLimit = new Hsv(0, 159, 94);
                //      Hsv upperLimit = new Hsv(120, 255, 255); w ciemno, nawet ok
                Hsv upperLimit = new Hsv(170, 255, 242);



                Image<Gray, byte> imageHSVDest = frame.InRange(lowerLimit, upperLimit);               
                CvInvoke.GaussianBlur(imageHSVDest, imageHSVDest, new System.Drawing.Size(3, 3), 2, 2);

                Image<Gray, byte> imageHSVDest_revert = imageHSVDest.Not();





                //   CvInvoke.CvtColor(imageHSVDest, gray, ColorConversion.Rgb2Gray);



                    const double dp = 1;
                    const double minDist = 50;
                    const double param1 = 35;
                    const double param2 = 25;
                    const int minRadius = 1;
                    const int maxRadius = 50;


                    #region circle detection
                    double cannyThreshold = 180.0;
                    double circleAccumulatorThreshold = 120;


           //     Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
           //     Mat hier = new Mat();

           //     CvInvoke.FindContours(imageHSVDest, contours, hier, Emgu.CV.CvEnum.RetrType.List, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxNone);

                //  CvInvoke.DrawContours(imageHSVDest, contours, 0, new MCvScalar(55, 55, 55), 2);
            //    imageHSVDest = imageHSVDest.Canny(100,1);

                CircleF[] circles = CvInvoke.HoughCircles(imageHSVDest, HoughModes.Gradient,dp, minDist, param1, param2, minRadius, maxRadius);



                    #endregion

                    #region draw circles
                    circleImage.SetTo(new MCvScalar(0));
                    foreach (CircleF circle in circles)
                    {
                        CvInvoke.Circle(frame, System.Drawing.Point.Round(circle.Center), (int)circle.Radius,
                        new Bgr(System.Drawing.Color.Blue).MCvScalar, 2);
                       // MessageBox.Show("znaleziono okrag");
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
