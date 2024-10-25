﻿using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;

namespace UngCamTuanKietFall2024RazorPages
{
    public class SignalRHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task RefreshData()
        {
            await Clients.All.SendAsync("RefreshData");
        }

    }
}
