using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Data.SQLite;
namespace CloudAgentConfig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //判断按键是不是要输入的类型。
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
                e.Handled = true;
            //小数点的处理。
            if ((int)e.KeyChar == 46)                           //小数点
            {
                if (textBox1.Text.Length <= 0)
                    e.Handled = true;   //小数点不能在第一位
                else
                {
                    e.Handled = true;
                    SendKeys.Send("{TAB}");
                    return;
                }
            }
            if(textBox1.Text.Length >= 3 && (int)e.KeyChar != 8)
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
         
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //判断按键是不是要输入的类型。
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
                e.Handled = true;
            //小数点的处理。
            if ((int)e.KeyChar == 46)                           //小数点
            {
                if (textBox2.Text.Length <= 0)
                    e.Handled = true;   //小数点不能在第一位
                else
                {
                    e.Handled = true;
                    SendKeys.Send("{TAB}");
                    return;
                }
            }
            if (textBox2.Text.Length >= 3 && (int)e.KeyChar != 8)
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
            if (textBox2.Text.Length <= 0 && (int)e.KeyChar == 8)
                textBox1.Focus();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //判断按键是不是要输入的类型。
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
                e.Handled = true;
            //小数点的处理。
            if ((int)e.KeyChar == 46)                           //小数点
            {
                if (textBox3.Text.Length <= 0)
                    e.Handled = true;   //小数点不能在第一位
                else
                {
                    e.Handled = true;
                    SendKeys.Send("{TAB}");
                    return;
                }
            }
            if (textBox3.Text.Length >= 3 && (int)e.KeyChar != 8)
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
            if (textBox3.Text.Length <= 0 && (int)e.KeyChar == 8)
                textBox2.Focus();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //判断按键是不是要输入的类型。
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
                e.Handled = true;
            //小数点的处理。
            if ((int)e.KeyChar == 46)                           //小数点
            {
                if (textBox4.Text.Length <= 0)
                    e.Handled = true;   //小数点不能在第一位
                else
                {
                    e.Handled = true;
                    SendKeys.Send("{TAB}");
                    return;
                }
            }
            if (textBox4.Text.Length >= 3 && (int)e.KeyChar != 8)
            {
                e.Handled = true;
               
            }
            if (textBox4.Text.Length <= 0 && (int)e.KeyChar == 8)
                textBox3.Focus();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            //判断按键是不是要输入的类型。
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 )
                e.Handled = true;
           
            if (textBox5.Text.Length >= 3 && (int)e.KeyChar != 8)
            {
                e.Handled = true;

            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            //判断按键是不是要输入的类型。
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8)
                e.Handled = true;

            if (textBox6.Text.Length >= 3 && (int)e.KeyChar != 8)
            {
                e.Handled = true;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 判断IP的合法性 
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    if (string.IsNullOrEmpty((c as TextBox).Text))
                    {
                        MessageBox.Show("有设置选项未填写。", "错误!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            foreach (Control c in this.panel1.Controls)
            {
                if (c is TextBox)
                {
                    if (string.IsNullOrEmpty((c as TextBox).Text))
                    {
                        MessageBox.Show("COS服务器IP未填写。", "错误!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (Convert.ToInt32(c.Text) >= 256)
                    {
                        MessageBox.Show("请输入正确的IP地址。", "错误!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }


            #endregion
            #region 服务程序注册表操作
            string ip = textBox1.Text + "." + textBox2.Text + "." + textBox3.Text + "." + textBox4.Text;
            int interval = Convert.ToInt32(textBox5.Text);
            int timeout = Convert.ToInt32(textBox6.Text);
            using (SQLiteConnection conn = new SQLiteConnection(@"data source=.\CloudAgent.db"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();                   
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    SQLiteTable tb = new SQLiteTable("server");
                    tb.Columns.Add(new SQLiteColumn("id", true));
                    tb.Columns.Add(new SQLiteColumn("ip", ColType.Text));
                    sh.CreateTable(tb);
                    SQLiteTable tb2 = new SQLiteTable("setting");
                    tb2.Columns.Add(new SQLiteColumn("id", true));
                    tb2.Columns.Add(new SQLiteColumn("hostset", ColType.Integer));
                    tb2.Columns.Add(new SQLiteColumn("interval", ColType.Decimal, false, false,false, "0.0"));
                    tb2.Columns.Add(new SQLiteColumn("timeout", ColType.Decimal, false, false, false, "0.0"));
                    sh.CreateTable(tb2);
                    sh.BeginTransaction();
                    try
                    {
                        var dic = new Dictionary<string, object>();                        
                        dic["ip"] = ip;
                        sh.Insert("server", dic);
                        var dicData = new Dictionary<string, object>();
                        dicData["interval"] = interval;
                        dicData["timeout"] = timeout;
                        DataTable dt = sh.Select("select * from setting order by id; ");
                        if(dt.Rows.Count>0)   
                        sh.Update("setting", dicData, "id", 1);
                        else
                        sh.Insert("setting", dicData);
                        sh.Commit();
                    }
                    catch
                    {
                        sh.Rollback();
                    }


                    conn.Close();
                }
            }
            Environment.Exit(1);
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}