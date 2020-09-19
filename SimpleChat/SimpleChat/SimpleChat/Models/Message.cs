using System;
using Xamarin.Forms;

namespace SimpleChat.Models
{
    public class Message
    {
        public string UserId { get; set; }

        public string Text { get; set; }

        public DateTime MessageDateTime { get; set; }

        public LayoutOptions HorizontalOptions { 
            get
            {
                return App.User.Id == this.UserId ? LayoutOptions.EndAndExpand : LayoutOptions.StartAndExpand;
            } 
        }
    }
}
