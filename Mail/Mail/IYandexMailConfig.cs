using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail
{
    public interface IYandexMailConfig
    {
        string Username { get; }
        string Password { get; }
    }
}
