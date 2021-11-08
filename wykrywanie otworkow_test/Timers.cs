using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace wykrywanie_otworkow_test
{
    public class Timers
    {
        public System.Timers.Timer mTimer;
        public void SetTimer()
        {
            // Create a timer with a ONE second interval.
            mTimer = new System.Timers.Timer(5000);
            // Hook up the Elapsed event for the timer. 
            //mTimer.Elapsed += OnTimedEvent;
            mTimer.AutoReset = false;
            mTimer.Enabled = true;
        }

        // I have problem with update label.content here, so I wrote it (OnTimedEvent()) in the MainWindow.cs

        //public void OnTimedEvent(object source, ElapsedEventArgs e)
        //{
        //   // MessageBox.Show("Timer");
        //    MainWindow.manual_log_enabled = true;

        //    Application.Current.Dispatcher.Invoke((Action)delegate
        //    {
        //        var set = new MainWindow();
        //        set.TimerDone();
        //    });
        //    //Application.Current.Dispatcher.Invoke((Action)delegate {
        //    //    var timerDone = new MainWindow.SetControlText();
        //    //    timerDone.TimerDone();

        //    MainWindow.myWindow.Dispatcher.Invoke(new Action(delegate ()
        //    {

        //        MainWindow.myWindow.label_state.Content = "  2.Zamontuj SECURITY CAP,\r\n WYKRYWANIE NADAL TRWA, sprawdź ich obecność! \n Aby nadać ręcznie loga naciśnij Enter \n Aby przerwać test naciśnij ESC";
        //        MainWindow.myWindow.label_state.Background = System.Windows.Media.Brushes.Orange;
        //    }));
        //}     




    }


}



