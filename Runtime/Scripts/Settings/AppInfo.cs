// 
// MaluLib-AppInfo.cs
//  
// Copyright © 2018 張崇億
//  
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of 
// the Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions: 
//  
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software. 
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;

namespace MaluLib
{
    [System.Serializable]
    public class AppSetting
    {
        public float version;
        public int screenWidth;
        public int screenHeight;
        public bool isFullScreen;
        public string defaultLanguage;
        public bool autoSave;
        public bool logEnable;

        public AppSetting(float version, int screenWidth, int screenHeight, bool isFullScreen,
                        string defaultLanguage = "zh-hant", bool autoSave = true, bool logEnable = true, string ip = "127.0.0.1", int port = 8000)
        {
            this.version = version;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.isFullScreen = isFullScreen;
            this.defaultLanguage = defaultLanguage;
            this.autoSave = autoSave;
            this.logEnable = logEnable;

        }
    }
    public struct AppData
    {
        public int launchCount;
        public int playCount;
        public int playCompletedCount;
        public string language;
    }
    public class AppInfo : Setting<AppInfo>
    {
        public int launchCount
        {
            get { return this.data.launchCount; }
            set { this.data.launchCount = value; }
        }
        public int playCount
        {
            get { return this.data.playCount; }
            set { this.data.playCount = value; }
        }

        public string language
        {
            get { return this.data.language; }
            set { this.data.language = value; }
        }
        public const string APP_CONFIG_FILENAME = "Config";
        public const string APP_DATA_PATH = "Save";
        public const string APP_DATA_FILENAME = "AppData";

        public AppSetting setting;
        private AppData data;
        private bool isTransferData = false;
        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = CheckSingleton(instance, this);
        }

        private void Start()
        {
            if (!isTransferData)
            {
                ApplicationData.LoadConfigData(this);
                InitAppData();
                data.launchCount++;
            }
# if UNITY_STANDALONE_WIN
            Screen.SetResolution(setting.screenWidth, setting.screenHeight, setting.isFullScreen);
            Debug.unityLogger.logEnabled = setting.logEnable;
#endif        
        }


        /// <summary>
        /// Callback sent to all game objects before the application is quit.
        /// </summary>
        private void OnApplicationQuit()
        {
            ApplicationData.SaveJsonFile(data, APP_DATA_FILENAME, APP_DATA_PATH);
        }

        /// <summary>
        /// 轉換AppInfo單例物件內部資料
        /// </summary>
        /// <param name="_old">old AppInfo</param>
        /// <param name="_new">new AppInfo</param>
        protected override void TransferData(AppInfo _old, AppInfo _new)
        {
            isTransferData = true;
            _new.setting = _old.setting;
            _new.data = _old.data;
        }

        /// <summary>
        /// 初始化app data 取得目前存檔資訊
        /// </summary>
        private void InitAppData()
        {
            string dataString = ApplicationData.LoadJsonFile(APP_DATA_FILENAME, APP_DATA_PATH);

            if (dataString != null)
            {
                data = JsonConvert.DeserializeObject<AppData>(dataString);
            }
            else
            {
                data = new AppData();
                data.language = setting.defaultLanguage;
                data.launchCount = 0;
                data.playCount = 0;
                ApplicationData.SaveJsonFile(data, APP_DATA_FILENAME, APP_DATA_PATH);
            }
        }

        [HorizontalGroup("test")]
        [Button(ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void CreateConfig()
        {
            ApplicationData.SaveConfigData(this);
        }
        [HorizontalGroup("test")]
        [Button(ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void LoadConfig()
        {
            ApplicationData.LoadConfigData(this);
        }
    }

}
