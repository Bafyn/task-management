using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using TaskManagement.API.HostedServices;
using TaskManagement.Application.Commands.AddTask;
using TaskManagement.Application.Consumers.Contracts;
using TaskManagement.Application.Mapping.Profiles;
using TaskManagement.Application.Resilience.Policies;
using TaskManagement.Application.Services;
using TaskManagement.Application.Services.Contracts;
using TaskManagement.Domain.Repositories;
using TaskManagement.Persistence.Context;
using TaskManagement.Persistence.Repositories;
using TaskManagement.Persistence.UnitOfWork;
using TaskManagement.ServiceBus.Configuration;

namespace TaskManagement.API;

internal static class ServiceCollectionSetup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTaskManagementDbContext(configuration);

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add repositories
        services.AddScoped<ITaskRepository, TaskRepository>();

        return services;
    }

    public static IServiceCollection AddRequestHandlingServices(this IServiceCollection services)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<AddTaskCommand>());
        services.AddAutoMapper(typeof(TaskProfile));

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceBusProducer, ServiceBusHandler>();
        services.AddTransient<IServiceBusConsumer, ServiceBusHandler>();
        services.AddSingleton<IResiliencePolicyProvider, ResiliencePolicyProvider>();

        return services;
    }

    public static IServiceCollection AddMessageConsumers(this IServiceCollection services)
    {
        var messageConsumerGenericType = typeof(IMessageConsumer<>);
        var messageConsumerTypes = messageConsumerGenericType.Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == messageConsumerGenericType))
            .Select(t => new
            {
                Service = t.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == messageConsumerGenericType),
                Implementation = t
            });

        foreach (var messageConsumerType in messageConsumerTypes)
        {
            services.AddScoped(messageConsumerType.Service, messageConsumerType.Implementation);
        }

        return services;
    }

    public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMQConfiguration = configuration.GetSection(nameof(RabbitMQConfiguration)).Get<RabbitMQConfiguration>();
        var serviceName = configuration.GetValue<string>(Constants.ServiceName);

        var connectionFactory = new ConnectionFactory()
        {
            HostName = rabbitMQConfiguration.Host,
            Port = rabbitMQConfiguration.Port,
            UserName = rabbitMQConfiguration.Username,
            Password = rabbitMQConfiguration.Password,
            VirtualHost = "/",
            ClientProvidedName = serviceName,
            DispatchConsumersAsync = true
        };

        var connection = connectionFactory.CreateConnection();

        // Declare message broker entities.
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(ServiceBus.Constants.ExchangeName, ExchangeType.Fanout);
            channel.QueueDeclare(ServiceBus.Constants.QueueName, durable: false, exclusive: false, autoDelete: false);
            channel.QueueBind(ServiceBus.Constants.QueueName, ServiceBus.Constants.ExchangeName, string.Empty);

            channel.Close();
        }

        services.AddSingleton(connection);
        services.AddHostedService<ServiceBusJob>();

        return services;
    }

    private static IServiceCollection AddTaskManagementDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TaskManagementDB");

        services.AddDbContext<TaskManagementDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString, opt =>
            {
                opt.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(1), null);
            });

            builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }
}
