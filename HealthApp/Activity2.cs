using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Linq;

namespace HealthApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class Activity2 : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.SetBackgroundDrawableResource(Resource.Drawable.top_bar_gradient);

            //Window.SetBackgroundDrawableResource(Resource.Drawable.top_bar_gradient);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity2);

            FindViewById<EditText>(Resource.Id.symptomInput).TextChanged += SymptomInputChange;

            FindViewById<Button>(Resource.Id.nextButton).Click += NextClick; 

            var buttons = new Button[]
                {
                    FindViewById<Button>(Resource.Id.symptom1),
                    FindViewById<Button>(Resource.Id.symptom2),
                    FindViewById<Button>(Resource.Id.symptom3) };

            foreach (var button in buttons)
            {
                button.Click += Button_Click;
                button.Visibility = ViewStates.Invisible;
            }

            //FindViewById<Button>(Resource.Id.button).Click += StartClick;

            //SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar1));

        }

        private void NextClick(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activity3));
            StartActivity(intent);
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            GlobalSettings._questionControler.AddSimptome(((Button)sender).Tag.ToString());

            FindViewById<TextView>(Resource.Id.symptomsList).Text += $"· {((Button)sender).Tag.ToString()} \n";
        }

        private void SymptomInputChange(object sender, Android.Text.TextChangedEventArgs e)
        {
            if(FindViewById<EditText>(Resource.Id.symptomInput).Text.Length >= 3)
            {
                GlobalSettings._questionControler.Simptomes(FindViewById<EditText>(Resource.Id.symptomInput).Text);

                var symptomes = GlobalSettings._questionControler.allSimptomes.Take(3).ToArray();

                var buttons = new Button[] 
                {
                    FindViewById<Button>(Resource.Id.symptom1), 
                    FindViewById<Button>(Resource.Id.symptom2), 
                    FindViewById<Button>(Resource.Id.symptom3)
                };


                for(int i = 0; i < symptomes.Count(); i++)
                {
                    buttons[i].Visibility = ViewStates.Visible;
                    buttons[i].Tag = symptomes[i].display;
                    buttons[i].Text = symptomes[i].shortDisplay;
                }
            }
        }
    }
}
