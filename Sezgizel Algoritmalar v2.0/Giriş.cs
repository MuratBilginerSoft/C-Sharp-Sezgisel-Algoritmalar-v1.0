using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sezgizel_Algoritmalar_v2._0
{
    public partial class Giriş : Form
    {
        // Region Yöntemi daha temiz kod yazabilmek için kodları bölgesel gruplayan bir yöntem burada bunu kullandık.
        #region Tanımlamalar 


        Formaç yeniform = new Formaç(); // Formaç isimli Classımızdan bir nesne türettik.
        TavlamaBenzetimi tavlamayeni = new TavlamaBenzetimi(); // TavlamaBenzetimi Formumuzndan bir nesne yeni bir form türettik.
        TabuArama tabuyeni = new TabuArama(); // Tabu Arama Formundan bir nesne türettik.
        GenetikAlgoritma genetikyeni = new GenetikAlgoritma(); // Genetik Algoritma Formumuzdan bir nesne türettik.

        #endregion

        // endregion Region yönteminin kapanış kodu.

        public Giriş()
        {
            InitializeComponent();
        }

        private void Giriş_Load(object sender, EventArgs e)
        {

        }

        // TOPLU NOT: Aşağıdaki kodlar diğer formların açılmasını sağlayacak kodlardır. Yazmış olduğumuz Public FormAç sınıfının git() metodunu kullanmaktadır. Sınıfla ilgili detaylı bilgiyi Solutuion Explorerdan FormAç sınıfına girerek görebilirsiniz.

        private void button1_Click(object sender, EventArgs e)
        {
            // Tavlama Benzetimi Algoritması Butonu

            yeniform.git(this, tavlamayeni);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Tabu Arama Algoritması Butonu

            yeniform.git(this, tabuyeni);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Genetik Algoritma Butonu

            yeniform.git(this, genetikyeni);
        }
    }
}
