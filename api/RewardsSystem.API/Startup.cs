using AutoMapper;
using FluentValidation;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RewardsSystem.API.HealthChecks;
using RewardsSystem.Common.Mapping;
using RewardsSystem.Persistence.DbContexts;
using RewardsSystem.Service.Commands;
using SkyGen.Producer.ProducerPortal.Service.Common.Behaviors;
using System.Reflection;

namespace RewardsSystem.API;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        string connection = Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<CustomerDbContext>(options => options.UseSqlServer(connection));
        services.AddControllersWithViews();
        services.AddRazorPages();

        services.AddHealthChecks()
            .AddDbContextCheck<CustomerDbContext>()
            .AddSqlServer(connection)
            .AddCheck<MemoryHealthCheck>("Memory");
        services.AddHealthChecksUI(opt =>
        {
            opt.SetEvaluationTimeInSeconds(15);
        }).AddInMemoryStorage();

        services.AddMediatR(typeof(CreateCustomerCommand).GetTypeInfo().Assembly);
        services.AddValidatorsFromAssembly(typeof(CreateCustomerCommandValidator).GetTypeInfo().Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMapperProfile());
        });
        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

    }
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpoints.MapHealthChecksUI(setup =>
            {
                setup.UIPath = "/hc-ui";
                setup.ApiPath = "/hc-json";
            });
        });

        app.MapControllers();

        app.Run();
    }
}
