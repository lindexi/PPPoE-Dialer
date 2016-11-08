using System;
using System.Collections.Generic;
using System.Text;
//方法一
using System.Runtime;
using System.Runtime.InteropServices;
//方法二 Net2.0新增类库
using System.Net.NetworkInformation;

namespace pppoe_dialer.Model
{
    public class InternetCheck : IDisposable
    {
        public InternetCheck()
        {
            _time = new System.Threading.Timer(LoopConnect, null,0, 10000);
        }

        private void LoopConnect(object o)
        {
            if (PingIpOrDomainName() == false)
            {
                OnNotConnected?.BeginInvoke(null, null);
                //Dispose();
            }

            if (IsConnectInternet() == false)
            {
                OnNotConnected?.BeginInvoke(null, null);
                //Dispose();
            }
        }

        public Action OnNotConnected { set; get; }

        private System.Threading.Timer _time;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(int Description, int ReservedValue);

        #region 方法一
        /// <summary>
        /// 用于检查网络是否可以连接互联网,true表示连接成功,false表示连接失败 
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectInternet()
        {
            int Description = 0;
            return InternetGetConnectedState(Description, 0);
        }
        #endregion

        #region 方法二

        public static bool PingIpOrDomainName()
        {
            string str = "www.baidu.com";
            return PingIpOrDomainName(str);
        }

        /// <summary>
        /// 用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令),true表示Ping成功,false表示Ping失败 
        /// </summary>
        /// <param name="strIpOrDName">输入参数,表示IP地址或域名</param>
        /// <returns></returns>
        public static bool PingIpOrDomainName(string strIpOrDName)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objPinReply = objPingSender.Send(strIpOrDName, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (strInfo == "Success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    _time.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~InternetCheck() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
        #endregion
    }
}
