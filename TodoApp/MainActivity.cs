using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.App;
using Android.Content;
using TodoApp.Services;
using Android.Views;
using Xamarin.Essentials;

namespace TodoApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var usernameEditText = FindViewById<EditText>(Resource.Id.usernameTextView);
            var passwordEditText = FindViewById<EditText>(Resource.Id.passwordTextView);
            var loginButton = FindViewById<Button>(Resource.Id.loginButton);

            var loginService = new LoginService();

            loginButton.Click += async delegate
            {
                if (!string.IsNullOrEmpty(usernameEditText.Text) || !string.IsNullOrEmpty(passwordEditText.Text))
                {
                    Preferences.Set("username", usernameEditText.Text);
                    Preferences.Set("password", passwordEditText.Text);
                    await loginService.Login(usernameEditText.Text, passwordEditText.Text);
                    Intent intent = new Intent(this, typeof(TodoListActivity));
                    StartActivity(intent);
                }
                else
                {
                    Toast toast = Toast.MakeText(this, "Enter correct password or username", ToastLength.Long);
                    toast.SetGravity(GravityFlags.Center, 0, 0);
                    toast.Show();
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