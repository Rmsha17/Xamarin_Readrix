using Newtonsoft.Json;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Readrix.Services
{
    class Service
    {
        public async Task<bool> RegistrationReaderAsync(string name, String email , string password , string phone ,string address , string image)
        {
            bool Response = false;
            await Task.Run(() => {
                
                var client = new HttpClient();
                var reader = new Reader
                {
                    READER_EMAIL = email,
                    READER_NAME = name,
                    READER_CONTACT = phone,
                    READER_PASSWORD = password,
                    READER_ADDRESS = address,
                    READER_IMAGE = image

                };
                var json = JsonConvert.SerializeObject(reader);
                HttpContent httpcontent = new StringContent(json);
                httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                Task<HttpResponseMessage> response = client.PostAsync("https://readrixapi.azurewebsites.net/" + "api/readers/postreader", httpcontent);
                var mystring = response.GetAwaiter().GetResult();
                if (response.Result.IsSuccessStatusCode)
                {
                    Response = true;
                }
            });
            return Response;
        }

        public async Task<List<Category>> Getcategory()
        {
            var client = new HttpClient();
            var uri = "https://readrixapi.azurewebsites.net/api/Category/getcategory";
            var result = await client.GetStringAsync(uri);
            List<Category> list = JsonConvert.DeserializeObject<List<Category>>(result);
            return list;
        }
    }
}
