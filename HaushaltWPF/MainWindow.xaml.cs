using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Kontoauszug|*.pdf";
            if (openFileDialog.ShowDialog() == true)
            {
                ContentWrapper.Children.Clear();

                var alleZahlungen = ReadFile(openFileDialog.FileName);

                foreach (var z in alleZahlungen)
                {
                    ZahlungEntry ze = new ZahlungEntry();
                    ze.DataContext = z;
                    ze.zahlung = z;
                    ContentWrapper.Children.Add(ze);
                }
                
            }
        }

        private List<Zahlung> ReadFile(string Filename)
        {
            List<Zahlung> alleZahlungen = new List<Zahlung>();

            PdfReader pdf_Reader = new PdfReader(Filename);

            for (int i = 1; i <= pdf_Reader.NumberOfPages; i++)
            {
                string sText = PdfTextExtractor.GetTextFromPage(pdf_Reader, i);

                string[] lines = sText.Split('\n');

                Zahlung Zahlung = null;
                foreach (var line in lines)
                {
                    string patternDatum = @"^(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\. (3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.";
                    if (Regex.IsMatch(line, patternDatum))
                    {
                        // Datum
                        Match mDatum = Regex.Match(line, patternDatum);

                        // Betrag
                        string patternBetrag = @"\d*\.{0,1}(\d)+,\d{2} (S|H)$";
                        Match mBetrag = Regex.Match(line, patternBetrag);

                        // Art der Zahlung
                        string patternZahlungsart = $"(?<={mDatum}).*(?={mBetrag})";
                        Match mZahlungsart = Regex.Match(line, patternZahlungsart);

                        if (Zahlung != null) { alleZahlungen.Add(Zahlung); }
                        Zahlung = new Zahlung();
                        Zahlung.Datum = mDatum.Value.Substring(0, 5);
                        Zahlung.Saldo = float.Parse(mBetrag.Value.Substring(0, mBetrag.Value.Length - 2));
                        Zahlung.Saldoart = mBetrag.Value.Substring(mBetrag.Value.Length - 1);
                        Zahlung.Zahlungsart = mZahlungsart.Value.Trim();
                    }
                    else if (Zahlung != null)
                    {
                        if (Regex.IsMatch(line, @"Übertrag auf Blatt |neuer Kontostand vom "))
                        {
                            alleZahlungen.Add(Zahlung);
                            Zahlung = null;
                        }
                        else
                        {
                            Zahlung.Verwendungszweck.Add(line.Trim());
                        }
                    }
                }
            }

            return alleZahlungen;
        }

        private void SaveCSV_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            #region Headline
            string headLine = "";

            foreach (var cat in Cat.GetAllCats())
                headLine += cat + ";";

            headLine += "Datum;Zahlungsart;Saldo;Saldoart;Verwendungszweck";

            sb.AppendLine(headLine);
            #endregion


            foreach (var c in ContentWrapper.Children)
            {
                var l = c as ZahlungEntry;
                var zahlung = l.zahlung;

                string line = "";

                foreach (var cat in Cat.GetAllCats())
                {
                    if (zahlung.Kategorie == cat)
                        line += zahlung.Saldo + ";";
                    else
                        line += ";";
                }

                line += $"{zahlung.Datum};{zahlung.Zahlungsart};{zahlung.Saldo};{zahlung.Saldoart};{zahlung.VerwendungszweckToString}";

                sb.AppendLine(line);
            }


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Datei|*.csv";
            saveFileDialog.FileName = "Kontoauszug_CSV_";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);

        }
    }


}
