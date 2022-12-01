using AppCitas.Service.DTOs;
using AppCitas.UnitTest.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppCitas.UnitTest.Tests
{
    public class LikesControllerTests
    {
        private string apiRoute = "api/likes";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUri;
        private string registeredObject;
        private HttpContent httpContent;
        public LikesControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("NotFound", "lisa", "Pa$$w0rd", "a")]
        public async Task AddLike_NotFound(string statusCode, string username, string password, string userLiked)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


            requestUri = $"{apiRoute}/" + userLiked;

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }


        [Theory]
        [InlineData("BadRequest", "lisa", "Pa$$w0rd", "lisa")]
        public async Task AddLike_BadRequest(string statusCode, string username, string password, string userLiked)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


            requestUri = $"{apiRoute}/" + userLiked;

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        

        [Theory]
        [InlineData("BadRequest", "lisa", "Pa$$w0rd", "todd")]
        public async Task AddLike_BadRequest2(string statusCode, string username, string password, string userLiked)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


            requestUri = $"{apiRoute}/" + userLiked;

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            _client.DefaultRequestHeaders.Authorization = null;
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd", "todd", "liked")]
        [InlineData("OK", "lisa", "Pa$$w0rd", "todd", "likedby")]
        public async Task GetUserLikes_OK(string statusCode, string username, string password, string userliked, string predicate)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            requestUri = $"{apiRoute}/" + userliked;

            httpResponse = await _client.PostAsync(requestUri, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;

            var userLiked = await LoginHelper.LoginUser(userliked, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userLiked.Token);

            requestUri = $"{apiRoute}" + "?predicate=" + predicate;

            // Act
            httpResponse = await _client.GetAsync(requestUri);
            _client.DefaultRequestHeaders.Authorization = null;

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        #region Privated methods
        private static string GetRegisterObject(RegisterDto registerDto)
        {
            var entityObject = new JObject()
            {
                { nameof(registerDto.Username), registerDto.Username },
                { nameof(registerDto.KnownAs), registerDto.KnownAs },
                { nameof(registerDto.Gender), registerDto.Gender },
                { nameof(registerDto.DateOfBirth), registerDto.DateOfBirth },
                { nameof(registerDto.City), registerDto.City },
                { nameof(registerDto.Country), registerDto.Country },
                { nameof(registerDto.Password), registerDto.Password }
            };

            return entityObject.ToString();
        }

        private static string GetRegisterObject(LoginDto loginDto)
        {
            var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };
            return entityObject.ToString();
        }

        private StringContent GetHttpContent(string objectToEncode)
        {
            return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
        }

        #endregion
    }
}
