using MassTransit;
using Microsoft.EntityFrameworkCore;
using POCEventSourcing.Options;
using POCEventSourcing.ReplicationJob;
using POCEventSourcing.ReplicationJob.Consumers;
using POCEventSourcing.ReplicationJob.DB;
using POCEventSourcing.ReplicationJob.Interfaces;
using POCEventSourcing.ReplicationJob.Repositories;

Microsoft.Extensions.Hosting.IHost host = 
    Host
        .CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {   
            services.AddDbContext<POCReplicationDbContext>(builder =>
            {
                var connectionString = hostContext.Configuration.GetConnectionString("replicationDb");

                builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.Configure<AuditLogTableStorageOptions>(hostContext.Configuration.GetSection(nameof(AuditLogTableStorageOptions)));
            services.AddScoped<IReplicationPersonRepository, ReplicationPersonRepository>();

            var serviceBusConnectionString = hostContext.Configuration.GetConnectionString("serviceBus");

            services.AddMassTransit(config =>
            {
                config.AddConsumer<ReplicationConsumer>();

                config.UsingAzureServiceBus((context, azureCfg) =>
                {
                    azureCfg.Host(serviceBusConnectionString);

                    azureCfg.ReceiveEndpoint("entityEventChangesQueue", queueCfg =>
                    {
                        queueCfg.MaxConcurrentCalls = 100;
                        queueCfg.MaxAutoRenewDuration = TimeSpan.FromMinutes(30);

                        var tableOptions = new AuditLogTableStorageOptions();

                        hostContext
                            .Configuration
                            .GetSection(nameof(AuditLogTableStorageOptions))
                            .Bind(tableOptions);

                        //queueCfg.Consumer<ReplicationConsumer>(() => new ReplicationConsumer(tableOptions));
                    });

                    azureCfg.ConfigureEndpoints(context);
                });
            });
                       
            //services.AddMassTransitHostedService();
            services.AddHostedService<Worker>();

            EntityNameTypeMapper.LoadMappings();
            EntityProcessorTypeMapper.LoadMappings();

            var serviceProvider = services.BuildServiceProvider();

            DependencyResolver.SetServiceProvider(serviceProvider);
        })
        .Build();

await host.RunAsync();
