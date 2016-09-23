// CSC 483 - HelloWorld-TestMobileApp

using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;

namespace test_android_app
{
    [Activity(Label = @"HelloWorld-TestMobileApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public int ClickCount { get; set; } = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our buttons from the layout resource,
            // and attach a events to them
            Button clickIncrementerButton = FindViewById<Button>(Resource.Id.ClickIncrementer);
            Button megaClickButton = FindViewById<Button>(Resource.Id.MegaClickButton);
            Button clickResetButton = FindViewById<Button>(Resource.Id.ClickCountReset);
            Button sendEmailButton = FindViewById<Button>(Resource.Id.sendEmailButton);
            CheckBox enableEmailCheckBox = FindViewById<CheckBox>(Resource.Id.checkBoxEnableEmail);
            EditText enterEmailTextButton = FindViewById<EditText>(Resource.Id.emailText);
            EditText enterEmailButton = FindViewById<EditText>(Resource.Id.editEmail);

            EnableDisableResetButton(clickResetButton, ClickCount);

            clickIncrementerButton.Click += delegate
            {
                ClickCount++;
                clickIncrementerButton.Text = string.Format("{0} clicks!", ClickCount);
                EnableDisableResetButton(clickResetButton, ClickCount);
            };

            megaClickButton.Click += delegate
            {
                ClickCount += 5;
                clickIncrementerButton.Text = string.Format("{0} clicks!", ClickCount);
                EnableDisableResetButton(clickResetButton, ClickCount);

                // Can only use the +5 "Mega Click" once (before reset).
                megaClickButton.Enabled = false;
            };

            clickResetButton.Click += delegate
            {
                ClickCount = 0;
                clickIncrementerButton.Text = "No Clicks Yet!";
                EnableDisableResetButton(clickResetButton, ClickCount);

                // Resetting click count, so the user is now allowed to use another "Mega Click."
                megaClickButton.Enabled = true;
            };

            enableEmailCheckBox.Click += delegate
            {
                if (enableEmailCheckBox.Checked)
                {
                    enterEmailTextButton.Enabled = true;
                    enterEmailButton.Enabled = true;
                    sendEmailButton.Enabled = true;
                }
                else
                {
                    enterEmailTextButton.Enabled = false;
                    enterEmailButton.Enabled = false;
                    sendEmailButton.Enabled = false;
                }
            };

            sendEmailButton.Click += delegate
            {
                var email = new Intent(Intent.ActionSend);

                email.PutExtra(Intent.ExtraEmail, new string[] {"adcaldwe@geneva.edu"});    // Working 09/23/2016
                email.PutExtra(Intent.ExtraCc, enterEmailButton.Text);                  // Not Working 09/23/2016
                email.PutExtra(Intent.ExtraSubject, "Hello World Email");   // Working 09/23/2016
                email.PutExtra(Intent.ExtraText, new string[]                // Not Working 09/23/2016
                {
                    "Hello from Xamarin.Android!\n",
                    "Number of clicks = " + ClickCount + "\n",
                    enterEmailTextButton.Text
                });

                email.SetType("message/rfc822");

                try
                {
                    StartActivity(email);
                }
                catch (Android.Content.ActivityNotFoundException ex)
                {
                    Toast.MakeText(this, "There are no email applications installed.", ToastLength.Short).Show();
                }
            };
        }

        private static void EnableDisableResetButton(Button clickResetButton, int clicks)
        {
            if (clicks > 0)
            {
                clickResetButton.Enabled = true;
            }
            else
            {
                clickResetButton.Enabled = false;
            }
        }
    }
}

