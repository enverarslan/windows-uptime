using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Uptime
{
    public partial class Uptime : Form
    {
        public Uptime()
        {
            InitializeComponent();
            this.CreateContextMenu();
        }

        /*
         * Get current uptime.
         */
        public TimeSpan Up
        {
            get
            {
                using (var uptime = new PerformanceCounter("System", "System Up Time"))
                {
                    uptime.NextValue();
                    return TimeSpan.FromSeconds(uptime.NextValue());
                }
            }
        }     

        /*
         * When form loaded get current uptime with a second interval...
         */
        private void Form1_Load(object sender, EventArgs e)
        {
            var ct = Up.ToString("dd\\:hh\\:mm\\:ss");
            label1.Text = ct;
            notify.Text = ct;
            notify.BalloonTipText = "System up and running since " + ct;
            notify.ShowBalloonTip(100);

            timer1.Interval = 1000;         
            timer1.Tick += new EventHandler(delegate (object o, EventArgs a){
                var t = Up.ToString("dd\\:hh\\:mm\\:ss");
                label1.Text = t;
                notify.Text = t;
            });
            timer1.Enabled = true;
            timer1.Start();

           
        }

        /*
         * When double clicked toggle form.
         */
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;                
            }
            else
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized;
            }
            
        }

        /*
         * Create Notify Context Menu.
         */
        private void CreateContextMenu()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem menu_item = new MenuItem();           
            
            menu_item.Index = 0;
            menu_item.Text = "Exit";
            menu_item.Click += new System.EventHandler(delegate { this.Close(); });

            menu.MenuItems.AddRange(new MenuItem[] { menu_item });
            notify.ContextMenu = menu;
        }     
    }
}
