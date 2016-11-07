using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pppoe_dialer.ViewModel
{
    public class PPoPModel : NotifyProperty
    {
        public PPoPModel()
        {
            Account = AccountGoverment.PPoEAccount.Account;
            CreatConnect();
            Notify("请输入账号密码", _color);

            DialeEnable = true;
            HangupEnable = false;
        }

        public bool DialeEnable
        {
            set
            {
                _dialeEnable = value;
                OnPropertyChanged();
            }
            get
            {
                return _dialeEnable;
            }
        }

        public bool HangupEnable
        {
            set
            {
                _hangupEnable = value;
                OnPropertyChanged();
            }
            get
            {
                return _hangupEnable;
            }
        }


        public Model.Account Account
        {
            set;
            get;
        }

        public Model.DialerNotify DialerNotify
        {
            set
            {
                _dialerNotify = value;
                OnPropertyChanged();
            }
            get
            {
                return _dialerNotify;
            }
        }

        public void PPoEdiale()
        {
            if(string.IsNullOrEmpty(Account.UserName) || string.IsNullOrEmpty(Account.Key))
            {
                Notify("请输入账号密码", _errorColor);
                return;
            }

            DialeEnable = false;
            HangupEnable = false;
            string username = Account.UserName.Replace("\\r", "\r").Replace("\\n", "\n");
            string key = Account.Key;
            _ppoeDiale.Diale(username, key, Account.ConnectName);

            AccountGoverment.PPoEAccount.SaveUseNameKey();
        }

        public void Hangup()
        {
            DialeEnable = false;
            HangupEnable = false;

            _ppoeDiale.Hangup();
        }

        private bool _dialeEnable;
        private bool _hangupEnable;


        private Action OnConnectFailure { set; get; }

        private Model.PPoEDiale _ppoeDiale;
        private Model.DialerNotify _dialerNotify;

        private void CreatConnect()
        {
            string connectName = Account.ConnectName;
            _ppoeDiale = new Model.PPoEDiale()
            {
                OnDialeExcep = DialeExcep,
                OnDialeFailure = DialeFailure,
                OnDialeSuccess = DialeSuccess,
                OnhangupFilure = HangupFilure,
                OnhangupSuccess = HangupSuccess
            };

            OnConnectFailure = ConnectFailure;


            try
            {

                _ppoeDiale.CreateConnect(connectName);
            }
            catch (Model.ConnectionException)
            {
                OnConnectFailure?.Invoke();
            }
        }

        private System.Windows.Media.SolidColorBrush _errorColor =
            new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(
                     //0xa6, 0x63, 0x00
                     0xff, 0x31, 0x00
                    ));
        private System.Windows.Media.SolidColorBrush _color =
             new System.Windows.Media.SolidColorBrush(
                 System.Windows.Media.Color.FromRgb(0x68, 0x9c, 0xd2));


        private void Notify(string str)
        {
            System.Windows.Media.SolidColorBrush color = _color;
            if (DialerNotify != null)
            {
                color = DialerNotify.Color;
            }

            Notify(str, color);
        }

        private void Notify(string str, System.Windows.Media.SolidColorBrush color)
        {
            DialerNotify = new Model.DialerNotify()
            {
                Color = color,
                Str = str
            };
        }

        private void ConnectFailure()
        {
            Notify("创建PPPoE连接失败", _errorColor);
        }

        private void DialeFailure()
        {
            string str = "拨号失败";
            Notify(str, _errorColor);

            DialeEnable = true;
            HangupEnable = false;
        }

        private void DialeExcep()
        {
            string str = "拨号异常";
            Notify(str, _errorColor);

            DialeEnable = true;
            HangupEnable = false;
        }

        private void DialeSuccess(string str)
        {
            Notify(str, _color);

            DialeEnable = false;
            HangupEnable = true;

            new Task(() =>
            {
                str = AppDomain.CurrentDomain.BaseDirectory + "pppoe.exe";
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;//不显示程序窗口
                p.Start();//启动程序

                //向cmd窗口发送输入信息
                p.StandardInput.WriteLine(str + "&exit");

                p.StandardInput.AutoFlush = true;
                //p.StandardInput.WriteLine("exit");
                //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
                //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



                //获取cmd窗口的输出信息
                string output = p.StandardOutput.ReadToEnd();

                //StreamReader reader = p.StandardOutput;
                //string line=reader.ReadLine();
                //while (!reader.EndOfStream)
                //{
                //    str += line + "  ";
                //    line = reader.ReadLine();
                //}

                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
            }).Start();
        }

        private void HangupFilure()
        {
            string str = "注销失败";
            Notify(str, _errorColor);

            DialeEnable = false;
            HangupEnable = true;
        }

        private void HangupSuccess()
        {
            string str = "注销成功";
            Notify(str, _color);

            DialeEnable = true;
            HangupEnable = false;

            try
            {
                Process[] process = Process.GetProcesses();

                process.Where(temp => temp.ProcessName == "pppoe").First().Kill();
            }
            catch
            {

            }
        }
    }
}
