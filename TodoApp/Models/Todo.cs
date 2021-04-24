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

namespace TodoApp.Models
{
    public class Todo
    {
        public int id { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public bool marked_as_done { get; set; }
        public string created_at { get; set; }
        public int image { get; set; }
    }
}