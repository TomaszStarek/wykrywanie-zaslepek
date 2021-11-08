using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Timers;

namespace wykrywanie_otworkow_test
{
    public class Stream
    {

        public VideoCapture _capture;

        public Stream()
        {
            _capture = new VideoCapture();
        }
        public void run_streaming()
        {
            ComponentDispatcher.ThreadIdle += Streaming;
        }
        public void stop_streaming()
        {
            ComponentDispatcher.ThreadIdle -= Streaming;
        }


        public void Streaming(object sender, System.EventArgs e)
        {

            using (Image<Hsv, byte> frame = _capture.QueryFrame().ToImage<Hsv, byte>())
            using (UMat gray = new UMat())
            using (Mat triangleRectangleImage = new Mat(frame.Size, DepthType.Cv8U, 3)) //image to draw triangles and rectangles on
            using (Mat circleImage = new Mat(frame.Size, DepthType.Cv8U, 3)) //image to draw circles on
            using (Mat lineImage = new Mat(frame.Size, DepthType.Cv8U, 3)) //image to drtaw lines on
            {

                Hsv lowerLimit = new Hsv(Parameters.lower_H, Parameters.lower_S, Parameters.lower_V);
                Hsv upperLimit = new Hsv(Parameters.high_H, Parameters.high_S, Parameters.high_V);


                Image<Gray, byte> imageHSVDest = frame.InRange(lowerLimit, upperLimit);
                CvInvoke.GaussianBlur(imageHSVDest, imageHSVDest, new System.Drawing.Size(7, 7), 2, 2);

                Image<Gray, byte> imageHSVDest_revert = imageHSVDest.Not();


                #region circle detection

                CircleF[] circles = CvInvoke.HoughCircles(imageHSVDest, HoughModes.Gradient, Parameters.dp, Parameters.minDist, Parameters.param1, Parameters.param2, Parameters.minRadius, Parameters.maxRadius);
                #endregion

                #region draw circles
                circleImage.SetTo(new MCvScalar(0));
                foreach (CircleF circle in circles)
                {
                    CvInvoke.Circle(frame, System.Drawing.Point.Round(circle.Center), (int)circle.Radius + 20,
                    new Bgr(System.Drawing.Color.White).MCvScalar, 10);
                    CvInvoke.PutText(frame, System.Drawing.Point.Round(circle.Center).ToString() + "radius: " + circle.Radius.ToString(), System.Drawing.Point.Round(circle.Center), FontFace.HersheyDuplex, 0.5,
                                    new MCvScalar(255, 0, 0));

                    if (circle.Center.X > Parameters.X_lcap_min && circle.Center.X < Parameters.X_lcap_max)
                    {
                        PointF lewa_zaslepka = new PointF(40.5F, 450F);

                        CvInvoke.PutText(frame, "Lewa wykryta", System.Drawing.Point.Round(lewa_zaslepka),
                        FontFace.HersheyDuplex, 0.8, new MCvScalar(255, 255, 255));

                        MainWindow._myWindow._circle_detected[0] = true;
                        MainWindow._myWindow.Dispatcher.Invoke(new Action(() => MainWindow._myWindow.l_n_wykryto_lewa.Visibility = Visibility.Visible));
                    }

                    if (circle.Center.X > Parameters.X_Pcap_min && circle.Center.X < Parameters.X_Pcap_max)
                    {
                        PointF prawa_zaslepka = new PointF(360.5F, 450F);

                        CvInvoke.PutText(frame, "Prawa wykryta", System.Drawing.Point.Round(prawa_zaslepka),
                        FontFace.HersheyDuplex, 0.8, new MCvScalar(255, 255, 255));

                        MainWindow._myWindow._circle_detected[1] = true;
                        MainWindow._myWindow.Dispatcher.Invoke(new Action(() => MainWindow._myWindow.l_n_wykryto_prawa.Visibility = Visibility.Visible));
                    }
                }

                if (MainWindow._myWindow._circle_detected[0] && MainWindow._myWindow._circle_detected[1])
                {

                    MainWindow._myWindow.timer.mTimer.Stop();
                    MainWindow._myWindow._circle_detected[0] = MainWindow._myWindow._circle_detected[1] = false;
                    MainWindow._manual_log_enabled = false;

                    save.SaveToJpeg(MainWindow._myWindow.image_capture, MainWindow._barcode);
                    save.SaveLog(@MainWindow._barcode);

                    MainWindow._myWindow.Dispatcher.Invoke(new Action(() => MainWindow._myWindow.label_state.Content = "TEST OK, zeskanuj kolejną płytę"));
                    MainWindow._myWindow.Dispatcher.Invoke(new Action(() => MainWindow._myWindow.label_state.Background = System.Windows.Media.Brushes.LawnGreen));
                    MainWindow._myWindow.Dispatcher.Invoke(new Action(() => MainWindow._myWindow.textBox.Text = ""));
                    MainWindow._myWindow.Dispatcher.Invoke(new Action(() => MainWindow._myWindow.textBox.IsReadOnly = false));
                    MainWindow._myWindow.Dispatcher.Invoke(new Action(() => MainWindow._myWindow.textBox.Focus()));
                    MainWindow._myWindow.Dispatcher.Invoke(new Action(() => MainWindow._myWindow.textBox.SelectAll()));
                    MainWindow._myWindow.Dispatcher.Invoke(new Action(() => MainWindow._myWindow.Podaj_barkode.Content = "Zeskanuj kolejną płytę"));
                    MainWindow._myWindow.Dispatcher.Invoke(new Action(() => MainWindow._myWindow.l_n_wykryto_lewa.Visibility = Visibility.Hidden));
                    MainWindow._myWindow.Dispatcher.Invoke(new Action(() => MainWindow._myWindow.l_n_wykryto_prawa.Visibility = Visibility.Hidden));
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


                //#displayregion
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

                        MainWindow._myWindow.image_stream.Source = bitmap;
                        //if (bitmap.CanFreeze)
                        //{
                        //    bitmap.Freeze();
                        //}

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

                        MainWindow._myWindow.image_capture.Source = bitmap2;
                        //if (bitmap2.CanFreeze)
                        //{
                        //    bitmap2.Freeze();
                        //}
                    }
                }
                //#endDisplayRegion

            }
        }



    }
}
