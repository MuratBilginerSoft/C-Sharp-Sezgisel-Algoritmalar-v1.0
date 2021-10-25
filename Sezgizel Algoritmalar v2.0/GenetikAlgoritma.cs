using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Sezgizel_Algoritmalar_v2._0
{
    public partial class GenetikAlgoritma : Form
    {
        #region Tanımlamalar

        OpenFileDialog file = new OpenFileDialog(); // Dosyalara erişim sağlayabilecek nesnemizi türettik.
        Graphics çizimyap; // Çizim yapacak değişkenimiz.
        Pen kalem = new System.Drawing.Pen(Color.White, 2); // Güzergah için çizgi çizecek olan kalemimiz.
        Brush dolgu = new SolidBrush(Color.Black); // Elipsleri dolduracak olan fırçamızı tanımladık.
        Font yazı = new Font("Georgia", 12, FontStyle.Bold); // Lokasyonları yazacak olan grafik nesnesi.
        Brush dolgu2 = new SolidBrush(Color.White); // Lokasyon isimlerini yazacak olan fırçamız
        DataTable tablo; // Veritabanındaki değerleri geçiçi olarak aldığımız tablomuz.

        Random r = new Random(); // Rastgele değer üretebilmek için Random nesnesi türettik.

        İşlemler işlem = new İşlemler(); // İşlemler sınıfındaki metodları kullanabilmek için yeni nesne türettik.

        int[] pop1 = new int[22];
        int[] pop2 = new int[22];
        int[] pop3 = new int[22];
        int[] pop4 = new int[22];
        int[] pop5 = new int[22];
        int[] pop6 = new int[22];
        int[] pop7 = new int[22];
        int[] pop8 = new int[22];
        int[] pop9 = new int[22];
        int[] pop10 = new int[22];
        int[] pop11= new int[22];
        int[] pop12 = new int[22];
        int[] pop13 = new int[22];
        int[] pop14 = new int[22];
        int[] pop15 = new int[22];
        int[] pop16 = new int[22];
        int[] pop17 = new int[22];
        int[] pop18 = new int[22];
        int[] pop19 = new int[22];
        int[] pop20 = new int[22];

        string[] popisim1 = new string[22];
        string[] popisim2 = new string[22];
        string[] popisim3 = new string[22];
        string[] popisim4 = new string[22];
        string[] popisim5 = new string[22];
        string[] popisim6 = new string[22];
        string[] popisim7 = new string[22];
        string[] popisim8 = new string[22];
        string[] popisim9 = new string[22];
        string[] popisim10 = new string[22];
        string[] popisim11 = new string[22];
        string[] popisim12 = new string[22];
        string[] popisim13 = new string[22];
        string[] popisim14 = new string[22];
        string[] popisim15 = new string[22];
        string[] popisim16 = new string[22];
        string[] popisim17 = new string[22];
        string[] popisim18 = new string[22];
        string[] popisim19 = new string[22];
        string[] popisim20 = new string[22];

        int[] eniyidizilim = new int[22];
        #endregion

        #region Metodlar 

        public void dosyaismial()
        {
            file.Filter = "Excel Dosyası |*.xlsx| Excel Dosyası |*.xls";
            file.ShowDialog();
            textBox1.Text = file.SafeFileName;
            dosyayolu = file.FileName;
            excelebağlan();

        }

        public void excelebağlan()
        {
            OleDbConnection bağlan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + dosyayolu + "; Extended Properties=Excel 12.0");
            bağlan.Open();
            DataTable tablo = bağlan.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string[] sayfaisimleri = new string[tablo.Rows.Count];
            int a1 = 0;
            foreach (DataRow sayfaismi in tablo.Rows)
            {
                comboBox1.Items.Add(sayfaismi["TABLE_NAME"].ToString().Replace("'", ""));
                sayfaisimleri[a1] = sayfaismi["TABLE_NAME"].ToString().Replace("'", "");
                a1++;
            }

            bağlan.Close();

        }

        public void excelverioku(string sayfa) // Exceldeki verileri okuyacak olan metodumuz.
        {

            OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=" + dosyayolu + "; Extended Properties=Excel 12.0"); // Veritabanı bağlantımız.
            baglanti.Open(); // Bağlantımızı açtık.
            string sorgu = "select * from [" + sayfa + "] "; // Hangi verilerin getirilececeğini belirlediğimiz SQL sorgumuz.
            OleDbDataAdapter data_adaptor = new OleDbDataAdapter(sorgu, baglanti); // Değerleri geçiçi olarak alan dataataptörümüz.
            baglanti.Close(); // Bağlantıyı kapattık. Eğer kapatmaz isek veri kayıplarına neden olabiliriz.

            tablo = new DataTable(); // Veri tabanınındaki değerleri geçici olarak tutacak tablomuzu oluşturduk.
            data_adaptor.Fill(tablo); // Sorgu ile DataAdapter çektiğimizi verileri Fill metodu ile tablomuza aktardık.
            grid_view.DataSource = tablo; // Tablomuzdaki değerleri DataGriview de görüntüledik.

        }

        public void lokasyonçiz()
        {
            for (int i = 0; i < tablo.Rows.Count; i++)
            {
                çizimyap = pictureBox1.CreateGraphics();
                çizimyap.FillEllipse(dolgu, Convert.ToInt32(tablo.Rows[i]["X"].ToString()), Convert.ToInt32(tablo.Rows[i]["Y"].ToString()), 20, 20);
                çizimyap.DrawString(tablo.Rows[i]["Lokasyon"].ToString(), yazı, dolgu2, Convert.ToInt32(tablo.Rows[i]["X"].ToString()), Convert.ToInt32(tablo.Rows[i]["Y"].ToString()));
            }

        }

        int id = 1;
        double toplam = 0;
        double eniyiçözüm = 1000000;
        double mesafe = 0;

        public void uzaklıkhesapla(int[] z1)
        {
            toplam = 0; // Metod her çağrıldığında toplam değeri 0 olmalı.

            for (int i3 = 0; i3 < 21; i3++)
            {
                mesafe = Math.Sqrt(Math.Pow((int.Parse(tablo.Rows[z1[i3]]["X"].ToString()) - int.Parse(tablo.Rows[z1[i3 + 1]]["X"].ToString())), 2) + Math.Pow((int.Parse(tablo.Rows[z1[i3]]["Y"].ToString()) - int.Parse(tablo.Rows[z1[i3 + 1]]["Y"].ToString())), 2));

                toplam += mesafe;
                toplam = Math.Round(toplam);
            }

            chart1.Series["Series1"].Points.AddXY(id, toplam);
        
        }

        public void populasyoseçimi(string[] y, int[] x)
        {
            for (int i1 = 1; i1 < 21; i1++)
            {
                do
                {
                    x1 = r.Next(1, 21);

                } while (Array.IndexOf(x, x1) != -1);

                x[i1] = x1;
                y[i1] = tablo.Rows[x1]["Lokasyon"].ToString();
                sıralama += y[i1]+" - ";

            }

            uzaklıkhesapla(x);
            sıralama += "Depo";
            int sıra = listView1.Items.Count;
            listView1.Items.Add(id.ToString());
            listView1.Items[sıra].SubItems.Add(sıralama);
            listView1.Items[sıra].SubItems.Add(toplam.ToString());
            id++;

            if (toplam<eniyiçözüm)
            {
                eniyiçözüm = toplam;

                for (int i2 = 0; i2 < 22; i2++)
                {
                    eniyidizilim[i2] = x[i2];
                }
               
                textBox8.Text = sıralama;
                textBox9.Text = eniyiçözüm.ToString();
                textBox11.Text = sıralama;
                textBox10.Text = eniyiçözüm.ToString();
            }

            sıralama = "Depo - ";
        }

        public void rotaçiz(int[] k1)
        {
            çizimyap = pictureBox1.CreateGraphics();

            for (int i = 0; i < 21; i++)
            {
                çizimyap.DrawLine(kalem, Convert.ToInt32(tablo.Rows[k1[i]]["X"]), Convert.ToInt32(tablo.Rows[k1[i]]["Y"]), Convert.ToInt32(tablo.Rows[k1[i + 1]]["X"]), Convert.ToInt32(tablo.Rows[k1[i + 1]]["Y"]));
            }

        }

        #endregion

        #region Değişkenler

        #region Genetik Algoritma Parametreleri

        public static double ç_olasılığı;
        public static double m_olasılığı;
        public static int p_büyüklüğü;

        public static int x1 = 0;

        public static string sıralama = "Depo - ";

        #endregion

        static string dosyayolu; // Alınacak olan Excel yada Access dosyasının yolunu tutacak değişken.

        #endregion


        public GenetikAlgoritma()
        {
            InitializeComponent();
        }

        private void GenetikAlgoritma_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 22; i=i+21)
            {
                pop1[i] = 0; pop2[i] = 0; pop3[i] = 0; pop4[i] = 0; pop5[i] = 0;

                pop6[i] = 0; pop7[i] = 0; pop8[i] = 0; pop9[i] = 0; pop10[i] = 0;

                pop11[i] = 0; pop12[i] = 0; pop13[i] = 0; pop14[i] = 0; pop15[i] = 0;

                pop16[i] = 0; pop17[i] = 0; pop18[i] = 0; pop19[i] = 0; pop20[i] = 0;

                popisim1[0] = "Depo"; popisim2[0] = "Depo"; popisim3[0] = "Depo"; popisim4[0] = "Depo";
                popisim5[0] = "Depo"; popisim6[0] = "Depo"; popisim7[0] = "Depo"; popisim8[0] = "Depo";
                popisim9[0] = "Depo"; popisim10[0] = "Depo"; popisim11[0] = "Depo"; popisim12[0] = "Depo";
                popisim13[0] = "Depo"; popisim14[0] = "Depo"; popisim15[0] = "Depo"; popisim16[0] = "Depo";
                popisim17[0] = "Depo"; popisim18[0] = "Depo"; popisim19[0] = "Depo"; popisim20[0] = "Depo";
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dosyaismial();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            excelverioku(comboBox1.SelectedItem.ToString());
            lokasyonçiz();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ç_olasılığı = double.Parse(textBox2.Text);
            m_olasılığı = double.Parse(textBox3.Text);
            p_büyüklüğü = int.Parse(textBox7.Text);

            #region Populasyon Üye Seçimi

            for (int z1 = 0; z1 < 20; z1++)
            {
                switch (z1)
                {
                    case 0: { populasyoseçimi(popisim1, pop1); break; }
                    case 1: { populasyoseçimi(popisim2, pop2); break; }
                    case 2: { populasyoseçimi(popisim3, pop3); break; }
                    case 3: { populasyoseçimi(popisim4, pop4); break; }
                    case 4: { populasyoseçimi(popisim5, pop5); break; }
                    case 5: { populasyoseçimi(popisim6, pop6); break; }
                    case 6: { populasyoseçimi(popisim7, pop7); break; }
                    case 7: { populasyoseçimi(popisim8, pop8); break; }
                    case 8: { populasyoseçimi(popisim9, pop9); break; }
                    case 9: { populasyoseçimi(popisim10, pop10); break; }
                    case 10: { populasyoseçimi(popisim11, pop11); break; }
                    case 11: { populasyoseçimi(popisim12, pop12); break; }
                    case 12: { populasyoseçimi(popisim13, pop13); break; }
                    case 13: { populasyoseçimi(popisim14, pop14); break; }
                    case 14: { populasyoseçimi(popisim15, pop15); break; }
                    case 15: { populasyoseçimi(popisim16, pop16); break; }
                    case 16: { populasyoseçimi(popisim17, pop17); break; }
                    case 17: { populasyoseçimi(popisim18, pop18); break; }
                    case 18: { populasyoseçimi(popisim19, pop19); break; }
                    case 19: { populasyoseçimi(popisim20, pop20); break; }


                }
            }

            rotaçiz(eniyidizilim);
            #endregion
            



        }
    }
}
