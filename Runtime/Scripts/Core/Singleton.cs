// 
// MaluLib-Singleton.cs
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
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		protected static T _instance;

		protected static readonly object _synObject = new object();

		#region Property Message
		public static T instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_synObject)
					{
						if (_instance == null)
						{
							_instance = FindObjectOfType<T>();
						}
					}

					if (_instance == null)
					{
						Init();

						Debug.Log("An instance of " + typeof(T) +
							" is needed in the scene, but there is none. Created automatically");
					}
				}

				return _instance;
			}
		}

		public static  bool IsExistInstance	{get {return _instance != null;}}
		#endregion

		void Awake()
		{
			RegisterInstance ();
		}

		public static T Init()
		{
			if(_instance == null)
			{
				GameObject obj = new GameObject(typeof(T).ToString());
				_instance = obj.AddComponent<T>();
			}

			return _instance;
		}

		public void RegisterInstance () 
		{
			if (_instance == null)
			{
				_instance = GetComponent<T>();
			}
			else if(_instance != this)
			{
				DestroyImmediate(gameObject);
			}
		}
	}
}

