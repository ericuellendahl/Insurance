using Insurance.Hiring.Adapter.Consumer;
using Insurance.Shared.Settings;
using MassTransit;

namespace Insurance.Hiring.Extensions
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
            var rabbitSettings = configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<PropostStatusChangedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    
                    cfg.ReceiveEndpoint("contract-approved-queue", e =>
                    {
                        e.ConfigureConsumer<PropostStatusChangedConsumer>(context);
                    });

                    cfg.Host(rabbitSettings?.HostName, (ushort)rabbitSettings.Port, rabbitSettings.VirtualHost, h =>
                    {
                        h.Username(rabbitSettings.UserName);
                        h.Password(rabbitSettings.Password);
                    });
                });
            });

            return services;
        }
    }
}
