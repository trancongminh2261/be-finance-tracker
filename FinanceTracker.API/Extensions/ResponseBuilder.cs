using FinanceTracker.Extensions;
using FinanceTracker.Utilities;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace FinanceTracker.API.Extensions
{
    public sealed class ResponseBuilder
    {
        private static ResponseBuilder instance;
        private static JObject dataMessage;
        private ResponseBuilder()
        {
            string jsonString = File.ReadAllText("ResponeMessageMultiLanguage.json");
            dataMessage = JObject.Parse(jsonString);
        }

        public AppDomainResult Build(AppException exception, string language = "vi")
        {
            var responseData = dataMessage["Response"][exception.Code.ToString()];
            string message = exception.Message;
            int statusCode = 500;
            if(responseData != null)
            {
                message = responseData["Message"][language].Value<string>();
                if(message != null && exception.Param != null)
                {
                    message = string.Format(message, exception.Param);
                }
                statusCode = responseData["StatusCode"].Value<int>();
            }
            return new AppDomainResult()
            {
                Message = message,
                StatusCode = statusCode
            };
        }

        public static ResponseBuilder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResponseBuilder();
                }
                return instance;
            }
        }
    }
}
