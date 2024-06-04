using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using MyEmpApi.Data;


namespace MyEmpApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register the DatabaseHelper with DI container
            var connectionString = builder.Configuration.GetConnectionString("EmployeeAppCon");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'EmployeeAppCon' not found.");
            }

            builder.Services.AddSingleton(new DatabaseHelper(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });

                // Open Swagger UI page in default browser
                OpenSwaggerPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Configure CORS to accept requests from any origin
            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void OpenSwaggerPage()
        {
            var baseAddress = $"http://localhost:5270/swagger/index.html";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(baseAddress) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", baseAddress);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", baseAddress);
            }
        }
    }
}
