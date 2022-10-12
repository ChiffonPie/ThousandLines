using System.IO;
using UnityEditor;
using UnityEngine;

public class CacheClean : EditorWindow
{
    [MenuItem("Tools/Cleanup")]
    protected static void Cleanup()
    {
        bool isCash = Caching.ClearCache();
        Debug.LogError("캐시 삭제 성공 여부 : " + isCash);
        PlayerPrefs.DeleteAll();
    }
}