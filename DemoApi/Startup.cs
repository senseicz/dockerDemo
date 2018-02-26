using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DemoApi.Services;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace DemoApi
{
    public class Startup
    {
        private IContainer _container;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the _container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ProfileService>();
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Generate Random Data API", Version = "v1" });
            });

            var builder = new ContainerBuilder();

            
            builder.Register(c =>
                {
                    return Bus.Factory.CreateUsingRabbitMq(sbc =>
                        sbc.Host("rabbit", "dockerdemo", h =>
                        {
                            h.Username("docker");
                            h.Password("docker");
                        })
                    );
                })
                .As<IBusControl>()
                .As<IPublishEndpoint>()
                .SingleInstance();
            

            builder.Populate(services);

            _container = builder.Build();

            // Create the IServiceProvider based on the _container.
            return new AutofacServiceProvider(_container);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Generate Random Data API V1");
            });

            

            //resolve the bus from the _container
            var bus = _container.Resolve<IBusControl>();
            //start the bus
            var busHandle = TaskUtil.Await(() => bus.StartAsync());

            //register an action to call when the application is shutting down
            lifetime.ApplicationStopping.Register(() => busHandle.Stop());

            
        }
    }
}
