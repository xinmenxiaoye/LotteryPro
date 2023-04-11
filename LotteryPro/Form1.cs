using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LotteryPro
{
    public partial class Form1 : Form
    {
        private Selector ObjSelector = new Selector();
        private PrintDocument printDoc = new PrintDocument();//创建打印对象,可以调用方法设定选中的打印机，目前只有一台，所以不需设置

        public Form1()
        {

            InitializeComponent();
            this.btnClear.Enabled = false;
            this.btnAgree.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnPrint.Enabled = false;
            this.printDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.LotteryPrintPage);
        }
        #region  拖动窗体的实现

        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//标签是否为左键
        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }
        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }
        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }

        }

        #endregion


        private void btnClo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }






        private void btnGroup_Click(object sender, EventArgs e)
        {
            //this.ObjSelector.CreatGroupNums(Convert.ToInt32(this.txtGroup.Text.Trim()));
            //Convert.ToInt32(this.txtGroup.Text.Trim());
            //for (int i = 0; i < Convert.ToInt32(this.txtGroup.Text.Trim()); i++)
            //{
            //   this.ObjSelector.SelectedNums.Add(this.ObjSelector.CreateNum());
            //}
            this.ObjSelector.CreatGroupNums(Convert.ToInt32(this.txtGroup.Text.Trim()));
            ShowSeletedNums();
            this.btnPrint.Enabled = true;
            this.btnClear.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnRandom.Enabled = true;
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            string[] selectNum =
              {
                this.txtNum1.Text,
                this.txtNum2.Text,
                this.txtNum3.Text,
                this.txtNum4.Text,
                this.txtNum5.Text,
                this.txtNum6.Text,
                this.txtNum7.Text,
            };
            this.ObjSelector.SelectedNums.Add(selectNum);
            ShowSeletedNums();

        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            this.RandomTimer.Start();
            this.btnAgree.Enabled = true;
            this.btnRandom.Enabled = false;

        }

        private void btnAgree_Click(object sender, EventArgs e)
        {
            this.RandomTimer.Stop();
            string[] selectNum =
            {
                this.lblNum1.Text,
                this.lblNum2.Text,
                this.lblNum3.Text,
                this.lblNum4.Text,
                this.lblNum5.Text,
                this.lblNum6.Text,
                this.lblNum7.Text,
            };
            this.ObjSelector.SelectedNums.Add(selectNum);
            ShowSeletedNums();
        }
        private void ShowSeletedNums()
        {
            this.lbNumList.Items.Clear();
            this.lbNumList.Items.AddRange(this.ObjSelector.GetPrintNums().ToArray());
            this.btnPrint.Enabled = true;
            this.btnClear.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnRandom.Enabled = true;

        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.lbNumList.Items.Count == 0 || this.lbNumList.SelectedItem == null)
            {
                return;
            }
            int index = this.lbNumList.SelectedIndex;
            ObjSelector.SelectedNums.RemoveAt(index);
            ShowSeletedNums();
            if (ObjSelector.SelectedNums.Count == 0)
            {
                this.btnClear.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPrint.Enabled = false;
            }

        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            this.lbNumList.Items.Clear();
            this.ObjSelector.SelectedNums.Clear();
            this.lblNum1.Text = "0";
            this.lblNum2.Text = "0";
            this.lblNum3.Text = "0";
            this.lblNum4.Text = "0";
            this.lblNum5.Text = "0";
            this.lblNum6.Text = "0";
            this.lblNum7.Text = "0";
            this.txtNum1.Text = "";
            this.txtNum2.Text = "";
            this.txtNum3.Text = "";
            this.txtNum4.Text = "";
            this.txtNum4.Text = "";
            this.txtNum5.Text = "";
            this.txtNum6.Text = "";
            this.txtNum7.Text = "";
            this.btnClear.Enabled = false;
            this.btnAgree.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnPrint.Enabled = false;

        }

        private void RandomTimer_Tick(object sender, EventArgs e)
        {
            string[] numList = ObjSelector.CreateNum();
            this.lblNum1.Text = numList[0];
            this.lblNum2.Text = numList[1];
            this.lblNum3.Text = numList[2];
            this.lblNum4.Text = numList[3];
            this.lblNum5.Text = numList[4];
            this.lblNum6.Text = numList[5];
            this.lblNum7.Text = numList[6];

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.printDoc.Print();
        }
        private void LotteryPrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string serialNum = DateTime.Now.ToString("yyyyMMddHHmmssms");//流水号（生成条码使用）
            this.ObjSelector.PrintLottery(e, serialNum, ObjSelector.GetPrintNums());//调用前面编写的打印方法，为了省略代码，objSelector.GetPrintedNums()删除了，其返回类型为List<string>
        }
        private void lbNumList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }




}
