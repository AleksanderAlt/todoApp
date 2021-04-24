using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using TodoApp.Services;
using Android.Views;
using Android.Support.V7.App;

namespace TodoApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            var dataService = new RemoteDataService();

            var usernameEditText = FindViewById<EditText>(Resource.Id.usernameTextView);
            var passwordEditText = FindViewById<EditText>(Resource.Id.passwordTextView);
            var loginButton = FindViewById<Button>(Resource.Id.loginButton);
            var helloTextView = FindViewById<TextView>(Resource.Id.helloTextView);
            var listView = FindViewById<ListView>(Resource.Id.taskListView);
            var todoTextView = FindViewById<TextView>(Resource.Id.todoTextView);
            var addTodoButton = FindViewById<Button>(Resource.Id.addTodoButton);

            loginButton.Click += async delegate
            {
                var username = usernameEditText.Text.Trim();
                var password = passwordEditText.Text.Trim();
                if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
                {
                    try
                    {
                        var userData = await dataService.GetUser(username, password);
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
                            var selectedItem = taskData[args.Position];
                            await dataService.DeleteTodo(selectedItem.id.ToString(), userData.access_token);
                            var updatedTaskData = await dataService.GetTodos(userData.access_token);

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
                        Toast toast = Toast.MakeText(this, "Enter correct password or username", ToastLength.Long);
                        toast.SetGravity(GravityFlags.Center, 0, 0);
                        toast.Show();
                    }
                }
            };

            addTodoButton.Click += async delegate
            {
                var username = usernameEditText.Text.Trim();
                var password = passwordEditText.Text.Trim();
                var newTodo = todoTextView.Text.Trim();
                var userData = await dataService.GetUser(username, password);
                if (!string.IsNullOrEmpty(newTodo))
                {
                    await dataService.AddTodo(newTodo, userData.access_token);
                    var taskData = await dataService.GetTodos(userData.access_token);
                    todoTextView.Text = "";

                    int count = 0;
                    foreach (var item in taskData)
                    {
                        count++;
                    }

                    helloTextView.Text = $"Hello {userData.firstname}! You have {count} tasks.";

                    listView.Adapter = new TodoAdapter(this, taskData);
                }
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}