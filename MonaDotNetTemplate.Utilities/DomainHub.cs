using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Utilities
{
    public interface IDomainHub
    {
        [HubMethodName("send-notification")]
        Task SendNotification(object notification);
    }

    public class DomainHub : Hub<IDomainHub>
    {
        /// <summary>
        /// Thêm vào nhóm
        /// </summary>
        /// <param name="id">Id người dùng</param>
        /// <returns></returns>
        [HubMethodName("join")]
        public async Task Join(string id)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, string.Format("UserId_{0}", id));
        }

        /// <summary>
        /// Rời khỏi nhóm
        /// </summary>
        /// <param name="id">Id người dùng</param>
        /// <returns></returns>
        [HubMethodName("leave")]
        public async Task Leave(string id)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, string.Format("UserId_{0}", id));
        }
    }
}
