﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Messages
{
    public class CancelOrder
    : ICommand
    {
        public string OrderId { get; set; }
    }
}
