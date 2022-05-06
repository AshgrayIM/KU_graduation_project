using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Database;
using System;

public class FirebaseManager : MonoBehaviour
{
    public string DBurl = "https://kuproject-e0a48-default-rtdb.firebaseio.com/";
    DatabaseReference reference;

    void Start(){
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(DBurl);
    }
}
