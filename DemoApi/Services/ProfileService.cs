using System;
using System.Net.Http;
using System.Threading.Tasks;
using Messages;
using Newtonsoft.Json;

namespace DemoApi.Services
{
    public class ProfileService
    {
        //private const string ProfileServiceHost = "localhost";
        private const string ProfileServiceHost = "nodejsapi";
        private const string ProfileServicePort = "5000";

        public async Task<UserProfile> GetExternalProfile(int profileId)
        {
            var externalApiUri = new Uri($"http://{ProfileServiceHost}:{ProfileServicePort}/api/profile/{profileId}");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            var profileApiResponse = await client.GetStringAsync(externalApiUri);

            if (!string.IsNullOrEmpty(profileApiResponse))
            {
                try
                {
                    var profile = JsonConvert.DeserializeObject<UserProfile>(profileApiResponse);
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
