using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DemoApi.Domain;
using Newtonsoft.Json;

namespace DemoApi.Services
{
    public class ProfileService
    {
        //private const string ProfileServiceHost = "localhost";
        private const string ProfileServiceHost = "profileapi";
        private const string ProfileServicePort = "5000";

        public async Task<Profile> GetExternalProfile(int profileId)
        {
            Profile profile = null;
            var externalApiUri = new Uri($"http://{ProfileServiceHost}:{ProfileServicePort}/api/profile/{profileId}");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            var profileApiResponse = await client.GetStringAsync(externalApiUri);

            if (!string.IsNullOrEmpty(profileApiResponse))
            {
                try
                {
                    profile = JsonConvert.DeserializeObject<Profile>(profileApiResponse);
                    return profile;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            throw new Exception("Unable to get response from external profile api.");
        }
    }
}
