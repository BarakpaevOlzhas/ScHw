﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientInfo
    {
        public string Name { set; get; }
        public TcpClient Client { set; get; }
    }
}
