using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteShutdownPC.Helper
{
    public class FirebaseHelper
    {
        const string auth = "~Firebase Secret~";
        public static FirebaseClient firebase = new FirebaseClient("https://~Your Firebase Url~.firebaseio.com/",
            new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(auth)
            });
        public static string mainChild = "Main";
        public FirebaseHelper()
        {

        }

        //Get data from firebase
        public async Task<FBDatas> GetData(string name)
        {
            try
            {
                var AllData = await firebase
                 .Child(mainChild)
                 .OnceAsync<FBDatas>();

                var list = AllData.Select(item => new FBDatas
                {
                    Name = item.Object.Name,
                    Shutdown = item.Object.Shutdown,
                    Timer = item.Object.Timer,
                    Cancel = item.Object.Cancel,
                }).ToList();

                return new FBDatas
                {
                    Name = list[0].Name,
                    Timer = list[0].Timer,
                    Shutdown = list[0].Shutdown,
                    Cancel = list[0].Cancel,
                };
            }
            catch
            {
                return null;
            }
        }

        //Update data on firebase
        public async void UpdateData(string name, bool shutdown, string timer, bool cancel)
        {
            try
            {
                await firebase
                    .Child(mainChild)
                    .Child(name)
                    .PutAsync(new FBDatas() { Name = name, Shutdown = shutdown, Timer = timer, Cancel = cancel });
            }
            catch (Exception) { }
        }

        //Add new data to firebase
        public async Task AddData(string name)
        {
            await firebase
                .Child(mainChild)
                .Child(name)
                .PutAsync(new FBDatas() { Name = name, Shutdown = false, Timer = "0", Cancel = false });
        }
    }

    public class FBDatas //Firebase Datas
    {
        public string Name { get; set; }
        public bool Shutdown { get; set; }
        public string Timer { get; set; }
        public bool Cancel { get; set; }
    }
}
