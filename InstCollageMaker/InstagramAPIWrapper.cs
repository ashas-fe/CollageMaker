using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace InstCollageMaker
{
    public class InstagramAPI
    {
        private readonly string apiBaseUrl = "https://api.instagram.com/v1";

        private HttpClient _httpClient;

        public InstagramAPI()
        {            
            _httpClient = new HttpClient();
        }

        private async Task<T> SendAsync<T>(HttpRequestMessage request) where T : class
        {

            HttpResponseMessage response;
            response = await _httpClient.SendAsync(request);

            //TODO: Error Handling
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(responseBody);
            }

            return null;
        }

        public async Task<string> GetUserID(string user_name)
        {
            //https://api.instagram.com/v1/users/search?q=ashas.fe&count=1
            var request = new HttpRequestMessage(HttpMethod.Get, apiBaseUrl + "/users/search?q=" + user_name + "&count=1&client_id=b88d662bf62c4e3f99bfadd781ac6ff1");//&access_token=" + apiAccessToken);
            var searchResponse = await SendAsync<InstagramResponse>(request);
            if (searchResponse.data.Count == 0)
                return null;
            var result = searchResponse.data[0].id;
            return result;
        }

        public async Task<ObservableCollection<PhotoModel>> GetPhotos(string user_id)
        {
            //https://api.instagram.com/v1/users/3082112/media/recent
            var request = new HttpRequestMessage(HttpMethod.Get, apiBaseUrl + "/users/" + user_id + "/media/recent?client_id=b88d662bf62c4e3f99bfadd781ac6ff1");//?access_token=" + apiAccessToken);
            var photosResponse = await SendAsync<PhotosResponse>(request);
            if (photosResponse.data.Count == 0)
                return null;
            var result = new ObservableCollection<PhotoModel>();
            foreach (var u in photosResponse.data)
            {
                if (string.Compare(u.type, "image") == 0)
                {
                    result.Add(new PhotoModel() { likes = u.likes.count, imgURL = u.images.standard_resolution.url });
                }
            }
            return result;
        }
    }

    public class InstagramUser
    {
        public string username { get; set; }
        public string bio { get; set; }
        public string website { get; set; }
        public string profile_picture { get; set; }
        public string full_name { get; set; }
        public string id { get; set; }
    }

    public class InstagramResponse
    {
        public List<InstagramUser> data { get; set; }
    }

    public class UsersMedia
    {
        public string type { get; set; }
        public string link { get; set; }

        [JsonProperty("likes")]
        public Likes likes { get; set; }
        public Images images { get; set; }
    }

    public class PhotosResponse
    {
        public List<UsersMedia> data { get; set; }
    }

    public class Likes
    {
        public int count { get; set; }
    }
    
    public class Images
    {
        public StandartResolution standard_resolution { get; set; }
    }

    public class StandartResolution
    {
        public string url { get; set; }
    }
}