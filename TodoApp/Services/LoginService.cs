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
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class LoginService
    {
        public Login login = new Login();
        public async Task<Login> Login(string username, string password)
        {
            login.username = username;
            login.password = password;

            return login;
        }
    }
}