// 
// MaluLib-AudioCreationExtensions.cs
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

/**
	*20180208
	*聲音的擴充功能
	*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaluLib.Core;

namespace MaluLib
{
	public static class AudioCreationExtensions
	{
		/// <summary>
		/// Play the audio clip.
		/// </summary>
		/// <param name="audio"></param>
		/// <param name="clip"></param>
		/// <returns></returns>
		public static AudioSource PlayClip(this AudioSource audio, AudioClip clip)
		{
			ExtensionMethodHelper helper = audio.gameObject.GetComponent<ExtensionMethodHelper>();
			if (helper != null)
			{
				if (helper.coroutine != null)
				{
					helper.StopCoroutine(helper.coroutine);
					helper.coroutine = null;
				}
			}

			audio.clip = clip;
			audio.Play();
			return audio;
		}

		/// <summary>
		/// Sets a callback that will be fired when the audio starts
		/// </summary>
		/// <param name="audio"></param>
		/// <param name="callback"></param>
		/// <returns></returns>
		public static AudioSource OnStart(this AudioSource audio,MaluCallback callback)
		{
			callback();
			return audio;
		}
		/// <summary>
		/// Sets a callback that will be fired the moment the audio plays completion
		/// </summary>
		/// <param name="audio"></param>
		/// <param name="callback"></param>
		/// <returns></returns>
		public static AudioSource OnComplete(this AudioSource audio,MaluCallback callback)
		{
			ExtensionMethodHelper helper = audio.gameObject.GetComponent<ExtensionMethodHelper>();
			if (helper == null)
			{
				helper = audio.gameObject.AddComponent<ExtensionMethodHelper>();
			}
			helper.coroutine =  Complete(callback, audio.clip.length);
			helper.StartCoroutine(helper.coroutine);
			return audio;
		}

		/// <summary>
		/// Stop the audio clip
		/// </summary>
		/// <param name="audio"></param>
		/// <returns></returns>
		public static AudioSource StopClip(this AudioSource audio)
		{
			ExtensionMethodHelper helper = audio.gameObject.GetComponent<ExtensionMethodHelper>();
			if (helper != null)
			{
				if (helper.coroutine != null)
				{
					helper.StopCoroutine(helper.coroutine);
					helper.coroutine = null;
				}
			}
			audio.Stop();
			return audio;
		}
		
		/// <summary>
		/// Wait the audio complete and do callback 
		/// </summary>
		/// <param name="callback"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		internal static IEnumerator Complete(MaluCallback callback,float time)
		{
			yield return new WaitForSeconds(time);
			callback();
			yield return null;
		}
	}
}