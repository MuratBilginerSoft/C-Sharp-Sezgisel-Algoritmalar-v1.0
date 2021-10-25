using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb; // Bu uzayı access yada excel dosyalarına erişebilmek üzerinde işlem yapmak için ekliyoruz.

namespace Sezgizel_Algoritmalar_v2._0
{
    public partial class TavlamaBenzetimi : Form
    {
        #region Ekstra İşlemler

        // Toplam ı yenile içie alacağım.

        #endregion

        #region Tanımlamalar

        OpenFileDialog file = new OpenFileDialog(); // Dosyalara erişim sağlayabilecek nesnemizi türettik.
        Graphics çizimyap; // Çizim yapacak değişkenimiz.
        Pen kalem = new System.Drawing.Pen(Color.White, 2); // Güzergah için çizgi çizecek olan kalemimiz.
        Brush dolgu = new SolidBrush(Color.Black); // Elipsleri dolduracak olan fırçamızı tanımladık.
        Font yazı = new Font("Georgia", 12, FontStyle.Bold); // Lokasyonları yazacak olan grafik nesnesi.
        Brush dolgu2 = new SolidBrush(Color.White); // Lokasyon isimlerini yazacak olan fırçamız
        DataTable tablo; // Veritabanındaki değerleri geçiçi olarak aldığımız tablomuz.

        string[] seçilenpozisyon1 = new string[21]; // Seçilen pozisyonun harfkarşılığını tutacak dizi.
        int[] seçilenpozisyon2 = new int[21]; // Seçilen pozisyonun indexini tutacak dizi.
        int[] eniyiyol = new int[22]; // Son adımda en iyi çözümü tutacak dizi.

        Random r = new Random(); // Rastgele değer üretebilmek için Random nesnesi türettik.

        İşlemler işlem = new İşlemler(); // İşlemler sınıfındaki metodları kullanabilmek için yeni nesne türettik.


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
           DataTable tablo = bağlan.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,null);
           string[] sayfaisimleri = new string[tablo.Rows.Count];
           int a1 = 0;
           foreach (DataRow sayfaismi in tablo.Rows)
           {
               comboBox1.Items.Add(sayfaismi["TABLE_NAME"].ToString().Replace("'",""));
               sayfaisimleri[a1] = sayfaismi["TABLE_NAME"].ToString().Replace("'", "");
               a1++;
           }

           bağlan.Close();
        // şimdi abi şöyle bir problem düşünün bir kargo şirketisiniz gün içinde dağımtım için 200 farklı adrese gitmeniz gerek koordinatlar var hangi düzende gidersek daha az km yapıp daha az gaz yalarız maliyeti düşürüüz olayımız bu şimdi 10 farklı yer bile olsa 10! farklı seçim oluyorsiz düşünün 200 olunca ne oluru :D aynen abi algoritmalar üzerine çalışıyorum
        }

        public void excelverioku(string sayfa) // Exceldeki verileri okuyacak olan metodumuz.
        {

            OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=" + dosyayolu + "; Extended Properties=Excel 12.0"); // Veritabanı bağlantımız.
            baglanti.Open(); // Bağlantımızı açtık.
            string sorgu = "select * from ["+sayfa+"] "; // Hangi verilerin getirilececeğini belirlediğimiz SQL sorgumuz.
            OleDbDataAdapter data_adaptor = new OleDbDataAdapter(sorgu, baglanti); // Değerleri geçiçi olarak alan dataataptörümüz.
            baglanti.Close(); // Bağlantıyı kapattık. Eğer kapatmaz isek veri kayıplarına neden olabiliriz.

            tablo = new DataTable(); // Veri tabanınındaki değerleri geçici olarak tutacak tablomuzu oluşturduk.
            data_adaptor.Fill(tablo); // Sorgu ile DataAdapter çektiğimizi verileri Fill metodu ile tablomuza aktardık.
            grid_view.DataSource = tablo; // Tablomuzdaki değerleri DataGriview de görüntüledik.
        
        }

        public void uzaklıkhesapla() // İki mesafe arası uzaklığı hesaplayacak olan metodumuz.
        {
            toplam = 0; // Metod her çağrıldığında toplam değeri 0 olmalı.

            for (int i = 0; i < 21; i++)
            {
                if (i==20)
                {
                    mesafe = Math.Sqrt(Math.Pow((int.Parse(tablo.Rows[seçilenpozisyon2[i]]["X"].ToString()) - int.Parse(tablo.Rows[seçilenpozisyon2[0]]["X"].ToString())), 2) + Math.Pow((int.Parse(tablo.Rows[seçilenpozisyon2[i]]["Y"].ToString()) - int.Parse(tablo.Rows[seçilenpozisyon2[0]]["Y"].ToString())), 2));
                }

                else
                {
                    mesafe = Math.Sqrt(Math.Pow((int.Parse(tablo.Rows[seçilenpozisyon2[i]]["X"].ToString()) - int.Parse(tablo.Rows[seçilenpozisyon2[i + 1]]["X"].ToString())), 2) + Math.Pow((int.Parse(tablo.Rows[seçilenpozisyon2[i]]["Y"].ToString()) - int.Parse(tablo.Rows[seçilenpozisyon2[i + 1]]["Y"].ToString())), 2));
                }

                toplam += mesafe;
                toplam = Math.Round(toplam);
            }
        
        }

        #endregion

        #region Değişkenler

        static string dosyayolu; // Alınacak olan Excel yada Access dosyasının yolunu tutacak değişken.

        int d1 = 0; // İlk seçimde rastgele değeri tutacak olan değişken
        int id = 1; // Seçilen yol sayısını tutacak
        int sıra = 0; // Listwiev yazılma sırası için değişken.

        #region Tavlama Parametreleri

        double ilksıcaklık, sıcaklıkdüş, ilkp1, sonp1,sonsıcaklık; 
        int iterasyon=0;
        double eniyiçözüm = 0;

        #endregion

        double mesafe = 0; // İki mesafe arasındaki uzaklığı tutacak. 
        double toplam = 0; // Her seçim için toplam mesafeyi tutacak. 

        int fonk1 = 0; // Komşuluk fonksiyonunun ilk rastgele değerini tutucak değişken
        int fonk2 = 0; // Komşuluk fonksiyonunun ikinci rastgele değerini tutacak değişken

        int f1 = 0; // Amaç fonksiyonu değerini tutacak değişken
        int f2 = 0; // İkinci komşuluk fonksiyonu değerini tutacak değişken 

        string sıralama = ""; // Gidiş sıralamasını tutacak değişken

        #endregion


        public TavlamaBenzetimi()
        {
            InitializeComponent();
        }

        private void TavlamaBenzetimi_Load(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dosyaismial();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            excelverioku(comboBox1.SelectedItem.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tablo.Rows.Count; i++)
            {
                çizimyap = pictureBox1.CreateGraphics();
                çizimyap.FillEllipse(dolgu, Convert.ToInt32(tablo.Rows[i]["X"].ToString()), Convert.ToInt32(tablo.Rows[i]["Y"].ToString()), 20, 20);
                çizimyap.DrawString(tablo.Rows[i]["Lokasyon"].ToString(), yazı, dolgu2, Convert.ToInt32(tablo.Rows[i]["X"].ToString()), Convert.ToInt32(tablo.Rows[i]["Y"].ToString()));
            }
          
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            #region Açıklama Oku

            /* Çizim metodlarını tabcontrol ün click olayından tekrar çağırmamızın sebebi?
             * Grafik çizimleri nesneler üzerinde kalıcı olarak çizilmemiştir. O yüzden Tabcontrol üzerinden sekme değiştirmemiz yada çalıştığımız formu minimize hale getirmemiz yada form değiştirmemiz
             * çizdirdiğimiz şekillerin silinmesine neden olacaktır. O yüzden Click olayına metodlarımızı bağlayarak bu sorunun üstesinden geldik.
             */

            #endregion

            çizimyap = pictureBox1.CreateGraphics(); // Çizim yap grafik nesnesinin Picturebox1 üzerine çizim yapacağını gösterdik.

            #region Lokasyonları Çiz

            for (int i = 0; i < tablo.Rows.Count; i++)
            {
                /* Lokasyon konumlarını çizdirmek için öncelikle dolu bir elips çizdirdik FillEllipse metodu ile.
                 * Daha son Lokasyon isimlerini gösterebilmek için DrawString metodunu kullanarak isimleri dolu dolu elipsler içinde konumladık.  */ 
                çizimyap.FillEllipse(dolgu, Convert.ToInt32(tablo.Rows[i]["X"].ToString()), Convert.ToInt32(tablo.Rows[i]["Y"].ToString()), 20, 20);
                çizimyap.DrawString(tablo.Rows[i]["Lokasyon"].ToString(), yazı, dolgu2, Convert.ToInt32(tablo.Rows[i]["X"].ToString()), Convert.ToInt32(tablo.Rows[i]["Y"].ToString()));
            }

            #endregion

            #region Güzergahları Çiz

            // Son çözümün şekil üzerinde de gösterimini sağlamak için güzergah çizimini yaptığımız metod.
            // Düz çizgiler için Drawline metodunu kullandık.

            for (int i = 0; i < 21; i++)
            { 
                çizimyap.DrawLine(kalem, Convert.ToInt32(tablo.Rows[eniyiyol[i]]["X"]), Convert.ToInt32(tablo.Rows[eniyiyol[i]]["Y"]), Convert.ToInt32(tablo.Rows[eniyiyol[i + 1]]["X"]), Convert.ToInt32(tablo.Rows[eniyiyol[i + 1]]["Y"]));
            }

            #endregion
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox7.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
                {
                    MessageBox.Show("Boş alanları doldurunuz", "Uyarı");
                }

                else
                {
                    listView1.Items.Clear();
                    id = 0;

                    ilksıcaklık = double.Parse(textBox2.Text);
                    sıcaklıkdüş = double.Parse(textBox3.Text);
                    iterasyon = int.Parse(textBox4.Text);
                    ilkp1 = double.Parse(textBox5.Text);
                    sonp1 = double.Parse(textBox6.Text);
                    sonsıcaklık = double.Parse(textBox7.Text);


                    #region İlk Değer Seçimi

                    işlem.rastgeleseçim2(seçilenpozisyon2, seçilenpozisyon1, d1, tablo);

                    for (int i = 0; i < 21; i++)
                    {
                        sıralama += seçilenpozisyon1[i] + " - ";
                    }

                    sıralama += "Depo";

                    uzaklıkhesapla();

                    sıra = listView1.Items.Count;

                    listView1.Items.Add(id.ToString());
                    listView1.Items[sıra].SubItems.Add(sıralama);
                    listView1.Items[sıra].SubItems.Add(toplam.ToString());

                    eniyiçözüm = toplam;
                    textBox8.Text = sıralama;
                    textBox9.Text = eniyiçözüm.ToString();
                    chart1.Series["Series1"].Points.AddXY(id,toplam);
                  

                    #endregion

                    for (double t = ilksıcaklık; t > sonsıcaklık; t=t-sıcaklıkdüş)
                    {
                        for (int döngü = 1; döngü <= iterasyon; döngü++)
                        {
                            do
                            {
                               fonk1 = r.Next(1, 21);
                               fonk2 = r.Next(0, 20);

                               f1=Convert.ToInt32(Math.Round(1/(fonk1*ilkp1*döngü*sonp1))+fonk2);

                               fonk1 = r.Next(1, 21);
                               fonk2 = r.Next(0, 20);

                               f2 =Convert.ToInt32(Math.Round(1 / (fonk1 * ilkp1 * döngü * sonp1)) + fonk2);

                            } while (f1<1 || f1>20 || f1==f2 || f2<1 || f2>20);

                           

                            int l1 = seçilenpozisyon2[f1];
                            int l2 = seçilenpozisyon2[f2];

                            seçilenpozisyon2[f1] = l2;
                            seçilenpozisyon2[f2] = l1;

                            sıralama = "";
                            seçilenpozisyon1[0] = "Depo";
                            sıralama += seçilenpozisyon1[0] + " - ";

                            for (int i = 1; i < 21; i++)
                            {
                                seçilenpozisyon1[i] = tablo.Rows[seçilenpozisyon2[i]]["Lokasyon"].ToString();
                                sıralama += seçilenpozisyon1[i] + " - ";

                            }

                            sıralama += "Depo";
                            uzaklıkhesapla();


                            id++;
                            sıra = listView1.Items.Count;

                            listView1.Items.Add(id.ToString());
                            listView1.Items[sıra].SubItems.Add(sıralama);
                            listView1.Items[sıra].SubItems.Add(toplam.ToString());

                            double kontrol=eniyiçözüm-toplam;
                            chart1.Series["Series1"].Points.AddXY(id, toplam);


                            if (kontrol<0)
                            {
                                double u1 = r.Next(1, 10);
                                u1 = u1 / 10;
                                double u2= 1 / (Math.Exp(Math.Abs(kontrol)/t)*ilkp1);
                               

                                if (u1<u2)
                                {
                                    seçilenpozisyon2[f1] = l1;
                                    seçilenpozisyon2[f2] = l2;
                                }
                            }

                            else
                            {
                                eniyiçözüm = toplam;
                                textBox8.Text = sıralama;
                                textBox9.Text = eniyiçözüm.ToString();
                                textBox11.Text = sıralama;
                                textBox10.Text = eniyiçözüm.ToString();
                                for (int i = 0; i < 21; i++)
                                {
                                    eniyiyol[i] = seçilenpozisyon2[i];
                                }

                                eniyiyol[21] = 0;
                               
                            }

                        }
                    }

                    Pen kalem = new System.Drawing.Pen(Color.White,2);
                    çizimyap = pictureBox1.CreateGraphics();

                    for (int i = 0; i < 21; i++)
                    {
                        çizimyap.DrawLine(kalem,Convert.ToInt32(tablo.Rows[eniyiyol[i]]["X"]),Convert.ToInt32(tablo.Rows[eniyiyol[i]]["Y"]),Convert.ToInt32(tablo.Rows[eniyiyol[i+1]]["X"]),Convert.ToInt32(tablo.Rows[eniyiyol[i+1]]["Y"]));
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Değerleri doğru tipte ve aralıkta girdiğinizden emin olup tekrar deneyiniz.", "Uyarı");
            }
        }
    }
}
