using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using AssetBundles;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Skyunion
{
    public class AssetsManager : GameModule<AssetsManager>
    {
        public static string AppContentDataUri
        {
            get
            {
                string dataPath = Application.dataPath;
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return dataPath + "/Raw/";
                }

                if (Application.platform == RuntimePlatform.Android)
                {
                    return "jar:file//" + dataPath + "!/assets/";
                }

                return dataPath + "/StreamingAssets/";
            }
        }

        public static string AppPersistentDataUri
        {
            get
            {
                return Application.persistentDataPath + "/";
            }
        }

        public static T LoadPrefabs<T>(string path) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            string strName = "Assets/Prefabs/" + path + ".prefab";
            T go = AssetDatabase.LoadAssetAtPath<T>(strName);
            return go;
#else
            string strName = AppContentDataUri + path;
            Debug.Log(strName);
            AssetBundle bundle = AssetBundle.LoadFromFile(strName);
            T go = bundle.LoadAllAssets<T>()[0];
            return go;
#endif
        }

        public static T LoadAnimationController<T>(string path) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            string strName = "Assets/Prefabs/" + path + ".prefab";
            T go = AssetDatabase.LoadAssetAtPath<T>(strName);
            return go;
#else
            string strName = AppContentDataUri + path;
            Debug.Log(strName);
            AssetBundle bundle = AssetBundle.LoadFromFile(strName);
            T go = bundle.LoadAllAssets<T>()[0];
            return go;
#endif
        }

		public static T Load<T>(string path) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            string strName = "Assets/" + path;
            T go = AssetDatabase.LoadAssetAtPath<T>(strName);
			return go;
#else
			string strName = AppContentDataUri + path;
			Debug.Log(strName);
			AssetBundle bundle = AssetBundle.LoadFromFile(strName);
			T go = bundle.LoadAllAssets<T>()[0];
			return go;
#endif
		}


        private IEnumerator Start()
        {
            AssetBundleManager.SetDevelopmentAssetBundleServer();

            var request = AssetBundleManager.Initialize();

            if (request != null)
                yield return StartCoroutine(request);
            
            //AssetBundleLoadAssetOperation loadRequest = AssetBundleManager.LoadAssetAsync("prefab", "Cube", typeof(GameObject));
            //if (loadRequest == null)
            //    yield break;

            //yield return StartCoroutine(loadRequest);

            //GameObject prefab = loadRequest.GetAsset();
            ////如果讀取成功, 則創建實體
            //if (prefab != null)
            //    GameObject.Instantiate(prefab);

            //yield return new WaitForSeconds(5f);
            ////釋放"prefab"這個bundle
            //AssetBundleManager.UnloadAssetBundle("prefab");
        }
    }
}
