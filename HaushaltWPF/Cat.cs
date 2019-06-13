using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaushaltWPF
{
    public static class Cat
    {
        //public static List<string> fixCat = new List<string>
        //{
        //    "Krankenversicherung",
        //    "Unfallversicherung",
        //    "Lebensversicherung",
        //    "Haftpflicht",
        //    "Hausrat",
        //    "Wohngebäudeversicherung",
        //    "Kfz Versicherung",
        //    "Kfz Steuer",
        //    "Abo",
        //    "Aus- & Weiterbildung",
        //    "ParkhausgebührSAD",
        //    "ADAC",
        //    "domainfactory",
        //    "HD Plus",
        //    "GEZ",
        //    "Telefon & Internet",
        //    "Darlehen (easyCredit)",
        //    "Darlehen (120070084)",
        //    "Darlehen (920070084)",
        //    "Darlehen (220070084)",
        //    "Darlehen (MünchnerHypo)",
        //    "Kreditkarte",
        //    "Rücklage",
        //    "Einkommen"
        //};

        //public static List<string> flexCat = new List<string>
        //{
        //    "Lebensmittel",
        //    "Freizeit",
        //    "Körperpflege / Gesundheit",
        //    "Haushalt / Möbel",
        //    "Kleidung",
        //    "Gaststätten",
        //    "Amazon",
        //    "Elektronikmärkte",
        //    "Abhebung",
        //    "Sonstiges"
        //};

        public static string[] fixCat { get { return File.ReadAllLines(@"Kategorien\fixCat.txt"); } }

        public static string[] flexCat { get { return File.ReadAllLines(@"Kategorien\flexCat.txt"); } }

        public static List<string> GetAllCats()
        {
            List<string> ret = new List<string>();
            ret.AddRange(fixCat);
            ret.AddRange(flexCat);

            return ret;
        }
    }
}
