using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace pppoe_dialer.View
{
    /// <summary>
    /// PPoPDiale.xaml 的交互逻辑
    /// </summary>
    public partial class PPoPDiale : Window
    {
        public PPoPDiale()
        {
            View = new ViewModel.PPoPModel();
            InitializeComponent();
            DataContext = View;


            Closing += (s, e) =>
            {
                View.Hangup();
            };
        }

        private NotifyIcon notifyIcon;

        private void Pallets()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "最小化到托盘";
            this.notifyIcon.ShowBalloonTip(2000);
            this.notifyIcon.Text = "PPoE";
            this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            this.notifyIcon.Visible = true;

            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.Show();
                    notifyIcon.Visible = false;
                    WindowState = WindowState.Normal;
                }
            });

        }

        private ViewModel.PPoPModel View { set; get; }

        private void Dialer_Click(object sender, RoutedEventArgs e)
        {
            View.PPoEdiale();
        }

        private void Hangup_Click(object sender, RoutedEventArgs e)
        {
            View.Hangup();
        }

        private void Minimized(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Pallets();

                this.Hide();
            }
        }
    }
}
