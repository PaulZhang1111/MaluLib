// 
// MaluLib-ThreadReceive.cs
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

#if NET_2_0 || NET_4_6

using System;
using System.Threading;
using System.Collections;
using System.IO.Ports;

namespace MaluLib.IO
{
    public abstract class ThreadReceive
    {
        public SerialPort _serialPort;
        private string portName;
        private int baudRate;
        private int readTimeout = 100;
        private Thread ioThread = null;

        //輸出
        private Queue outputQueue;
        //輸入
        private Queue inputQueue;
        private int QueueLenght = 1;

        /// <summary>
        /// 線程狀態
        /// </summary>
        public bool looping = true;


        // Constructor
        public ThreadReceive(string portName, int baudRate) {
            this.portName = portName;
            this.baudRate = baudRate;
        }
        
        public ThreadReceive(string portName, int baudRate, int readTimeout) {
                this.portName = portName;
                this.baudRate = baudRate;
                this.readTimeout = readTimeout;
        }

        public ThreadReceive(string portName, int baudRate, int readTimeout, int QueueLenght) {
                this.portName = portName;
                this.baudRate = baudRate;
                this.readTimeout = readTimeout;
                this.QueueLenght = QueueLenght;
        }

        /// <summary>
        /// 使用SerialPort打開序列埠
        /// </summary>
        public void OpenSerialPort() { 
            _serialPort = new SerialPort(this.portName, this.baudRate); 
            _serialPort.ReadTimeout = this.readTimeout; 
            try
            {
                _serialPort.Open();
            }
            catch(Exception e){
                Console.WriteLine("{0} Exception caught."+ e);
            }
        }

        /// <summary>
        /// 創建並啟動線程
        /// </summary>
        public void StartThread() { 
            outputQueue = Queue.Synchronized( new Queue() );
            inputQueue  = Queue.Synchronized( new Queue() );
            try
			{
                ioThread = new Thread (ThreadLoop);
                ioThread.Start ();
            }
            catch(Exception e){
                Console.WriteLine("{0} Exception caught."+ e);
            }
        }

        /// <summary>
        /// 此方法用於停止線程
        /// </summary>
        public void StopThread () { 
            // avoid thread issues.
            lock (this) {
                looping = false;
            }
        }

        /// <summary>
        /// 此方法用於回傳線程是否還在循環
        /// </summary>
        /// <returns></returns>
        public bool ThreadIsLooping () {
            // avoid thread issues.
            lock (this) {
                return looping;
            }
        }
        public string ReadQueueThread(){ 
            if (inputQueue.Count == 0)
                return null;

            return (string)inputQueue.Dequeue ();
        }
        public void WriteThread(string dataToSend){ // add the data to the write Queue. Independent from the protocol.
            outputQueue.Enqueue (dataToSend);
        }

        /// <summary>
        /// 主線程循環
        /// </summary>
        public void ThreadLoop() { 
            while (ThreadIsLooping()){
                // 讀取資料
                object dataComingFromDevice = ReadProtocol();
                if (dataComingFromDevice != null) {
                    if (inputQueue.Count < QueueLenght){
                            inputQueue.Enqueue(dataComingFromDevice);
                    }
            }
                // 發送資料
                if (outputQueue.Count != 0){
                    object dataToSend = outputQueue.Dequeue();
                    SendProtocol(dataToSend);
                }
            }

            _serialPort.Close(); //關閉 SerialPort
        }

        /// <summary>
        /// 讀取協議（json格式）
        /// </summary>
        /// <returns></returns>
        public abstract string ReadProtocol();

        /// <summary>
        /// 發送協議（json格式）
        /// </summary>
        /// <returns></returns>
        public abstract void SendProtocol(object message); 
    }
}

#endif