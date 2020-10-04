// 
// MaluLib-TimerExtensions.cs
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

using System;
using UnityEngine;

namespace MaluLib
{
    /// <summary>
    /// 擴充方法 <see cref="Timer"/>s.
    /// </summary>
    public static class TimerExtensions
    {
        /// <summary>
        /// 將計時器擴充到其他物件身上。
        /// </summary>
        /// <param name="behaviour">被附屬的MonoBehaviour物件</param>
        /// <param name="duration">計時器的執行秒數。</param>
        /// <param name="onComplete">計時器完成時要觸發的事件。</param>
        /// <param name="onUpdate">計時器更新時觸發得事件，會回傳當前秒數</param>
        /// <param name="isLooped">計時器是否結束後重新計時。</param>
        /// <param name="useRealTime">計時器是否用實際時間（不受Unity Time Scale 影響）</param>
        /// <param name="DestroyOwner">計時器可以附屬在其他物件身上，當物件刪除後此計時器會跟著刪除，避免產生空物件錯誤。</param>
        /// <returns>回傳當前註冊計時器的物件</returns>
        public static void AttachTimer(this MonoBehaviour behaviour, float duration, Action onComplete,
            Action<float> onUpdate = null, bool isLooped = false, bool useRealTime = false)
        {
            Timer.Register(duration, onComplete, onUpdate, isLooped, useRealTime, behaviour);
        }

        public static Timer OnUpdate(this Timer timer, Timer.TimerUpdateCallback callback){
            timer.onUpdate = callback;
            return timer;
        }

        public static Timer OnComplete(this Timer timer, Timer.TimerCompletedCallback callback){
            timer.onComplete = callback;
            return timer;
        }
    }
}