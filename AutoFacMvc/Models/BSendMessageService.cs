using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFacMvc.Models
{
    [MessagePlatformAttribute(MPlatform.B平台)]
    public class BSendMessageService : IMessage
    {
        public string Say(string word)
        {
            return "B平台"+word;
        }        
    }
}