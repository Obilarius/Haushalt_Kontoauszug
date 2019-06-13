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

namespace HaushaltWPF
{
    /// <summary>
    /// Interaction logic for ZahlungEntry.xaml
    /// </summary>
    public partial class ZahlungEntry : UserControl
    {
        public Zahlung zahlung { get; set; }
        public List<Control> allCBs { get; set; }

        public ZahlungEntry()
        {
            InitializeComponent();

            foreach (var fc in Cat.flexCat)
            {
                CheckBox cb = new CheckBox();
                cb.Content = fc;
                cb.Click += new RoutedEventHandler(Click_Kategorie_Checkbox);

                SP_flexCat.Children.Add(cb);
            }

            for (int i = 0; i < Cat.fixCat.Length; i++)
            {
                CheckBox cb = new CheckBox();
                cb.Content = Cat.fixCat[i];
                cb.Click += new RoutedEventHandler(Click_Kategorie_Checkbox);

                if (i <= Cat.fixCat.Length / 2)
                    SP_fixCat_1.Children.Add(cb);
                else
                    SP_fixCat_2.Children.Add(cb);
            }

            allCBs = GetAllCBs();
        }

        private void Click_Kategorie_Checkbox(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;

            foreach (var c in allCBs)
            {
                var c2 = c as CheckBox;
                c2.IsChecked = false;
            }

            cb.IsChecked = true;
            zahlung.Kategorie = cb.Content.ToString();
        }

        private List<Control> GetAllCBs()
        {
            List<Control> ret = new List<Control>();

            foreach (Control c in SP_fixCat_1.Children)
                ret.Add(c);

            foreach (Control c in SP_fixCat_2.Children)
                ret.Add(c);

            foreach (Control c in SP_flexCat.Children)
                ret.Add(c);

            return ret;
        }
    }
}
