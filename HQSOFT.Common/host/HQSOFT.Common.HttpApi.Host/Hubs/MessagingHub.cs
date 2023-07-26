using DevExpress.Utils.Commands;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Volo.Abp.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace HQSOFT.Common.Hubs
{
    [Authorize]
    public class MessagingHub : AbpHub
    {
        public async Task SendMessage(string message)
        {
            message = $"{CurrentUser.UserName}: {message}";

            await Clients
                .User(CurrentUser.Id.ToString())
                .SendAsync("MessageBackToBlazor", message);

            //This works for sending to ALL
            //await Clients.All.SendAsync("MessageBackToBlazor", message);
        }
    }
}
