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
    public static class GlobalSettings
    {
        public static QuestionControler _questionControler = null;

        static GlobalSettings()
        {
            _questionControler = new QuestionControler();


        }

    }
}