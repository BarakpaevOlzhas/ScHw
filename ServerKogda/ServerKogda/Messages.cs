using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class Messages
    {
        public string NamePerson { set; get; }
        public string Text { set; get; }
        public List<string> ListName { set; get; } = new List<string>();
    }
}
