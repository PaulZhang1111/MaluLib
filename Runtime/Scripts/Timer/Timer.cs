// 
// MaluLib-Timer.cs
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

/*
* MaluLib Unity Timer
* 2019.07.08 init
*/
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

namespace MaluLib
{
    public class Timer
    {
        #region Public Properties/Fields
        public delegate void TimerCompletedCallback();
        public delegate void TimerUpdateCallback(float amount);

        public TimerCompletedCallback onComplete;
        public TimerUpdateCallback onUpdate;

        /// <summary>
        /// 計時時間長度
        /// </summary>
        public float duration { get; private set; }
        /// <summary>
        /// 是否需要結束後立刻重複計時
        /// </summary>
        public bool isLooped { get; set; }
        /// <summary>
        /// 計時器是否計時完成，如果中途被取消回傳false
        /// </summary>
        public bool isCompleted { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool usesRealTime { get; private set; }

        /// <summary>
        /// Timer是否暫停
        /// </summary>
        public bool isPaused
        {
            get { return this._timeElapsedBeforePause.HasValue; }
        }
        /// <summary>
        /// Timer是否取消
        /// </summary>
        public bool isCancelled
        {
            get { return this._timeElapsedBeforeCancel.HasValue; }
        }

        /// <summary>
        /// Timer是否計時結束
        /// </summary>
        public bool isDone
        {
            get { return this.isCompleted || this.isCancelled || this.isOwnerDestroyed; }
        }
        #endregion Public Properties/Fields


        #region Public Static Methods
        /// <summary>
        /// 註冊一個新的 Timer，由 manager 管理，所以當場景變化或 manager 刪除會自動清除註冊後的 timer。
        /// </summary>
        /// <param name="duration">計時器的執行秒數。</param>
        /// <param name="onComplete">計時器完成時要觸發的事件。</param>
        /// <param name="onUpdate">計時器更新時觸發得事件，會回傳當前秒數</param>
        /// <param name="isLooped">計時器是否結束後重新計時。</param>
        /// <param name="useRealTime">計時器是否用實際時間（不受Unity Time Scale 影響）</param>
        /// <param name="DestroyOwner">計時器可以附屬在其他物件身上，當物件刪除後此計時器會跟著刪除，避免產生空物件錯誤。</param>
        /// <returns>回傳當前註冊計時器的物件</returns>
        public static Timer Register(float duration, Action onComplete, Action<float> onUpdate = null,
            bool isLooped = false, bool useRealTime = false, MonoBehaviour autoDestroyOwner = null)
        {
            // create a manager object to update all the timers if one does not already exist.
            if (Timer._manager == null)
            {
                TimerManager managerInScene = Object.FindObjectOfType<TimerManager>();
                if (managerInScene != null)
                {
                    Timer._manager = managerInScene;
                }
                else
                {
                    GameObject managerObject = new GameObject { name = "TimerManager" };
                    Timer._manager = managerObject.AddComponent<TimerManager>();
                }
            }

            Timer timer = new Timer(duration, onComplete, onUpdate, isLooped, useRealTime, autoDestroyOwner);
            Timer._manager.RegisterTimer(timer);
            return timer;
        }
        public static Timer Register(float duration, bool isLooped = false, bool useRealTime = false, MonoBehaviour autoDestroyOwner = null)
        {
            // create a manager object to update all the timers if one does not already exist.
            if (Timer._manager == null)
            {
                TimerManager managerInScene = Object.FindObjectOfType<TimerManager>();
                if (managerInScene != null)
                {
                    Timer._manager = managerInScene;
                }
                else
                {
                    GameObject managerObject = new GameObject { name = "TimerManager" };
                    Timer._manager = managerObject.AddComponent<TimerManager>();
                }
            }

            Timer timer = new Timer(duration, isLooped, useRealTime, autoDestroyOwner);
            Timer._manager.RegisterTimer(timer);
            return timer;
        }
        
        /// <summary>
        /// 取消指定Timer
        /// </summary>
        /// <param name="timer">取消指定的Timer</param>
        public static void Cancel(Timer timer)
        {
            if (timer != null)
            {
                timer.Cancel();
            }
        }

        /// <summary>
        /// 暫停指定Timer
        /// </summary>
        /// <param name="timer">暫停指定的Timer</param>
        public static void Pause(Timer timer)
        {
            if (timer != null)
            {
                timer.Pause();
            }
        }

        /// <summary>
        /// 恢復指定Timer
        /// </summary>
        /// <param name="timer">恢復指定的Timer</param>
        public static void Resume(Timer timer)
        {
            if (timer != null)
            {
                timer.Resume();
            }
        }

        /// <summary>
        /// 取消所有已註冊Timer
        /// </summary>
        public static void CancelAllRegisteredTimers()
        {
            if (Timer._manager != null)
            {
                Timer._manager.CancelAllTimers();
            }

            // if the manager doesn't exist, we don't have any registered timers yet, so don't
            // need to do anything in this case
        }

        /// <summary>
        /// 暫停所有Timer
        /// </summary>
        public static void PauseAllRegisteredTimers()
        {
            // 如果 Manager 不存在就不做處理
            if (Timer._manager != null)
            {
                Timer._manager.PauseAllTimers();
            }
        }
        /// <summary>
        /// 恢復所有Timer
        /// </summary>
        public static void ResumeAllRegisteredTimers()
        {
            // 如果 Manager 不存在就不做處理
            if (Timer._manager != null)
            {
                Timer._manager.ResumeAllTimers();
            }
        }
        #endregion Public Static Methods

        #region Public Methods
        /// <summary>
        /// 取消此Timer
        /// </summary>
        public void Cancel()
        {
            if (this.isDone)
            {
                return;
            }

            this._timeElapsedBeforeCancel = this.GetTimeElapsed();
            this._timeElapsedBeforePause = null;
        }
        /// <summary>
        /// 暫停此Timer
        /// </summary>
        public void Pause()
        {
            if (this.isPaused || this.isDone)
            {
                return;
            }

            this._timeElapsedBeforePause = this.GetTimeElapsed();
        }
        /// <summary>
        /// 恢復此Timer
        /// </summary>
        public void Resume()
        {
            if (!this.isPaused || this.isDone)
            {
                return;
            }

            this._timeElapsedBeforePause = null;
        }
        /// <summary>
        /// 取得計時器已執行秒數。
        /// </summary>
        /// <returns></returns>
        public float GetTimeElapsed()
        {
            if (this.isCompleted || this.GetWorldTime() >= this.GetFireTime())
            {
                return this.duration;
            }

            return this._timeElapsedBeforeCancel ??
                this._timeElapsedBeforePause ??
                this.GetWorldTime() - this._startTime;
        }

        /// <summary>
        /// 取得計時器剩餘秒數。
        /// </summary>
        /// <returns></returns>
        public float GetTimeRemaining()
        {
            return this.duration - this.GetTimeElapsed();
        }
        /// <summary>
        /// 取得計時器已執行時間比例數值
        /// </summary>
        /// <returns>回傳 0 到 1 數值</returns>
        public float GetRatioComplete()
        {
            return this.GetTimeElapsed() / this.duration;
        }
        /// <summary>
        /// 取得計時器剩餘時間比例數值
        /// </summary>
        /// <returns>回傳 0 到 1 數值</returns>
        public float GetRatioRemaining()
        {
            return this.GetTimeRemaining() / this.duration;
        }

        #endregion Public Methods

        #region Private Static Properties/Fields
        /// <summary>
        /// 負責管理所有的Timer
        /// </summary>
        private static TimerManager _manager;

        #endregion Private Static Properties/Fields

        #region Private Properties/Fields
        private bool isOwnerDestroyed
        {
            get { return this._hasAutoDestroyOwner && this._autoDestroyOwner == null; }
        }

        private readonly Action _onComplete;
        private readonly Action<float> _onUpdate;
        private float _startTime;
        private float _lastUpdateTime;
        /// <summary>
        /// 暫存取消/暫停之前的時間
        /// </summary>
        private float? _timeElapsedBeforeCancel;
        private float? _timeElapsedBeforePause;

        /// <summary>
        /// Timer計數完成後會自動銷毀
        /// </summary>
        private readonly MonoBehaviour _autoDestroyOwner;
        private readonly bool _hasAutoDestroyOwner;

        #endregion Private Properties/Fields

        #region Private Constructor
        #endregion Private Constructor

        #region Private Constructor (use static Register method to create new timer)

        private Timer(float duration, Action onComplete, Action<float> onUpdate,
            bool isLooped, bool usesRealTime, MonoBehaviour autoDestroyOwner)
        {
            this.duration = duration;
            this._onComplete = onComplete;
            this._onUpdate = onUpdate;

            this.isLooped = isLooped;
            this.usesRealTime = usesRealTime;

            this._autoDestroyOwner = autoDestroyOwner;
            this._hasAutoDestroyOwner = autoDestroyOwner != null;

            this._startTime = this.GetWorldTime();
            this._lastUpdateTime = this._startTime;
        }
        private Timer(float duration, bool isLooped, bool usesRealTime, MonoBehaviour autoDestroyOwner)
        {
            this.duration = duration;
            this.isLooped = isLooped;
            this.usesRealTime = usesRealTime;

            this._autoDestroyOwner = autoDestroyOwner;
            this._hasAutoDestroyOwner = autoDestroyOwner != null;

            this._startTime = this.GetWorldTime();
            this._lastUpdateTime = this._startTime;
        }
        #endregion

        #region Private Methods
        private float GetWorldTime()
        {
            return this.usesRealTime ? Time.realtimeSinceStartup : Time.time;
        }
        private float GetFireTime()
        {
            return this._startTime + this.duration;
        }

        private float GetTimeDelta()
        {
            return this.GetWorldTime() - this._lastUpdateTime;
        }

        private void Update()
        {
            if (this.isDone)
            {
                return;
            }

            if (this.isPaused)
            {
                this._startTime += this.GetTimeDelta();
                this._lastUpdateTime = this.GetWorldTime();
                return;
            }

            this._lastUpdateTime = this.GetWorldTime();

            if (this._onUpdate != null)
            {
                this._onUpdate(this.GetTimeElapsed());
            }

            if (this.onUpdate != null){
                this.onUpdate(this.GetTimeElapsed());
            }

            if (this.GetWorldTime() >= this.GetFireTime())
            {

                if (this._onComplete != null)
                {
                    this._onComplete();
                }
                if (this.onComplete != null){
                    this.onComplete();
                }

                if (this.isLooped)
                {
                    this._startTime = this.GetWorldTime();
                }
                else
                {
                    this.isCompleted = true;
                }
            }
        }
        #endregion Private Methods


        #region Manager Class
        /// <summary>
        /// TimerManager會自動實例化無須手動添加。ㄋ
        /// </summary>
        private class TimerManager : MonoBehaviour
        {  
            private List<Timer> _timers = new List<Timer>();
            /// <summary>
            /// 緩衝添加Timer，不會在計時中加入
            /// </summary>
            private List<Timer> _timersToAdd = new List<Timer>();
            /// <summary>
            /// 註冊Timer
            /// </summary>
            /// <param name="timer"></param>
            public void RegisterTimer(Timer timer)
            {
                this._timersToAdd.Add(timer);
            }

            /// <summary>
            /// 取消所有Timer
            /// </summary>
            public void CancelAllTimers()
            {
                foreach (Timer timer in this._timers)
                {
                    timer.Cancel();
                }

                this._timers = new List<Timer>();
                this._timersToAdd = new List<Timer>();
            }
            /// <summary>
            /// 暫停所有Timer
            /// </summary>
            public void PauseAllTimers()
            {
                foreach (Timer timer in this._timers)
                {
                    timer.Pause();
                }
            }
            /// <summary>
            /// 恢復所有Timer
            /// </summary>
            public void ResumeAllTimers()
            {
                foreach (Timer timer in this._timers)
                {
                    timer.Resume();
                }
            }

            /// <summary>
            /// 更新所有Timer
            /// </summary>
            [UsedImplicitly]
            private void Update()
            {
                this.UpdateAllTimers();
            }

            private void UpdateAllTimers()
            {
                if (this._timersToAdd.Count > 0)
                {
                    this._timers.AddRange(this._timersToAdd);
                    this._timersToAdd.Clear();
                }

                foreach (Timer timer in this._timers)
                {
                    timer.Update();
                }

                this._timers.RemoveAll(t => t.isDone);
            }
        }
        #endregion Manager Class
    }
}

