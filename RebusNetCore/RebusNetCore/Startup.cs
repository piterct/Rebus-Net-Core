using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Messages;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Rebus.Transport.InMem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RebusNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure and register Rebus

            var nomeFila = "fila_rebus";

            services.AddRebus(configure => configure
               .Transport(t => t.UseInMemoryTransport(new InMemNetwork(), nomeFila))
               //.Transport(t => t.UseRabbitMq("amqp://localhost", nomeFila))
               .Subscriptions(s => s.StoreInMemory())
               .Routing(r =>
               {
                   r.TypeBased()
                       .MapAssemblyOf<Message>(nomeFila)
                       .MapAssemblyOf<RealizarPedidoCommand>(nomeFila)
                       .MapAssemblyOf<RealizarPagamentoCommand>(nomeFila);
               })
               .Sagas(s => s.StoreInMemory())
               .Options(o =>
               {
                   o.SetNumberOfWorkers(1);
                   o.SetMaxParallelism(1);
                   o.SetBusName("Demo Rebus");
               })
           );

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
