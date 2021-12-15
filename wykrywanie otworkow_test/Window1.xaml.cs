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
using System.IO;
using System.Threading.Tasks;

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

        private bool _passw_save;

        private void set_label_init_value()
        {
            Dispatcher.Invoke(new Action(() => label_low1.Content = Parameters.lower_H));
            Dispatcher.Invoke(new Action(() => label_low2.Content = Parameters.lower_S));
            Dispatcher.Invoke(new Action(() => label_low3.Content = Parameters.lower_V));

            Dispatcher.Invoke(new Action(() => label_h1.Content = Parameters.high_H));
            Dispatcher.Invoke(new Action(() => label_h2.Content = Parameters.high_S));
            Dispatcher.Invoke(new Action(() => label_h3.Content = Parameters.high_V));

            Dispatcher.Invoke(new Action(() => label_dp.Content = Parameters.dp));
            Dispatcher.Invoke(new Action(() => label_min_dist.Content = Parameters.minDist));
            Dispatcher.Invoke(new Action(() => label_p1.Content = Parameters.param1));

            Dispatcher.Invoke(new Action(() => label_p2.Content = Parameters.param2));
            Dispatcher.Invoke(new Action(() => label_min_R.Content = Parameters.minRadius));
            Dispatcher.Invoke(new Action(() => label_max_R.Content = Parameters.maxRadius));

            Dispatcher.Invoke(new Action(() => label_Xlcapmin.Content = Parameters.X_lcap_min));
            Dispatcher.Invoke(new Action(() => label_Xlcapmax.Content = Parameters.X_lcap_max));

            Dispatcher.Invoke(new Action(() => label_XPcapmin.Content = Parameters.X_Pcap_min));
            Dispatcher.Invoke(new Action(() => label_XPcapmax.Content = Parameters.X_Pcap_max));
        }


        private void param_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textbox_selected = (TextBox)sender;

            if (e.Key == Key.Return)
            {


                if (sender == tbox_low1)
                {
                    Dispatcher.Invoke(new Action(() => label_low1.Content = textbox_selected.Text));
                    Parameters.lower_H = Parameters.labelparam_convert(label_low1);
                }
                else if (sender == tbox_low2)
                {
                    Dispatcher.Invoke(new Action(() => label_low2.Content = textbox_selected.Text));
                    Parameters.lower_S = Parameters.labelparam_convert(label_low2);
                }
                else if (sender == tbox_low3)
                {
                    Dispatcher.Invoke(new Action(() => label_low3.Content = textbox_selected.Text));
                    Parameters.lower_V = Parameters.labelparam_convert(label_low3);
                }
                else if (sender == tbox_high1)
                {
                    Dispatcher.Invoke(new Action(() => label_h1.Content = textbox_selected.Text));
                    Parameters.high_H = Parameters.labelparam_convert(label_h1);
                }
                else if (sender == tbox_high2)
                {
                    Dispatcher.Invoke(new Action(() => label_h2.Content = textbox_selected.Text));
                    Parameters.high_S = Parameters.labelparam_convert(label_h2);
                }
                else if (sender == tbox_high3)
                {
                    Dispatcher.Invoke(new Action(() => label_h3.Content = textbox_selected.Text));
                    Parameters.high_V = Parameters.labelparam_convert(label_h3);
                }
                else if (sender == tbox_dp)
                {
                    Dispatcher.Invoke(new Action(() => label_dp.Content = textbox_selected.Text));
                    Parameters.dp = Parameters.labelparam_convert(label_dp);
                }
                else if (sender == tbox_m_dist)
                {
                    Dispatcher.Invoke(new Action(() => label_min_dist.Content = textbox_selected.Text));
                    Parameters.minDist = Parameters.labelparam_convert(label_min_dist);
                }
                else if (sender == tbox_p1)
                {
                    Dispatcher.Invoke(new Action(() => label_p1.Content = textbox_selected.Text));
                    Parameters.param1 = Parameters.labelparam_convert(label_p1);
                }
                else if (sender == tbox_p2)
                {
                    Dispatcher.Invoke(new Action(() => label_p2.Content = textbox_selected.Text));
                    Parameters.param2 = Parameters.labelparam_convert(label_p2);
                }
                else if (sender == tbox_min_r)
                {
                    Dispatcher.Invoke(new Action(() => label_min_R.Content = textbox_selected.Text));
                    Parameters.minRadius = (int)Parameters.labelparam_convert(label_min_R);
                }
                else if (sender == tbox_max_r)
                {
                    Dispatcher.Invoke(new Action(() => label_max_R.Content = textbox_selected.Text));
                    Parameters.maxRadius = (int)Parameters.labelparam_convert(label_max_R);
                }
                else if (sender == tbox_Xlcapmin)
                {
                    Dispatcher.Invoke(new Action(() => label_Xlcapmin.Content = textbox_selected.Text));
                    Parameters.X_lcap_min = (double)Parameters.labelparam_convert(label_Xlcapmin);
                }
                else if (sender == tbox_Xlcapmax)
                {
                    Dispatcher.Invoke(new Action(() => label_Xlcapmax.Content = textbox_selected.Text));
                    Parameters.X_lcap_max = (double)Parameters.labelparam_convert(label_Xlcapmax);
                }
                else if (sender == tbox_XPcapmin)
                {
                    Dispatcher.Invoke(new Action(() => label_XPcapmin.Content = textbox_selected.Text));
                    Parameters.X_Pcap_min = (double)Parameters.labelparam_convert(label_XPcapmin);
                }
                else if (sender == tbox_XPcapmax)
                {
                    Dispatcher.Invoke(new Action(() => label_XPcapmax.Content = textbox_selected.Text));
                    Parameters.X_Pcap_max = (double)Parameters.labelparam_convert(label_XPcapmax);
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (_passw_save)
            {
                Save.Save_param();
                MessageBox.Show("Zapisano!");
            }              
            else
                MessageBox.Show("Nie zapisano, wpisz hasło!");
        }

        private void textBox_passw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (@textBox_passw.Text.Contains("UTR"))
                {
                    _passw_save = true;
                    Dispatcher.Invoke(new Action(() => l_n_putpass.Content = "Hasło poprawne!"));
                    Dispatcher.Invoke(new Action(() => @textBox_passw.Text = ""));
                }
                else
                {
                    Dispatcher.Invoke(new Action(() => @textBox_passw.Text = ""));
                    Dispatcher.Invoke(new Action(() => l_n_putpass.Content = "Hasło niepoprawne!"));
                    _passw_save = false;
                }               
                
            }

        }
    }
}
