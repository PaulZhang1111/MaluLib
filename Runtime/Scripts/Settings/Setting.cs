// 
// MaluLib-Setting.cs
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

using UnityEngine;
using System.Collections;

namespace MaluLib
{
    /// <summary>
    /// 設定抽象類別：繼承此類別可以方便在不同場景使用不同資料結構
    /// </summary>
    public abstract class Setting<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance = default(T);

		protected T CheckSingleton(T _old, T _new)
        {
            if (_old != null)
            {
                TransferData(_old, _new);

                // Delete the old one
                Destroy(_old.gameObject);
            }

            return _new;
        }
        /// <summary>
        /// 每當Unity加載場景時。 腳本已經重置，以免丟失原始設置。
        /// 將數據從舊實例傳輸到新實例。
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
		protected abstract void TransferData(T _old, T _new);
	}
}
