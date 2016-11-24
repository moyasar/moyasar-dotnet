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
using Xprema.Core.Repsitory;
using XpremaCore.Adapters;

namespace XpremaCore.CompanyActiveity
{
    [Activity(Label = "AcCompanyList", MainLauncher = true)]
    public class AcCompanyList : Activity
    {
        private CommandComany cmd;
        private ListView CompanyList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ComanyMenu);
            // Create your application here
            CompanyList = FindViewById<ListView>(Resource.Id.CompanyList);
            cmd = new CommandComany();
        var q =     cmd.GetAllCompanies();
            CompanyList.Adapter = new CompanyListAdapter(this,q);

        }
    }
}