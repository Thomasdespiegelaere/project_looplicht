using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace berekeningen
{
    internal class berekening
    {
		private double tijd;

		public double Tijd
		{
			get { return tijd; }
			set 
			{
				tijd = value;
            }
		}

		private int afstand;

		public int Afstand
		{
			get { return afstand; }
			set 
			{
				afstand = value;
            }
		}

		private int aantalLeds;

		public int AantalLeds
		{
			get { return aantalLeds; }
			set { aantalLeds = value; }
		}
		public double BerekenWachtTijd()
		{
			double vertraging = ((tijd / aantalLeds) * 1000.0);
			return vertraging;
        }
	}
}
