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
using berekeningen;

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
        private void SendLedData(object? sender, EventArgs e)
        {            
            if (_serialPort != null && _serialPort.IsOpen)
            {
                for (int i = 0; i < _data.Length; i++)
                {
                    _data[i] = 125;
                    _serialPort.Write(_data, 0, _data.Length);
                }
                _dispatcherTimer.Stop();
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            byte[] data = new byte[96];
            _serialPort.Write(data, 0, 96);          
            _serialPort.Dispose();
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            berekening vertraging = new berekening();
            vertraging.Tijd = Convert.ToDouble(tbx_tijd.Text);
            vertraging.Afstand = Convert.ToInt16(tbx_afstand.Text);
            vertraging.AantalLeds = 32;

            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(vertraging.BerekenWachtTijd());
            _dispatcherTimer.Tick += SendLedData;
            _dispatcherTimer.Start();
        }
    }
}
