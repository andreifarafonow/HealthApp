using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HealthApp.HealthApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HealthApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class Activity3 : Activity
    {
        static int _currentStep = 1;

        Button[] buttons = null;

        string questionnaireId = "", linkId = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            GlobalSettings._questionControler.SendSimptomes();
            //GlobalSettings._questionControler.StartQuestion();

            Window.SetBackgroundDrawableResource(Resource.Drawable.top_bar_gradient);


            //Window.SetBackgroundDrawableResource(Resource.Drawable.top_bar_gradient);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity3);

            buttons = new Button[]
            {
                FindViewById<Button>(Resource.Id.answer1),
                FindViewById<Button>(Resource.Id.answer2),
                FindViewById<Button>(Resource.Id.answer3),
                FindViewById<Button>(Resource.Id.answer4),
                FindViewById<Button>(Resource.Id.answer5),
                FindViewById<Button>(Resource.Id.answer6)
            };

            foreach(var button in buttons)
            {
                button.Click += AnswerClick;
            }

            ReloadQues();
        }

        private void ReloadQues()
        {
            foreach (var button in buttons)
            {
                button.Visibility = ViewStates.Invisible;
            }

            var data = GlobalSettings._questionControler.StartQuestion(_currentStep.ToString());

            linkId = data.linkId;
            questionnaireId = data.questionnaireId;

            Regex regex = new Regex("\"display\": \"(.*?)\"");

            for (int i = 0; i < data.answers.Length; i++)
            {
                buttons[i].Visibility = ViewStates.Visible;
                buttons[i].Tag = data.answers[i];
                buttons[i].Text = "       " + regex.Match(data.answers[i]).Groups[1].Value;
            }

            FindViewById<TextView>(Resource.Id.qHead).Text = data.question;
            FindViewById<TextView>(Resource.Id.qSubHead).Text = data.desc;
        }

        private void AnswerClick(object sender, EventArgs e)
        {
            string answer = ((Button)sender).Tag.ToString();

            string resp = GlobalSettings._questionControler.SendAnswer(answer, questionnaireId, linkId);

            int o = 0;

            if (resp != "{}")
            {
                _currentStep++;
                ReloadQues();
            }
            else
            {
                Intent intent = new Intent(this, typeof(Activity4));
                StartActivity(intent);
            }
        }
    }
}