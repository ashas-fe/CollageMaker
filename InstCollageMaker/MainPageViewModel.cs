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
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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

        private WriteableBitmap img;
        public WriteableBitmap Img
        {
            get
            {
                return img;
            }
            set
            {
                img = value;
                NotifyPropertyChanged("Img");
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

            string ImgURL1 = null;
            string ImgURL2 = null;
            string ImgURL3 = null;
            string ImgURL4 = null;

            if (photosSort.Count>0)
                ImgURL1 = photosSort[0].imgURL;
            if (photosSort.Count > 1)
                ImgURL2 = photosSort[1].imgURL;
            else
                ImgURL2 = ImgURL1;
            if (photosSort.Count > 2)
                ImgURL3 = photosSort[2].imgURL;
            else
                ImgURL3 = ImgURL2;
            if (photosSort.Count > 3)
                ImgURL4 = photosSort[3].imgURL;
            else
                ImgURL4 = ImgURL3;

            string[] images = new string[] { ImgURL1, ImgURL2, ImgURL3, ImgURL4 };
            
            CollageMaker cm = new CollageMaker();
            Img = await cm.MakeCollage(images);

        }

    }
}
