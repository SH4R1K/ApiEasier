using ApiEasier.Server.Dto;
using ApiEasier.Server.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace ApiEasier.Server.Hubs
{
    public class ApiListHub : Hub<IApiListHub>
    {
    }
}
