using System.Web.Http;
using FunctionsAppSample;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;

namespace TestProject
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("", typeof(BadRequestResult))]
        [InlineData("QueryParamValue", typeof(OkResult))]
        [InlineData("ThisStringCausesTheFunctionToThrowAnError", typeof(InternalServerErrorResult))]
        public async Task Function_Returns_Correct_StatusCode(string queryParam, Type expectedResult)
        {
            //Arrange
            var qc = new QueryCollection(new Dictionary<string, StringValues>{{"q", new StringValues(queryParam)}});
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Query)
                .Returns(() => qc);

            var logger = Mock.Of<ILogger>();
            //Act
            var response = await Function1.Run(request.Object, logger);
            //Assert
            Assert.True(response.GetType() == expectedResult);
        }
    }
}