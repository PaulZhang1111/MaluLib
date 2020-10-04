// 
// MaluLib-ApplicationData.cs
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
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace MaluLib
{
    public static class ApplicationData
	{   
        public const string JSON_FILE_EXTENSION = ".json";

        /// <summary>
        /// 讀取 json 檔案
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string LoadJsonFile( string fileName, string path = null){
            string filePath;
            string data;
            path = (path == null) ? "":path+"/";
            filePath = path+fileName+JSON_FILE_EXTENSION;
            if (File.Exists(filePath)){
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
				{
                    StreamReader srReader = new StreamReader(stream); 
                    data = srReader.ReadToEnd();
				}
            }
            else{
                data = null;
                Debug.LogError(string.Format("File error: {0} does not exist.",filePath));
            }
            return data;
        }

        /// <summary>
        /// 將資料儲存為 json file
        /// </summary>
        /// <param name="obj">任何型態資料</param>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="path">檔案路徑</param>
        /// <typeparam name="T"></typeparam>
        public static void SaveJsonFile<T>(T obj, string fileName, string path = null){
            string filePath;
            string data = JsonConvert.SerializeObject(obj);
            // Debug.Log(data);
            path = (path == null) ? "":path+"/";
            filePath = path+fileName+JSON_FILE_EXTENSION;

            if (!System.IO.File.Exists(filePath))
                Directory.CreateDirectory(path);
    
            using (var stream = File.Open(filePath,FileMode.Create)){
                byte[] info = new UTF8Encoding(true).GetBytes(data);
                stream.Write(info, 0, info.Length);
            }
        }

        #region App Config

        /// <summary>
        /// 儲存 App 配置
        /// </summary>
        /// <param name="appInfo">限制型別參數</param>
        /// <typeparam name="T"></typeparam>
        public static void SaveConfigData<T>(this T appInfo) where T:AppInfo{
            string filePath = AppInfo.APP_CONFIG_FILENAME+JSON_FILE_EXTENSION;
            AppSetting _appSetting = appInfo.setting;

            using (var stream = File.Open(filePath,FileMode.Create))
            {
                appInfo.setting = _appSetting;
                // writing data in string
                string dataasstring = JsonConvert.SerializeObject(_appSetting);
                byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
                stream.Write(info, 0, info.Length);
            }	
        }

        /// <summary>
        /// 儲存 App 配置
        /// </summary>
        /// <param name="appInfo">限制型別參數</param>
        /// <param name="_appSetting">App 配置設定</param>
        /// <typeparam name="T"></typeparam>
        public static void SaveConfigData<T>(this T appInfo, AppSetting _appSetting) where T:AppInfo{
            string filePath = AppInfo.APP_CONFIG_FILENAME+JSON_FILE_EXTENSION;

            using (var stream = File.Open(filePath,FileMode.Create))
            {
                appInfo.setting = _appSetting;
                // writing data in string
                string dataasstring = JsonConvert.SerializeObject(_appSetting);
                byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
                stream.Write(info, 0, info.Length);
            }	
        }

        /// <summary>
        /// 讀取 App 配置
        /// </summary>
        /// <param name="appInfo">限制型別參數</param>
        /// <typeparam name="T"></typeparam>
        public static void LoadConfigData<T>(this T appInfo) where T:AppInfo
        {
            string filePath = AppInfo.APP_CONFIG_FILENAME+JSON_FILE_EXTENSION;
            AppSetting _appSetting;

            if (File.Exists(filePath)){
				using (FileStream stream = new FileStream(filePath, FileMode.Open))
				{
                    StreamReader srReader = new StreamReader(stream); 
                    string data = srReader.ReadToEnd();
                    _appSetting = JsonConvert.DeserializeObject<AppSetting>(data);
                    appInfo.setting = _appSetting;
				}
            }
            else{
                SaveConfigData(appInfo);
            }
        }

        #endregion App Config
    }
}
