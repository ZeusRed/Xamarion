﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MediaSample
{
    public partial class MediaPage : ContentPage
    {
        public MediaPage()
        {
            InitializeComponent();
            Func<object> func = () =>
            {
                var layout = new RelativeLayout();
                var image = new Image()
                {
                    Source = "add.png"
                };
                //image.Source = ImageSource.FromFile("add.png");

                Func<RelativeLayout, double> ImageHeight = (p) => image.Measure(layout.Width, layout.Height).Request.Height;
                Func<RelativeLayout, double> ImageWidth = (p) => image.Measure(layout.Width, layout.Height).Request.Width;

                layout.Children.Add(image,
                            Constraint.RelativeToParent(parent => parent.Width / 4 - ImageWidth(parent) / 4),
                            Constraint.RelativeToParent(parent => parent.Height / 4 - ImageHeight(parent) / 4)
                );
                return layout;
            };
           
            takePhoto.Clicked += async (sender, args) =>
            {

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    //Directory = "Test",
                    //SaveToAlbum = true,
                    //CompressionQuality = 75,
                    //CustomPhotoSize = 50,
                    //PhotoSize = PhotoSize.MaxWidthHeight,
                    //MaxWidthHeight = 2000,
                    //DefaultCamera = CameraDevice.Front,
                    OverlayViewProvider = func

                });

                if (file == null)
                    return;

                DisplayAlert("File Location", file.Path, "OK");

                image.Source = ImageSource.FromStream(() =>
          {
              var stream = file.GetStream();
              file.Dispose();
              return stream;
          });
            };

            pickPhoto.Clicked += async (sender, args) =>
  {
      if (!CrossMedia.Current.IsPickPhotoSupported)
      {
          DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
          return;
      }
      var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
      {
          PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

      });


      if (file == null)
          return;

      image.Source = ImageSource.FromStream(() =>
          {
              var stream = file.GetStream();
              file.Dispose();
              return stream;
          });
  };

            takeVideo.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
                {
                    DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                {
                    Name = "video.mp4",
                    Directory = "DefaultVideos",
                });

                if (file == null)
                    return;

                DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");

                file.Dispose();
            };

            pickVideo.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsPickVideoSupported)
                {
                    DisplayAlert("Videos Not Supported", ":( Permission not granted to videos.", "OK");
                    return;
                }
                var file = await CrossMedia.Current.PickVideoAsync();

                if (file == null)
                    return;

                DisplayAlert("Video Selected", "Location: " + file.Path, "OK");
                file.Dispose();
            };
        }
    }
}
