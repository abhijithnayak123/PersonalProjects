﻿using System;
using System.Runtime.Serialization;

namespace MGI.Peripheral.Server.Data
{
    public class ConvertStreamRequest
    {
        [DataMember]
        public string CheckFrontImage { get; set; }

        [DataMember]
        public string CheckBackImage { get; set; }

    }
}