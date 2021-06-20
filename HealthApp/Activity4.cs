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
    using System.Text.RegularExpressions;

    namespace HealthApp
    {
        [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
        public class Activity4 : Activity
        {

            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                GlobalSettings._questionControler.SendSimptomes();
                //GlobalSettings._questionControler.StartQuestion();

                Window.SetBackgroundDrawableResource(Resource.Drawable.top_bar_gradient);


                //Window.SetBackgroundDrawableResource(Resource.Drawable.top_bar_gradient);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                SetContentView(Resource.Layout.activity4);


                var result = GlobalSettings._questionControler.FinalRequest();

                FindViewById<TextView>(Resource.Id.resultView).Text = result.Title;
                FindViewById<TextView>(Resource.Id.res).Text = $"({result.Probalility}% вероятность)";
                FindViewById<TextView>(Resource.Id.resultView).Text = $"({result.Probalility}% вероятность)";
            }
        }
    }
}