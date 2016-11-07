using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pppoe_dialer.Model
{
    public class ConnectionException : Exception
    {
        public ConnectionException(string message, Exception innerException) :
            base(message, innerException)
        {

        }
    }
}
