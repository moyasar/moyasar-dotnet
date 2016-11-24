using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xprema.Core.Model;

namespace XpremaCore.Adapters
{
   public class CompanyListAdapter:BaseAdapter<Company>
   {
     private  List<Company> items;
     private  Activity context;

       public CompanyListAdapter(Activity c, List<Company> ls ):base()
       {
           this.items = ls;
           this.context = c;
       }
       public override long GetItemId(int position)
       {
           return position;
       }

       public override View GetView(int position, View convertView, ViewGroup parent)
       {
           var item = items[position];
           if (convertView==null)
           {
               convertView = context.LayoutInflater.Inflate(Resource.Layout.XpremaListViewRow, null);
           }
           convertView.FindViewById<TextView>(Resource.Id.CompanyName).Text = item.ComanyName;
            convertView.FindViewById<TextView>(Resource.Id.Description).Text = item.Description;
            convertView.FindViewById<TextView>(Resource.Id.Price).Text = "10 SDG.";
            convertView.FindViewById<ImageView>(Resource.Id.imageView1).SetImageResource(Resource.Drawable.Icon);
            return convertView;
       }

       public override int Count => items.Count;

       public override Company this[int position] => items[position];
   }
}