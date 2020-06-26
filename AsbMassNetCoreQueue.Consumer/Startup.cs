using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AsbMassNetCoreQueue.Consumer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = "Endpoint=sb://servicebusqueuesnetcore.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=blablakey";
            var newOrdersQueue = "new-orders"; // need to make sure the queue name is written correctly

            services.AddMassTransit(serviceCollectionConfigurator =>
            {
                serviceCollectionConfigurator.AddConsumer<OrderConsumer>();

                //Consumers - Receivers
                //Message Creators - Senders
                //would normally be in different applications

                serviceCollectionConfigurator.AddBus
                    (registrationContext => Bus.Factory.CreateUsingAzureServiceBus
                                                    (configurator =>
                                                    {
                                                        configurator.Host(connectionString);

                                                        /*
                                                         For a consumer to receive messages, the consumer must be connected to a receive endpoint. 
                                                        This is done during bus configuration, particularly within the configuration of a receive endpoint.
                                                        https://masstransit-project.com/usage/consumers.html#consumer*/

                                                        configurator.ReceiveEndpoint(newOrdersQueue, endpointConfigurator =>
                                                        {
                                                            endpointConfigurator.ConfigureConsumer<OrderConsumer>(registrationContext);
                                                        });
                                                    }
                                                    )
                     );

            });

            //need to always start the bus, so it behaves correctly
            services.AddSingleton<IHostedService, BusHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
