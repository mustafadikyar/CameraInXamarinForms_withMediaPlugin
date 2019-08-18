using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace CameraApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
       
        }

        private async void TakePhoto_OnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Error", "No camera avaliable.", "OK"); return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "SamplePhotos",
                    Name = $"Img-{DateTime.Now}.jpg"
                });

            if (file == null)
                return;

            PathLabel.Text = file.AlbumPath;

            MainImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        private async void PickPhoto_OnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Ooops", "Pick photo is not supported", "OK"); return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null) return;

            PathLabel.Text = $"Photo path {file.Path}";

            MainImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        private async void TakeVideo_OnClicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Error", "No camera avaliable.", "OK"); return;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
            {
                Quality = Plugin.Media.Abstractions.VideoQuality.High,
                Directory = "SampleVideo",
                Name = $"Vid-{DateTime.Now}.mp4"
            });

            if (file == null)
            {
                return;
            }

            file.Dispose();
        }

        private async void PickVideo_OnClicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickVideoSupported)
            {
                await DisplayAlert("Ooops", "Pick video is not supported", "OK"); return;
            }

            var file = await CrossMedia.Current.PickVideoAsync();

            if (file == null)
            {
                return;
            }

            file.Dispose();
        }
    }
}
