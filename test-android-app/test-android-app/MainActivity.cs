// CSC 483 - HelloWorld-TestMobileApp
// With help (code) from: Sven-Michael Stübe @ http://stackoverflow.com/questions/39678310/add-email-body-text-and-send-email-from-xamarin-android-app/39681516#39681516
using System;
using System.Net.Mail;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System.Text;
using Java.Interop;

namespace test_android_app
{
    [Activity(Label = @"HelloWorld-TestMobileApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        private Button _clickIncrementerButton;
        private Button _megaClickButton;
        private Button _clickResetButton;
        private Button _sendEmailButton;
        private CheckBox _enableEmailCheckBox;
        private EditText _enterEmailAddressButton;
        private EditText _enterEmailTextButton;
        private int ClickCount { get; set; } = 0;

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
            _sendEmailButton = FindViewById<Button>(Resource.Id.sendEmailButton);
            _enableEmailCheckBox = FindViewById<CheckBox>(Resource.Id.checkBoxEnableEmail);
            _enterEmailAddressButton = FindViewById<EditText>(Resource.Id.editEmail);
            _enterEmailTextButton = FindViewById<EditText>(Resource.Id.emailText);

            EnableDisableResetButton(_clickResetButton, ClickCount);
        }

        protected override void OnStart()
        {
            base.OnStart();

            // Register Event Handlers
            _clickIncrementerButton.Click += IncrementerButtonOnClick;
            _megaClickButton.Click += MegaClickButtonOnClick;
            _clickResetButton.Click += ClickResetButtonOnClick;
            _enableEmailCheckBox.Click += EnableEmailCheckBoxOnClick;
            _sendEmailButton.Click += SendEmailButtonOnClick;
        }

        protected override void OnStop()
        {
            // Deregister Event Handlers
            _sendEmailButton.Click -= SendEmailButtonOnClick;
            _enableEmailCheckBox.Click -= EnableEmailCheckBoxOnClick;
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

        private void EnableEmailCheckBoxOnClick(object sender, EventArgs eventArgs)
        {
            if (_enableEmailCheckBox.Checked)
            {
                _enterEmailAddressButton.Enabled = true;
                _enterEmailTextButton.Enabled = true;
                _sendEmailButton.Enabled = true;
            }
            else
            {
                _enterEmailAddressButton.Enabled = false;
                _enterEmailTextButton.Enabled = false;
                _sendEmailButton.Enabled = false;
            }
        }

        private void SendEmailButtonOnClick(object sender, EventArgs eventArgs)
        {
            // Following Code Adapted From Morten Godrim Jensen @ http://stackoverflow.com/questions/30255789/how-to-send-a-mail-in-xamarin-using-system-net-mail-smtpclient
            // Build the Body of the Email
            var emailBody = new StringBuilder();
            emailBody.AppendLine("Hello from Xamarin.Android!");
            emailBody.AppendFormat("Number of clicks = {0}", ClickCount);
            emailBody.AppendLine();
            emailBody.Append(_enterEmailTextButton.Text);

            // Set Credentials
            string username = "------@gmail.com";
            string password = "------";
            System.Net.NetworkCredential netCred = new System.Net.NetworkCredential(username, password);

            // Set Up Email Parameters
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(_enterEmailAddressButton.Text);
            mailMessage.Subject = "Test Email Message";
            mailMessage.From = new MailAddress("------@gmail.com");
            mailMessage.Body = emailBody.ToString();

            // Set Up SMTP Client
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = netCred;
            smtpClient.Port = 465;

            // Attempt to Send Email
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Unable to Send Email " + ex, ToastLength.Short).Show();
            }
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

