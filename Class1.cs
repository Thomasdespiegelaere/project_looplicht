using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Looplicht
{
    internal class berekening
    {
		public bool start { get; set; }
		public berekening()
		{
			start = true;
		}
		private string tijd;
		public string Tijd
		{
			get { return tijd; }
			set 
			{
				if (value != "")
				{
                    if (value.Contains('.'))
                    {
                        value = value.Replace('.', ',');
                        tijd = value;
                    }
                    tijd = value;
                }
				else
				{
					Message();
                    start = false;
                }
            }
		}

		private string afstand;

		public string Afstand
		{
			get { return afstand; }
			set 
			{
                if (value != "")
                {
					if (value.Contains('.'))
					{
                        value = value.Replace('.', ',');
                        afstand = value;
                    }
                    afstand = value;
                }
                else
                {
					Message();
                    start = false;
                } 
            }
		}

		private int aantalLeds;

		public int AantalLeds
		{
			get { return aantalLeds; }
			set { aantalLeds = value; }
		}

		/// <summary>
		/// berkend de tijd tussen de leds.
		/// </summary>
		/// <returns>double</returns>
		public double BerekenWachtTijd()
		{
			double vertraging = ((Convert.ToDouble(tijd) / aantalLeds) * 1000.0) - 182;  // 182 is ongeveer de verstuurtijd van seriele COM
			return vertraging;															 // + de tijd die het duurt om de loop door te nemen.
        }
		public void Message()
		{
            MessageBox.Show("Vul alle vakken in.");
        }
    }
}
