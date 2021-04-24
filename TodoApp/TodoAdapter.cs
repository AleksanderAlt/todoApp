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
using TodoApp.Models;

namespace TodoApp
{
    public class TodoAdapter : BaseAdapter<Todo>
    {
        List<Todo> _items;
        Activity _context;

        public TodoAdapter(Activity context, List<Todo> items)
        {
            _context = context;
            _items = items;
        }

        public override Todo this[int position]
        {
            get { return _items[position]; }
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = _context.LayoutInflater.Inflate(Resource.Layout.task_row_layout, null);
            view.FindViewById<TextView>(Resource.Id.taskTitleTextView).Text = _items[position].title;
            view.FindViewById<TextView>(Resource.Id.createdAtTextView).Text = _items[position].created_at;
            view.FindViewById<TextView>(Resource.Id.idTextView).Text = _items[position].id.ToString();

            if (!_items[position].marked_as_done)
            {
                _items[position].image = Resource.Drawable.todo_icon;
                view.FindViewById<ImageView>(Resource.Id.todoImageView).SetImageResource(_items[position].image);
            }
            return view;
        }
    }
}