using System;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using Memory.Win64;
using Memory.Utils;
using System.IO;
using System.Net.NetworkInformation;
using static Testicles.ConsoleH;
using System.Windows.Forms.VisualStyles;

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
        ulong StartGame;
        int i = 0;
        MemoryHelper64 helper;
        






        private void Form1_Load(object sender, EventArgs e)
        {
            //Ping ping = new Ping();
            ConsoleHelper.AllocConsole();
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Console.WriteLine("[AdamHook DEBUG] Loaded");





            Process p = Process.GetProcessesByName("hoi4").FirstOrDefault();
            if (p == null)
            {
                label14.Text = "Hoi4 Not Connected: Restart";
                Console.WriteLine("[AdamHook DEBUG] CANNOT CONNECT TO HOI4. RESTART PROGRAM");
                return;

            }
            label14.Text = "Hoi4 Connected";
            Console.WriteLine("[AdamHook DEBUG] HOI4 CONNECTED.");
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


            //Pain. Memory address is for 1.10.8, Figuring out 1.12 still :(
            //StartGame = helper.GetBaseAddress(0xEBD230);


            timer1.Start();
        }

      


        //Refresh to see new memory value
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            i++;
            if(i == 100)
            { Console.WriteLine("[AdamHook DEBUG] LOOP x100"); i = 0; }
            
            // FOW
            if (helper.ReadMemory<Byte>(baseAddrFOW).ToString() == "0")
            {
                label4.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrFOW).ToString() == "1")
            {
                label4.Text = "On";
            }

            // Allow Traits
            if (helper.ReadMemory<Byte>(baseAddrAT).ToString() == "0")
            {
                label5.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrAT).ToString() == "1")
            {
                label5.Text = "On";
            }

            // Debug
            if (helper.ReadMemory<Byte>(baseAddrDBG).ToString() == "0")
            {
                label6.Text = "Off";
            }
            else if (helper.ReadMemory<Byte>(baseAddrDBG).ToString() == "1")
            {
                label6.Text = "On";
            }

            label1.Text = helper.ReadMemory<Int32>(targetAddrTS).ToString();

            //1 and 0 instead of on and off
            //label4.Text = helper.ReadMemory<Byte>(baseAddrFOW).ToString();
            //label5.Text = helper.ReadMemory<Byte>(baseAddrAT).ToString();
            //label6.Text = helper.ReadMemory<Byte>(baseAddrDBG).ToString();

            //label11.Text = helper.ReadMemory<Byte>(StartGame).ToString();


        }

        //BUTTONS
        private void button1_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Int32>(targetAddrTS, Int32.Parse(textBox1.Text));
            Console.WriteLine("[AdamHook DEBUG] 0x25E9D00 4Byte (INT32) + Offset 0xE40 | 4Byte Value set to " + textBox1.Text);
        }  
        private void button7_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrDBG, Byte.Parse("1"));
            Console.WriteLine("[AdamHook DEBUG] 0x25E9829 | Byte Value set to 1 ");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrFOW, Byte.Parse("0"));
            Console.WriteLine("[AdamHook DEBUG] 0x24C945A | Byte Value set to 0 ");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrAT, Byte.Parse("0"));
            Console.WriteLine("[AdamHook DEBUG] 0x24C9438 | Byte Value set to 0 ");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrDBG, Byte.Parse("0"));
            Console.WriteLine("[AdamHook DEBUG] 0x25E9829 | Byte Value set to 0 ");
        }

       

        private void button5_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrAT, Byte.Parse("1"));
            Console.WriteLine("[AdamHook DEBUG] 0x24C9438 | Byte Value set to 1 ");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //DO NOT USE THIS!
            helper.WriteMemory<Byte>(StartGame, Byte.Parse("40"));
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://discord.com/invite/wtBS8Dbp7X");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            helper.WriteMemory<Byte>(baseAddrFOW, Byte.Parse("1"));
            Console.WriteLine("[AdamHook DEBUG] 0x24C945A | Byte Value set to 1 ");
        } 

        //Youtube Link
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/@AdamDX1337");
        }
    }
}








// Evil Adam Notes to add more space or something //

/* 
I think I am going to try to learn how to sigscan, I will probably have to learn a bunch more but my main goal is to somehow get a start game function.
That would singlehandedly change everything, a free start game hack??? I only know of one other hack that has it, it costs money and is somehow browser-based.
Considering the website is called "jewishtricks" I probably wouldn't trust it. Well Anyways, off 2 learn ( thisll take awhile :[ )


 
 */
