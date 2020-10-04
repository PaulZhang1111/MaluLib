using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MaluLib;

[System.Serializable]
public class TestData{
    public int number;
    public string name;
}
public class SaveDataExample : MonoBehaviour
{
    public static SaveDataExample instance;
    public TestData testData;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    /// <summary>
    /// 使用MaluLib儲存系統是為了方便改寫各種資料
    /// </summary>
    public void TestSaveData(){
        testData = new TestData();
        testData.name = "Test";
        testData.number = 123;
        ApplicationData.SaveJsonFile(testData, "TestSave", AppInfo.APP_DATA_PATH);
    }


    public void SaveData(){
        ApplicationData.SaveJsonFile(testData, "TestSave", AppInfo.APP_DATA_PATH);
    }
}
