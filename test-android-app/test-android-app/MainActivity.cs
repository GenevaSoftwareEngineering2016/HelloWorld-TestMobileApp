// CSC 483 - HelloWorld-TestMobileApp
using Android.App;
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

