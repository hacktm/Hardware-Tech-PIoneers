using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestLEdBoard
{
    public interface MainInterface
    {
        
        //this is an interface between our functional code and the Api used when receiving data 
        void On_msg_recieved(string data);
    }
}
