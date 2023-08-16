using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Memory.Win64;
using Memory.Utils;

namespace Testicles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ulong targetAddrTS;
        ulong baseAddrFOW, baseAddrDBG, baseAddrAT;
        MemoryHelper64 helper;
        private void Form1_Load(object sender, EventArgs e)
        {
            Process p = Process.GetProcessesByName("hoi4").FirstOrDefault();
            if (p == null) return;

            helper = new MemoryHelper64(p);

            // getting addresses for FOW(0x24C945A), tagswitch TS(0x25E9D00) (0xE40), debug DBG(0x25E9829) and allowtraits AT(0x24C9438)

            //Tag Switch
            ulong baseAddrTS = helper.GetBaseAddress(0x25E9D00);
            int[] offsetTS = { 0xE40 };
            targetAddrTS = MemoryUtils.OffsetCalculator(helper, baseAddrTS, offsetTS);


            //These ones only need base address because I got the static address without pointers, only tag switching needs pointers
            //Also Only Tagswitch is 4bytes, everything else is a byte

            //FOW
            baseAddrFOW = helper.GetBaseAddress(0x24C945A);

            //AllowTraits
            baseAddrAT = helper.GetBaseAddress(0x24C9438);

            //Debug
            baseAddrDBG = helper.GetBaseAddress(0x25E9829);


            //DYNAMIC LABELS
            label1.Text = helper.ReadMemory<Int32>(targetAddrTS).ToString();
            label4.Text = helper.ReadMemory<Byte>(baseAddrFOW).ToString();
            label5.Text = helper.ReadMemory<Byte>(baseAddrAT).ToString();
            label6.Text = helper.ReadMemory<Byte>(baseAddrDBG).ToString();
            
            //MANUAL LABELS
            label2.Text = "Current Country ID: ";
            label3.Text = "Enter country ID and press tagswitch to change countries";
            timer1.Start();
        }

      


        //Refresh to see new memory value
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            label1.Text = helper.ReadMemory<Int32>(targetAddrTS).ToString();
            label4.Text = helper.ReadMemory<Byte>(baseAddrFOW).ToString();
            label5.Text = helper.ReadMemory<Byte>(baseAddrAT).ToString();
            label6.Text = helper.ReadMemory<Byte>(baseAddrDBG).ToString();


        }

        //BUTTONS
        private void button1_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Int32>(targetAddrTS, Int32.Parse(textBox1.Text));
        }  
        private void button7_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrDBG, Byte.Parse("1"));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrFOW, Byte.Parse("0"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrAT, Byte.Parse("0"));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrDBG, Byte.Parse("0"));
        }

       

        private void button5_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrAT, Byte.Parse("1"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrFOW, Byte.Parse("1"));
        } 

        //Youtube Link
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/@AdamDX1337");
        }
    }
}
