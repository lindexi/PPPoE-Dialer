using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Collections;

namespace pppoe_dialer.ViewModel
{
    class AccountGoverment
    {
        public AccountGoverment()
        {
            Account = new Model.Account();
            ReleaseResource();
            ReadUseNameKey();
        }

        /// <summary>
        /// 保存用户账号密码
        /// </summary>
        public void SaveUseNameKey()
        {
            try
            {
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(Account);
                using (StreamWriter stream = new StreamWriter(
                    new FileStream("data", FileMode.Create)))
                {
                    stream.Write(str);
                }
            }
            catch (Exception e)
            {
                throw new IOException("保存失败", e);
            }
        }



        public Model.Account Account
        {
            set;
            get;
        }

        public static AccountGoverment PPoEAccount
        {
            set
            {
                _ppoeAccount = value;
            }
            get
            {
                return _ppoeAccount ?? (_ppoeAccount = new AccountGoverment());
            }
        }
        private static AccountGoverment _ppoeAccount;

        /// <summary>
        /// 获取用户账号密码
        /// </summary>
        private void ReadUseNameKey()
        {
            Model.Account account=null;
            try
            {
                using (StreamReader stream = new StreamReader(
                    new FileStream("data", FileMode.Open)))
                {
                    string str = stream.ReadToEnd();
                    account = Newtonsoft.Json.JsonConvert.DeserializeObject<Model.Account>(str);
                }
            }
            catch (FileNotFoundException)
            {
                
            }
            catch
            {

            }

            if (account != null)
            {
                Account = account;
            }
        }

        private void ReleaseResource()
        {
            string str = "pppoe.exe";
            if (File.Exists(str))
            {
                return;
            }

           // byte[] buffer = pppoe_dialer.Properties.Resources.pppoe;
          //  buffer = (byte[])App.Current.Resources["pppoe"];
            foreach(DictionaryEntry temp in pppoe_dialer.Properties.Resources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture,true,true))
            {
                try
                {
                    string file = (string)temp.Key;
                    file = file.Replace("0", ".");
                    Write((byte[])temp.Value, file);
                }
                catch
                {

                }
            }
            //using (Stream stream=new FileStream(str, FileMode.Create))
            //{
            //    stream.Write(buffer, 0, buffer.Length);
            //}
        }

        private void Write(byte[] buffer,string file)
        {
            try
            {
                using (Stream stream = new FileStream(file, FileMode.Create))
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            catch 
            {
                
            }
        }
    }
}
