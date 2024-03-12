using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GetUsersData 
{


    
    public static async Task<string> GetData(string url)
    {
        string result = "";

        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        result = await response.Content.ReadAsStringAsync();
        return result;
    }
  
}
