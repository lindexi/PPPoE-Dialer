using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pppoe_dialer.Model
{
    public class DialerNotify:ViewModel.NotifyProperty
    {
        public DialerNotify()
        {
           
        }

        public string Str
        {
            set
            {
                _str = value;
                OnPropertyChanged();
            }
            get
            {
                return _str;
            }
        }


        public System.Windows.Media.SolidColorBrush Color
        {
            set
            {
                _color = value;
                OnPropertyChanged();
            }
            get
            {
                return _color;
            }
        }
        private string _str;
        private System.Windows.Media.SolidColorBrush _color;
    }
}
