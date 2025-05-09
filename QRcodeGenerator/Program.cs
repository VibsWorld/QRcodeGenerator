namespace QRcodeGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton(x => new JsonReaderAndWriter());
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            });
            var app = builder.Build();

            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            AddDefaultOptions(app);
            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void AddDefaultOptions(WebApplication app)
        {
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //Clear any DefaultFileNames if already there
            defaultFilesOptions.DefaultFileNames.Clear();
            //Add the default HTML Page to the DefaultFilesOptions Instance
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            //Setting the Default Files
            //Pass the DefaultFilesOptions Instance to the UseDefaultFiles Middleware Component
            app.UseDefaultFiles(defaultFilesOptions);
        }
    }
}
