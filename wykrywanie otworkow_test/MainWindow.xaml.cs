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

        public static MainWindow _myWindow;
        public static string _barcode = "";
        public bool[] _circle_detected = new bool[2];
        public static bool _manual_log_enabled;

        public Timers timer { get; set; }

        public Stream stream;

        public MainWindow()
        {
            InitializeComponent();
            //SetTimer();
            _myWindow = this;
            timer = new Timers();
            timer.SetTimer();
            timer.mTimer.Elapsed += OnTimedEvent;
            timer.mTimer.Stop();
            stream = new Stream();
            Dispatcher.Invoke(new Action(() => textBox.Focus()));
            Dispatcher.Invoke(new Action(() => textBox.SelectAll()));
            Save.Read_param();
        }


        //due to problem with update labels content from another class with bitmapimage/frezze()/dispose() issue, I've made it in the MainWindow.cs 
        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _manual_log_enabled = true;

            Dispatcher.Invoke(new Action(delegate ()
            {

                label_state.Content = "  2.Zamontuj SECURITY CAP,\r\n WYKRYWANIE NADAL TRWA, sprawdź ich obecność! \n Aby nadać ręcznie loga naciśnij Enter \n Aby przerwać test naciśnij ESC";
                label_state.Background = System.Windows.Media.Brushes.Orange;
            }));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Save.SaveToJpeg(image_capture, _barcode + ".jpg"); //C://tars//image.jpg            
        }


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
        }

        private async void OnKeyDownHandler(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Return)
            {
                
                if (_manual_log_enabled)
                {
                    _manual_log_enabled = false;
                    _circle_detected[0] = _circle_detected[1] = false;
                    stream.stop_streaming();
                    Save.SaveToJpeg(image_capture, _barcode);
                    await Save.SendLogMesTisAsync(_barcode);

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
                _barcode = textBox.Text;

                if (_barcode.Length == 27 && _barcode[0] == '1')
                {
                    _manual_log_enabled = false;
                    // aTimer.Start();
                    timer.mTimer.Start();

                    Dispatcher.Invoke(new Action(() => label_state.Content = "  2.Zamontuj SECURITY CAP,\r\n     czekam na ich wykrycie"));
                    Dispatcher.Invoke(new Action(() => Podaj_barkode.Content = "Zeskanowany numer seryjny:"));
                    textBox.IsReadOnly = true;
                    stream.run_streaming();
                }
                else
                {
                    Dispatcher.Invoke(new Action(() => label_state.Content = "1.Podaj poprawny barkode!"));
                    _barcode = textBox.Text = string.Empty;

                }

            }
            else if (e.Key == Key.Escape)
            {
                if (_manual_log_enabled)
                {
                    _manual_log_enabled = false;
                    _circle_detected[0] = _circle_detected[1] = false;
                    stream.stop_streaming();
                    Dispatcher.Invoke(new Action(() => label_state.Content = "Przerwano test, zeskanuj kolejną płytę"));
                    Dispatcher.Invoke(new Action(() => label_state.Background = System.Windows.Media.Brushes.Magenta));
                    Dispatcher.Invoke(new Action(() => textBox.Text = ""));
                    Dispatcher.Invoke(new Action(() => textBox.IsReadOnly = false));
                    Dispatcher.Invoke(new Action(() => textBox.Focus()));
                    Dispatcher.Invoke(new Action(() => textBox.SelectAll()));
                    Dispatcher.Invoke(new Action(() => Podaj_barkode.Content = "Zeskanuj kolejną płytę"));
                    Dispatcher.Invoke(new Action(() => l_n_wykryto_lewa.Visibility = Visibility.Hidden));
                    Dispatcher.Invoke(new Action(() => l_n_wykryto_prawa.Visibility = Visibility.Hidden));
                }
            }
        }


    }
}