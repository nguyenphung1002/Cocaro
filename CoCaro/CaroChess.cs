using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace CoCaro
{
    internal class CaroChess
    {
        public enum KetThuc
        {
            Hoa,
            Player1,
            Player2,
            Com
        }
        public static Pen pen;
        public static SolidBrush sbWhite;
        public static SolidBrush sbBlack;
        public static SolidBrush sbLime;
        private Oco[,] _MangOco;
        private Banco _Banco;
        private int _LuotChoi;
        private int _CheDoChoi;
        private bool _SanSang;
        private Stack<Oco> stkCacNuocCoDaDi;
        private Stack<Oco> stkCacNuocDaUndo;
        private KetThuc _Ketthuc;
        public bool SanSang { get => _SanSang; }
        public int CheDoChoi { get => _CheDoChoi; }
        public CaroChess()
        {
            pen = new Pen(Color.Red);
            sbBlack = new SolidBrush(Color.Black);
            sbWhite = new SolidBrush(Color.White);
            sbLime = new SolidBrush(Color.Lime);
            _Banco = new Banco(20, 20);
            _MangOco = new Oco[_Banco.SoDong, _Banco.SoCot];
            stkCacNuocCoDaDi = new Stack<Oco>();
            stkCacNuocDaUndo = new Stack<Oco>();
            _LuotChoi = 1;
        }
        public void VeBanCo(Graphics g)
        {
            _Banco.VeBanCo(g);
        }
        public void KhoiTaoMangOco()
        {
            for (int i = 0; i < _Banco.SoDong; i++)
            {
                for (int j = 0; j < _Banco.SoCot; j++)
                {
                    _MangOco[i, j] = new Oco(i, j, new Point(j * Oco._Chieurong, i * Oco._ChieuCao), 0);
                }
            }
        }
        public bool Danhco(int MouseX, int MouseY, Graphics g)
        {
            if (MouseX % Oco._Chieurong == 0 || MouseY % Oco._ChieuCao == 0)
                return false;
            int Cot = MouseX / Oco._Chieurong;
            int Dong = MouseY / Oco._ChieuCao;
            if (_MangOco[Dong, Cot].Sohuu != 0)
                return false;
            switch (_LuotChoi)
            {
                case 1:
                    _MangOco[Dong, Cot].Sohuu = 1;
                    _Banco.VeQuanCo(g, _MangOco[Dong, Cot].Vitri, sbBlack);
                    _LuotChoi = 2;
                    break;
                case 2:
                    _MangOco[Dong, Cot].Sohuu = 2;
                    _Banco.VeQuanCo(g, _MangOco[Dong, Cot].Vitri, sbWhite);
                    _LuotChoi = 1;
                    break;
                default:
                    MessageBox.Show("Lỗi ");
                    break;
            }
            stkCacNuocDaUndo = new Stack<Oco>();
            Oco oco = new Oco(_MangOco[Dong, Cot].Dong, _MangOco[Dong, Cot].Cot, _MangOco[Dong, Cot].Vitri, _MangOco[Dong, Cot].Sohuu);
            stkCacNuocCoDaDi.Push(oco);
            return true;
        }
        public void VeLaiQuanCo(Graphics g)
        {
            foreach (Oco oco in stkCacNuocCoDaDi)
            {
                if (oco.Sohuu == 1)
                    _Banco.VeQuanCo(g, oco.Vitri, sbBlack);
                else if (oco.Sohuu == 2)
                    _Banco.VeQuanCo(g, oco.Vitri, sbWhite);
            }
        }
        public void batdauPvsP(Graphics g)
        {
            _SanSang = true;
            stkCacNuocCoDaDi = new Stack<Oco>();
            stkCacNuocDaUndo = new Stack<Oco>();
            _LuotChoi = 1;
            _CheDoChoi = 1;
            KhoiTaoMangOco();
            VeBanCo(g);
        }
        public void batdauPvsCom(Graphics g)
        {
            _SanSang = true;
            stkCacNuocCoDaDi = new Stack<Oco>();
            stkCacNuocDaUndo = new Stack<Oco>();
            _LuotChoi = 1;
            _CheDoChoi = 2; 
            KhoiTaoMangOco();
            VeBanCo(g);
            KhoiTaoCom(g);
        }

        public void Undo(Graphics g)
        {
            if (stkCacNuocCoDaDi.Count != 0)
            {
                Oco oco = stkCacNuocCoDaDi.Pop();
                stkCacNuocDaUndo.Push(new Oco(oco.Dong, oco.Cot, oco.Vitri, oco.Sohuu));
                _MangOco[oco.Dong, oco.Cot].Sohuu = 0;
               // _Banco.XoaQuanCo(g, oco.Vitri, sbLime);
                if (_LuotChoi == 1)
                    _LuotChoi = 2;
                else
                    _LuotChoi = 1;
            }
            VeBanCo(g);
            VeLaiQuanCo(g);       
        }
        public void Redo(Graphics g)
        {
            if (stkCacNuocDaUndo.Count != 0)
            {
                Oco oco = stkCacNuocDaUndo.Pop();
                stkCacNuocCoDaDi.Push(new Oco(oco.Dong,oco.Cot, oco.Vitri, oco.Sohuu));
                _MangOco[oco.Dong, oco.Cot].Sohuu = oco.Sohuu;
                _Banco.VeQuanCo(g, oco.Vitri, oco.Sohuu == 1?sbBlack : sbWhite);
                if (_LuotChoi == 1)
                    _LuotChoi = 2;
                else
                    _LuotChoi = 1;
            }
        }
        #region Xét Thắng Thua 
        public void KetThucTroChoi()
        {
            switch(_Ketthuc)
            {
                case KetThuc.Hoa:
                    MessageBox.Show("Hoa!");
                    break;
                case KetThuc.Player1:
                    MessageBox.Show("Com thang!");
                    break;
                case KetThuc.Player2:
                    MessageBox.Show("Player2 thang!");
                    break;
                case KetThuc.Com:
                    MessageBox.Show("Com thang!");
                    break;
            }
            _SanSang = false;
        }
        public bool KiemTraThangThua()
        {
            if( stkCacNuocCoDaDi.Count == _Banco.SoCot *_Banco.SoDong)
            {
                _Ketthuc = KetThuc.Hoa; 
                return true;
            }
            foreach (Oco oco in stkCacNuocCoDaDi)
            {
                if(DuyetDuongDoc(oco.Dong, oco.Cot, oco.Sohuu) || DuyetDuongNgang(oco.Dong, oco.Cot, oco.Sohuu) || DuyetDuongCheoHuyen(oco.Dong, oco.Cot, oco.Sohuu) || DuyetDuongCheoSac(oco.Dong, oco.Cot, oco.Sohuu ))
                {
                    _Ketthuc = oco.Sohuu == 1 ? KetThuc.Player1 : KetThuc.Player2;
                    return true;

                }
            }
            return false;

        }   
        private bool DuyetDuongDoc(int currDong, int currCot, int currSoHuu)
        {
            if (currDong > _Banco.SoDong - 5)
                return false;
            int Dem;
            for(Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOco[currDong + Dem, currCot].Sohuu != currSoHuu)
                    return false;
            }
            if (currDong == 0 || currDong + Dem == _Banco.SoDong)
                return true;
            if (_MangOco[currDong - 1, currCot].Sohuu == 0 || _MangOco[currDong + Dem, currCot].Sohuu == 0)
                return true;
            return false;
        }
        private bool DuyetDuongNgang(int currDong, int currCot, int currSoHuu)
        {
            if (currCot > _Banco.SoCot - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOco[currDong , currCot + Dem].Sohuu != currSoHuu)
                    return false;
            }
            if (currCot == 0 || currCot + Dem == _Banco.SoCot )
                return true;
            if (_MangOco[currDong , currCot - 1].Sohuu == 0 || _MangOco[currDong , currCot + Dem].Sohuu == 0)
                return true;
            return false;
        }
        private bool DuyetDuongCheoHuyen(int currDong, int currCot, int currSoHuu)
        {
            if (currDong > _Banco.SoDong -5 || currCot > _Banco.SoCot - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOco[currDong + Dem, currCot + Dem].Sohuu != currSoHuu)
                    return false;
            }
            if (currDong  == 0 ||currDong + Dem == _Banco.SoDong || currCot == 0 || currCot + Dem == _Banco.SoCot)
                return true;
            if (_MangOco[currDong - 1, currCot - 1].Sohuu == 0 || _MangOco[currDong + Dem, currCot + Dem].Sohuu == 0)
                return true;
            return false;
        }
        private bool DuyetDuongCheoSac(int currDong, int currCot, int currSoHuu)
        {
            if (currDong < 4 ||  currCot > _Banco.SoCot - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOco[currDong - Dem, currCot + Dem].Sohuu != currSoHuu)
                    return false;
            }
            if (currDong == 4 || currDong == _Banco.SoDong - 1 || currCot == 0 || currCot + Dem == _Banco.SoCot)
                return true;
            if (_MangOco[currDong + 1, currCot - 1].Sohuu == 0 || _MangOco[currDong - Dem, currCot + Dem].Sohuu == 0)
                return true;
            return false;
        }
        #endregion
        #region Ai
        private long[] MangDiemTanCong = new long[7] {0, 9, 54, 162, 1458, 13112, 118008 };
        private long[] MangDiemPhongNgu = new long[7] {0, 3, 27, 99, 729, 6561, 59049 };
        public void KhoiTaoCom(Graphics g)
        {
            if(stkCacNuocCoDaDi.Count == 0)
            {
                Danhco(_Banco.SoCot / 2 * Oco._Chieurong + 1, _Banco.SoDong / 2 * Oco._ChieuCao + 1, g);
            }
            else
            {
                Oco oco = TimNuocDi();
                Danhco(oco.Vitri.X + 1, oco.Vitri.Y + 1, g);
            }
        }
        private Oco TimNuocDi()
        {
            Oco oCoResult = new Oco();
            long DiemCaoNhat = 0;
            for(int i = 0; i < _Banco.SoDong; i++ )
            {
                for(int j = 0; j < _Banco.SoCot; j++)
                {
                    if (_MangOco[i,j].Sohuu == 0)
                    {
                        long DiemTanCong = DiemTanCong_DuyetDuongDoc(i,j) + DiemTanCong_DuyetDuongNgang(i,j) +DiemTanCong_DuyetDuongCheoSac(i,j)+  DiemTanCong_DuyetDuongCheoHuyen(i,j) ;
                        long DiemPhongNgu = DiemPhongNgu_DuyetDuongDoc(i,j) + DiemPhongNgu_DuyetDuongNgang(i,j) + DiemPhongNgu_DuyetDuongCheoSac(i,j) + DiemPhongNgu_DuyetDuongCheoHuyen(i,j);
                        long DiemTam = DiemTanCong > DiemPhongNgu ? DiemTanCong : DiemPhongNgu;
                        if (DiemCaoNhat < DiemTam)
                        {
                            DiemCaoNhat = DiemTam ;
                            oCoResult = new Oco(_MangOco[i, j].Dong, _MangOco[i, j].Cot, _MangOco[i, j].Vitri, _MangOco[i, j].Sohuu);


                        }
                    }
                }
            }

            return oCoResult;
        }
        #region DiemTanCong
        private long DiemTanCong_DuyetDuongDoc(int currDong, int currCot)
        {
            long DiemTong = 0;             
            int SoQuanCoTa = 0;
            int SoQuanCoDich = 0;
            for(int Dem = 1; Dem < 6 && currDong + Dem < _Banco.SoDong; Dem++)
            {
                if (_MangOco[currDong + Dem, currCot].Sohuu == 1)
                    SoQuanCoTa++;
                else if (_MangOco[currDong + Dem, currCot].Sohuu == 2)
                {
                    SoQuanCoDich++;
                    break;
                }
                else             
                    break;             
            }
            for (int Dem = 1; Dem < 6 && currDong - Dem >= 0; Dem++)
            {
                if (_MangOco[currDong - Dem, currCot].Sohuu == 1)
                    SoQuanCoTa++;
                else if (_MangOco[currDong - Dem, currCot].Sohuu == 2)
                {
                    SoQuanCoDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanCoDich == 2)
                return 0;
            DiemTong -= MangDiemPhongNgu[SoQuanCoDich + 1]*2;
            //DiemTC += MangDiemTanCong[SoQuanCoTa];
            //DiemTong += DiemTC;
            DiemTong += MangDiemTanCong[SoQuanCoTa];
            return DiemTong;
        }
        private long DiemTanCong_DuyetDuongNgang(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanCoTa = 0;
            int SoQuanCoDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _Banco.SoCot; Dem++)
            {
                if (_MangOco[currDong, currCot + Dem].Sohuu == 1)
                    SoQuanCoTa++;
                else if (_MangOco[currDong, currCot + Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;
                    break;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0; Dem++)
            {
                if (_MangOco[currDong, currCot - Dem].Sohuu == 1)
                    SoQuanCoTa++;
                else if (_MangOco[currDong , currCot - Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanCoDich == 2)
                return 0;
            DiemTong -= MangDiemPhongNgu[SoQuanCoDich + 1]*2;
            //DiemTC += MangDiemTanCong[SoQuanCoTa];
            //DiemTong += DiemTC;
            DiemTong += MangDiemTanCong[SoQuanCoTa];
            return DiemTong;
        }
        private long DiemTanCong_DuyetDuongCheoSac(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanCoTa = 0;
            int SoQuanCoDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _Banco.SoCot && currDong - Dem >= 0; Dem++)
            {
                if (_MangOco[currDong - Dem, currCot + Dem].Sohuu == 1)
                    SoQuanCoTa++;
                else if (_MangOco[currDong - Dem, currCot + Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;
                    break;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong + Dem < _Banco.SoDong; Dem++)
            {
                if (_MangOco[currDong + Dem, currCot - Dem].Sohuu == 1)
                    SoQuanCoTa++;
                else if (_MangOco[currDong + Dem, currCot - Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanCoDich == 2)
                return 0;
            DiemTong -= MangDiemPhongNgu[SoQuanCoDich + 1]*2;
            //DiemTC += MangDiemTanCong[SoQuanCoTa];
            //DiemTong += DiemTC;
            DiemTong += MangDiemTanCong[SoQuanCoTa];
            return DiemTong;
        }
        private long DiemTanCong_DuyetDuongCheoHuyen(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanCoTa = 0;
            int SoQuanCoDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _Banco.SoCot && currDong + Dem < _Banco.SoDong; Dem++)
            {
                if (_MangOco[currDong + Dem, currCot + Dem].Sohuu == 1)
                    SoQuanCoTa++;
                else if (_MangOco[currDong + Dem, currCot + Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;
                    break;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong - Dem >= 0; Dem++)
            {
                if (_MangOco[currDong - Dem, currCot - Dem].Sohuu == 1)
                    SoQuanCoTa++;
                else if (_MangOco[currDong - Dem, currCot - Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanCoDich == 2)
                return 0;
            DiemTong -= MangDiemPhongNgu[SoQuanCoDich + 1]*2;
            //DiemTC += MangDiemTanCong[SoQuanCoTa];
            //DiemTong += DiemTC;
            DiemTong += MangDiemTanCong[SoQuanCoTa];
            return DiemTong;
        }
       
        #endregion
        #region DiemPhongNgu
        private long DiemPhongNgu_DuyetDuongDoc(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanCoTa = 0;
            int SoQuanCoDich = 0;
            for (int Dem = 1; Dem < 6 && currDong + Dem < _Banco.SoDong; Dem++)
            {
                if (_MangOco[currDong + Dem, currCot].Sohuu == 1)
                {
                    SoQuanCoTa++;
                    break;
                }
                else if (_MangOco[currDong + Dem, currCot].Sohuu == 2)
                {
                    SoQuanCoDich++;                  
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currDong - Dem >= 0; Dem++)
            {
                if (_MangOco[currDong - Dem, currCot].Sohuu == 1)
                {
                    SoQuanCoTa++;
                    break;
                }
                else if (_MangOco[currDong - Dem, currCot].Sohuu == 2)
                {
                    SoQuanCoDich++; 
                }
                else
                    break;
            }
            if (SoQuanCoTa == 2)
                return 0;        
            DiemTong += MangDiemPhongNgu[SoQuanCoDich];
            return DiemTong;
        }
        private long DiemPhongNgu_DuyetDuongNgang(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanCoTa = 0;
            int SoQuanCoDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _Banco.SoCot; Dem++)
            {
                if (_MangOco[currDong, currCot + Dem].Sohuu == 1)
                {
                    SoQuanCoTa++;
                    break;
                }
                else if (_MangOco[currDong, currCot + Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;               
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0; Dem++)
            {
                if (_MangOco[currDong, currCot - Dem].Sohuu == 1)
                {
                    SoQuanCoTa++;
                    break;
                }
                else if (_MangOco[currDong, currCot - Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;
                }
                else
                    break;
            }
            if (SoQuanCoTa == 2)
                return 0;
            DiemTong += MangDiemPhongNgu[SoQuanCoDich];
            return DiemTong;
        }
        private long DiemPhongNgu_DuyetDuongCheoSac(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanCoTa = 0;
            int SoQuanCoDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _Banco.SoCot && currDong - Dem >= 0; Dem++)
            {
                if (_MangOco[currDong - Dem, currCot + Dem].Sohuu == 1)
                {
                    SoQuanCoTa++;
                    break;
                }
                else if (_MangOco[currDong - Dem, currCot + Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong + Dem < _Banco.SoDong; Dem++)
            {
                if (_MangOco[currDong + Dem, currCot - Dem].Sohuu == 1)
                {
                    SoQuanCoTa++;
                    break;
                }
                else if (_MangOco[currDong + Dem, currCot - Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;
                }
                else
                    break;
            }
            if (SoQuanCoTa == 2)
                return 0;
            DiemTong += MangDiemPhongNgu[SoQuanCoTa];
            return DiemTong;
        }
        private long DiemPhongNgu_DuyetDuongCheoHuyen(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanCoTa = 0;
            int SoQuanCoDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _Banco.SoCot && currDong + Dem < _Banco.SoDong; Dem++)
            {
                if (_MangOco[currDong + Dem, currCot + Dem].Sohuu == 1)
                {
                    SoQuanCoTa++;
                    break;
                }
                else if (_MangOco[currDong + Dem, currCot + Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong - Dem >= 0; Dem++)
            {
                if (_MangOco[currDong - Dem, currCot - Dem].Sohuu == 1)
                {
                    SoQuanCoTa++;
                    break;
                }
                else if (_MangOco[currDong - Dem, currCot - Dem].Sohuu == 2)
                {
                    SoQuanCoDich++;
                }
                else
                    break;
            }
            if (SoQuanCoTa== 2)
                return 0;
            DiemTong += MangDiemPhongNgu[SoQuanCoTa];
            return DiemTong;
        }
       
        #endregion

        #endregion
    }
}
