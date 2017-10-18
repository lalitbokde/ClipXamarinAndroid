using System;
using Newtonsoft.Json;
using RestSharp;
using WeClip.Core.Common;
using System.Threading.Tasks;
using Android.Util;
using System.Collections.Generic;
using RestSharp.Authenticators;

namespace WeClip.Droid.Helper
{
    public static class RestSharpCall
    {
        public static T Get<T>(string url) where T : new()
        {
            var client = new RestClient(GlobalClass.BaseUrl + url);
            var request = new RestRequest(Method.GET);
            var returnval = new T();
            IRestResponse<T> response;
            if (!string.IsNullOrEmpty(GlobalClass.AccessToken))
            {
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", GlobalClass.AccessToken));
            }
            //val = GlobalClass.AccessToken;
            //request.AddParameter("Authorization", string.Format("Bearer " + GlobalClass.AccessToken), ParameterType.HttpHeader);
            try
            {
                response = client.Execute<T>(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    returnval = JsonConvert.DeserializeObject<T>(response.Content);
                    return returnval;
                }
                else
                {
                    Log.Debug("RestSharpCallGet", response.Content + " status-Code " + response.StatusCode + " Token " + GlobalClass.AccessToken);
                }
                return returnval;
            }
            catch (Exception ex)
            {
                Log.Debug("Error", ex.StackTrace);
                return returnval;
            }
        }

        public static List<T> GetList<T>(string url) where T : new()
        {
            var client = new RestClient(GlobalClass.BaseUrl + url);

            var request = new RestRequest(Method.GET);
            var returnList = new List<T>();
            string access = GlobalClass.AccessToken;
            IRestResponse<T> response;
            if (!string.IsNullOrEmpty(GlobalClass.AccessToken))
            {
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", GlobalClass.AccessToken));
            }
            try
            {
                response = client.Execute<T>(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    returnList = JsonConvert.DeserializeObject<List<T>>(response.Content);
                    return returnList;
                }
                else
                {
                    Log.Debug("RestSharpCallGetList", response.Content + " status-Code " + response.StatusCode + " Token " + GlobalClass.AccessToken);
                    return returnList;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Error", ex.StackTrace);
                return returnList;
            }
        }

        public static T Post<T>(object objectToPost, string url) where T : new()
        {
            var client = new RestClient(GlobalClass.BaseUrl + url);
            var request = new RestRequest(Method.POST);

            if (!string.IsNullOrEmpty(GlobalClass.AccessToken))
            {
                client.AddDefaultHeader("Authorization", "Bearer " + GlobalClass.AccessToken);
            }

            if (objectToPost != null)
            {
                var json = JsonConvert.SerializeObject(objectToPost);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
            }
            var response = client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
        }

        public static Token Login<Token>(string username, string password) where Token : new()
        {
            var client = new RestClient(GlobalClass.BaseUrl + "Account/LoginNew");
            client.Authenticator = new SimpleAuthenticator("username", username, "password", password);
            var request = new RestRequest(Method.POST);
            Token val = new Token();

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                val = JsonConvert.DeserializeObject<Token>(response.Content);
                return val;
            }
            else
            {
                Log.Debug("RestSharpCallLogin", response.Content + " status-Code " + response.StatusCode + " Token " + GlobalClass.AccessToken);
                return val;
            }
        }

        public static Task<T> PostAsync<T>(string url) where T : new()
        {
            var client = new RestClient(GlobalClass.BaseUrl + url);
            var taskCompletionSource = new TaskCompletionSource<T>();
            var request = new RestRequest(Method.POST);
            client.ExecuteAsync<T>(request, (response) => taskCompletionSource.SetResult(response.Data));
            return taskCompletionSource.Task;
        }

        public static T Put<T>(object objectToPost, string url) where T : new()
        {

            var client = new RestClient(GlobalClass.BaseUrl + url);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            string token = GlobalClass.AccessToken;
            if (!string.IsNullOrEmpty(GlobalClass.AccessToken))
            {
                client.AddDefaultHeader("Authorization", "Bearer " + GlobalClass.AccessToken);
            }

            if (objectToPost != null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddObject(objectToPost);
            }
            var json = JsonConvert.SerializeObject(objectToPost);
            var response = client.Execute<T>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            else
            {
                return default(T);
            }
        }

        public static T PostFile<T>(string filepath, string url) where T : new()
        {
            var client = new RestClient(GlobalClass.BaseUrl + url);
            var request = new RestRequest(Method.POST);
            if (!string.IsNullOrEmpty(GlobalClass.AccessToken))
            {
                client.AddDefaultHeader("Authorization", "Bearer " + GlobalClass.AccessToken);
            }
            request.AddFile("file", filepath, null);
            var response = client.Execute<JsonResult>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            else
            {
                return default(T);
            }
        }
    }
}