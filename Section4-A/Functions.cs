using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Section4_A
{
    /// <summary>
    /// A collection of sample Lambda functions that provide a REST api for doing simple math calculations. 
    /// </summary>
    public class Functions
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Functions()
        {
        }

        /// <summary>
        /// Root route that provides information about the other requests that can be made.
        ///
        /// PackageType is currently required to be set to LambdaPackageType.Image till the upcoming .NET 6 managed
        /// runtime is available. Once the .NET 6 managed runtime is available PackageType will be optional and will
        /// default to Zip.
        /// </summary>
        /// <returns>API descriptions.</returns>
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/")]
        public string Default()
        {
            var docs = @"Lambda Calculator Home:
You can make the following requests to invoke other Lambda functions perform calculator operations:
/add/{x}/{y}
/substract/{x}/{y}
/multiply/{x}/{y}
/divide/{x}/{y}
";
            return docs;
        }

        /// <summary>
        /// Perform x + y
        ///
        /// PackageType is currently required to be set to LambdaPackageType.Image till the upcoming .NET 6 managed
        /// runtime is available. Once the .NET 6 managed runtime is available PackageType will be optional and will
        /// default to Zip.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Sum of x and y.</returns>
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/add/{x}/{y}")]
        public int Add(int x, int y, ILambdaContext context)
        {
            context.Logger.LogInformation($"{x} plus {y} is {x + y}");
            return x + y;
        }

        /// <summary>
        /// Perform x - y.
        ///
        /// PackageType is currently required to be set to LambdaPackageType.Image till the upcoming .NET 6 managed
        /// runtime is available. Once the .NET 6 managed runtime is available PackageType will be optional and will
        /// default to Zip.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>x substract y</returns>
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/substract/{x}/{y}")]
        public int Substract(int x, int y, ILambdaContext context)
        {
            context.Logger.LogInformation($"{x} substract {y} is {x - y}");
            return x - y;
        }

        /// <summary>
        /// Perform x * y.
        ///
        /// PackageType is currently required to be set to LambdaPackageType.Image till the upcoming .NET 6 managed
        /// runtime is available. Once the .NET 6 managed runtime is available PackageType will be optional and will
        /// default to Zip.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>x multiply y</returns>
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/multiply/{x}/{y}")]
        public int Multiply(int x, int y, ILambdaContext context)
        {
            context.Logger.LogInformation($"{x} multiply {y} is {x * y}");
            return x * y;
        }

        /// <summary>
        /// Perform x / y.
        ///
        /// PackageType is currently required to be set to LambdaPackageType.Image till the upcoming .NET 6 managed
        /// runtime is available. Once the .NET 6 managed runtime is available PackageType will be optional and will
        /// default to Zip.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>x divide y</returns>
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/divide/{x}/{y}")]
        public int Divide(int x, int y, ILambdaContext context)
        {
            context.Logger.LogInformation($"{x} divide {y} is {x / y}");
            return x / y;
        }
        [LambdaFunction(Timeout = 3, MemorySize = 128)]
        [HttpApi(LambdaHttpMethod.Get, "/mod/{x}/{y}")]
        public int Mod(int x, int y, ILambdaContext context)
        {
            context.Logger.LogInformation($"{x} mod {y} is {x % y}");
            return x % y;
        }
     
    }
}
