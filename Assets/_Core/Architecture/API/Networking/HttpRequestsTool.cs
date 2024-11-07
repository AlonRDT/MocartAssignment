using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace Architecture.API.Networking
{
    public class HttpRequestsTool
    {
        #region Get

        /// <summary>
        /// send an HTTP get request to target uri
        /// </summary>
        /// <param name="uri"> target uri </param>
        /// <returns> response from server </returns>
        public async Task<string> SendGetRequest(string uri)
        {
            return await SendGetRequest(uri, "");
        }

        /// <summary>
        /// send an HTTP get request to target uri with a token for user verification
        /// </summary>
        /// <param name="uri"> target uri </param>
        /// <param name="token"> empty for no token or string for authentication </param>
        /// <returns> response from server </returns>
        public async Task<string> SendGetRequest(string uri, string token)
        {
            string output = "null";

            using (var client = new HttpClient())
            {
                if (token != "")
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    output = responseBody;
                }
                catch (HttpRequestException e)
                {
                    Debug.LogError($"Request error: {e.Message}, uri: {uri}\ntoken: {token}");
                }
            }

            return output;
        }

        #endregion

        #region Post

        /// <summary>
        /// send an HTTP post request to target uri
        /// </summary>
        /// <param name="uri"> target uri </param>
        /// <param name="data"> content of request body </param>
        /// <returns> response from server </returns>
        public async Task<string> SendPostRequest(string uri, string data)
        {
            return await SendPostRequest(uri, data, "");
        }

        /// <summary>
        /// send an HTTP post request to target uri with a token for user verification
        /// </summary>
        /// <param name="uri"> target uri </param>
        /// <param name="data"> content of request body </param>
        /// <param name="token"> empty for no token or string for authentication </param>
        /// <returns> response from server </returns>
        public async Task<string> SendPostRequest(string uri, string data, string token)
        {
            string output = "null";

            using (var client = new HttpClient())
            {
                if (token != "")
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(uri, content);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    output = responseBody;
                }
                catch (HttpRequestException e)
                {
                    Debug.LogError($"Request error: {e.Message}, url: {uri}, data: {data}");
                }
            }

            return output;
        }

        #endregion

        #region Put

        /// <summary>
        /// send an HTTP put request to target uri
        /// </summary>
        /// <param name="uri"> target uri </param>
        /// <param name="data"> content of request body </param>
        /// <returns> response from server </returns>
        public async Task<string> SendPutRequest(string uri, string data)
        {
            return await SendPutRequest(uri, data, "");
        }

        /// <summary>
        /// send an HTTP put request to target uri with a token for user verification
        /// </summary>
        /// <param name="uri"> target uri </param>
        /// <param name="data"> content of request body </param>
        /// <param name="token"> empty for no token or string for authentication </param>
        /// <returns> response from server </returns>
        public async Task<string> SendPutRequest(string uri, string data, string token)
        {
            string output = "null";

            using (var client = new HttpClient())
            {
                if (token != "")
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(uri, content);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    output = responseBody;
                }
                catch (HttpRequestException e)
                {
                    Debug.LogError($"Request error: {e.Message}");
                }
            }

            return output;
        }

        #endregion

        #region Delete

        /// <summary>
        /// send an HTTP delete request to target uri
        /// </summary>
        /// <param name="uri"> target uri </param>
        /// <returns> response from server </returns>
        public async Task<string> SendDeleteRequest(string uri)
        {
            return await SendDeleteRequest(uri, "");
        }

        /// <summary>
        /// send an HTTP remove request to target uri with a token for user verification
        /// </summary>
        /// <param name="uri"> target uri </param>
        /// <param name="token"> empty for no token or string for authentication </param>
        /// <returns> response from server </returns>
        public async Task<string> SendDeleteRequest(string uri, string token)
        {
            string output = "null";

            using (var client = new HttpClient())
            {
                if (token != "")
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.DeleteAsync(uri);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    output = responseBody;
                }
                catch (HttpRequestException e)
                {
                    Debug.LogError($"Request error: {e.Message}");
                }
            }

            return output;
        }

        #endregion
    }
}