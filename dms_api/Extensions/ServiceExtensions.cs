using DepartmentManagementSystem.API.Filters;
using DepartmentManagementSystem.Contracts;
using DepartmentManagementSystem.Entities;
using DepartmentManagementSystem.Entities.Helpers;
using DepartmentManagementSystem.Entities.Models;
using DepartmentManagementSystem.Repository;
using LoggingService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Task = DepartmentManagementSystem.Entities.Models.Task;

namespace InventoryManagementSystem.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureMsSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["mssqlconnection:connectionString"];
            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(connectionString));
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<ISortHelper<Department>, SortHelper<Department>>();
            services.AddScoped<IDataShaper<Department>, DataShaper<Department>>();
            services.AddScoped<ISortHelper<EmployeeFunction>, SortHelper<EmployeeFunction>>();
            services.AddScoped<IDataShaper<EmployeeFunction>, DataShaper<EmployeeFunction>>();
            services.AddScoped<ISortHelper<Equipment>, SortHelper<Equipment>>();
            services.AddScoped<IDataShaper<Equipment>, DataShaper<Equipment>>();
            services.AddScoped<ISortHelper<Facility>, SortHelper<Facility>>();
            services.AddScoped<IDataShaper<Facility>, DataShaper<Facility>>();
            services.AddScoped<ISortHelper<Rule>, SortHelper<Rule>>();
            services.AddScoped<IDataShaper<Rule>, DataShaper<Rule>>();
            services.AddScoped<ISortHelper<Task>, SortHelper<Task>>();
            services.AddScoped<IDataShaper<Task>, DataShaper<Task>>();
            services.AddScoped<ISortHelper<Tool>, SortHelper<Tool>>();
            services.AddScoped<IDataShaper<Tool>, DataShaper<Tool>>();
            services.AddScoped<ISortHelper<Utility>, SortHelper<Utility>>();
            services.AddScoped<IDataShaper<Utility>, DataShaper<Utility>>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        public static void RegisterFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidateMediaTypeAttribute>();
        }

        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.eses2.hateoas+json");
                }
                var xmlOutputFormatter = config.OutputFormatters.OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.eses2.hateoas+xml");
                }
            });
        }
    }
}
