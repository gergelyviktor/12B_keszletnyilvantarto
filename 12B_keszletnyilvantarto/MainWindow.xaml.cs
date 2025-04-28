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

        MySqlConnection kapcs = new MySqlConnection("server = server.fh2.hu;database = v2labgwj_12b; uid = v2labgwj_12b; password = '4W56FNhfKJfeZVhGwasG';");
        List<Termek> termekek = new List<Termek>();

        public MainWindow() {
            InitializeComponent();
            try {
                kapcs.Open();
                MySqlCommand parancs = new MySqlCommand("SELECT * FROM gergelyv_termek", kapcs);
                MySqlDataReader reader = parancs.ExecuteReader();
                while (reader.Read()) {
                    Termek ujTermek = new Termek(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
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
            try {
                kapcs.Open();
                new MySqlCommand($"insert into gergelyv_termek (cikkszam, megnevezes) values ('{txCikkszam.Text}','{txMegnevezes.Text}')", kapcs).ExecuteNonQuery();

                Termek ujTermek = new Termek(termekek.Max(x => x.Id) + 1, txCikkszam.Text, txMegnevezes.Text);
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
                new MySqlCommand($"update gergelyv_termek set cikkszam = '{txCikkszam.Text}', megnevezes = '{txMegnevezes.Text}' where id = {termek.Id}", kapcs).ExecuteNonQuery();
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
            //if (dgTermekek.SelectedItem is Termek) {
            var termek = (Termek)dgTermekek.SelectedItem;
            txCikkszam.Text = termek.Cikkszam;
            txMegnevezes.Text = termek.Megnevezes;
            //}
        }
    }
}
