using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DavidWinform
{
    public class Account
    {
        private object lockObj = new object();
        private int balance;
        private AccountBalanceForm form;
        private Random rd;

        public Account(int balance, AccountBalanceForm form)
        {
            this.balance = balance;
            this.form = form;
            rd = new Random();
        }

        public void Widthdraw(int cost)
        {
            if (balance <= 0)
                form.AddListBoxItem("账户余额已经小于0元，还想拿钱！");

            lock (lockObj)
            {
                if (balance >= cost)
                {
                    int original = balance;
                    balance = balance - cost;
                    string info = string.Format("当前线程：{0}，账户当前余额：{1}元，本次提取{2}元，账户剩余余额为：{3}", Thread.CurrentThread.Name, original, cost, balance);
                    form.AddListBoxItem(info);
                }
            }
        }

        public void DoTransation()
        {
            for (int i = 0; i < 100; i++)
            {
                Widthdraw(rd.Next(1, 100));
            }
        }
    }
}
