using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using TodoApp.Models;
using Newtonsoft.Json;
using RestSharp;

namespace TodoApp.Services
{
    public class RemoteDataService
    {
        public User user = new User();
        public async Task<User> GetUser(string username, string password)
        {
            RestClient client = new RestClient("http://demo2.z-bit.ee/");
            RestRequest request = new RestRequest("/users/get-token", Method.POST);
            request.AddJsonBody(new { username = username, password = password });
            var response = client.Execute(request);
            if (!string.IsNullOrEmpty(response.Content))
            {
                user = JsonConvert.DeserializeObject<User>(response.Content);
                return user;
            }
            else
            {
                throw new Exception((int)response.StatusCode + "-" + response.StatusCode.ToString());
            }
        }

        public List<Todo> todos = new List<Todo>();
        public async Task<List<Todo>> GetTodos(string access_token)
        {
            RestClient client = new RestClient("http://demo2.z-bit.ee/tasks");
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {access_token}");
            request.AddParameter("text/plain", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            todos = JsonConvert.DeserializeObject<List<Todo>>(response.Content);
            todos = todos.OrderByDescending(o => o.id).ToList();
            return todos.ToList();
        }

        public async Task<List<Todo>> DeleteTodo(string id, string access_token)
        {
            RestClient client = new RestClient("http://demo2.z-bit.ee/");
            RestRequest request = new RestRequest($"tasks/{id}", Method.DELETE);
            request.AddHeader("Authorization", $"Bearer {access_token}");
            request.AddParameter("text/plain", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return null;
        }

        public async Task<List<Todo>> AddTodo(string title, string access_token)
        {
            RestClient client = new RestClient("http://demo2.z-bit.ee/tasks");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {access_token}");
            request.AddJsonBody(new { title = title, desc = "" });
            request.AddParameter("text/plain", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return null;
        }
    }
}