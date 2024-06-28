using System;
using System.Collections.Generic;
using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Material.html
// https://docs.unity3d.com/ScriptReference/Renderer-material.html

public class MaterialMapper : MonoBehaviour
{
    [SerializeField] private ScriptableObject materialDataStore;
    [SerializeField] private string dataStoreField;
    private Material scriptableObjectMaterial;
    private Material objectMaterial;
    private DataManager dataManager;

    void Start()
    {
        dataManager = DataManager.Instance;

        objectMaterial = GetComponent<Renderer>().material;
        
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Got Material" + objectMaterial.name + "on" + gameObject.name);
            Debug.Log("Material Mapper Start Complete");
        }
        // Initially getting the Material from the field and Null checking
        CollectScriptableObjectMaterial();
    }

    // https://discussions.unity.com/t/help-with-gettype-getfield-getvalue/206306
    void Update()
    {
        // Getting the Material from the field and Null checking
        CollectScriptableObjectMaterial();

        if (scriptableObjectMaterial != null && scriptableObjectMaterial != objectMaterial)
        {
                GetComponent<Renderer>().material = scriptableObjectMaterial;
                objectMaterial = scriptableObjectMaterial;

            if (dataManager.debugOnWarn == true)
            {
                Debug.LogWarning("Material Mapper - ScriptableObject Material Not Found");
            }
        }
    }
    // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/casting-and-type-conversions
    void CollectScriptableObjectMaterial()
    {
        if (materialDataStore != null && dataStoreField != null)
        {
            // Create variable type field - Get the field from the field name of dataStoreField
            var field = materialDataStore.GetType().GetField(dataStoreField);
            if (field.FieldType == typeof(Material))
            {
                // Then get the material from that field
                scriptableObjectMaterial = (Material)field.GetValue(materialDataStore);
            }
        }
        else if (dataManager.debugOnWarn == true)
        {
            Debug.LogWarning("Material Mapper - either ScriptableObject or ScriptableObject Field are wrong type or null");
        }
    }
}

