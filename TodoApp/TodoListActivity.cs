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
using TodoApp.Services;
using Xamarin.Essentials;

namespace TodoApp
{
    [Activity(Label = "TodoListActivity")]
    public class TodoListActivity : Activity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.tasklist_layout);

            var helloTextView = FindViewById<TextView>(Resource.Id.helloTextView);
            var listView = FindViewById<ListView>(Resource.Id.taskListView);
            var todoTextView = FindViewById<TextView>(Resource.Id.todoTextView);
            var addTodoButton = FindViewById<Button>(Resource.Id.addTodoButton);

            var dataService = new RemoteDataService();
            //var loginService = new LoginService();
            var un = Preferences.Get("username", "");
            var pw = Preferences.Get("password", "");

            try
                {
                var userData = await dataService.GetUser(un, pw);
                var taskData = await dataService.GetTodos(userData.access_token);
                
                int count = 0;
                foreach (var item in taskData)
                    {
                        count++;
                    }

                helloTextView.Text = $"Hello {userData.firstname}! You have {count} tasks.";

                listView.Adapter = new TodoAdapter(this, taskData);

                listView.ItemClick += async delegate (object sender, AdapterView.ItemClickEventArgs args)
                {
                    var updatedTaskData = await dataService.GetTodos(userData.access_token);
                    var selectedItem = updatedTaskData[args.Position];
                    await dataService.DeleteTodo(selectedItem.id.ToString(), userData.access_token);
                    updatedTaskData = await dataService.GetTodos(userData.access_token);

                    int updatedCount = 0;
                    foreach (var item in updatedTaskData)
                    {
                        updatedCount++;
                    }
                    helloTextView.Text = $"Hello {userData.firstname}! You have {updatedCount} tasks.";
                    listView.Adapter = new TodoAdapter(this, updatedTaskData);
                    Toast.MakeText(this, $"{selectedItem.title} deleted", ToastLength.Long).Show();
                    };
                }
            catch
            {
                Toast toast = Toast.MakeText(this, "ERROR", ToastLength.Long);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();
            }
                

            addTodoButton.Click += async delegate
            {
                var newTodo = todoTextView.Text.Trim();
                var userData = await dataService.GetUser(un, pw);
                if (!string.IsNullOrEmpty(newTodo))
                {
                    await dataService.AddTodo(newTodo, userData.access_token);
                    var taskAddedData = await dataService.GetTodos(userData.access_token);
                    todoTextView.Text = "";

                    int count = 0;
                    foreach (var item in taskAddedData)
                    {
                        count++;
                    }

                    helloTextView.Text = $"Hello {userData.firstname}! You have {count} tasks.";

                    listView.Adapter = new TodoAdapter(this, taskAddedData);
                }
            };
        }
    }
}