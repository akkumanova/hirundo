namespace Hirundo.Web
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Routing;
    using Hirundo.Model.Converters;
    using Hirundo.Model.Utils;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Ninject;

    public static class WebApiConfig
    {
        public static void Register(IKernel kernel, HttpConfiguration config)
        {
            config.DependencyResolver = new NinjectDependencyResolver(kernel);

            config.Formatters.JsonFormatter.SerializerSettings =
                new JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    Converters = { new ObjectIdConverter() },
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

            RegisterRoutes(config);
        }

        public static void RegisterRoutes(HttpConfiguration config)
        {
            // User
            MapRoute(config, HttpMethod.Get   , "api/user"                   , "User", "GetUserExists");
            MapRoute(config, HttpMethod.Get   , "api/user/{userId}"          , "User", "GetUser");
            MapRoute(config, HttpMethod.Get   , "api/user/{userId}/timeline" , "User", "GetTimeline");
            MapRoute(config, HttpMethod.Get   , "api/user/{userId}/comments" , "User", "GetComments");
            MapRoute(config, HttpMethod.Get   , "api/user/{userId}/favorites", "User", "GetFavorites");
            MapRoute(config, HttpMethod.Get   , "api/user/{userId}/followers", "User", "GetFollowers");
            MapRoute(config, HttpMethod.Get   , "api/user/{userId}/following", "User", "GetFollowing");
            MapRoute(config, HttpMethod.Post  , "api/user/{userId}/following", "User", "PostFollowing");
            MapRoute(config, HttpMethod.Delete, "api/user/{userId}/following", "User", "DeleteFollowing");

            // Comments
            MapRoute(config, HttpMethod.Post  , "api/comments"                     , "Comment", "PostComment");
            MapRoute(config, HttpMethod.Delete, "api/comments/{commentId}"         , "Comment", "DeleteComment");
            MapRoute(config, HttpMethod.Get   , "api/comments/{commentId}/details" , "Comment", "GetCommentDetails");
            MapRoute(config, HttpMethod.Post  , "api/comments/{commentId}/reply"   , "Comment", "PostReply");
            MapRoute(config, HttpMethod.Post  , "api/comments/{commentId}/retweet" , "Comment", "PostRetweet");
            MapRoute(config, HttpMethod.Get   , "api/comments/{commentId}/retweet" , "Comment", "GetRetweets");
            MapRoute(config, HttpMethod.Post  , "api/comments/{commentId}/favorite", "Comment", "PostFavorite");
            MapRoute(config, HttpMethod.Get   , "api/comments/{commentId}/favorite", "Comment", "GetFavorites");
        }

        private static void MapRoute(HttpConfiguration config, HttpMethod method, string route, string controller, string action)
        {
            config.Routes.MapHttpRoute(
                name: Guid.NewGuid().ToString(),
                routeTemplate: route,
                defaults: new { controller = controller, action = action },
                constraints: new { httpMethod = new HttpMethodConstraint(method) });
        }
    }
}
