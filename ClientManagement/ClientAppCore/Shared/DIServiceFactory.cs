using ClientAppCore.Entities;
using ClientAppCore.Infrastructure.Data;
using ClientAppCore.Infrastructure.Repositories;
using ClientAppCore.Interfaces;
using ClientAppCore.Services;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAppCore.Shared
{
    public static class DIServiceFactory
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(AutoMapperConfiguration.Configure());

            services.AddTransient(typeof(IClientService), typeof(ClientService));

            services.AddTransient(typeof(IClientRepository), typeof(ClientRepository));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddTransient<IGenericRepository<Client>, ClientRepository>();


            // Register IDbContextTransaction with scoped lifetime
            services.AddScoped<IDbContextTransaction>(provider =>
            {
                // Resolve your DbContext from the service provider
                var dbContext = provider.GetRequiredService<AppDbContext>();

                // Begin a new transaction and return the IDbContextTransaction instance
                return dbContext.Database.BeginTransaction();
            });
        }
    }
}
