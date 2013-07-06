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
    public partial class AccountBalanceForm : Form
    {
        private delegate void FormHandler(string str);
        private Account account;

        public AccountBalanceForm()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// 根据操作当前方法的线程是不是创建该对象的线程做出相应操作
        /// </summary>
        /// <param name="str"></param>
        public void AddListBoxItem(string str)
        {
            if (listBoxInfo.InvokeRequired)
            {
                FormHandler handler = new FormHandler(AddListBoxItem);
                listBoxInfo.Invoke(handler, str);
            }
            else
            {
                listBoxInfo.Items.Add(str);
            }
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            listBoxInfo.Items.Clear();
            List<Thread> threadLs = new List<Thread>();
            account = new Account(1000, this);
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(account.DoTransation));
                thread.Name = string.Format("Thread{0}", i + 1);
                threadLs.Add(thread);
            }

            foreach (Thread thread in threadLs)
            {
                thread.Start();
            }
        }
    }
}
