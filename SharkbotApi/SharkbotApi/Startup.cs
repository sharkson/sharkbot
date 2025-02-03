using ChatAnalyzer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NaturalLanguageService.Services;
using SharkbotApi.Services;
using SharkbotConfiguration;
using System;
using System.Collections.Generic;
using System.Net.Http;

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
            services.AddMvc(option => option.EnableEndpointRouting = false);

            var corsUrls = Configuration.GetSection("CORS").Get<List<string>>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder => builder.WithOrigins(corsUrls.ToArray()).AllowAnyHeader().AllowAnyMethod());
            });

            services.AddSingleton<ChatRequestValidationService, ChatRequestValidationService>();
            services.AddSingleton<ConversationRequestValidationService, ConversationRequestValidationService>();
            services.AddSingleton<ResponseRequestValidationService, ResponseRequestValidationService>();
            services.AddSingleton<ReactionRequestValidationService, ReactionRequestValidationService>();
            services.AddSingleton<ConversationService, ConversationService>();

            var client = new HttpClient();
            var naturalLanguageApiUrl = Configuration.GetSection("NaturalLanguage:ApiUrl").Value;
            client.BaseAddress = new Uri(naturalLanguageApiUrl);
            var naturalLanguageApiService = new NaturalLanguageApiService(client);

            var redditStalkerHttpClient = new HttpClient();
            var redditStalkerApiUrl = Configuration.GetSection("Stalker:Reddit").Value;
            redditStalkerHttpClient.BaseAddress = new Uri(redditStalkerApiUrl);

            var analyzationService = new AnalyzationService(new ConversationSubjectService(new ResponseSubjectService()), new ResponseAnalyzationService(), new ConversationTypeService(), new UserlessMessageService(), new ConversationReadingLevelService(), new ResponseSubjectService(), naturalLanguageApiService);
            var userService = new UserService.UserService(new UserService.UserNickNameService(), new UserService.UserPropertyService(new UserService.UserNaturalLanguageService(), new UserService.PropertyMatchService(), new UserService.PropertyFromQuestionService()), new UserService.OtherUserPropertyService(new UserService.UserNaturalLanguageService(), new UserService.PropertyMatchService()), new UserService.UserDerivedPropertyService(), new UserDatabase.Services.UserSaveService());
            var updateDatabaseService = new UpdateDatabasesService(new ConversationService(), analyzationService, new ConversationDatabase.Services.ConversationUpdateService(new ConversationDatabase.Services.ConversationSaveService()), userService);

            var matchService = new ConversationMatcher.Services.MatchService(new ConversationMatcher.Services.SubjectConfidenceService(), new ConversationMatcher.Services.MatchConfidenceService(new SentenceScoreService(new OpenieScoreService(), new SubjectPredicateObjectScoreService(new SubjectPredicateObjectTokenScoreService()), new TokenScoreService(), new SentimentScoreService(), new SentenceTypeScoreService(), new VoiceScoreService())), new ConversationMatcher.Services.GroupChatConfidenceService(), new ConversationMatcher.Services.UniqueConfidenceService(), new ConversationMatcher.Services.ReadingLevelConfidenceService(), new ConversationSteerService.ConversationPathService(new ConversationSteerService.Services.EdgeService(), new ConversationSteerService.Services.VerticeService(), new ConversationSteerService.Services.ShortestPathService()));
            var conversationMatchService = new SharkbotReplier.Services.ConversationMatchService(new ConversationMatcher.Services.BestMatchService(matchService));
            var userPropertyMatchService = new SharkbotReplier.Services.UserPropertyMatchService(new UserService.UserPropertyRetrievalService(new UserService.PropertyValueService(), new UserService.UserSelfPropertyRetrievalService(new UserService.UserNaturalLanguageService(), new UserService.PropertyValueService()), new UserService.UserNaturalLanguageService()), new UserService.BotPropertyRetrievalService(new UserService.BotSelfPropertyRetrievalService(new UserService.PropertyValueService(), new UserService.UserNaturalLanguageService()), new UserService.UserNaturalLanguageService(), new UserService.PropertyValueService()));
            var queueService = new QueueService(
                new BotService(new ConversationService(), analyzationService, 
                new SharkbotReplier.Services.ResponseService(conversationMatchService, userPropertyMatchService, new SharkbotReplier.Services.LyricsMatchService(), new GoogleMatchService.GoogleMatchService(new ScrapySharp.Network.ScrapingBrowser()), new SharkbotReplier.Services.UrbanDictionaryMatchService(), new SharkbotReplier.Services.SalutationService(), new SharkbotReplier.Services.ResponseConversionService(new UserService.UserPropertyService(new UserService.UserNaturalLanguageService(), new UserService.PropertyMatchService(), new UserService.PropertyFromQuestionService()), new UserService.PropertyValueService())),
                new SharkbotReplier.Services.ReactionService(new SharkbotReplier.Services.ConversationReactionMatchService(new ConversationMatcher.Services.BestReactionMatchService(matchService))),
                new ConversationDatabase.Services.ConversationUpdateService(new ConversationDatabase.Services.ConversationSaveService()), userService, updateDatabaseService), new ConversationService(), updateDatabaseService, new StalkerService(redditStalkerHttpClient));
            services.AddSingleton(queueService);

            ConfigurationService.AnalyzationVersion = Configuration.GetSection("NaturalLanguage:AnalyzationVersion").Value;

            UserDatabase.UserDatabase.userDirectory = Configuration.GetSection("UserDirectory").Value;
            UserDatabase.UserDatabase.LoadDatabase(UserDatabase.UserDatabase.userDirectory);

            ConversationDatabase.ConversationDatabase.conversationDirectory = Configuration.GetSection("ConversationDirectory").Value;
            ConversationDatabase.ConversationDatabase.LoadDatabase(ConversationDatabase.ConversationDatabase.conversationDirectory, analyzationService);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");
            app.UseCors("AllowSpecificOrigin");

            app.UseMvc();
        }
    }
}
