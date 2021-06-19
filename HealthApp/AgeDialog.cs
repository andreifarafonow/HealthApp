using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace HealthApp
{
    public class AgeDialog : DialogFragment
    {
        string pol = "";
        string age = "";

        View _view;

        public static AgeDialog NewInstance(Bundle bundle)
        {
            AgeDialog fragment = new AgeDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            _view = inflater.Inflate(Resource.Layout.layout_frm, container, false);

            _view.FindViewById<Button>(Resource.Id.button2).Click += AgeDialog_Click;

            _view.FindViewById<Button>(Resource.Id.maleButton).Click += MaleClick;
            _view.FindViewById<Button>(Resource.Id.femaleButton).Click += FemaleClick;

            

            return _view;
        }

        private void MaleClick(object sender, System.EventArgs e)
        {
            pol = "male";

            _view.FindViewById<Button>(Resource.Id.maleButton).SetBackgroundColor(new Android.Graphics.Color(18, 138, 146));
            _view.FindViewById<Button>(Resource.Id.femaleButton).SetBackgroundColor(new Android.Graphics.Color(255, 255, 255));
        }

        private void FemaleClick(object sender, System.EventArgs e)
        {
            pol = "female";
            
            _view.FindViewById<Button>(Resource.Id.femaleButton).SetBackgroundColor(new Android.Graphics.Color(18, 138, 146));
            _view.FindViewById<Button>(Resource.Id.maleButton).SetBackgroundColor(new Android.Graphics.Color(255, 255, 255));
        }

        private void AgeDialog_Click(object sender, System.EventArgs e)
        {
            age = _view.FindViewById<EditText>(Resource.Id.input_text).Text;

            Intent intent = new Intent(Activity, typeof(Activity2));
            StartActivity(intent);
        }

    
    }
}