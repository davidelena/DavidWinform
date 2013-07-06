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
    public partial class ThreadForm : Form
    {
        private delegate void AddMessageHandler(string message);
        ThreadMethod method;
        Thread thread1, thread2;

        public void AddMesssage(string message)
        {
            if (rcTextbox.InvokeRequired)
            {
                AddMessageHandler handler = AddMesssage;
                rcTextbox.Invoke(handler, message);
            }
            else
            {
                rcTextbox.AppendText(message);
            }
        }

        public ThreadForm()
        {
            InitializeComponent();
            method = new ThreadMethod(this);
            btnStart.Click += new EventHandler(btnStart_Click);
            btnAbort.Click += new EventHandler(btnAbort_Click);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            rcTextbox.Clear();
            method.shouldStop = false;
            thread1 = new Thread(method.Method_A);
            thread2 = new Thread(method.Method_B);
            thread1.IsBackground = true;
            thread2.IsBackground = true;
            thread1.Start("\n A线程开始了！");
            thread2.Start();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            method.shouldStop = true;
            thread1.Join(0);
            thread2.Join(0);
        }
    }

    public class ThreadMethod
    {
        public volatile bool shouldStop;
        private ThreadForm threadForm;

        public ThreadMethod(ThreadForm form)
        {
            this.threadForm = form;
        }

        public void Method_A(object obj)
        {
            string s = obj as string;
            threadForm.AddMesssage(s);
            while (shouldStop == false)
            {
                Thread.Sleep(100);
                threadForm.AddMesssage("a");
            }
            threadForm.AddMesssage("\n线程Method A已终止了。");
        }

        public void Method_B(object obj)
        {
            //string s = obj as string;
            //threadForm.AddMesssage(s);
            while (shouldStop == false)
            {
                Thread.Sleep(100);
                threadForm.AddMesssage("b");
            }
            threadForm.AddMesssage("\n线程Method B已终止了。");
        }
    }
}
