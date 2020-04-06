﻿using Agile.Config.Protocol;
using AgileConfig.Server.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AgileConfig.Server.Apisite.Controllers
{
    [Authorize]
    public class RemoteServerProxyController : Controller
    {
        private readonly IRemoteServerNodeProxy _remoteServerNodeProxy;
        private readonly IServerNodeService _serverNodeService;
        private readonly ILogger _logger;
        public RemoteServerProxyController(IRemoteServerNodeProxy remoteServerNodeProxy, IServerNodeService serverNodeService, ILoggerFactory loggerFactory)
        {
            _remoteServerNodeProxy = remoteServerNodeProxy;
            _serverNodeService = serverNodeService;
            _logger = loggerFactory.CreateLogger<RemoteServerProxyController>();
        }

        [HttpPost]
        public async Task<IActionResult> Client_Offline(string address, string clientId)
        {
            var action = new WebsocketAction { Action = "offline" };
            var result = await _remoteServerNodeProxy.OneClientDoActionAsync(address, clientId, action);
            _logger.LogInformation("Request remote node {0} 's action OneClientDoAction {1} .", address, result ? "success" : "fail");

            return Json(new
            {
                success = true,
            });
        }

        [HttpPost]
        public async Task<IActionResult> AllClients_Reload(string address)
        {
            var action = new WebsocketAction { Action = "reload" };
            var nodes = await _serverNodeService.GetAllNodesAsync();
            var result = await _remoteServerNodeProxy.AllClientsDoActionAsync(address, action);
            _logger.LogInformation("Request remote node {0} 's action AllClientsDoAction {1} .", address, result ? "success" : "fail");

            return Json(new
            {
                success = true,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Client_Reload(string address, string clientId)
        {
            var action = new WebsocketAction { Action = "reload" };
            var result = await _remoteServerNodeProxy.OneClientDoActionAsync(address, clientId, action);
            _logger.LogInformation("Request remote node {0} 's action OneClientDoAction {1} .", address, result ? "success" : "fail");

            return Json(new
            {
                success = true,
            });
        }
    }
}