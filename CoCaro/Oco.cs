using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CoCaro
{
    internal class Oco
    {
        public const int _Chieurong = 25;
        public const int _ChieuCao = 25;

        private int _Dong;
        public int Dong { get => _Dong; set => _Dong = value; }        

        private int _Cot;
        public int Cot { get => _Cot; set => _Cot = value; }
        
        private Point _Vitri;
        public Point Vitri { get => _Vitri; set => _Vitri = value; }
       
        private int _Sohuu; 
        public int Sohuu { get => _Sohuu; set => _Sohuu = value; }
        
        public Oco()
        {

        }
        public Oco( int dong, int cot, Point vitri, int sohuu )
        {
            _Dong = dong;
            _Cot = cot;
            _Vitri = vitri;
            _Sohuu = sohuu;
        }
    }
}
