using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Football.Common
{
   

    public class FootballHub : Hub
    {
        public async Task SendPlayerUpdate()
        {
            await Clients.All.SendAsync("PlayerUpdated");
        }
    }

}
