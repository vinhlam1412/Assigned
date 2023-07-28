using DevExpress.Utils.Commands;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Volo.Abp.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using HQSOFT.Common.HQNotifications;

namespace HQSOFT.Common.Hubs
{
    //[Authorize]
    public class MessagingHub : AbpHub
    {
        public async Task SendMessage(HQNotificationDto message)
        {
            await Clients.All.SendAsync("MessageBackToBlazor", message);
        }
    }
}
