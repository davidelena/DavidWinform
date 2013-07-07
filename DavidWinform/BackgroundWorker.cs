using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DavidWinform
{
    public partial class BackgroundWorker : Form
    {
        public BackgroundWorker()
        {
            InitializeComponent();
            btnStop.Enabled = false;
            backgroundWorkerItem.WorkerReportsProgress = true;
            backgroundWorkerItem.WorkerSupportsCancellation = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            richTextBackground.Text = "开始产生1~10000的随机数...\n\n";
            backgroundWorkerItem.RunWorkerAsync();
        }

        private void backgroundWorkerItem_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgWorker = sender as BackgroundWorker;
            Random r = new Random();
            int numCount = 0;
            while (!backgroundWorkerItem.CancellationPending)
            {
                int num = r.Next(10000);
                if (num % 5 == 0)
                {
                    numCount++;
                    backgroundWorkerItem.ReportProgress(0, num);
                    Thread.Sleep(1000);
                }
            }
            e.Result = numCount;
        }

        private void backgroundWorkerItem_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int num = (int)e.UserState;
            richTextBackground.Text += num + " ";
        }

        private void backgroundWorkerItem_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                richTextBackground.Text += string.Format("\n\n当前产生了{0}个能被5整除的随机数！", (int)e.Result);
            }
            else
            {
                richTextBackground.Text += string.Format("\n\n操作过程中产生了错误..." + e.Error);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            backgroundWorkerItem.CancelAsync();
        }
    }
}
