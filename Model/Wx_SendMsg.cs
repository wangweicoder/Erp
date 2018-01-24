using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Wx_SendMsg
    {


      
            public string touser { set; get; }
            public string template_id { set; get; }
            public string url { set; get; }
            public object data { set; get; }

        
    }
}
