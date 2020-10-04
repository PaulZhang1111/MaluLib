// 
// MaluLib-CPUInfo.cs
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
using System.IO;
using System.Threading;

public class CPUInfo
{

    public static readonly string statPath = "/proc/stat";

    public static string[] GetStatInfoArray()
    {
        return File.ReadAllLines(statPath);
    }

    public static float cpuUsageRate = 0;

    public static void Update()
    {
        new Thread(Update2).Start();
    }

    public static void Update2()
    {
        string line;
        string[] values;
        float cpu1, cpu2, idle1, idle2;

        //------------------------

        line = GetStatInfoArray()[0];
        values = Split(line);
        cpu1 = int.Parse(values[1]) + int.Parse(values[2]) + int.Parse(values[3]) + int.Parse(values[5]) + int.Parse(values[6]) + int.Parse(values[7]) + int.Parse(values[8]);
        idle1 = int.Parse(values[4]);

        //------------------------

        Thread.Sleep(10);

        line = GetStatInfoArray()[0];
        values = Split(line);
        cpu2 = int.Parse(values[1]) + int.Parse(values[2]) + int.Parse(values[3]) + int.Parse(values[5]) + int.Parse(values[6]) + int.Parse(values[7]) + int.Parse(values[8]);
        idle2 = int.Parse(values[4]);

        //------------------------

        cpuUsageRate = (cpu2 - cpu1) / ((cpu2 + idle2) - (cpu1 + idle1)) * 100;
        //      cpuUsageRate = (cpu2 - cpu1 - (idle2 - idle1)) / (cpu2 - cpu1) * 100 ; 
        //      http://blog.csdn.net/jk110333/article/details/8683478
    }

    static string[] Split(string data)
    {
        data = data.Replace(" ", " ");
        data = data.Replace(" ", " ");
        data = data.Replace(" ", " ");
        return data.Split(' ');
    }

}
#endif
