using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace desktop1
{
     public partial class Form1 : Form
     {
        [System.Runtime.InteropServices.DllImport("user32.dll",
        CharSet = System.Runtime.InteropServices.CharSet.Auto,
        CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        static uint MOUSEEVENTF_DOWN;
        static uint MOUSEEVENTF_UP;
        Dictionary<Keys, string> myKeys = new Dictionary<Keys, string>(12);
        Keys bindKey;
        
        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = comboBox1.Items[0].ToString();
            comboBox2.Text = comboBox2.Items[0].ToString();
            textBox4.Enabled = false;
            this.KeyPreview = true;
            myKeys.Add(Keys.F1, "F1");
            myKeys.Add(Keys.F2, "F2");
            myKeys.Add(Keys.F3, "F3");
            myKeys.Add(Keys.F4, "F4");
            myKeys.Add(Keys.F5, "F5");
            myKeys.Add(Keys.F6, "F6");
            myKeys.Add(Keys.F7, "F7");
            myKeys.Add(Keys.F8, "F8");
            myKeys.Add(Keys.F9, "F9");
            myKeys.Add(Keys.F10, "F10");
            myKeys.Add(Keys.F11, "F11");
            myKeys.Add(Keys.F12, "F12");      
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox3.Enabled = true;
            }
            if(checkBox1.Checked==false)
            {
                textBox3.Enabled = false;
                textBox3.Clear();
            }
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar)) return;
            else
                e.Handled = true;
            if (e.KeyChar == '\b' && textBox3.Text.Length>0)
            {
                textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1);
                textBox3.Select(textBox3.Text.Length, 0);
            }
            else
            {
                return;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
                foreach (KeyValuePair<Keys, string> keyValue in myKeys)
                {
                    if (comboBox3.Text == keyValue.Value)
                    {
                        bindKey = keyValue.Key;
                    }
                }
                if (e.KeyCode == Keys.F4)
                {
                    e.Handled = true;
                }
                var counterClicks = 1;
                Stopwatch stopwatch = new Stopwatch();
                if (e.KeyCode==bindKey)
                {
                
                    int X = Control.MousePosition.X;
                    int Y = Control.MousePosition.Y;
                    //LEFTDOWN = 0x0002 , LEFTUP = 0x0004, RIGHTDOWN = 0x0008, RIGHTUP = 0x0010
                    if (comboBox2.Text == "right")
                    {
                        MOUSEEVENTF_DOWN = 0x0008;
                        MOUSEEVENTF_UP = 0x0010;
                    }
                    else
                    {
                        MOUSEEVENTF_DOWN = 0x0002;
                        MOUSEEVENTF_UP = 0x0004;
                    }
                    if (checkBox1.Checked == true)
                    {
                        counterClicks = Convert.ToInt32(textBox3.Text);

                    }
                    int interval = Convert.ToInt32((hours.Value * 3600000) + (minutes.Value * 60000) + (seconds.Value * 1000) + (santisecond.Value * 100) + (millisecond.Value * 10) + microsecond.Value);
                    if (comboBox1.Text == "single")
                    {
                        
                        stopwatch.Start();
                         for (int i = counterClicks; i >0; i--)
                        {
                            singleClick(X, Y);
                            Thread.Sleep(interval);
                        }
                        stopwatch.Stop();
                        

                    }
                    else
                    {
                        stopwatch.Start();
                        for (int i = counterClicks; i > 0; i--)
                        {
                            doubleClick(X, Y);
                            Thread.Sleep(interval);
                        }
                        stopwatch.Stop();
                        
                    }
                }
            label1.Text = String.Format("{0:N4} sec", stopwatch.ElapsedTicks / 10000000f);
        }
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }
        private void singleClick(int X,int Y)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_DOWN | MOUSEEVENTF_UP, X, Y, 0, 0);
        }
        private void doubleClick(int X,int Y)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_DOWN | MOUSEEVENTF_UP, X, Y, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_DOWN | MOUSEEVENTF_UP, X, Y, 0, 0);
        }

        private void microsecond_ValueChanged(object sender, EventArgs e)
        {

            if(microsecond.Value>=100)
            {
                millisecond.Value+= Math.Truncate(microsecond.Value/100); 
                microsecond.Value = microsecond.Value%100;
            }
        }

        private void millisecond_ValueChanged(object sender, EventArgs e)
        {
            if (millisecond.Value>=10)
            {
                santisecond.Value+=Math.Truncate(millisecond.Value/10);
                millisecond.Value =millisecond.Value%10;
            }
        }

        private void santisecond_ValueChanged(object sender, EventArgs e)
        {
            if(santisecond.Value>=10)
            {
                seconds.Value += Math.Truncate(santisecond.Value / 10);
                santisecond.Value = santisecond.Value % 10 ;
            }
        }

        private void seconds_ValueChanged(object sender, EventArgs e)
        {
            if(seconds.Value>=60)
            {
                minutes.Value+=Math.Truncate(seconds.Value/60);
                seconds.Value = seconds.Value%60;
            }
        }

        private void minutes_ValueChanged(object sender, EventArgs e)
        {
            if(minutes.Value>=60)
            {
                hours.Value+=Math.Truncate(minutes.Value/60);
                minutes.Value = minutes.Value%60;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private void addToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if(openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        groupBox1.BackgroundImage = new Bitmap(openFileDialog1.FileName);
        //        groupBox1.BackgroundImageLayout = ImageLayout.Stretch;
        //    }
        //}
    }
}
