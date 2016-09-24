// CSC 483 - HelloWorld-TestMobileApp

using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace test_android_app
{
    [Activity(Label = @"HelloWorld-TestMobileApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public int ClickCount { get; set; } = 0;
        public string EmailAddress { get; set; }
        public string EmailText { get; set; }

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
            EditText enterEmailAddressButton = FindViewById<EditText>(Resource.Id.editEmail);
            EditText enterEmailTextButton = FindViewById<EditText>(Resource.Id.emailText);

            EnableDisableResetButton(clickResetButton, ClickCount);

            clickIncrementerButton.Click += delegate
            {
                ClickCount++;
                clickIncrementerButton.Text = string.Format("{0} clicks!", ClickCount);
                EnableDisableResetButton(clickResetButton, ClickCount);
            };

            megaClickButton.Click += delegate
            {
                ClickCount *= 5;
                clickIncrementerButton.Text = string.Format("{0} clicks!", ClickCount);
                EnableDisableResetButton(clickResetButton, ClickCount);

                // Can only use the x5 "Mega Click" once (before reset).
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
                    enterEmailAddressButton.Enabled = true;
                    enterEmailTextButton.Enabled = true;
                    sendEmailButton.Enabled = true;
                }
                else
                {
                    enterEmailAddressButton.Enabled = false;
                    enterEmailTextButton.Enabled = false;
                    sendEmailButton.Enabled = false;
                }
            };

            enterEmailAddressButton.TextChanged += delegate
            {
                EmailAddress = enterEmailAddressButton.Text;
            };

            enterEmailTextButton.TextChanged += delegate
            {
                EmailText = enterEmailTextButton.Text;
            };

            sendEmailButton.Click += delegate
            {
                List<string> emailBody = new List<string>
                {
                    "Hello from Xamarin.Android!\n",
                    "Number of clicks = " + ClickCount + "\n",
                    EmailText
                };

                var email = new Intent(Intent.ActionSend);

                email.PutExtra(Intent.ExtraEmail, new string[] {EmailAddress}); // Working 09/24/2016
                email.PutExtra(Intent.ExtraSubject, "Hello World Email");       // Working 09/24/2016
                email.PutStringArrayListExtra(Intent.ExtraText, emailBody);     // NOT Working 09/24/2016

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

