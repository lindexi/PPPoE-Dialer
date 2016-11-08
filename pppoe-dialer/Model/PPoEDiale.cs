using DotRas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pppoe_dialer.Model
{
    public class PPoEDiale
    {
        public Action OnDialeFailure { set; get; }
        public Action OnDialeExcep { set; get; }
        public Action<string> OnDialeSuccess { set; get; }
        public Action OnhangupFilure { set; get; }
        public Action OnhangupSuccess { set; get; }
        public void Hangup()
        {
            try
            {
                System.Collections.ObjectModel.ReadOnlyCollection<RasConnection> conList = RasConnection.GetActiveConnections();
                foreach (RasConnection con in conList)
                {
                    con.HangUp();
                }
                OnhangupSuccess?.Invoke();
                //System.Threading.Thread.Sleep(1000);
                //lb_status.Content = "注销成功";
                //lb_message.Content = "已注销";
                //dial.IsEnabled = true;
                //hangup.IsEnabled = false;
            }
            catch (Exception)
            {
                //lb_status.Content = "注销出现异常";
                OnhangupFilure?.Invoke();
            }
        }

        public void Diale(string username, string key,string connectName)
        {
            try
            {
                RasDialer dialer = new RasDialer();
                dialer.EntryName = connectName;
                dialer.PhoneNumber = " ";
                dialer.AllowUseStoredCredentials = true;
                dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User);
                dialer.Credentials = new System.Net.NetworkCredential(username, key);
                dialer.Timeout = 500;
                RasHandle myras = dialer.Dial();
                while (myras.IsInvalid)
                {
                    //lb_status.Content = "拨号失败";
                    OnDialeFailure?.Invoke();
                }
                if (!myras.IsInvalid)
                {
                    //lb_status.Content = "拨号成功! ";
                    RasConnection conn = RasConnection.GetActiveConnectionByHandle(myras);
                    RasIPInfo ipaddr = (RasIPInfo)conn.GetProjectionInfo(RasProjectionType.IP);
                    //lb_message.Content = "获得IP： " + ipaddr.IPAddress.ToString();
                    OnDialeSuccess?.Invoke("获得IP： " + ipaddr.IPAddress.ToString());
                    //dial.IsEnabled = false;
                    //hangup.IsEnabled = true;
                }
            }
            catch (Exception e)
            {
                //lb_status.Content = "拨号出现异常";
                OnDialeExcep?.Invoke();
            }
        }

        public void CreateConnect(string connectName)
        {
            RasDialer dialer = new RasDialer();
            RasPhoneBook book = new RasPhoneBook();
            try
            {
                book.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User));
                if (book.Entries.Contains(connectName))
                {
                    book.Entries[connectName].PhoneNumber = " ";
                    book.Entries[connectName].Update();
                }
                else
                {
                    System.Collections.ObjectModel.ReadOnlyCollection<RasDevice> readOnlyCollection = RasDevice.GetDevices();
                    RasDevice device = RasDevice.GetDevices().Where(o => o.DeviceType == RasDeviceType.PPPoE).First();
                    RasEntry entry = RasEntry.CreateBroadbandEntry(connectName, device);
                    entry.PhoneNumber = " ";
                    book.Entries.Add(entry);
                }
            }
            catch (Exception e)
            {
                throw new ConnectionException("创建PPPoE连接失败", e);
            }
        }
    }
}
