﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CommonData.HIRATA
{
    public class MessageBase
    {
        public int Type;
        public MmfEventClientEventType PType
        {
            get { return (MmfEventClientEventType)Type; }
            set { Type = (int)value; }
        }
    }
}
