using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pppoe_dialer.Model
{
    public class Account : ViewModel.NotifyProperty
    {
        public Account()
        {

        }

        public string UserName
        {
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
            get
            {
                return _userName;
            }
        }

        public string Key
        {
            set
            {
                _key = value;
                OnPropertyChanged();
            }
            get
            {
                return _key;
            }
        }

        public string ConnectName
        {
            set
            {
                _connectName = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_connectName))
                {
                    _connectName = "PPPoEDialer";
                }
                return _connectName;
            }
        }

        private string _userName;
        private string _key;
        private string _connectName;
    }
}
