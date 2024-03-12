using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData 
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Gender { get; private set; }
    public string IpAddress { get; private set; }
    public UserData(int id, string fName, string lName, string email, string gender, string address)
    {
        Id = id;
        FirstName = fName;
        LastName = lName;
        Email = email;
        Gender = gender;
        IpAddress = address;
    }
}

[Serializable]
public class UserDataJson
{
    public int id;
    public string first_name;
    public string last_name;
    public string email;
    public string gender;
    public string ip_address;
}

[Serializable]
public class JsonUsersData
{
    public UserDataJson[] data;
}
