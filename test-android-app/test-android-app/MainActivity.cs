// CSC 483 - HelloWorld-TestMobileApp
// With help (code) from: Sven-Michael Stübe @ http://stackoverflow.com/questions/39678310/add-email-body-text-and-send-email-from-xamarin-android-app/39681516#39681516
using System;
using Android.App;
using Android.Widget;
using Android.OS;

namespace test_android_app
{
    [Activity(Label = @"HelloWorld-TestMobileApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        private Button _clickIncrementerButton;
        private Button _megaClickButton;
        private Button _clickResetButton;
        private int ClickCount { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our buttons from the layout resource,
            // and attach a events to them
            _clickIncrementerButton = FindViewById<Button>(Resource.Id.ClickIncrementer);
            _megaClickButton = FindViewById<Button>(Resource.Id.MegaClickButton);
            _clickResetButton = FindViewById<Button>(Resource.Id.ClickCountReset);

            EnableDisableResetButton(_clickResetButton, ClickCount);
        }

        protected override void OnStart()
        {
            base.OnStart();

            // Register Event Handlers
            _clickIncrementerButton.Click += IncrementerButtonOnClick;
            _megaClickButton.Click += MegaClickButtonOnClick;
            _clickResetButton.Click += ClickResetButtonOnClick;
        }

        protected override void OnStop()
        {
            // Deregister Event Handlers
            _clickResetButton.Click -= ClickResetButtonOnClick;
            _megaClickButton.Click -= MegaClickButtonOnClick;
            _clickIncrementerButton.Click -= IncrementerButtonOnClick;

            base.OnStop();
        }

        private void IncrementerButtonOnClick(object sender, EventArgs eventArgs)
        {
            ClickCount++;
            _clickIncrementerButton.Text = string.Format("{0} clicks!", ClickCount);
            EnableDisableResetButton(_clickResetButton, ClickCount);
        }

        private void MegaClickButtonOnClick(object sender, EventArgs eventArgs)
        {
            ClickCount *= 5;
            _clickIncrementerButton.Text = string.Format("{0} clicks!", ClickCount);
            EnableDisableResetButton(_clickResetButton, ClickCount);

            // Can only use the x5 "Mega Click" once (before reset).
            _megaClickButton.Enabled = false;
        }

        private void ClickResetButtonOnClick(object sender, EventArgs eventArgs)
        {
            ClickCount = 0;
            _clickIncrementerButton.Text = "No Clicks Yet!";
            EnableDisableResetButton(_clickResetButton, ClickCount);

            // Resetting click count, so the user is now allowed to use another "Mega Click."
            _megaClickButton.Enabled = true;
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

