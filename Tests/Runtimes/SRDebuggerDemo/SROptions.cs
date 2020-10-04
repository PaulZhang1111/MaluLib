using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

/// <summary>
/// 教學文件： https://www.stompyrobot.uk/tools/srdebugger/documentation/#options_tab
/// </summary>
public partial class SROptions {


	[Category("數字設定")] 
	public int number {
		get { return SaveDataExample.instance.testData.number; }
		set { SaveDataExample.instance.testData.number = value; }
	}
    [Category("名字設定")] 
    public string name {
		get { return SaveDataExample.instance.testData.name; }
		set { SaveDataExample.instance.testData.name = value; }
	}

    public void SaveData() {
		Debug.Log("儲存成功");
		SaveDataExample.instance.SaveData();
	}
}