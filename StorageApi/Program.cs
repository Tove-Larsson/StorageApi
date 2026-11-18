using Microsoft.EntityFrameworkCore;

namespace StorageApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("StorageApiContext") ?? throw new InvalidOperationException("Connection string 'StorageApiContext' not found.");
			
			builder.Services.AddDbContext<StorageApiContext>(options => options.UseSqlServer(connectionString));

			builder.Services.AddControllers();
			builder.Services.AddOpenApi();


			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/openapi/v1.json", "StorageApi");
				});
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
