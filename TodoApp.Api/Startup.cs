using System.Security.Claims;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Business.Schema;
using TodoApp.Business.Services;

namespace TodoApp.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITodosService, TodosService>();
            services.AddSingleton<TodoType>();
            services.AddSingleton<TodosQuery>();
            services.AddSingleton<TodosSchema>();
            services.AddSingleton<TodoCreateInputType>();
            services.AddSingleton<TodoUpdateInputType>();
            services.AddSingleton<TodosMutation>();
            services.AddSingleton<TodosSubscription>();
            services.AddSingleton<TodoEventType>();
            services.AddSingleton<ITodoEventService, TodoEventService>();
            services.AddSingleton<IDependencyResolver>(
                c => new FuncDependencyResolver(type => c.GetRequiredService(type)));
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddGraphQL(_ =>
                {
                    _.EnableMetrics = true;
                    _.ExposeExceptions = true;
                }).AddUserContextBuilder(httpContext => new GraphQLUserContext {User = httpContext.User})
                .AddWebSockets();
            
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(opt =>
            {
                opt.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseWebSockets();

            app.UseGraphQLWebSockets<TodosSchema>("/graphql");
            app.UseGraphQL<TodosSchema>("/graphql");

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = "/ui/playground"
            });
        }
    }
    
    public class GraphQLUserContext
    {
        public ClaimsPrincipal User { get; set; }
    }
}