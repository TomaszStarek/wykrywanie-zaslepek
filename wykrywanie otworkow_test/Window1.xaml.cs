using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace wykrywanie_otworkow_test
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            set_label_init_value();
        }

        private void set_label_init_value()
        {
            Dispatcher.Invoke(new Action(() => label_low1.Content = display.lower_H));
            Dispatcher.Invoke(new Action(() => label_low2.Content = display.lower_S));
            Dispatcher.Invoke(new Action(() => label_low3.Content = display.lower_V));

            Dispatcher.Invoke(new Action(() => label_h1.Content = display.high_H));
            Dispatcher.Invoke(new Action(() => label_h2.Content = display.high_S));
            Dispatcher.Invoke(new Action(() => label_h3.Content = display.high_V));

            Dispatcher.Invoke(new Action(() => label_dp.Content = display.dp));
            Dispatcher.Invoke(new Action(() => label_min_dist.Content = display.minDist));
            Dispatcher.Invoke(new Action(() => label_p1.Content = display.param1));

            Dispatcher.Invoke(new Action(() => label_p2.Content = display.param2));
            Dispatcher.Invoke(new Action(() => label_min_R.Content = display.minRadius));
            Dispatcher.Invoke(new Action(() => label_max_R.Content = display.maxRadius));
        }



        private void param_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textbox_selected = (TextBox)sender;

            if (e.Key == Key.Return)
            {


                if (sender == tbox_low1)
                {
                    Dispatcher.Invoke(new Action(() => label_low1.Content = textbox_selected.Text));
                    display.lower_H = display.labelparam_convert(label_low1);
                }
                else if (sender == tbox_low2)
                {
                    Dispatcher.Invoke(new Action(() => label_low2.Content = textbox_selected.Text));
                    display.lower_S = display.labelparam_convert(label_low2);
                }
                else if (sender == tbox_low3)
                {
                    Dispatcher.Invoke(new Action(() => label_low3.Content = textbox_selected.Text));
                    display.lower_V = display.labelparam_convert(label_low3);
                }


                else if (sender == tbox_high1)
                {
                    Dispatcher.Invoke(new Action(() => label_h1.Content = textbox_selected.Text));
                    display.high_H = display.labelparam_convert(label_h1);
                }
                else if (sender == tbox_high2)
                {
                    Dispatcher.Invoke(new Action(() => label_h2.Content = textbox_selected.Text));
                    display.high_S = display.labelparam_convert(label_h2);
                }
                else if (sender == tbox_high3)
                {
                    Dispatcher.Invoke(new Action(() => label_h3.Content = textbox_selected.Text));
                    display.high_V = display.labelparam_convert(label_h3);
                }


                else if (sender == tbox_dp)
                {
                    Dispatcher.Invoke(new Action(() => label_dp.Content = textbox_selected.Text));
                    display.dp = display.labelparam_convert(label_dp);
                }
                else if (sender == tbox_m_dist)
                {
                    Dispatcher.Invoke(new Action(() => label_min_dist.Content = textbox_selected.Text));
                    display.minDist = display.labelparam_convert(label_min_dist);
                }
                else if (sender == tbox_p1)
                {
                    Dispatcher.Invoke(new Action(() => label_p1.Content = textbox_selected.Text));
                    display.param1 = display.labelparam_convert(label_p1);
                }

                else if (sender == tbox_p2)
                {
                    Dispatcher.Invoke(new Action(() => label_p2.Content = textbox_selected.Text));
                    display.param2 = display.labelparam_convert(label_p2);
                }
                else if (sender == tbox_min_r)
                {
                    Dispatcher.Invoke(new Action(() => label_min_R.Content = textbox_selected.Text));
                    display.minRadius = (int)display.labelparam_convert(label_min_R);
                }
                else if (sender == tbox_max_r)
                {
                    Dispatcher.Invoke(new Action(() => label_max_R.Content = textbox_selected.Text));
                    display.maxRadius = (int)display.labelparam_convert(label_max_R);
                }


            }

        }

        private void textChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox_selected = (TextBox)sender;

            if (System.Text.RegularExpressions.Regex.IsMatch(textbox_selected.Text, "[^0-9]"))
            {
                MessageBox.Show("Proszę wpisywać tylko cyfry...");
                textbox_selected.Text = textbox_selected.Text.Remove(textbox_selected.Text.Length - 1);
            }
        }
    }
}
