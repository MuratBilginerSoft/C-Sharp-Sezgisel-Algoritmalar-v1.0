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
    class İşlemler
    {
        public void rastgeleseçim(int[] x, string[] y, string[] u, int[] k, int d, DataTable z)
        {
            Random r=new Random();
            y[0] = "Depo";
            x[0] = 0;
            u[0] = "Depo";
            k[0] = 0;

            for (int i = 1; i < 21; i++)
            {
                do
                {
                    d = r.Next(1, 21);
                } while (Array.IndexOf(x, d) != -1);

                x[i] = d;
                k[i] = d;
                u[i] = z.Rows[d]["Lokasyon"].ToString();
                y[i] = z.Rows[d]["Lokasyon"].ToString();
            }
        }

        public void rastgeleseçim2(int[] x, string[] y, int d, DataTable z)
        {
            Random r = new Random();
            y[0] = "Depo";
            x[0] = 0;
          
            for (int i = 1; i < 21; i++)
            {
                do
                {
                    d = r.Next(1, 21);
                } while (Array.IndexOf(x, d) != -1);

                x[i] = d;
                y[i] = z.Rows[d]["Lokasyon"].ToString();
            }
        }

        public void dosyaismial(OpenFileDialog x,TextBox y,string z,Form t)
        {
            t = new Form();
            x.Filter = "Excel Dosyası |*.xlsx| Excel Dosyası |*.xls";
            x.ShowDialog();
            y.Text = x.SafeFileName;
            z = x.FileName;
            
        }


    }
}
