using MySql.Data.MySqlClient;
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

namespace _12B_keszletnyilvantarto {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        MySqlConnection kapcs = new MySqlConnection("server = srv1.tarhely.pro;database = v2labgwj_12b_gergelyv; uid = v2labgwj_12b_gergelyv; password = '';");
        List<Termek> termekek = new List<Termek>();

        public MainWindow() {
            InitializeComponent();
            try {
                kapcs.Open();
                MySqlCommand parancs = new MySqlCommand("SELECT * FROM keszlet", kapcs);
                MySqlDataReader reader = parancs.ExecuteReader();
                while (reader.Read()) {
                    Termek ujTermek = new Termek(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3));
                    termekek.Add(ujTermek);
                }
                dgTermekek.ItemsSource = termekek;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            finally {
                kapcs.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            // felvitel
            try {
                kapcs.Open();
                new MySqlCommand($"insert into keszlet (cikkszam, megnevezes) values ('{txCikkszam.Text}','{txMegnevezes.Text}')", kapcs).ExecuteNonQuery();

                Termek ujTermek = new Termek(txCikkszam.Text, txMegnevezes.Text, 0,-1);
                termekek.Add(ujTermek);
                dgTermekek.Items.Refresh();
                txCikkszam.Clear();
                txMegnevezes.Clear();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            finally {
                kapcs.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            // módosítás
            try {
                kapcs.Open();
                var termek = (Termek)dgTermekek.SelectedItem;
                new MySqlCommand($"update keszlet set megnevezes = '{txMegnevezes.Text}' where cikkszam = '{txCikkszam.Text}'", kapcs).ExecuteNonQuery();
                termek.Cikkszam = txCikkszam.Text;
                termek.Megnevezes = txMegnevezes.Text;
                dgTermekek.Items.Refresh();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            finally {
                kapcs.Close();
            }
        }

        private void dgTermekek_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (dgTermekek.SelectedItem is Termek) {
                var termek = (Termek)dgTermekek.SelectedItem;
                txCikkszam.Text = termek.Cikkszam;
                txMegnevezes.Text = termek.Megnevezes;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            // törlés
            try {
                kapcs.Open();
                var termek = (Termek)dgTermekek.SelectedItem;
                new MySqlCommand($"delete from keszlet where cikkszam = '{termek.Cikkszam}'", kapcs).ExecuteNonQuery();
                termekek.Remove(termek);
                dgTermekek.Items.Refresh();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            finally {
                kapcs.Close();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            lbDebug.Content = e.Key.ToString();
            foreach (var item in termekek) {
                if (item.Cikkszam[0].ToString() == e.Key.ToString()) {
                    txCikkszam.Text = item.Cikkszam;
                    txMegnevezes.Text = item.Megnevezes;
                    dgTermekek.SelectedItem = item;
                    break;
                }
            }
        }
    }
}
