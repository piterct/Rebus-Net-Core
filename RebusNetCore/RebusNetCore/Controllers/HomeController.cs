using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pedido.Commands;
using Rebus.Bus;
using RebusNetCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RebusNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBus _bus;

        public HomeController(IBus bus)
        {
            _bus = bus;
        }

        public IActionResult Index()
        {
            _bus.Send(new RealizarPedidoCommand { AggregateRoot = Guid.NewGuid() }).Wait();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
