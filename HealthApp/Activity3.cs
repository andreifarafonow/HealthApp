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

namespace HealthApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class Activity3 : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            GlobalSettings._questionControler.SendSimptomes();
            GlobalSettings._questionControler.StartQuestion();

            Window.SetBackgroundDrawableResource(Resource.Drawable.top_bar_gradient);

            //Window.SetBackgroundDrawableResource(Resource.Drawable.top_bar_gradient);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity3);
        }
    }
}