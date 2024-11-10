using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Architecture.API.Events;
using Newtonsoft.Json;

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
        public async Task<string> SendGetRequestAsync(string uri)
        {
            return await SendGetRequestAsync(uri, "");
        }

        /// <summary>
        /// send an HTTP get request to target uri with a token for user verification
        /// </summary>
        /// <param name="uri"> target uri </param>
        /// <param name="token"> empty for no token or string for authentication </param>
        /// <returns> response from server </returns>
        public async Task<string> SendGetRequestAsync(string uri, string token)
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

        /// <summary>
        /// send get request to uri on main thread and raises event with converted object from json
        /// </summary>
        /// <param name="uri"> target address </param>
        /// <param name="successEvent"> event data will passed to on succesfull request </param>
        /// <param name="failEvent"> event called if request failed getting or parsing response </param>
        /// <returns> noting, sends data back through event system </returns>
        public IEnumerator SendGetRequest<T>(string uri, string successEvent, string failEvent)
        {
            // Create a UnityWebRequest object for a GET request
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                // Check for network errors or HTTP errors
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                    EventDispatcher.Raise(failEvent);
                }
                else
                {
                    // Get the response text (assuming it's a JSON or text response)
                    string responseText = webRequest.downloadHandler.text;

                    try
                    {
                        T output = JsonConvert.DeserializeObject<T>(responseText);
                        EventDispatcher<T>.Raise(successEvent, output);
                    }
                    catch
                    {
                        Debug.LogError($"Could not deserilize get repsone to target {typeof(T).Name}, response: {responseText}");
                        EventDispatcher.Raise(failEvent);
                    }
                }
            }
        }

        /// <summary>
        /// send get request to uri on main thread and raises event with string from response
        /// </summary>
        /// <param name="uri"> target address </param>
        /// <param name="successEvent"> event data will passed to on succesfull request </param>
        /// <param name="failEvent"> event called if request failed getting or parsing response </param>
        /// <returns> noting, sends data back through event system </returns>
        public IEnumerator SendGetRequest(string uri, string successEvent, string failEvent)
        {
            // Create a UnityWebRequest object for a GET request
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                // Check for network errors or HTTP errors
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                    EventDispatcher.Raise(failEvent);
                }
                else
                {
                    // Get the response text (assuming it's a JSON or text response)
                    string responseText = webRequest.downloadHandler.text;
                    EventDispatcher<string>.Raise(successEvent, responseText);
                }
            }
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

        /// <summary>
        /// send post request to uri on main thread and raises event with converted object from json
        /// </summary>
        /// <param name="uri"> target address </param>
        /// <param name="jsonData"> data sent in the body of request of type json </param>
        /// <param name="successEvent"> event data will passed to on succesfull request </param>
        /// <param name="failEvent"> event called if request failed getting or parsing response </param>
        /// <returns> noting, sends data back through event system </returns>
        public IEnumerator SendPostRequest<T>(string uri, string jsonData, string successEvent, string failEvent)
        {
            // Create a UnityWebRequest object for a GET request
            using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
            {
                // Set up the body of the request
                byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
                webRequest.downloadHandler = new DownloadHandlerBuffer();

                // Set the request headers
                webRequest.SetRequestHeader("Content-Type", "application/json");

                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                // Check for network errors or HTTP errors
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                    EventDispatcher.Raise(failEvent);
                }
                else
                {
                    // Get the response text (assuming it's a JSON or text response)
                    string responseText = webRequest.downloadHandler.text;

                    try
                    {
                        T output = JsonConvert.DeserializeObject<T>(responseText);
                        EventDispatcher<T>.Raise(successEvent, output);
                    }
                    catch
                    {
                        Debug.LogError($"Could not deserilize get repsone to target {typeof(T).Name}, response: {responseText}");
                        EventDispatcher.Raise(failEvent);
                    }
                }
            }
        }

        /// <summary>
        /// send post request to uri on main thread and raises event with string from response
        /// </summary>
        /// <param name="uri"> target address </param>
        /// <param name="jsonData"> data sent in the body of request of type json </param>
        /// <param name="successEvent"> event data will passed to on succesfull request </param>
        /// <param name="failEvent"> event called if request failed getting or parsing response </param>
        /// <returns> noting, sends data back through event system </returns>
        public IEnumerator SendPostRequest(string uri, string jsonData, string successEvent, string failEvent)
        {
            // Create a UnityWebRequest object for a GET request
            using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
            {
                // Set up the body of the request
                byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
                webRequest.downloadHandler = new DownloadHandlerBuffer();

                // Set the request headers
                webRequest.SetRequestHeader("Content-Type", "application/json");

                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                // Check for network errors or HTTP errors
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                    EventDispatcher.Raise(failEvent);
                }
                else
                {
                    // Get the response text (assuming it's a JSON or text response)
                    string responseText = webRequest.downloadHandler.text;
                    EventDispatcher<string>.Raise(successEvent, responseText);
                }
            }
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

        /// <summary>
        /// send post request to uri on main thread and raises event with converted object from json
        /// </summary>
        /// <param name="uri"> target address </param>
        /// <param name="jsonData"> data sent in the body of request of type json </param>
        /// <param name="successEvent"> event data will passed to on succesfull request </param>
        /// <param name="failEvent"> event called if request failed getting or parsing response </param>
        /// <returns> noting, sends data back through event system </returns>
        public IEnumerator SendPutRequest<T>(string uri, string jsonData, string successEvent, string failEvent)
        {
            // Create a UnityWebRequest object for a GET request
            using (UnityWebRequest webRequest = new UnityWebRequest(uri, "PUT"))
            {
                // Set up the body of the request
                byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
                webRequest.downloadHandler = new DownloadHandlerBuffer();

                // Set the request headers
                webRequest.SetRequestHeader("Content-Type", "application/json");

                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                // Check for network errors or HTTP errors
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                    EventDispatcher.Raise(failEvent);
                }
                else
                {
                    // Get the response text (assuming it's a JSON or text response)
                    string responseText = webRequest.downloadHandler.text;

                    try
                    {
                        T output = JsonConvert.DeserializeObject<T>(responseText);
                        EventDispatcher<T>.Raise(successEvent, output);
                    }
                    catch
                    {
                        Debug.LogError($"Could not deserilize get repsone to target {typeof(T).Name}, response: {responseText}");
                        EventDispatcher.Raise(failEvent);
                    }
                }
            }
        }

        /// <summary>
        /// send post request to uri on main thread and raises event with string from response
        /// </summary>
        /// <param name="uri"> target address </param>
        /// <param name="jsonData"> data sent in the body of request of type json </param>
        /// <param name="successEvent"> event data will passed to on succesfull request </param>
        /// <param name="failEvent"> event called if request failed getting or parsing response </param>
        /// <returns> noting, sends data back through event system </returns>
        public IEnumerator SendPutRequest(string uri, string jsonData, string successEvent, string failEvent)
        {
            // Create a UnityWebRequest object for a GET request
            using (UnityWebRequest webRequest = new UnityWebRequest(uri, "PUT"))
            {
                // Set up the body of the request
                byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
                webRequest.downloadHandler = new DownloadHandlerBuffer();

                // Set the request headers
                webRequest.SetRequestHeader("Content-Type", "application/json");

                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                // Check for network errors or HTTP errors
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                    EventDispatcher.Raise(failEvent);
                }
                else
                {
                    // Get the response text (assuming it's a JSON or text response)
                    string responseText = webRequest.downloadHandler.text;
                    EventDispatcher<string>.Raise(successEvent, responseText);
                }
            }
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

        /// <summary>
        /// send delete request to uri on main thread and raises event with converted object from json
        /// </summary>
        /// <param name="uri"> target address </param>
        /// <param name="successEvent"> event data will passed to on succesfull request </param>
        /// <param name="failEvent"> event called if request failed getting or parsing response </param>
        /// <returns> noting, sends data back through event system </returns>
        public IEnumerator SendDeleteRequest<T>(string uri, string successEvent, string failEvent)
        {
            // Create a UnityWebRequest object for a GET request
            using (UnityWebRequest webRequest = UnityWebRequest.Delete(uri))
            {
                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                // Check for network errors or HTTP errors
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                    EventDispatcher.Raise(failEvent);
                }
                else
                {
                    // Get the response text (assuming it's a JSON or text response)
                    string responseText = webRequest.downloadHandler.text;

                    try
                    {
                        T output = JsonConvert.DeserializeObject<T>(responseText);
                        EventDispatcher<T>.Raise(successEvent, output);
                    }
                    catch
                    {
                        Debug.LogError($"Could not deserilize get repsone to target {typeof(T).Name}, response: {responseText}");
                        EventDispatcher.Raise(failEvent);
                    }
                }
            }
        }

        /// <summary>
        /// send delete request to uri on main thread and raises event with string from response
        /// </summary>
        /// <param name="uri"> target address </param>
        /// <param name="successEvent"> event data will passed to on succesfull request </param>
        /// <param name="failEvent"> event called if request failed getting or parsing response </param>
        /// <returns> noting, sends data back through event system </returns>
        public IEnumerator SendDeleteRequest(string uri, string successEvent, string failEvent)
        {
            // Create a UnityWebRequest object for a GET request
            using (UnityWebRequest webRequest = UnityWebRequest.Delete(uri))
            {
                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                // Check for network errors or HTTP errors
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                    EventDispatcher.Raise(failEvent);
                }
                else
                {
                    // Get the response text (assuming it's a JSON or text response)
                    string responseText = webRequest.downloadHandler.text;
                    EventDispatcher<string>.Raise(successEvent, responseText);
                }
            }
        }

        #endregion
    }
}