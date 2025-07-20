using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Utilities
{
    public interface idomainHub
    {
        [HubMethodName("send-notification")]
        Task SendNotification(object notification);
    }

    public class DomainHub : Hub<idomainHub>
    {
        /// <summary>
        /// Thêm vào nhóm
        /// </summary>
        /// <param name="id">id người dùng</param>
        /// <returns></returns>
        [HubMethodName("join")]
        public async Task Join(string id)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, string.Format("Userid_{0}", id));
        }

        /// <summary>
        /// Rời khỏi nhóm
        /// </summary>
        /// <param name="id">id người dùng</param>
        /// <returns></returns>
        [HubMethodName("leave")]
        public async Task Leave(string id)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, string.Format("Userid_{0}", id));
        }
    }
}
