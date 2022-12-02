using System;
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
					MessageBox.Show("geef een tijd in");
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
                    MessageBox.Show("geef een afstand in");
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
    }
}
