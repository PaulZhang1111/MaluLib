// 
// MaluLib-SerialPortStream.cs
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
using System.IO.Ports;
using System.Threading;
using System.Collections;

namespace MaluLib.IO
{
    public class SerialPortStream:ThreadReceive
    {
        public SerialPortStream(string portName, int baudRate ) : base(portName, baudRate ) {
	    }
        public SerialPortStream(string portName, int baudRate, int readTimeout ) : base(portName, baudRate, readTimeout ) {
	    }
        public SerialPortStream(string portName, int baudRate, int readTimeout, int QueueLenght) : base(portName, baudRate, readTimeout, QueueLenght) {
	    }
        
        public override string ReadProtocol() {
            return _serialPort.ReadLine();
        }

        public override void SendProtocol(object message) {
            _serialPort.WriteLine((string) message);
        }
    }
}

#endif