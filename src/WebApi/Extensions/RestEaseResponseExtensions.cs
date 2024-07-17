using System.Net;
using System.Text.Json;
using Contracts.Notifications;
using RestEase;

namespace WebApi.Extensions
{
    public static class RestEaseResponseExtensions
    {
        private static JsonSerializerOptions Options => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<Result<T>> ProcessApiResponseAsync<T>(this Response<T> response)
        {
            try
            {
                if (response.ResponseMessage.IsSuccessStatusCode)
                    return new Result<T>(response.GetContent());

                var content = await response.ResponseMessage.Content.ReadAsStringAsync();

                return DeserializeErrorResult<T>(content, response.ResponseMessage.StatusCode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static Result<T> DeserializeErrorResult<T>(string content, HttpStatusCode statusCode)
        {
            try
            {
                var errorResult = JsonSerializer.Deserialize<Result<T>>(content, Options);

                if (errorResult != null && errorResult.Errors.Count > 0)
                    return errorResult;
            }
            catch (JsonException)
            {
                // Log or handle the JSON exception if necessary
            }

            var errors = new List<Error>
            {
                new Error(statusCode.ToString(),content) { Code = statusCode.ToString(), Message = content }
            };

            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    return new Result<T>(default, errors);
                case HttpStatusCode.NotFound:
                    errors[0].Message = "Resource not found.";
                    return new Result<T>(errors, content);
                case HttpStatusCode.InternalServerError:
                    errors[0].Message = "Internal server error.";
                    return new Result<T>(default, errors);
                default:
                    return new Result<T>(default, errors);
            }
        }
    }
}
