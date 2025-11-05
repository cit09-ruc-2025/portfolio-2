using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebServiceLayer;

namespace BackendTests
{
    public class WebServiceTests
    {
        private const string UserApi = "http://localhost:5089/api/user";
        private const string LoginApi = "http://localhost:5089/api/login";
        private const string ReviewApi = "http://localhost:5089/api/review";
        private const string PlaylistApi = "http://localhost:5089/api/playlist";

        // api/user
        [Fact]
        public void ApiUsers_CreateWithValidArguments_Created()
        {
            var newUser = new
            {
                email = "abc@abc.com",
                username = "abc",
                password = "hello"
            };

            var (_, statusCode) = PostData(UserApi, newUser, null);

            Assert.Equal(HttpStatusCode.Created, statusCode);
        }

        //api/review
        [Fact]
        public void ApiReviews_InsertWithValidArguments_Created()
        {
            var token = LoginAndGetToken("abc", "hello");
            Assert.NotNull(token);

            var update = new
            {
                rating = 8,
                review = "good"
            };

            var statusCode = PutData($"{ReviewApi}/tt0344510", update, token);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void ApiReviews_InsertWithInValidArguments_Created()
        {
            var token = LoginAndGetToken("abc", "hello");
            Assert.NotNull(token);

            var update = new
            {
                rating = 100,
                review = "good"
            };

            var statusCode = PutData($"{ReviewApi}/tt0344510", update, token);

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }
        [Fact]
        public void ApiReviews_InsertWithAnauthorized_Created()
        {

            var update = new
            {
                rating = 100,
                review = "good"
            };

            var statusCode = PutData($"{ReviewApi}/tt0344510", update, null);

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        //api/playlist
        [Fact]
        public void ApiPlaylist_InsertWithValidArguments_Created()
        {
            var token = LoginAndGetToken("abc", "hello");
            Assert.NotNull(token);

            var playlist = new
            {
                title = "Test",
                description = "test"
            };

            var (newPlaylist, statusCode) = PostData($"{PlaylistApi}/create", playlist, token);

            DeleteData($"{PlaylistApi}/{newPlaylist["Id"]}", token);


            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        public void ApiPlaylist_AddToPlaylist_Created()
        {
            var token = LoginAndGetToken("abc", "hello");
            Assert.NotNull(token);

            var playlist = new
            {
                title = "Test",
                description = "test"
            };

            var (newPlaylist, _) = PostData($"{PlaylistApi}/create", playlist, token);

            var media = new
            {
                itemId = "tt0344815",
                isMedia = true
            };

            var (_, statusCode) = PostData($"{PlaylistApi}/{newPlaylist["Id"]}/add", media, token);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }


        // Helpers

        private string? LoginAndGetToken(string username, string password)
        {
            var loginPayload = new { username, password };
            var (responseJson, statusCode) = PostData(LoginApi, loginPayload, null);

            var token = responseJson["token"]?.ToString();

            return token;
        }

        (JObject, HttpStatusCode) PostData(string url, object content, string? token)
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<JObject>(data);
            return (result ?? new JObject(), response.StatusCode);
        }

        HttpStatusCode PutData(string url, object content, string? token)
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var response = client.PutAsync(
                url,
                new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json")).Result;
            return response.StatusCode;
        }

        HttpStatusCode DeleteData(string url, string? token)
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }

    }
}