using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace GTSPhotoLibraryTools
{
    public class Utils
    {
        [DllImport("__Internal")]
        private static extern void RequestPermissionC(string callbackObj, string callbackFunc);
        [DllImport("__Internal")]
        private static extern void SaveImageToPhotoLibraryC(string imagePath);

        public static void RequestPermission(string callbackObj, string callbackFunc)
        {
#if UNITY_IOS
            RequestPermissionC(callbackObj, callbackFunc);
#elif UNITY_ANDROID
#endif
        }

        public static void SaveImageToPhotoLibrary(Texture2D texture)
        {

#if UNITY_IOS
            var filepath = String.Format("{0}/{1}.png", Application.persistentDataPath, DateTime.Now.Ticks);
            File.WriteAllBytes(filepath, texture.EncodeToPNG());
            SaveImageToPhotoLibraryC(filepath);
#elif UNITY_ANDROID
#endif
        }

        public static IEnumerator CaptureScreen(Action<Texture2D> action)
        {
            yield return new WaitForEndOfFrame();

            var width = Screen.width;   // for Taking Picture
            var height = Screen.height; // for Taking Picture
            var camera = Camera.main.GetComponent<Camera>();
            var renderTex = new RenderTexture(width, height, 24);
            camera.targetTexture = renderTex;
            RenderTexture.active = renderTex;
            camera.Render();
            var screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenshot.Apply(); //false
            RenderTexture.active = null;
            camera.targetTexture = null;

            action(screenshot);
        }
    }
}
