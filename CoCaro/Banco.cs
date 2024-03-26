using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace CoCaro
{
    internal class Banco
    {
        private int _SoDong;

        private int _SoCot;
          public int SoDong { get => _SoDong; }
          public int SoCot { get => _SoCot; }

        public Banco()
        {
            _SoDong = 0;
            _SoCot = 0;
        }
        public Banco(int soDong, int soCot)
        {
            _SoDong = soDong;
            _SoCot = soCot; 
        }

      
     

        public void VeBanCo ( Graphics g) 
        {
            for( int i = 0; i < _SoCot; i++)
            {
                g.DrawLine(CaroChess.pen, i * Oco._Chieurong, 0, i * Oco._Chieurong, _SoDong * Oco._ChieuCao); 
            }
            for (int j = 0; j < _SoDong; j++)
            {
                g.DrawLine(CaroChess.pen, 0, j * Oco._ChieuCao, _SoCot * Oco._Chieurong, j * Oco._ChieuCao );
            }
        }

        public void VeQuanCo(Graphics g, Point point, SolidBrush sb )
        {
            g.FillEllipse(sb, point.X , point.Y , Oco._Chieurong  , Oco._ChieuCao ); 
        }
        /*public void XoaQuanCo(Graphics g, Point point, SolidBrush sb)
        {
            g.FillRectangle(sb, point.X + 1, point.Y + 1, Oco._Chieurong - 2, Oco._ChieuCao - 2);
        }*/
    }
}
