using System;


[Serializable]
public class JsonAvatarsData
{
    public JsonAvatarsData() { }
    public JsonAvatarsData(int size)
    {
        Data = new AvatarData[size];
        for(int i = 0; i < size; i++)
        {
            Data[i] = new AvatarData();
        }
    }

    public AvatarData[] Data;
}
