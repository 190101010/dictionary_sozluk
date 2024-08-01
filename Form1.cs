using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;

namespace dictionary_sozluk
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        string connstring = "server = localhost; uid = root; pwd = 12345; database = dict_db";

        private void button1_Click(object sender, EventArgs e)
        {
            string englishWord = textBox1.Text;
            string turkishWord = textBox2.Text;


            using (MySqlConnection con = new MySqlConnection(connstring))
            {
                try
                {
                    con.Open();
                    string ekle = "INSERT INTO dict (ing, tr) VALUES (@ing, @tr)";

                    using (MySqlCommand cmd = new MySqlCommand(ekle, con))
                    {
                        cmd.Parameters.AddWithValue("@ing", englishWord);
                        cmd.Parameters.AddWithValue("@tr", turkishWord);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Veri başarıyla eklendi.");
                        textBox1.Clear();
                        textBox2.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string englishWord = textBox1.Text;
            string turkishWord = textBox2.Text;

            using (MySqlConnection con = new MySqlConnection(connstring))
            {

                try
                {

                    con.Open();
                    string güncelle = "UPDATE dict SET tr = @tr WHERE ing = @ing";
                    using (MySqlCommand cmd = new MySqlCommand(güncelle, con))
                    {
                        cmd.Parameters.AddWithValue("@ing", englishWord);
                        cmd.Parameters.AddWithValue("@tr", turkishWord);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Veri başarıyla güncellendi.");
                        }
                        else
                        {
                            MessageBox.Show("Güncellenecek kelime bulunamadı.");
                        }
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    con.Close();
                }
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            string englishWord = textBox1.Text;
            string turkishWord = textBox2.Text;

            using (MySqlConnection con = new MySqlConnection(connstring))
            {

                try
                {

                    con.Open();
                    string sil = "delete from dict where ing=@ing and tr = @tr";
                    using (MySqlCommand cmd = new MySqlCommand(sil, con))
                    {

                        cmd.Parameters.AddWithValue("@ing", englishWord);
                        cmd.Parameters.AddWithValue("@tr", turkishWord);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Veri başarıyla silindi.");
                        }
                        else
                        {
                            MessageBox.Show("Silinecek kelime bulunamadı.");
                        }
                        
                    }
                    con.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    con.Close();
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string englishWord = textBox1.Text;
            string turkishWord = textBox2.Text;

            using (MySqlConnection con = new MySqlConnection(connstring))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM dict WHERE ing LIKE @englishWord";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Parametreyi sorguya ekliyoruz ve joker karakter kullanıyoruz.
                        cmd.Parameters.AddWithValue("@ing", "%" + englishWord + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                            {
                                sb.AppendLine(reader["ing"].ToString());
                            }

                            richTextBox1.Text = sb.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

    }
}


