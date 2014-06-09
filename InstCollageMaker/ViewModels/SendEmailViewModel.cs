using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Venetasoft.WP.Net;

namespace InstCollageMaker
{
    public class SendEmailViewModel
    {
        public SendEmailViewModel()
        {
            SendEmail = new DelegateCommand(() =>
            {
                  MailMessage mailMessage = new MailMessage();

                  mailMessage.UserName = "ashaswinphone@gmail.com";
                  mailMessage.Password = "winphone8";
                  mailMessage.AccountType = MailMessage.AccountTypeEnum.Gmail;
                  mailMessage.From = "myapp@mycompany.com";
                  //set mail data

                  mailMessage.To = Email;
                  mailMessage.Subject = "Collage";
                  mailMessage.Body = "Collage:";

                  string image_name = "collage.jpg";
                  using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                  {
                      if (myIsolatedStorage.FileExists(image_name))
                      {
                          IsolatedStorageFileStream fStream = myIsolatedStorage.OpenFile(image_name, FileMode.Open, FileAccess.Read);
                          mailMessage.AddAttachment(fStream.Name);
                      }
                  }

                  mailMessage.Send();
            });
        }
        
        public ICommand SendEmail { get; set; }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                NotifyPropertyChanged("Email");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
