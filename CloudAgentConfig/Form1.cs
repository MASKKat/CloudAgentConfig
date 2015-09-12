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
using System.Threading;
using System.Diagnostics;

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
            #region 服务程序数据库操作
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
                    tb2.Columns.Add(new SQLiteColumn("key", ColType.Text));
                    tb2.Columns.Add(new SQLiteColumn("value", ColType.Integer));
                  
                    sh.CreateTable(tb2);
                    sh.BeginTransaction();
                    try
                    {
                        var dic = new Dictionary<string, object>();                        
                        dic["ip"] = ip;
                        sh.Insert("server", dic);
                        var dicData = new Dictionary<string, object>();
                        var dicData2 = new Dictionary<string, object>();
                        var dicData3 = new Dictionary<string, object>();
                        dicData["key"] = "interval";
                        dicData["value"] = interval;
                        dicData2["key"] = "timeout";
                        dicData2["value"] = timeout;
                        dicData3["key"] = "hostset";
                        dicData3["value"] = "";
                        DataTable dt = sh.Select("select * from setting where key='interval';");
                        if(dt.Rows.Count>0)   
                        sh.Update("setting", dicData, "key", "interval");
                        else
                        sh.Insert("setting", dicData);
                        DataTable dt2 = sh.Select("select * from setting where key='timeout';");
                        if (dt2.Rows.Count > 0)
                            sh.Update("setting", dicData2, "key", "timeout");
                        else
                            sh.Insert("setting", dicData2);
                        DataTable dt3 = sh.Select("select * from setting where key='hostset';");
                        if (dt3.Rows.Count == 0)
                            sh.Insert("setting", dicData3);                    
                        sh.Commit();
                    }
                    catch
                    {
                        sh.Rollback();
                    }


                    conn.Close();
                }
            }

            #endregion
            #region 是否打开服务
           
            if (radioButton1.Checked) {
               
                Thread ce = new Thread(delegate ()
                {
                    Process p = new Process();
                    string path = System.Environment.CurrentDirectory;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.FileName = path + @"\CloudAgent.exe";
                    //MessageBox.Show(p.StartInfo.FileName);
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.EnableRaisingEvents = true;        
                    p.StartInfo.Arguments = "-install";
                    try
                    {
                       // MessageBox.Show(radioButton1.Checked.ToString());
                        p.Start();
                        
                        p.WaitForExit();
                    }
                    catch (System.ComponentModel.Win32Exception err)
                    {
                        MessageBox.Show("系统找不到指定的程序文件。\r{2}");
                        p.Close();
                        return;
                    }

                    p.Close();
                });
                ce.IsBackground = false;
                ce.Start();
                Thread ce2 = new Thread(delegate ()
                {
                    Process p = new Process();
                    string path = System.Environment.CurrentDirectory;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.FileName = path + @"\AutoOnOffLine.exe";
                    //MessageBox.Show(p.StartInfo.FileName);
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.EnableRaisingEvents = true;
                    p.StartInfo.Arguments = "-install";
                    try
                    {
                        // MessageBox.Show(radioButton1.Checked.ToString());
                        p.Start();

                        p.WaitForExit();
                    }
                    catch (System.ComponentModel.Win32Exception err)
                    {
                        MessageBox.Show("系统找不到指定的程序文件。\r{2}");
                        p.Close();
                        return;
                    }

                    p.Close();
                });
                ce2.IsBackground = false;
                ce2.Start();

            }
            if (radioButton2.Checked)
            {
                Thread ce = new Thread(delegate ()
                {
                    Process p = new Process();
                    string path = System.Environment.CurrentDirectory;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.FileName = path+@"\CloudAgent.exe";
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.EnableRaisingEvents = true;
                    p.StartInfo.Arguments = "-remove";
                    try
                    {
                        p.Start();
                        p.WaitForExit();
                    }
                    catch (System.ComponentModel.Win32Exception err)
                    {
                        MessageBox.Show("系统找不到指定的程序文件。\r{2}");
                        p.Close();
                        return;
                    }

                    p.Close();
                });
                ce.IsBackground = false;
                ce.Start();
                Thread ce2 = new Thread(delegate ()
                {
                    Process p = new Process();
                    string path = System.Environment.CurrentDirectory;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.FileName = path + @"\AutoOnOffLine.exe";
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.EnableRaisingEvents = true;
                    p.StartInfo.Arguments = "-remove";
                    try
                    {
                        p.Start();
                        p.WaitForExit();
                    }
                    catch (System.ComponentModel.Win32Exception err)
                    {
                        MessageBox.Show("系统找不到指定的程序文件。\r{2}");
                        p.Close();
                        return;
                    }

                    p.Close();
                });
                ce2.IsBackground = false;
                ce2.Start();
            }
            #endregion
           /* Environment.Exit(1);*/
           Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"data source=.\CloudAgent.db"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    sh.BeginTransaction();
                    try {
                        DataTable dt = sh.Select("select * from setting where key='interval';");
                        if (dt.Rows.Count > 0)
                        {
                            textBox5.Text = dt.Rows[0]["value"].ToString();

                        }
                        DataTable dt2 = sh.Select("select * from setting where key='timeout';");
                        if (dt2.Rows.Count > 0)
                        {
                            textBox6.Text = dt2.Rows[0]["value"].ToString();
                        }
                        DataTable dt3 = sh.Select("select * from server ;");
                        if (dt3.Rows.Count > 0)
                        {
                            string cosip = dt3.Rows[0]["ip"].ToString();
                           
                            string[] s = cosip.Split(new char[] { '.' });
                            textBox1.Text = s[0];
                            textBox2.Text = s[1];
                            textBox3.Text = s[2];
                            textBox4.Text = s[3];
                            for (int i=0;i< dt3.Rows.Count;i++)
                            {
                                comboBox1.Items.Add(dt3.Rows[i]["ip"].ToString());
                            }
                        }
                        sh.Commit();
                    }
                    catch
                    {
                        sh.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cosip = comboBox1.Text;
            string[] s = cosip.Split(new char[] { '.' });
            textBox1.Text = s[0];
            textBox2.Text = s[1];
            textBox3.Text = s[2];
            textBox4.Text = s[3];
           
        }
    }
}