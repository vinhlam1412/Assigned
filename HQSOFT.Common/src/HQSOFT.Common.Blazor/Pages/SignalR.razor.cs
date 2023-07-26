using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HQSOFT.Common.Blazor.Pages.Common.Hubs;

namespace HQSOFT.Common.Blazor.Pages
{
    public partial class SignalR : ComponentBase
    {
        private HubConnection connection;
        private string notificationMessage;
        protected override void OnInitialized()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44323/myhub")
                .Build();

            connection.On<string>("ReceiveMessage", message =>
            {
                notificationMessage = message;
                StateHasChanged();
            });

            connection.StartAsync();
        }

        private async Task SendMessage()
        {
            await connection.SendAsync("SendMessage", "This is a test notification");
        }
    }
}
