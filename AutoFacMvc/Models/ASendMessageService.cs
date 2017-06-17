using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFacMvc.Models
{
    [MessagePlatformAttribute(MPlatform.A平台)]
    public class ASendMessageService: IMessage
    {
        public string Say(string word)
        {
            return "A平台"+word;
        }

        
    }
}