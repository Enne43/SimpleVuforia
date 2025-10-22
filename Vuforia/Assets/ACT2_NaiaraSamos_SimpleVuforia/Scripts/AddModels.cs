using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
//using GLTFast;

public class AddModels : MonoBehaviour
{
    #region SINGLETON
    public static AddModels instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else { 
            Destroy(gameObject);
        }
    }
    #endregion


    public List<GameObject> models = new();

    // /storage/emulated/<userid>/Android/data/<packagename>/files

   
}
