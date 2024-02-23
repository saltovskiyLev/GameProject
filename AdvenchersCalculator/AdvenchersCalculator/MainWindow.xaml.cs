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
using System.IO;

namespace AdvenchersCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, Persones> characters = new Dictionary<string, Persones>();
        string N1;
        string N2;
        int ResultFight1;
        int ResultFight2;

        int Rand1;
        int Rand2;

        public MainWindow()
        {
            InitializeComponent();
            string[] files;
            files = GetFiles(@"C:\Users\Admin\Documents\GitHub\GameProject\AdvenchersCalculator\AdvenchersCalculator\pers");
            characters = ReadPers(files);
            FightR1.ItemsSource = characters.Keys;
            FightR2.ItemsSource = characters.Keys;
            string N1;
            string N2;

            Random r1 = new Random();
            Rand1 = r1.Next(0, 12);

            Random r2 = new Random();
            Rand2 = r2.Next(0, 12);

        }

        string[] GetFiles(string path)
        {
            string[] s = Directory.GetFiles(path);
            return s;
        }

        Dictionary<string, Persones> ReadPers(string[] files)
        {
            Dictionary<string, Persones> characters = new Dictionary<string, Persones>();
            Persones Pers;
            string[] pers;
            for(int i = 0; i < files.Length; i++)
            {
                Pers = new Persones();


                pers = File.ReadAllLines(files[i]);

                Pers.A = int.Parse(pers[0]);
                Pers.B = int.Parse(pers[1]);
                Pers.C = int.Parse(pers[2]);
                Pers.D = int.Parse(pers[3]);
                Pers.F = int.Parse(pers[4]);
                Pers.name = pers[5];


                characters.Add(pers[5], Pers);


            }
            return characters;
        }

        private void BFight_Click(object sender, RoutedEventArgs e)
        {
            if(FightR1.SelectedIndex == -1 || FightR2.SelectedIndex == -1)
            {
                return;
            }
            Fight f = new Fight();
            f.TBName1.Text = FightR1.SelectedItem as string;
            f.TBName2.Text = FightR2.SelectedItem as string;

            /*int A1 = characters[f.TBDamage1.Text].D - characters[f.TBDamage2.Text].A;
            int A2 = characters[f.TBDamage2.Text].D - characters[f.TBDamage1.Text].A;*/
            N1 = f.TBName1.Text;
            N2 = f.TBName2.Text;

            SPComboPairs.Children.Add(f);
            f.Container = SPComboPairs;
        }

        private void BCalculate_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 2; i < SPComboPairs.Children.Count; i++)
            {
                Fight f = SPComboPairs.Children[i] as Fight;

                ResultFight1 = characters[N1].D - characters[N2].A;
                ResultFight2 = characters[N2].D - characters[N1].A;

                int nowR1 = characters[N1].C;
                int nowR2 = characters[N2].C;


                if(Rand1 <= nowR1)
                {
                    ResultFight1 = ResultFight1 - 2;
                }

                if (Rand2 <= nowR2)
                {
                    ResultFight2 = ResultFight2 - 2;
                }

                f.TBDamage1.Text = ResultFight1.ToString();
                f.TBDamage2.Text = ResultFight2.ToString();
            }
        }
    }
}
