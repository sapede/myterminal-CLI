using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CocoaDI;

class Program
{

    static Task Main(string[] args)
    {
        var arg = new[] { "bater-ponto" };
        var builder = ConsoleApp.CreateBuilder(arg);

        builder.ConfigureHostConfiguration(hostConfig =>
        {
            //hostConfig.SetBasePath(Directory.GetCurrentDirectory());
            hostConfig.AddJsonFile("A:\\repos\\CocoaDI\\CocoaDI\\appsettings.json", optional: false);
        });

        builder.ConfigureServices((ctx, services) =>
        {

            // Register appconfig.json to IOption<MyConfig>
            services.Configure<MyConfig>(ctx.Configuration.GetSection(@"ConnectionStrings"));
            services.AddScoped<IBotService, BotService>();
            // Register EntityFramework database context
            //services.AddDbContext<MyDbContext>();

        });

        var app = builder.Build();
        app.AddCommands<Tangerino>();
        app.AddRootCommand((ConsoleAppContext ctx, IOptions<MyConfig> config, string name) => { });
        app.Run();

        return Task.CompletedTask;
    }

    public class Tangerino : ConsoleAppBase
    {
        private readonly ILogger<DatabaseApp> logger;
        private readonly IOptions<MyConfig> config;
        private readonly IBotService botService;

        public Tangerino(ILogger<DatabaseApp> logger, IOptions<MyConfig> config, IBotService botService)
        {
            this.config = config;
            this.botService = botService;
            this.logger = logger;
        }

        [Command("bater-ponto")]
        public void BaterPonto()
        {
            botService.BaterPonto();
            logger.LogInformation("Bati o Ponto");
        }
    }

    // ----
    public class Daemon : ConsoleAppBase
    {
        [RootCommand]
        public async Task Run()
        {
            // you can write infinite-loop while stop request(Ctrl+C or docker terminate).
            try
            {
                while (!Context.CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        Console.WriteLine("Wait One Minutes");
                    }
                    catch (Exception ex)
                    {
                        // error occured but continue to run(or terminate).
                        Console.WriteLine(ex.Message, "Found error");
                    }

                    // wait for next time
                    await Task.Delay(2000, Context.CancellationToken);
                }
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                // you can write finally exception handling(without cancellation)
            }
            finally
            {
                // you can write cleanup code here.
            }
        }
    }

    [Command("db")]
    public class DatabaseApp : ConsoleAppBase, IAsyncDisposable
    {
        readonly ILogger<DatabaseApp> logger;
        readonly MyDbContext dbContext;
        readonly IOptions<MyConfig> config;

        // you can get DI parameters.
        public DatabaseApp(ILogger<DatabaseApp> logger, IOptions<MyConfig> config, MyDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.config = config;
        }

        [Command("select")]
        public async Task QueryAsync(int id)
        {
            // select * from...
            await Task.Delay(500);
            logger.LogInformation("Cheguei select");
        }

        // also allow defaultValue.
        [Command("insert")]
        public async Task InsertAsync(string value, int id = 0)
        {
            // insert into...
            await Task.Delay(500);
            logger.LogInformation("insert select");
        }

        // support cleanup(IDisposable/IAsyncDisposable)
        public async ValueTask DisposeAsync()
        {
            await dbContext.DisposeAsync();
        }
    }
}