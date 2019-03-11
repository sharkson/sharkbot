using StanfordNaturalLanguageService;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using VaderSharp;

namespace NaturalLanguageAnalyzationApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            var modelRootFolder = @"M:\sharkbot\stanford-corenlp-3.9.1-models";
            container.RegisterFactory<IMessageAnalyzationService>(c => new MessageAnalyzationService(modelRootFolder, new SentenceTypeService(new InterrogativeService(), new DeclarativeService(), new ImperativeService(), new ExclamatoryService()), new TokenService(), new OpenieService(), new VoiceService(), new SubjectService(), new ObjectService(), new PredicateService(), new SentimentAnalyzationService(new SentimentIntensityAnalyzer())), new SingletonLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
