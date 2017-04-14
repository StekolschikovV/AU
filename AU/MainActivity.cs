using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Graphics.Drawables;

namespace AU
{
    [Activity(Label = "АртУльтра", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText name, phone, date;
        Button btnColl;

        Intent Ddata;
        Result code;
        int Rcode;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            name = FindViewById<EditText>(Resource.Id.editTextName);
            phone = FindViewById<EditText>(Resource.Id.editTextPhone);
            date = FindViewById<EditText>(Resource.Id.editTextDate); 
            btnColl = FindViewById<Button>(Resource.Id.buttonColl);
            Button button = FindViewById<Button>(Resource.Id.myButton);

            var actionBar = this.ActionBar;
            actionBar.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.ParseColor("#981b07")));


            button.Click += delegate {
                Intent = new Intent();
                Intent.SetType("image/*");
                Intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), 99);
            };

            btnColl.Click += delegate {
                var mailIntent = new Intent(Intent.ActionSend);
                mailIntent.SetType("message/rfc822");
                mailIntent.PutExtra(Intent.ExtraEmail, new string[] { "mou.mail.com@gmail.com" });
                mailIntent.PutExtra(Intent.ExtraSubject, "заказ из приложения");
                mailIntent.PutExtra(Intent.ExtraText, name.Text + " --- " + phone.Text + " --- " + date.Text);
                if (Rcode == 99)
                    mailIntent.PutExtra(Intent.ExtraStream, Ddata.Data);

                try { StartActivity(mailIntent); popup("Письмо переданно на отправку!"); }
                catch { popup("настройте почту на вашем устройстве"); }

            };

        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Rcode = requestCode;
            code = resultCode;
            Ddata = data;
            if (Rcode == 99)
                popup("Каратинка выбра!");
        }

        public void popup(string s)
        {
            Toast.MakeText(this, s, ToastLength.Short).Show();
        }
    }
}

