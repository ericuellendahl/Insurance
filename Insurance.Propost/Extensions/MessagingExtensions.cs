using Insurance.Propost.Adapter.EventPublisher;
using Insurance.Propost.Domain.Ports;
using Insurance.Shared.Settings;
using MassTransit;

namespace Insurance.Propost.Extensions
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
            var rabbitSettings = configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitSettings?.HostName, (ushort)rabbitSettings.Port, rabbitSettings.VirtualHost, h =>
                    {
                        h.Username(rabbitSettings.UserName);
                        h.Password(rabbitSettings.Password);
                    });
                });
            });

            services.AddScoped<IEventPublisher, MassTransitEventPublisher>();

            return services;
        }
    }
}
