using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Net;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Section2;

public class Function
{
    #region Private Fields

    private readonly DynamoDBContext dynamoDBContext;

    #endregion Private Fields

    #region Public Constructors

    public Function()
    {
        dynamoDBContext = new DynamoDBContext(new AmazonDynamoDBClient());
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        return request.RequestContext.Http.Method.ToUpper() switch
        {
            "GET" => await HandleGetRequest(request),
            "POST" => await HandlePostRequest(request),
            "DELETE" => await HandleDeleteRequest(request)
        };
    }

    #endregion Public Methods

    #region Private Methods

    private static APIGatewayHttpApiV2ProxyResponse BadResponse(string message)
    {
        return new APIGatewayHttpApiV2ProxyResponse()
        {
            Body = message,
            StatusCode = (int)HttpStatusCode.BadRequest
        };
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> HandleDeleteRequest(APIGatewayHttpApiV2ProxyRequest request)
    {
        request.PathParameters.TryGetValue("userId", out var userIdStr);
        if (Guid.TryParse(userIdStr, out Guid userId))
        {
            await dynamoDBContext.DeleteAsync<User>(userId);
            return new APIGatewayHttpApiV2ProxyResponse
            {
                Body = "Delete",
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        return BadResponse("Record not found to delete");
    }
    private async Task<APIGatewayHttpApiV2ProxyResponse> HandleGetRequest(APIGatewayHttpApiV2ProxyRequest request)
    {
        request.PathParameters.TryGetValue("userId", out var userIdStr);
        if (Guid.TryParse(userIdStr, out Guid userId))
        {
            var user = await dynamoDBContext.LoadAsync<User>(userId);
            if (user != null)
            {
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    Body = JsonSerializer.Serialize(user),
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }
        return new APIGatewayHttpApiV2ProxyResponse
        {
            Body = "Invalid userId in path",
            StatusCode = (int)HttpStatusCode.NotFound
        };
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> HandlePostRequest(APIGatewayHttpApiV2ProxyRequest request)
    {
        var user = JsonSerializer.Deserialize<User>(request.Body);
        if (user == null)
        {
            return BadResponse("Invalid user details");
        }

        await dynamoDBContext.SaveAsync(user);
        return new APIGatewayHttpApiV2ProxyResponse
        {
            Body = (string?)JsonSerializer.Serialize(user),
            StatusCode = (int)HttpStatusCode.Created
        };
    }

    #endregion Private Methods
}