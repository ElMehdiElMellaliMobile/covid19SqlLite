﻿using covid19.Models;
using covid19.TablesSql;
using covid19.Tools;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using Plugin.Geolocator;
using Plugin.Permissions.Abstractions;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace covid19
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
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Stopwatch sw = new Stopwatch();

            sw.Start();
            var listItems = await App.Database.GetItemsAsync();
            sw.Stop();

            Console.WriteLine($"Count ={listItems.Count()}");
            Console.WriteLine($"Time Elapsed={sw.Elapsed}");

            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
           {
               var isSaved = Task.Run(async () => await saveDataAsync());

               CrossToastPopUp.Current.ShowToastMessage("Message");

               return isSaved.Result; // To keep repeating or false to stop.
           });


        }
        public async Task<bool> saveDataAsync()
        {
            var hasPermission = await Utils.CheckPermissions(Permission.Location);
            if (!hasPermission)
                return false;


            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 100;
            var timeout = new TimeSpan(0, 0, 1);
            var position = await locator.GetPositionAsync(timeout, null, true);

            double speedInMetersPerSecond = position.Speed;
            if (speedInMetersPerSecond <= 0)
            {
                return false;
            }
            //var fakedat = new FakeData()
            //{
            //    date = DateTime.Now,
            //    latitude = "33.5722086" ,
            //    longitude = "-7.7270774",
            //    deviceId = CrossDeviceInfo.Current.Id
            //};

            //var result = JsonConvert.SerializeObject(fakedat);

            //var TableData = new TableResultatModel();

            //TableData.Data = result;

            //var test = await App.Database.SaveItemAsync(TableData);

            return true;
        }
    }
}
