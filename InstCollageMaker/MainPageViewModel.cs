using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace InstCollageMaker
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        public MainPageViewModel()
        {
            GetCollage = new DelegateCommand(()=>
            {
                LoadData();
            });
            IsUserFound = true;
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

        private string imgUrl1;
        public string ImgURL1 { 
            get
            {
                return imgUrl1;
            }
            set
            {
                imgUrl1 = value;
                NotifyPropertyChanged("ImgURL1");
            }
        }

        private string imgUrl2;
        public string ImgURL2
        {
            get
            {
                return imgUrl2;
            }
            set
            {
                imgUrl2 = value;
                NotifyPropertyChanged("ImgURL2");
            }
        }

        private string imgUrl3;
        public string ImgURL3
        {
            get
            {
                return imgUrl3;
            }
            set
            {
                imgUrl3 = value;
                NotifyPropertyChanged("ImgURL3");
            }
        }

        private string imgUrl4;
        public string ImgURL4
        {
            get
            {
                return imgUrl4;
            }
            set
            {
                imgUrl4 = value;
                NotifyPropertyChanged("ImgURL4");
            }
        }

        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                NotifyPropertyChanged("UserName");
            }
        }

        public ICommand GetCollage { get; set; }

        private bool isUserFound;
        public bool IsUserFound
        {
            get
            {
                return isUserFound;
            }
            set
            {
                isUserFound = value;
                NotifyPropertyChanged("IsUserFound");
            }
        }

        public async void LoadData()
        {
            ImgURL1 = null;
            ImgURL2 = null;
            ImgURL3 = null;
            ImgURL4 = null;
            IsUserFound = true;

            InstagramAPI apiClient = new InstagramAPI();
            string user_id = await apiClient.GetUserID(UserName);
            if (user_id == null)
            {
                IsUserFound = false;
                return;
            }
            ObservableCollection<PhotoModel> photos = await apiClient.GetPhotos(user_id);
            ObservableCollection<PhotoModel> photosSort = new ObservableCollection<PhotoModel>(
            photos.OrderByDescending(PhotoModel => PhotoModel.likes));
            if (photosSort.Count>0)
                ImgURL1 = photosSort[0].imgURL;
            if (photosSort.Count > 1)
                ImgURL2 = photosSort[1].imgURL;
            if (photosSort.Count > 2)
                ImgURL3 = photosSort[2].imgURL;
            if (photosSort.Count > 3)
                ImgURL4 = photosSort[3].imgURL;

        }

    }
}
