# Unity 相簿工具 Plugin

iOS / Android 相簿相關工具

## 目前功能

- RequestPermission(string callbackObj, string callbackFunc) : 要求相簿存取權限
- SaveImageToPhotoLibrary(Texture2D texture) : 將照片存放到相簿中
- CaptureScreen() : 抓取 mainCamera 截圖

## 自動處理

- iOS

  - 加入 Photos.framework
  - 在 info.plist 加入 NSPhotoLibraryUsageDescription = ""
  - 在 info.plist 加入 UIRequiresFullScreen = true

## 使用方式

- 複製到專案中
- 範例

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GTSPhotoLibraryTools;

public class DemoScript : MonoBehaviour
{

    private Texture2D screenshot;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTestClick()
    {
        StartCoroutine(CaptureScreen());
    }

    IEnumerator CaptureScreen()
    {
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
        yield return StartCoroutine(Utils.CaptureScreen((screenshot) =>
        {
            this.screenshot = screenshot;
            Utils.RequestPermission(this.name, "OnPermission");
            GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        }));
    }

    void OnPermission()
    {
        Utils.SaveImageToPhotoLibrary(screenshot);
    }
}

```
