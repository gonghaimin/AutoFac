using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFacMvc.Models
{
    public class MessagePlatformAttribute:Attribute
    {
        public MPlatform Platform { get; set; }
        public MessagePlatformAttribute(MPlatform en)
        {
            this.Platform = en;
        }
    }
}