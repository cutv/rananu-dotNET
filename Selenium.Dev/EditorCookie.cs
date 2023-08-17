using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Dev
{
  public  class EditorCookie
    {
        public string domain { get; set; }
        public long expirationDate { get; set; }
        public bool hostOnly { get; set; }
        public bool httpOnly { get; set; }
        public string name { get; set; }
        public string path { get; set; }
      //  public string sameSite { get; set; }
        public bool secure { get; set; }
        public string session { get; set; }
        public string value { get; set; }

    }
}
