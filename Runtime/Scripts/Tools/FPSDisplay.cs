// 
// MaluLib-FPSDisplay.cs
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
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
	// Change the Font Size
    public int m_fontSize = 4;     
	public Color m_textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);

    private float deltaTime = 0.0f;


    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }


    private void OnGUI()
    {


        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * m_fontSize / 100;
        style.normal.textColor = m_textColor;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
    

}
#endif
