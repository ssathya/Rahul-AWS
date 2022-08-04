using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Section4;

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
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Get,"users/{userId}")]
    public async Task<User> FunctionHandlerAsync(string userId, ILambdaContext context)
    {
        Guid.TryParse(userId, out var id);
        var user = await dynamoDBContext.LoadAsync<User>(id);
        return user;
    }
}
