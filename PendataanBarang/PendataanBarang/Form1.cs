using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DGVPrinterHelper;

namespace PendataanBarang
{
    public partial class Form1 : Form
    {
        // Rizki Januar Irmansyah
        // Mendeklarasikan Variabel Dan Melakukan Koneksi Pada Database
        private SqlCommand cmd;
        private DataSet ds;
        private SqlDataAdapter da;

        SqlConnection Conn = new SqlConnection(@"Data Source=DESKTOP-5C2L5M1\YFSERVER;Initial Catalog=DB_LATIHAN;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }

        // Membuat Fungsi untuk membersihkan Isi Dari Text Box
        void Bersihkan()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }

        // Membuat Fungsi untuk menampilkan data dari Database
        void TampilBarang()
        {
            try
            {
                Conn.Open();
                cmd = new SqlCommand("Select * from TBL_BARANG", Conn);
                ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "TBL_BARANG");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "TBL_BARANG";
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception G)
            {
                MessageBox.Show(G.ToString());
            }
            finally
            {
                Conn.Close();
            }
        }

        // Membuat fungsi untuk mencari data yang ada Data Grid View
        void CariBarang()
        {
            try
            {
                Conn.Open();
                cmd = new SqlCommand("Select * from TBL_BARANG where KodeBarang like '%" + textBox8.Text + "%' OR NamaBarang like '%" + textBox8.Text + "%' OR Tanggal like '%" + textBox8.Text + "%'", Conn);
                ds = new DataSet();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "TBL_BARANG");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "TBL_BARANG";
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception G)
            {
                MessageBox.Show(G.ToString());
            }
            finally
            {
                Conn.Close();
            }
        }

        // Moch Yogi Firmansyah
        // Membuat fungsi untuk menuliskan kode secara otomatis setelah urutan terakhir
        void KodeOtomatis()
        {
            long hitung;
            string urutan;
            SqlDataReader dr;
            Conn.Open();
            cmd = new SqlCommand("Select KodeBarang from TBL_BARANG where KodeBarang in(select max(KodeBarang) from TBL_BARANG) order by KodeBarang desc", Conn);
            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                hitung = Convert.ToInt64(dr[0].ToString().Substring(dr["KodeBarang"].ToString().Length - 3, 3)) + 1;
                string joinstr = "000" + hitung;
                urutan = "BRG" + joinstr.Substring(joinstr.Length - 3, 3);
            }
            else
            {
                urutan = "BRG001";
            }
            dr.Close();
            textBox1.Text = urutan;
            Conn.Close();
        }

        // Melakukan pemanggilan fungsi Bersihkan, TampilBarang, Dan KodeOtomatis pada Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            Bersihkan();
            TampilBarang();
            KodeOtomatis();
        }

        // Melakukan input data yang akan disimpan pada Database dan ditampilkan pada Data Grid View
        private void InputButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" || textBox5.Text.Trim() == "" || textBox6.Text.Trim() == "")
            {
                MessageBox.Show("Data Belum Lengkap!");
            }
            else
            {
                try
                {
                    Conn.Open();
                    cmd = new SqlCommand("Insert Into TBL_BARANG values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + textBox7.Text + "')", Conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Berhasil Ditambahkan");
                    Conn.Close();
                    TampilBarang();
                    Bersihkan();
                    KodeOtomatis();
                }
                catch (Exception X)
                {
                    MessageBox.Show(X.ToString());
                }
            }
        }

        // Apabila baris dari Data Grid View di klik, semua data yang ada dibaris tersebut akan
        // dimunculkan pada text box
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["KodeBarang"].Value.ToString();
                textBox2.Text = row.Cells["NamaBarang"].Value.ToString();
                textBox3.Text = row.Cells["JenisBarang"].Value.ToString();
                textBox4.Text = row.Cells["HargaBarang"].Value.ToString();
                textBox5.Text = row.Cells["JumlahBarang"].Value.ToString();
                textBox6.Text = row.Cells["SatuanBarang"].Value.ToString();
            }
            catch (Exception X)
            {
                MessageBox.Show(X.ToString());
            }
        }

        // Fahmi Faqur
        // Melakukan Ubah Data pada Database, Setelah meng-klik baris pada Data Grid View,
        // Data yang akan diubah
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" || textBox5.Text.Trim() == "" || textBox6.Text.Trim() == "")
            {
                MessageBox.Show("Data Belum Lengkap!");
            }
            else
            {
                try
                {
                    Conn.Open();
                    cmd = new SqlCommand("Update TBL_BARANG set NamaBarang='" + textBox2.Text + "',JenisBarang='" + textBox3.Text + "',HargaBarang='" + textBox4.Text + "',JumlahBarang='" + textBox5.Text + "',SatuanBarang='" + textBox6.Text + "' where KodeBarang='" + textBox1.Text + "'", Conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Berhasil Diubah");
                    Conn.Close();
                    TampilBarang();
                    Bersihkan();
                    KodeOtomatis();
                }
                catch (Exception X)
                {
                    MessageBox.Show(X.ToString());
                }
            }
        }

        // Melakukan Ubah Data pada Database, Setelah meng-klik baris pada Data Grid View,
        // Data yang akan diubah
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Apakah Yakin Anda Ingin Menghapus Data Barang " + textBox2.Text + " ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                try
                {
                    Conn.Open();
                    cmd = new SqlCommand("Delete TBL_BARANG where KodeBarang='" + textBox1.Text + "'", Conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Berhasil DiHapus");
                    Conn.Close();
                    TampilBarang();
                    Bersihkan();
                    KodeOtomatis();
                }
                catch (Exception X)
                {
                    MessageBox.Show(X.ToString());
                }
        }

        // Melakukan Refresh / Membersihkan semua data yang ada di Text Box
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            TampilBarang();
            Bersihkan();
            KodeOtomatis();
        }

        // Memanggil fungsi CariBarang untuk mencari data yang ada di Data Grid View
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            CariBarang();
        }

        // Melakukan Print data yang ada pada Data Grid View
        private void PrintButton_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Laporan Data Barang";
            printer.SubTitle = string.Format("Tanggal {0}", DateTime.Now.Date.ToString("dddd-MMMM-yyyy"));
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(dataGridView1);
        }
    }
}
