using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Telephony;
using Android.Views;

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

        private const int IDM_CALL = 1;
        private const int IDM_SITE = 2;
        private const int IDM_CLOSE = 3;

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

            phone.Text = GetMyPhoneNumber();


            button.Click += delegate {
                Intent = new Intent();
                Intent.SetType("image/*");
                Intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), 99);
            };

            // клик на нижнюю кнопку
            btnColl.Click += delegate {
                var mailIntent = new Intent(Intent.ActionSend);
                mailIntent.SetType("message/rfc822");
                mailIntent.PutExtra(Intent.ExtraEmail, new string[] { "info@artultra.ru" });
                mailIntent.PutExtra(Intent.ExtraSubject, "заказ из приложения");
                mailIntent.PutExtra(Intent.ExtraText, "Имя: " + name.Text + "; " + "Телефон: " + phone.Text + "; " + "Дата: " + date.Text);
                if (Rcode == 99)
                    mailIntent.PutExtra(Intent.ExtraStream, Ddata.Data);
                try { StartActivity(mailIntent); popup("Письмо переданно на отправку!"); }
                catch { popup("Настройте почту на вашем устройстве"); }

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


        [assembly: Xamarin.Forms.Dependency(typeof(PhoneNumberService))]
        public string GetMyPhoneNumber()
        {
            TelephonyManager mgr = Application.Context.GetSystemService(Context.TelephonyService) as TelephonyManager;
            return mgr.Line1Number;
        }

        //Определение создания меню
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);
            int groupId = 0;

            IMenuItem menuItem1 = menu.Add(groupId, IDM_CALL, Menu.None, "Позвонить");
            IMenuItem menuItem2 = menu.Add(groupId, IDM_SITE, Menu.None, "Перейти на сайт");
            IMenuItem menuItem3 = menu.Add(groupId, IDM_CLOSE, Menu.None, "Закрыть");

            return true;
        }
        //Описание обработчика события выбора пункта меню
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            string selectedItem = string.Empty;

            switch (item.ItemId)
            {
                case IDM_CALL:
                    var uri = Android.Net.Uri.Parse("tel:+7(495)7291394");
                    var intent = new Intent(Intent.ActionDial, uri);
                    StartActivity(intent);
                    break;
                case IDM_SITE:
                    var uriss = Android.Net.Uri.Parse("http://artultra.ru/");
                    var intentt = new Intent(Intent.ActionView, uriss);
                    StartActivity(intentt);
                    break;
                case IDM_CLOSE:
                    Finish();
                    selectedItem = "Close";
                    break;
                default:
                    return false;
            }
           
            return true;
        }

        // Сообщения
        public void popup(string s)
        {
            Toast.MakeText(this, s, ToastLength.Short).Show();
        }
    }
}

