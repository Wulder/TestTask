using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;


public class DataManager : Singleton<DataManager>
{

    public event Action<JsonUsersData> OnDataFileDownloaded;
    public event Action OnUsersJsonCacheLoaded;
    public event Action<UserData[]> OnUsersDataReady;
    public event Action OnLoadFavorites;
    public event Action OnDownloadImages;



    [field: SerializeField] public string UsersJsonPath;
    [field: SerializeField] public string FavoritesFilePath;
    [field: SerializeField] public string UsersDataUrl;
    [field: SerializeField] public string AvatarsDataUrl;


    public JsonUsersData CachedUsersData { get; private set; }
    public FavoritesData Favorites;
    public UserData[] UsersData { get; private set; }
    public JsonAvatarsData AvatarsData { get; private set; }
    public TexturesCache[] TexturesCaches { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        Debug.Log(Application.persistentDataPath);
        if (!LoadUsersCache())
        {
            DownloadUsersData();
        }
        else
        {
            ConvertJsonToUsersData();
        }


        OnDownloadImages += () => { LoadTexturesInCache(); };

        LoadFavorites();
        LoadAvatarData();

       
    }

    

    private async void ConvertJsonToUsersData()
    {
        UsersData = await Task.Run(() =>
        {
            return ConvertUsersDataJson(CachedUsersData);
        });
        OnUsersDataReady?.Invoke(UsersData);
    }
    private async void DownloadUsersData()
    {
        Debug.Log("Start downloading file");
        string data = await GetUsersData.GetData(UsersDataUrl);
        Debug.Log("File donwloaded");
        var jsonData = JsonUtility.FromJson<JsonUsersData>(data);
        CachedUsersData = jsonData;
        OnDataFileDownloaded?.Invoke(jsonData);
        ConvertJsonToUsersData();
        SaveUsersJson(data);
    }
    public UserData[] ConvertUsersDataJson(JsonUsersData json)
    {
        UserData[] result = new UserData[json.data.Length];

        for (int i = 0; i < json.data.Length; i++)
        {
            UserDataJson userDataJson = json.data[i];
            result[i] = new UserData(userDataJson.id, userDataJson.first_name, userDataJson.last_name, userDataJson.email, userDataJson.gender, userDataJson.ip_address);
        }
        return result;
    }
    private bool LoadUsersCache()
    {
        string filePath = $"{Application.persistentDataPath}/{UsersJsonPath}";

        if (File.Exists(filePath))
        {
            try
            {
                string jsonText = File.ReadAllText(filePath);
                JsonUsersData jsonData = (JsonUsersData)JsonUtility.FromJson(jsonText, typeof(JsonUsersData));
                CachedUsersData = jsonData;
                OnUsersJsonCacheLoaded?.Invoke();
                Debug.Log($"UsersDataJson loaded from cache");
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }


        }

        Debug.Log("Can't load UsersData cache");
        return false;



    }
    private async void SaveUsersJson(JsonUsersData data)
    {
        Debug.Log("Start saving UsersDataJson");
        string filePath = @$"{Application.persistentDataPath}/{UsersJsonPath}";
        await Task.Run(() =>
        {
            var stringData = JsonUtility.ToJson(data);
            CreateAndWriteFile(filePath, stringData);
        });
        Debug.Log("UsersData json saved");
    }
    private async void SaveUsersJson(string json)
    {
        Debug.Log("Start saving UsersDataJson");
        string filePath = @$"{Application.persistentDataPath}/{UsersJsonPath}";
        await Task.Run(() =>
        {
            CreateAndWriteFile(filePath, json);
        });
        Debug.Log("UsersData json saved");
    }
    public async void LoadFavorites()
    {
        string filePath = $@"{Application.persistentDataPath}/{FavoritesFilePath}";
        await Task.Run(() =>
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();



                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    Favorites = (FavoritesData)bf.Deserialize(fs);
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Favorites = new FavoritesData();
                SaveFavorites();
            }

            Debug.Log($"Loaded favorites {Favorites}");

        });
        OnLoadFavorites?.Invoke();

    }
    public async void SaveFavorites()
    {
        if (Favorites == null) return;

        string filePath = $@"{Application.persistentDataPath}/{FavoritesFilePath}";
        await Task.Run(() =>
        {
            BinaryFormatter bf = new BinaryFormatter();



            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                bf.Serialize(fs, Favorites);
                fs.Close();
            }
        });
    }
    private void CreateAndWriteFile(string path, string data)
    {
        if (!File.Exists(path))
        {
            FileStream fs = File.Create(path);
            fs.Close();
        }

        File.WriteAllText(path, data);
    }

    #region Images
    private void LoadRandomImages()
    {


        string pic1 = "https://drive.google.com/uc?export=download&id=1SONTFoqZgbe7ZLhl58964kGJEQh7BNGF";
        string pic2 = "https://drive.google.com/uc?export=download&id=1S-PWkuo64z0ngTFmKuZg9gr8XosGwvUJ";
        string pic3 = "https://drive.google.com/uc?export=download&id=165nynqlvzA6NX2AzwJqQNJtkpy-GVjpb";
        string pic4 = "https://drive.google.com/uc?export=download&id=13x80bf0F-js00hh29mSo_wDzFC344uys";
        string pic5 = "https://drive.google.com/uc?export=download&id=1sxrGfCYQJzHT_6Nj9ujGVttEASevvHuz";


        StartCoroutine(DownloadImage(pic1, "1.png"));
        StartCoroutine(DownloadImage(pic2, "2.png"));
        StartCoroutine(DownloadImage(pic3, "3.png"));
        StartCoroutine(DownloadImage(pic4, "4.png"));
        StartCoroutine(DownloadImage(pic5, "5.png"));
    }

    int downloadedImagesCOunt = 0;
    IEnumerator DownloadImage(string MediaUrl, string fileName)
    {
        string filePath = $@"{Application.persistentDataPath}/{fileName}";
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            byte[] itemBGBytes = tex.EncodeToPNG();
            System.IO.File.WriteAllBytes(filePath, itemBGBytes);
        }

        downloadedImagesCOunt++;
        if(downloadedImagesCOunt >= 5)
            OnDownloadImages?.Invoke();

        yield return null;
    }

    private void LoadAvatarData()
    {
        if (!File.Exists(@$"{Application.persistentDataPath}/{AvatarsDataUrl}")) 
        {
            
            AvatarsData = new JsonAvatarsData(5);
            
                      
            AvatarsData.Data[0].Id = 3;
            AvatarsData.Data[0].FileName = "1.png";
                      
            AvatarsData.Data[1].Id = 2;
            AvatarsData.Data[1].FileName = "2.png";
                      
            AvatarsData.Data[2].Id = 4;
            AvatarsData.Data[2].FileName = "3.png";
                      
            AvatarsData.Data[3].Id = 6;
            AvatarsData.Data[3].FileName = "4.png";
                      
            AvatarsData.Data[4].Id = 8;
            AvatarsData.Data[4].FileName = "5.png";

            SaveAvatarData();
            LoadRandomImages();
        }
        else
        {
            string json = File.ReadAllText($@"{Application.persistentDataPath}/{AvatarsDataUrl}");
            AvatarsData = (JsonAvatarsData)JsonUtility.FromJson<JsonAvatarsData>(json);
            Debug.Log("AvatarsData loaded");
            OnDownloadImages?.Invoke();
        }
    }

    private void SaveAvatarData()
    {
        string path = $@"{Application.persistentDataPath}/{AvatarsDataUrl}";
        CreateAndWriteFile(path, JsonUtility.ToJson(AvatarsData));
    }

    private void LoadTexturesInCache()
    {
        TexturesCaches = new TexturesCache[5];

        for(int i = 0; i < TexturesCaches.Length;i++)
        {
            string filePath = $@"{Application.persistentDataPath}/{AvatarsData.Data[i].FileName}";
            Texture2D tex = new Texture2D(256, 256);
            var fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            TexturesCaches[i] = new TexturesCache(AvatarsData.Data[i].Id, tex);
        }
        Debug.Log($"Textures cache is ready");
    }
    #endregion

}

public struct TexturesCache
{
    public TexturesCache(int id, Texture2D tex)
    {
        Id = id;
        Texture = tex;
    }

    public int Id;
    public Texture2D Texture;
}
