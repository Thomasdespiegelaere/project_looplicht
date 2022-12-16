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
using System.Windows.Controls.Primitives;

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
        double _val;
        int _img_switch = 0;

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

            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            _dispatcherTimer.Tick += leds;

            _val = -370;

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
                Array.Clear(_data, 0, _data.Length);
                _serialPort.Write(_data, 0, 96);
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
                    pbStatus.Value += 1;
                    _val += 23.125;
                    img_run.Margin = new Thickness(_val, 0, 0, 0);
                    if (_img_switch % 3 == 0)
                    {
                        img_run.Source = new BitmapImage(new Uri("/3.png", UriKind.Relative));
                    }
                    else if (_img_switch % 3 == 1)
                    {
                        img_run.Source = new BitmapImage(new Uri("/4.png", UriKind.Relative));
                    }
                    else
                    {
                        img_run.Source = new BitmapImage(new Uri("/2.png", UriKind.Relative));
                    }
                }
                else
                {
                    _dispatcherTimer.Stop();
                    _i = 0;
                    _val = -370;
                    img_run.Margin = new Thickness(_val, 0, 0, 0);
                    pbStatus.Value = 0;
                    Array.Clear(_data, 0, _data.Length);
                    _serialPort.Write(_data, 0, 96);
                }
                if (_i > 6)
                {
                    _data[_i - 6] = 0;
                    _data[_i - 7] = 0;
                    _data[_i - 8] = 0;
                    _serialPort.Write(_data, 0, _data.Length);
                }
            }
            _img_switch += 1;
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            berekening berekening = new berekening();
            berekening.Tijd = tbx_tijd.Text;
            berekening.Afstand = tbx_afstand.Text;
            berekening.AantalLeds = 32;

            if (berekening.start == true)
            {
                _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(berekening.BerekenWachtTijd());
                _dispatcherTimer.Start();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sldr_Blue.Width = scp_main.ActualWidth / 3 - 20;
            sldr_Red.Width = scp_main.ActualWidth / 3 - 20;
            sldr_Green.Width = scp_main.ActualWidth / 3 - 20;
        }
        private void theme(SolidColorBrush background, SolidColorBrush borderbrush, SolidColorBrush textcolor)
        {
            scp_main.Background = background;
            gbx_com.Foreground = textcolor;
            gbx_tijd.Foreground = textcolor;
            gbx_afstand.Foreground = textcolor;
            gbx_com.BorderBrush = borderbrush;
            gbx_tijd.BorderBrush = borderbrush;
            gbx_afstand.BorderBrush = borderbrush;
            btn_start.Background = background;
            btn_start.BorderBrush = background;
            mb.Background = background;
            mb.BorderBrush = borderbrush;
            mb.Foreground = textcolor;
        }

        private void btn_theme_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btn_DarkMode)
            {
                theme(new SolidColorBrush(Color.FromRgb(48, 48, 48)), new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.White));
            }
            else
            {
                theme(new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Gray), new SolidColorBrush(Colors.Black));
            }
        }
    }
}
