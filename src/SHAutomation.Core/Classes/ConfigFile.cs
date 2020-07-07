﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAutomation.Core.Classes
{
    public class ConfigFile
    {
        public string RedisEndpoint { get; set; }
        public string RedisPassword { get; set; }
        public int RedisPort { get; set; }
        public bool RedisUseSSL { get; set; }
        public string BranchMatchRegex { get; set; }
    }
}