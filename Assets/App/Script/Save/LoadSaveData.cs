using System;
using UnityEngine;
using System.IO;
using UnityEngine.Serialization;

namespace BT.Save
{
    public class LoadSaveData : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RSE_LoadData rseCommandLoad;
        [SerializeField] private RSE_SaveData rseCommandSave;
        [SerializeField] private RSE_ClearData rseCommandClear;
        [SerializeField] private RSO_ContentSaved rsoContentSaved;
        
        private string filepath;

        private void OnEnable()
        {
            rseCommandLoad.action += LoadFromJson;
            rseCommandSave.action += SaveToJson;
            rseCommandClear.action += ClearContent;
        }

        private void OnDisable()
        {
            rseCommandLoad.action -= LoadFromJson;
            rseCommandSave.action -= SaveToJson;
        }

        private void Start()
        {
            filepath = Application.persistentDataPath + $"/Save_{Application.productName}.json";

            if (FileAlreadyExist()) LoadFromJson();
            else SaveToJson();
        }

        private void SaveToJson()
        {
            string infoData = JsonUtility.ToJson(rsoContentSaved.Value,true);
            File.WriteAllText(filepath, infoData);
        }

        private void LoadFromJson()
        {
            string infoData = File.ReadAllText(filepath);
            rsoContentSaved.Value = JsonUtility.FromJson<ContentSaved>(infoData);
        }

        private void ClearContent()
        {
            rsoContentSaved.Value = new ContentSaved();
            SaveToJson();
        }

        private bool FileAlreadyExist()
        {
            return File.Exists(filepath);
        }
        
    }   
}
