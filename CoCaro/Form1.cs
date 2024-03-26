using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CoCaro
{
    public partial class frmCoCaro : Form
    {
        private CaroChess caroChess;
        private Graphics grs;
        public frmCoCaro()
        {
            InitializeComponent();
            btnPvsP.Click +=  new EventHandler(PvsP);
            caroChess = new CaroChess();
            caroChess.KhoiTaoMangOco();
            grs = pnlBanCo.CreateGraphics();
            pvsComToolStripMenuItem.Click += new EventHandler(PvsCom_Click);
            btnPvsCom.Click += new EventHandler(PvsCom_Click); 
        }

        /*private void BtnPvsP_Click2(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnPvsP_Click1(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnPvsP_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        } */

        private void tmChu_Tick(object sender, EventArgs e)
        {
            lblChuoiChu.Location = new Point(lblChuoiChu.Location.X, lblChuoiChu.Location.Y - 2);
            if(lblChuoiChu.Location.Y + lblChuoiChu.Height < 0)
            {
                lblChuoiChu.Location = new Point(lblChuoiChu.Location.X, pnlChu.Height);
            }
        }

        private void frmCoCaro_Load(object sender, EventArgs e)
        {
            lblChuoiChu.Text = "Dat duoc 1 duong thang, duong \ncheo, hoac duong ngang voi 5 o \nnhanh nhat la nguoi chien thang!";
            tmChu.Enabled = true;
          
        }

        private void pnlBanCo_Paint(object sender, PaintEventArgs e)
        {
            caroChess.VeBanCo(grs);
            caroChess.VeLaiQuanCo(grs);
        }

        private void pnlBanCo_MouseClick(object sender, MouseEventArgs e)
        {
            if (!caroChess.SanSang)
                return;
            if (caroChess.Danhco(e.X, e.Y, grs))
            {
                if (caroChess.KiemTraThangThua())
                    caroChess.KetThucTroChoi();
                else
                {
                    if (caroChess.CheDoChoi == 2)
                    {
                        caroChess.KhoiTaoCom(grs);
                        if (caroChess.KiemTraThangThua())
                            caroChess.KetThucTroChoi();
                    }
                }
            }
            
        }

        private void PvsP(object sender, EventArgs e)
        {
            grs.Clear(pnlBanCo.BackColor);
            caroChess.batdauPvsP(grs);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grs.Clear(pnlBanCo.BackColor);
            caroChess.Undo(grs); 
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            caroChess.Redo(grs);
        }
        private void PvsCom_Click(object sender, EventArgs e)
        {
            grs.Clear(pnlBanCo.BackColor);
            caroChess.batdauPvsCom(grs);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}