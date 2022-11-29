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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO.Ports;
using System.Threading;
using System.Windows.Markup;
using Looplicht;

namespace project_looplicht
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort _serialPort;
        byte[] _data;
        DispatcherTimer _dispatcherTimer;
        int _i;
        public MainWindow()
        {
            InitializeComponent();

            cbxPortName.Items.Add("None");
            foreach (string s in SerialPort.GetPortNames())
                cbxPortName.Items.Add(s);

            _serialPort = new SerialPort();
            _serialPort.BaudRate = 9600;
            _serialPort.StopBits = StopBits.One;
            _serialPort.DataBits = 8;
            _serialPort.Parity = Parity.None;

            _data = new byte[96];

            _dispatcherTimer = new DispatcherTimer();            
        }

        private void cbxPortName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();

                if (cbxPortName.SelectedItem.ToString() != "None")
                {
                    _serialPort.PortName = cbxPortName.SelectedItem.ToString();
                    _serialPort.Open();
                }
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                byte[] data = new byte[96];
                _serialPort.Write(data, 0, 96);
                _serialPort.Dispose();
            }                      
        }
        public void leds(object? sender, EventArgs e)
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                if (!(_i >= 95))
                {
                    _data[_i] = Convert.ToByte(sldr_Red.Value);
                    _data[_i + 1] = Convert.ToByte(sldr_Green.Value);
                    _data[_i + 2] = Convert.ToByte(sldr_Blue.Value);
                    _serialPort.Write(_data, 0, _data.Length);
                    _i += 3;
                }
                else
                {
                    _i = 0;
                    _dispatcherTimer.Stop();
                    _data[95] = 0;
                    _data[94] = 0;
                    _data[93] = 0;
                    _serialPort.Write(_data, 0, 96);
                }
                if (_i > 3)
                {
                    _data[_i - 3] = 0;
                    _data[_i - 4] = 0;
                    _data[_i - 5] = 0;
                    _serialPort.Write(_data, 0, _data.Length);
                }
            }
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            berekening berekening = new berekening();
            berekening.Tijd = tbx_tijd.Text;
            berekening.Afstand = tbx_afstand.Text;
            berekening.AantalLeds = 32;

            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(berekening.BerekenWachtTijd());
            _dispatcherTimer.Tick += leds;
            _dispatcherTimer.Start();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sldr_Blue.Width = scp_main.ActualWidth / 3 - 20;
            sldr_Red.Width = scp_main.ActualWidth / 3 - 20;
            sldr_Green.Width = scp_main.ActualWidth / 3 - 20;
        }

        private void cbx_darkmode_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)cbx_darkmode.IsChecked)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(48, 48, 48));
                scp_main.Background = brush;
                gbx_com.Foreground = Brushes.White;
                gbx_tijd.Foreground = Brushes.White;
                gbx_afstand.Foreground = Brushes.White;
                gbx_com.BorderBrush = Brushes.White;
                gbx_tijd.BorderBrush = Brushes.White;
                gbx_afstand.BorderBrush = Brushes.White;
                lbl_darkmode.Foreground = Brushes.White;
                btn_start.Background = brush;
                btn_start.BorderBrush = brush;
            }
            else
            {
                scp_main.Background = Brushes.White;
                gbx_com.Foreground = Brushes.Black;
                gbx_tijd.Foreground = Brushes.Black;
                gbx_afstand.Foreground = Brushes.Black;
                gbx_com.BorderBrush = Brushes.Gray;
                gbx_tijd.BorderBrush = Brushes.Gray;
                gbx_afstand.BorderBrush = Brushes.Gray;
                lbl_darkmode.Foreground = Brushes.Black;
                btn_start.Background = Brushes.White;
                btn_start.BorderBrush = Brushes.White;
            }
        }
    }
}
