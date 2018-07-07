using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharkbotConfiguration;
using System.Collections.Generic;

namespace SharkbotApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var corsUrls = Configuration.GetSection("CORS").Get<List<string>>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder => builder.WithOrigins(corsUrls.ToArray()).AllowAnyHeader().AllowAnyMethod());
            });

            ConfigurationService.AnalyzationVersion = Configuration.GetSection("NaturalLanguage:AnalyzationVersion").Value;

            NaturalLanguageService.NaturalLanguageService.LoadAnalyzationData(Configuration.GetSection("NaturalLanguage:Models:en-sent").Value, Configuration.GetSection("NaturalLanguage:Models:en-token").Value, Configuration.GetSection("NaturalLanguage:Models:en-pos-maxent").Value, Configuration.GetSection("NaturalLanguage:Models:en-chunker").Value, Configuration.GetSection("NaturalLanguage:POSTagValues").Value, Configuration.GetSection("NaturalLanguage:LowValueNouns").Value);

            UserDatabase.UserDatabase.userDirectory = Configuration.GetSection("UserDirectory").Value;
            UserDatabase.UserDatabase.LoadDatabase(UserDatabase.UserDatabase.userDirectory);

            ConversationDatabase.ConversationDatabase.conversationDirectory = Configuration.GetSection("ConversationDirectory").Value;
            ConversationDatabase.ConversationDatabase.LoadDatabase(ConversationDatabase.ConversationDatabase.conversationDirectory);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");

            app.UseMvc();

            app.UseCors("AllowSpecificOrigin");
        }
    }
}
