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
    public partial class TabuArama : Form
    {
        #region Tanımlamalar

        OpenFileDialog file = new OpenFileDialog(); // Dosyalara erişim sağlayabilecek nesnemizi türettik.
        Graphics çizimyap; // Çizim yapacak değişkenimiz.
        Pen kalem = new System.Drawing.Pen(Color.White, 2); // Güzergah için çizgi çizecek olan kalemimiz.
        Brush dolgu = new SolidBrush(Color.Black); // Elipsleri dolduracak olan fırçamızı tanımladık.
        Font yazı = new Font("Georgia", 12, FontStyle.Bold); // Lokasyonları yazacak olan grafik nesnesi.
        Brush dolgu2 = new SolidBrush(Color.White); // Lokasyon isimlerini yazacak olan fırçamız
        DataTable tablo; // Veritabanındaki değerleri geçiçi olarak aldığımız tablomuz.

        string[] seçilenpozisyon1 = new string[22]; // Seçilen pozisyonun harfkarşılığını tutacak dizi.
        int[] seçilenpozisyon2 = new int[22]; // Seçilen pozisyonun indexini tutacak dizi.
        int[] eniyiyol = new int[22]; // Son adımda en iyi çözümü tutacak dizi.
        int[] tabuhafıza = new int[th]; // Tabu hafızasının dizisi
        string[] eniyidizilim1=new string[22]; // Döngü anında en iyi dizilimi tutacak dizi.
        int[] eniyidizilim2 = new int[22]; // Döngü anında en iyi dizilimin sırasını tutacak dizilim.

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

        public void uzaklıkhesapla() // İki mesafe arası uzaklığı hesaplayacak olan metodumuz.
        {
            toplam = 0; // Metod her çağrıldığında toplam değeri 0 olmalı.

            for (int i3 = 0; i3 < 21; i3++)
            {
                mesafe = Math.Sqrt(Math.Pow((int.Parse(tablo.Rows[seçilenpozisyon2[i3]]["X"].ToString()) - int.Parse(tablo.Rows[seçilenpozisyon2[i3 + 1]]["X"].ToString())), 2) + Math.Pow((int.Parse(tablo.Rows[seçilenpozisyon2[i3]]["Y"].ToString()) - int.Parse(tablo.Rows[seçilenpozisyon2[i3 + 1]]["Y"].ToString())), 2));

                toplam += mesafe;
                toplam = Math.Round(toplam);
            }

            chart1.Series["Series1"].Points.AddXY(id, toplam);
            chart2.Series["Series1"].Points.AddXY(id, toplam);
        }

        public void listviewyaz(ListView list)
        {
            sıra = list.Items.Count; // Listview in o anki sırasını aldık. İndex 0 dan saymaya başlar.    

            list.Items.Add(id.ToString());
            list.Items[sıra].SubItems.Add(sıralama);
            list.Items[sıra].SubItems.Add(toplam.ToString());
        }

        public void sıralamayaz()
        {
            textBox8.Text = sıralama;
            textBox9.Text = eniyiçözüm.ToString();
            textBox10.Text = eniyiçözüm.ToString();
            textBox11.Text = sıralama;
        }

        public void komşulukseçme()
        {
            do
            {
                fonk1 = r.Next(1, 21);
                fonk2 = r.Next(0, 20);

                f1 = Convert.ToInt32(Math.Round(1 / (fonk1 * 0.5 * iterasyon * 0.001)) + fonk2);

                fonk1 = r.Next(1, 21);
                fonk2 = r.Next(0, 20);

                f2 = Convert.ToInt32(Math.Round(1 / (fonk1 * 0.5 * iterasyon * 0.001)) + fonk2);

            } while (f1 < 1 || f1 > 20 || f1 == f2 || f2 < 1 || f2 > 20);

            int l1 = seçilenpozisyon2[f1];
            int l2 = seçilenpozisyon2[f2];

            seçilenpozisyon2[f1] = l2;
            seçilenpozisyon2[f2] = l1;

            sıralama = "";
            sıralama += seçilenpozisyon1[0] + " - ";
        
        }

        public void tabusıralama(double[] x, double[] y, double[] z, double[] t)
        {
            Array.Sort(x); // İlk dizideki toplam değerleri küçükten büyüğe sıraladık.

            int u1 = 0;

            foreach (int item in x) // Zet 1 deki değerleri tek tek aldık.
            {

                for (int p = 0; p < x.Length; p++) // Dizinin uzunluğu kadar döngünün tekrarlamasını sağladık.
                {
                    if (item == y[p]) // Gelen değer zet2 dizisindeki değerle eş mi bunun kontrolünü yaptık.
                    {
                        // Eğer eşit ise; 

                        t[u1] = z[p];  // o an kaçıncı adımsa zet4 e doğru listview sıralamasını aldık.
                        u1++; // u1 değerini 1 artırdık.
                        break;  // Break komutu işe döngüyü bitirdik.
                    }

                }
            }
        
        }

        public void sıralamaoluştur()
        {
            for (int k1 = 1; k1 < 21; k1++) // Seçtiğimiz değerleri dizimize yazdırdık.
            {
                seçilenpozisyon1[k1] = tablo.Rows[seçilenpozisyon2[k1]]["Lokasyon"].ToString();
                sıralama += seçilenpozisyon1[k1] + " - ";
            }

            sıralama += seçilenpozisyon1[21]; // Sıralamaya yazdırdık.

            uzaklıkhesapla(); // Uzaklık Hesapladık
        
        }

        public void rotaçiz()
        {
            çizimyap = pictureBox1.CreateGraphics();

            for (int i = 0; i < 21; i++)
            {
                çizimyap.DrawLine(kalem, Convert.ToInt32(tablo.Rows[seçilenpozisyon2[i]]["X"]), Convert.ToInt32(tablo.Rows[seçilenpozisyon2[i]]["Y"]), Convert.ToInt32(tablo.Rows[seçilenpozisyon2[i + 1]]["X"]), Convert.ToInt32(tablo.Rows[seçilenpozisyon2[i + 1]]["Y"]));
            }
        
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

        #endregion

        #region Değişkenler

        #region Zetta Değişkenler

        static string dosyayolu; // Alınacak olan Excel yada Access dosyasının yolunu tutacak değişken.

        int d1 = 0; // İlk seçimde rastgele değeri tutacak olan değişken
        int id = 1; // Seçilen yol sayısını tutacak
        int id2 = 1; // Tabuda seçilen adeti tutacak
        public static int sıra = 0; // Listwiev yazılma sırası için değişken.
        public static int sıra2 = 0; // Listview2 nin yazılma sırası için değişken

        string sıralama = ""; // Gidiş sıralamasını tutacak değişken

        #endregion

        #region Tabu Parametreleri

        public static int th = 0; // Tabu hafızasının değişkenleri
        public static int iterasyon=0; // Ana Döngü içindeki döngünün sayısını tutacak değişken
        public static int durdurma=0; // Ana döngünün sayısını tutacak değişken
        public static int tabu=0; // Ana tabu sayısını tutacak değişken
        public static int tabudöngü =0; // Döngü içindeki tabu sayısını tutacak değişken
        public static double eniyiçözüm; // En iyi çözüm değeri tutulacak değişken

        public static double bölümeniyi = 0;

        public static string eniyidizilim = "";

        public static int i3 = 0, i4 = 0;

        public static int p2 = 0;

        public static int p8 = 0;

        public static int p4 = 0;
        public static int o1 = 0;


        #endregion

        #region Uzaklık Parametreleri

        double mesafe = 0; // İki mesafe arasındaki uzaklığı tutacak. 
        double toplam = 0; // Her seçim için toplam mesafeyi tutacak. 

        #endregion

        #region Komşuluk Parametreleri

        int fonk1 = 0; // Komşuluk fonksiyonunun ilk rastgele değerini tutucak değişken
        int fonk2 = 0; // Komşuluk fonksiyonunun ikinci rastgele değerini tutacak değişken

        int f1 = 0; // Amaç fonksiyonu değerini tutacak değişken
        int f2 = 0; // İkinci komşuluk fonksiyonu değerini tutacak değişken 

        #endregion

        #endregion

        public TabuArama()
        {
            InitializeComponent();
        }

        private void TabuArama_Load(object sender, EventArgs e)
        {
            seçilenpozisyon1[0] = "Depo";
            seçilenpozisyon2[0] = 0;
            eniyidizilim1[0] = "Depo";
            eniyidizilim2[0] = 0;
            seçilenpozisyon2[21] = 0;
            eniyidizilim2[21] = 0;
            seçilenpozisyon1[21] = "Depo";
            eniyidizilim1[21] = "Depo";
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

        private void button3_Click(object sender, EventArgs e)
        {
            // Lokasyon Çizimini Yaptırdık
           
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            lokasyonçiz();
            rotaçiz();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            #region Parametreleri Al

            durdurma = int.Parse(textBox2.Text);
            iterasyon = int.Parse(textBox3.Text);
            th = int.Parse(textBox4.Text);
            tabudöngü = int.Parse(textBox5.Text);

            #endregion

            #region Hata Kontrolü

            if (iterasyon < tabudöngü)
            {
                MessageBox.Show("Tabu Döngü değeri İterasyon Değerinden Büyük Olamaz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            #endregion

            #region Program Çalışır

            else // Tabu Döngü değeri İterasyon değerinden büyük değilse program çalışır.
            {
                #region Tabu Dizileri

                double[] zet1 = new double[iterasyon]; // Zet1 toplam değerlerini tutacak.
                double[] zet2 = new double[iterasyon]; // Zet2 toplam değerlerini tutacak.             
                double[] zet3 = new double[iterasyon]; // Zet3 Listview den sıra numarasını tutacak.
                double[] zet4 = new double[iterasyon]; // Zet4 Listview den sıra numarasını tutacak.
                double[,] zet = new double[th, 2];     // Tabu hafızada hem sıra numarası hemde toplam değeri tutacak.

                #endregion

                #region Değer Rastgele Üretilerek Başlayacaksa

                if (radioButton1.Checked == true)
                {
                    #region İlkDeğer

                    işlem.rastgeleseçim(seçilenpozisyon2, seçilenpozisyon1, eniyidizilim1, eniyidizilim2, d1, tablo); // İlk değeri seçimini işlem İşlemler sınıfından çağırdığımız rastgeleseçim metodu ile gerçekleştirdik.

                    sıralama += seçilenpozisyon1[0] + " - ";

                    sıralamaoluştur();

                    listviewyaz(listView1); // Değerleri Yazdırdık

                    eniyiçözüm = toplam; // İlk değer üretmede üretilen toplam değeri en iyi çözüm kabul ettik.
                    bölümeniyi = toplam; // Bu andaki en iyi çözüm kabul ettik.

                    #endregion

                    #region Tabu Kontrolü

                    for (int i2 = 0; i2 < durdurma; i2++) // Döngünün toplam kaç kere döneceğini gösteren for döngüsü.
                    {
                        i3 = 0;

                        for (int j1 = 0; j1 < iterasyon; j1++) // Her döngünün kaç kere döneceğini belirleyen for döngüsü.
                        {
                            #region Komşuluk Seçme Genel Tüm Döngüye Ait

                            komşulukseçme();

                            sıralamaoluştur();

                            #endregion

                            #region i2 ye Göre İlk İterasyon

                            // İlk iterasyonda hafızada kontrol edilecek hiç bir değer olmadığı için ilk iterasyonda kontrol olmadan hafızaya değerleri yazdırıyoruz.

                            if (i2 == 0)
                            {
                                #region Toplam ve Sıra Al

                                id++; // Üretilen her değerin sıralama numarasını tutacak.
                                listviewyaz(listView1); // Üretilen Değeri Listview e yazdırdık.

                                // Döngü iterasyonun son adımına gelene kadar Toplam ve Listview deki o anki sıra değeri dizilere yazdırdı.

                                zet1[j1] = toplam;
                                zet2[j1] = toplam;
                                zet3[j1] = sıra;
                                zet4[j1] = sıra;

                                #endregion

                                #region Eniyi Çözüm Kontrolü

                                if (toplam < bölümeniyi)
                                {
                                    i3++;

                                    bölümeniyi = toplam;

                                    for (int q = 0; q < 22; q++)
                                    {
                                        eniyidizilim1[q] = seçilenpozisyon1[q];
                                        eniyidizilim2[q] = seçilenpozisyon2[q];
                                    }

                                }

                                #endregion

                                #region Sonuç

                                if (j1 == iterasyon - 1)
                                {
                                    #region Tabu Sıralama

                                    tabusıralama(zet1, zet2, zet3, zet4);

                                    for (p2 = 0; p2 < tabudöngü; p2++)
                                    {
                                        zet[p2, 0] = zet4[p2];
                                        zet[p2, 1] = zet1[p2];
                                    }

                                    sıra2 = -1;

                                    for (p2 = 0; p2 < tabudöngü*(i2+1); p2++)
                                    {
                                        sıra2++;
                                        listView2.Items.Add(id2.ToString());
                                        listView2.Items[sıra2].SubItems.Add(listView1.Items[Convert.ToInt32(zet[p2, 0])].SubItems[1].Text);
                                        listView2.Items[sıra2].SubItems.Add(zet[p2, 1].ToString());

                                        id2++;
                                    }

                                    #endregion

                                    #region İlk En iyi Çözümden Daha İyisi Yoksa

                                    if (i3 == 0)
                                    {
                                        listBox3.Items.Add(i2);
                                        listBox4.Items.Add("Bulunmadı");

                                        for (int i4 = 0; i4 < 22; i4++)
                                        {
                                            seçilenpozisyon1[i4] = eniyidizilim1[i4];
                                            seçilenpozisyon2[i4] = eniyidizilim2[i4];
                                        }

                                        komşulukseçme();

                                        for (int i5 = 0; i5 < 22; i5++)
                                        {
                                            eniyidizilim1[i5] = seçilenpozisyon1[i5];
                                            eniyidizilim2[i5] = seçilenpozisyon2[i5];
                                        }

                                        for (int k2 = 1; k2 < 21; k2++) // Seçtiğimiz değerleri dizimize yazdırdık.
                                        {
                                            seçilenpozisyon1[k2] = tablo.Rows[seçilenpozisyon2[k2]]["Lokasyon"].ToString();
                                            sıralama += seçilenpozisyon1[k2] + " - ";
                                        }

                                        sıralama += seçilenpozisyon1[21]; // Sıralamaya yazdırdık.

                                        uzaklıkhesapla(); // Uzaklık Hesapladık.
                                        bölümeniyi = toplam;

                                    }

                                    #endregion

                                    #region İlk En İyi Çözümden Daha İyisi Varsa

                                    else if (i3!=0)
                                    {
                                        listBox3.Items.Add(i2);
                                        listBox4.Items.Add("Bulundu");

                                        if (Convert.ToDouble(zet[0, 1]) < eniyiçözüm)
                                        {
                                            //textBox8.Text = listView1.Items[Convert.ToInt32(zet[0, 0])].SubItems[1].Text;
                                            //textBox9.Text = zet[0, 1].ToString() + " - " + listView1.Items[Convert.ToInt32(zet[0, 0])].SubItems[0].Text;
                                            //textBox10.Text = zet[0, 1].ToString() + " - " + listView1.Items[Convert.ToInt32(zet[0, 0])].SubItems[0].Text;
                                            //textBox11.Text = listView1.Items[Convert.ToInt32(zet[0, 0])].SubItems[1].Text;

                                            eniyiçözüm = Convert.ToDouble(zet[0, 1]);

                                            for (int i6 = 0; i6 < 22; i6++)
                                            {
                                                seçilenpozisyon1[i6] = eniyidizilim1[i6];
                                                seçilenpozisyon2[i6] = eniyidizilim2[i6];
                                            }

                                        }
                                    }

                                    #endregion
                                }
                                #endregion
                            }
                            #endregion

                            #region Genel Döngünün 1. Adımından Sonraki Değerler   

                            if (i2 != 0)
                            {
                                #region Yeni Değer Üret

                                do
                                {
                                    i4 = 0;

                                    komşulukseçme();

                                    sıralamaoluştur(); // Bu metod içinde uzaklık hesaplanıyor.

                                    #region Tabu Listesinde Varmı Kontrol Et

                                    sıra2 = listView2.Items.Count;

                                    for (int i7 = 0; i7 < sıra2; i7++)
                                    {
                                        if (sıralama == listView2.Items[i7].SubItems[1].Text)
                                        {
                                            i4++;
                                            break;
                                        }
                                    }

                                } while (i4 != 0);
                                    #endregion

                                #endregion

                                #region Toplam ve Sıra Al

                                id++; // Üretilen her değerin sıralama numarasını tutacak.
                                listviewyaz(listView1); // Üretilen Değeri Listview e yazdırdık.

                                // Döngü iterasyonun son adımına gelene kadar Toplam ve Listview deki o anki sıra değeri dizilere yazdırdı.

                                zet1[j1] = toplam;
                                zet2[j1] = toplam;
                                zet3[j1] = sıra;
                                zet4[j1] = sıra;

                                #endregion

                                #region Eniyi Çözüm Kontrolü

                                i3 = 0;

                                if (toplam < bölümeniyi)
                                {
                                    i3++;

                                    bölümeniyi = toplam;

                                    for (int q = 0; q < 22; q++)
                                    {
                                        eniyidizilim1[q] = seçilenpozisyon1[q];
                                        eniyidizilim2[q] = seçilenpozisyon2[q];
                                    }

                                }

                                #endregion

                                #region Sonuç

                                if (j1 == iterasyon - 1)
                                {
                                    #region Tabu Sıralama

                                    tabusıralama(zet1, zet2, zet3, zet4);

                                    p4 = 0;
                                    o1 = 0;

                                    for (int p3 = p2; p3 < tabudöngü * (i2 + 1); p3++)
                                    {
                                        if (p3 < th)
                                        {
                                            zet[p3, 0] = zet4[o1];
                                            zet[p3, 1] = zet1[o1];

                                            if (p3==th-1)
                                            {
                                                o1 = -1;
                                            }
                                        }

                                        else
                                        {
                                            if (p4>=th)
                                            {
                                                o1 = 0;
                                                p4 = 0;
                                                zet[p4, 0] = zet4[o1];
                                                zet[p4, 1] = zet1[o1];
                                            }

                                            else
                                            {
                                                zet[p4, 0] = zet4[o1];
                                                zet[p4, 1] = zet1[o1];
                                               
                                            }

                                            p4++;
                                        }

                                        o1++;
                                    }

                                    p2 = p4;
                                    p8 = 0;

                                    listView2.Items.Clear();

                                    id2 = 1;
                                    sıra2 = -1;

                                    for (int p6 = 0; p6 < tabudöngü * (i2 + 1); p6++)  
                                    {
                                        if (p6 < th)
                                        {
                                            sıra2++;
                                            listView2.Items.Add(id2.ToString());
                                            listView2.Items[sıra2].SubItems.Add(listView1.Items[Convert.ToInt32(zet[p6, 0])].SubItems[1].Text);
                                            listView2.Items[sıra2].SubItems.Add(zet[p6, 1].ToString());

                                            if (p6 == th - 1)
                                            {
                                                p8 = 0;
                                                sıra2 = -1;
                                            }

                                        }

                                        else
                                        {
                                            if (sıra2==th-1)
                                            {
                                               p8 = 0;
                                               sıra2 = -1; 
                                            }

                                            sıra2++;
                                            listView2.Items[sıra2].SubItems.Add(listView1.Items[Convert.ToInt32(zet[p8, 0])].SubItems[1].Text);
                                            listView2.Items[sıra2].SubItems.Add(zet[p8, 1].ToString());
                                            p8++;

                                               
                                        }

                                        id2++;
                                    }

                                    #endregion

                                    #region İlk En iyi Çözümden Daha İyisi Yoksa

                                    if (i3 == 0)
                                    {
                                        for (int i6 = 0; i6 < 22; i6++)
                                        {
                                            seçilenpozisyon1[i6] = eniyidizilim1[i6];
                                            seçilenpozisyon2[i6] = eniyidizilim2[i6];
                                        }

                                        komşulukseçme();

                                        for (int i7 = 0; i7 < 22; i7++)
                                        {
                                            eniyidizilim1[i7] = seçilenpozisyon1[i7];
                                            eniyidizilim2[i7] = seçilenpozisyon2[i7];
                                        }

                                    }
                                    #endregion

                                    #region İlk En İyi Çözümden Daha İyisi Varsa

                                    else
                                    {
                                        //if (Convert.ToDouble(zet[0, 1]) < eniyiçözüm)
                                        //{
                                            textBox8.Text = listView1.Items[Convert.ToInt32(zet[0, 0])].SubItems[1].Text;
                                            textBox9.Text = zet[0, 1].ToString() + " - " + listView1.Items[Convert.ToInt32(zet[0, 0])].SubItems[0].Text;
                                            textBox10.Text = zet[0, 1].ToString() + " - " + listView1.Items[Convert.ToInt32(zet[0, 0])].SubItems[0].Text;
                                            textBox11.Text = listView1.Items[Convert.ToInt32(zet[0, 0])].SubItems[1].Text;
                                            eniyiçözüm = Convert.ToDouble(zet[0, 1]);
                                            eniyidizilim = textBox8.Text;

                                            for (int i6 = 0; i6 < 22; i6++)
                                            {
                                                seçilenpozisyon1[i6] = eniyidizilim1[i6];
                                                seçilenpozisyon2[i6] = eniyidizilim2[i6];
                                            }

                                        //}
                                    }
                               
                                    #endregion
                                }


                                #endregion

                            }

                            #endregion
                        }
                    }

                    #endregion

                    rotaçiz();
                }

                #endregion
            }

            #endregion
        }

      
    }
}
