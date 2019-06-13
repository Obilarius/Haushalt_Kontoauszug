using System.Collections.Generic;

namespace HaushaltWPF
{
    public class Zahlung
    {
        public string Datum { get; set; }
        public string Zahlungsart { get; set; }
        public float Saldo { get; set; }
        public string Saldoart { get; set; }
        public List<string> Verwendungszweck { get; set; }
        public string Kategorie { get; set; }

        public Zahlung()
        {
            Verwendungszweck = new List<string>();
        }

        public string compVerwendungszweck
        {
            get
            {
                string ret = "";
                foreach (var v in Verwendungszweck)
                {
                    ret += v + "\n";
                }
                return ret;
            }
        }

        public string VerwendungszweckToString
        {
            get
            {
                string ret = "";
                foreach (var v in Verwendungszweck)
                    ret += v;
                return ret;
            }
        }
    }


}
