// 
// MaluLib-CPUUtilizationDisplay.cs
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

#if (UNITY_EDITOR)

using UnityEngine;



public class CPUUtilizationDisplay : MonoBehaviour
{
    public int m_fontSize = 4;     // Change the Font Size
	public Color m_textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);

    private void Start()
    {
        InvokeRepeating("Refresh", 0, 1);
    }

    private void Refresh()
    {
        CPUInfo.Update();
    }

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * m_fontSize / 100;
        style.normal.textColor = m_textColor;
        string text = "\nCPU 使用率：" + CPUInfo.cpuUsageRate.ToString("0.0") + " %";
        GUI.Label(rect, text, style);
    }

}

#endif
