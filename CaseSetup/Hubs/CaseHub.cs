using Microsoft.AspNetCore.SignalR;

namespace CaseSetup.Hubs;

public class CaseHub : Hub
{
    public async Task SendComment(int caseId, string comment, string author)
    {
        await Clients.All.SendAsync("CommentRecieved", caseId, comment, author);
    }

    public async Task NotifyCaseChanged(int caseId)
    {
        await Clients.All.SendAsync("CaseUpdated", caseId);
    }
    
}