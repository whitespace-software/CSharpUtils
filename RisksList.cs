using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
 
namespace callapi
{
    public class OIDCResult
    {
        public string id_token;
        public string session_id;
        public string name;
    }
 
    public class RisksResult
    {
        public string id;
        public string umr;
        public string Status;
        public string InsuredName;
    }
 
    class Program
    {
        static HttpClient client = new HttpClient();
 
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }
 
        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://dev.whitespace.co.uk");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
 
            string json = String.Empty, req = String.Empty;
 
            try
            {
                // set OIDC to be your renewable token
                const string OIDC = "OAQABAAAAAABeAFzDwllzTYGDLh_qYbH8o5QQPE-BZLeu2n0iZz3jDc5e7FCqr3fWgEK4g9jwpCk3o1M2cutJEYURrkXNzzFi8hlhj5W94r0tUQleS-L7bU1907-6AqJzxCt6-U_XdGuAG3WJCyZlvJ_VWF6StygCjYgzt2HaH4r37eHWRGWcPUpTSFd3WNTDJPAllG1vjYx5JtVY8XQDPWCZjJggx0WWlMHMPxMhJ6_JI2oWHYCitjN4sZYEftaub6EkVfksH7UGRKeA2yuFJv9hMBiyb6ozKIm82AZrZngMWTFujaqulZkmIZmv_2S9hc90_8kUS0d6Hkt2MLPNubHVkkqYv333eMyqIoj7KhX8R_RISpqiWIBpQISMvf0_2B95sSFVQGL4oxXGZojvtQhj4898l5dlX95YIoYtiqwQI7jpIUzGrf5mfOT-SUO-O3MNddOuX_tWWUFH9KW76S6W8nI7ig_AX5abxx1XwwxqCNYMw-HE4UW1-MQAoiiRHUkggteFAvJYIp0JeUxxsoxi5f76ZBWDoztpCAWze-yBHlegj4gvRUf4YjOdh1Yh9NHO3hXdW5d3vEEaq_qB4oR5mrHHQeXGZFy0LIyrI5U-YLPaZcy5y2kQS0Gn4LWrRR-xj_NMiRBRhVLKx8vdwHw_Z33IHBD9sKx4I1oqJBy5Tp5k2GH6BkCGHvcBUKaCOOtKwHad8Mbklj0plWxsEoCp49JeoZLNSTZJhIO1mHMVDqfCaGyCLZjm82Ih3H84wUkQypD21LYmzzUsc8mtSa61wza92eJ0peaiLpg03M20GW47hTL7rQjr1NmvR97jDJvL0lMkdsWUzSkDF8qH7zScVdws0vFeyaV5H8w1Wlrq36x6EB10viq86nX77et-fbrKF3jgmGcgAA";
                req = "/sync/sandboxbucket/_oidc_refresh?refresh_token=" + OIDC;
                HttpResponseMessage response;
               
 
                response = await client.GetAsync(req);
                json = await response.Content.ReadAsStringAsync();
 
                OIDCResult oidc = JsonConvert.DeserializeObject<OIDCResult>(json);
 
                req = "/api/risks";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", oidc.id_token );
 
                response = await client.GetAsync(req);
                json = await response.Content.ReadAsStringAsync();
 
                List<RisksResult> list = JsonConvert.DeserializeObject<List<RisksResult>>(json);
                Console.WriteLine(list.Count + " risks returned");
                foreach( var item in list )
                {
                    Console.WriteLine("{0} {1} {2} {3}", item.InsuredName, item.Status, item.umr, item.id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(req);
                Console.WriteLine(json);
            }
        }
    }
}