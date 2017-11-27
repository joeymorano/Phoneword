using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

public class NewMainPage : ContentPage
{
    Entry phoneNumberText;
    Button translateButton;
    Button callButton;
    string translatedNumber;


    public NewMainPage()
    {
        this.Padding = new Thickness(20, 20, 20, 20);

        StackLayout panel = new StackLayout
        {
            Spacing = 15
        };

        panel.Children.Add(new Label
        {
            Text = "Enter a Phoneword:",
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
        });

        panel.Children.Add(phoneNumberText = new Entry
        {
            Text = "1-855-XAMARIN",
        });

        panel.Children.Add(translateButton = new Button
        {
            Text = "Translate"
        });

        panel.Children.Add(callButton = new Button
        {
            Text = "Call",
            IsEnabled = false,
        });

        translateButton.Clicked += OnTranslate;
        callButton.Clicked += OnCall;
        this.Content = panel;
    }

    async void OnCall(object sender, System.EventArgs e)
    {
        if (await this.DisplayAlert(
            "Dial a Number",
            "Would you like to call " + translatedNumber + "?",
            "Yes",
            "No"))
        {
            var dialer = DependencyService.Get<IDialer>();
            if (dialer != null)
                await dialer.DialAsync(translatedNumber);
        }
    }

    private void OnTranslate(object sender, EventArgs e)
    {
        string enteredNumber = phoneNumberText.Text;
        translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
        if (!string.IsNullOrEmpty(translatedNumber))
        {
            callButton.IsEnabled = true;
            callButton.Text = "Call " + translatedNumber;
        }
        else
        {
            callButton.IsEnabled = false;
            callButton.Text = "Call";
        }
    }

}

public interface IDialer
{
    Task<bool> DialAsync(string number);
}