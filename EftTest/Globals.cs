using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EftTest.Entities;
using EFT;
using System.IO;
using EftTest.Sockets;
using EFT.UI;
using Comfort.Common;
using System.Collections;
namespace EftTest
{
    class Globals : MonoBehaviour
    {
        public static List<EntityItem> LootList = new List<EntityItem>();
        public static List<EntityCorpse> CorpseList = new List<EntityCorpse>();
        public static Camera MainCamera;
        public static GameWorld GameWorld;
        void Start()
        {
            File.WriteAllText("Loaded", "loaded");
            StartCoroutine(Sockets());
         
        }
        public static bool SocketsSetUp = false;
        IEnumerator Sockets()
        {
            for (; ; )
            {
                if (MonoBehaviourSingleton<PreloaderUI>.Instance != null && !SocketsSetUp)
                {
                    SocketServer.CreateServer();
                    SocketServer.AcceptClients();
                    SocketsSetUp = true;
                    StopCoroutine(Sockets());
                }
                yield return new WaitForEndOfFrame();
            }
        }
        public static bool Failed(FilesChecker.ICheckResult result)
        {
            return false;
        }
        public static bool Succeed(FilesChecker.ICheckResult result)
        {
            return true;
        }
        public static bool IsScreenPointVisible(Vector3 screenPoint)
        {
            return screenPoint.z > 0.01f && screenPoint.x > -5f && screenPoint.y > -5f && screenPoint.x < (float)UnityEngine.Screen.width && screenPoint.y < (float)UnityEngine.Screen.height;
        }

        public static Vector3 WorldToScreen(Vector3 worldpos)
        {
            worldpos = Globals.MainCamera.WorldToScreenPoint(worldpos);
            worldpos.y = (float)Screen.height - worldpos.y;
            return worldpos;
        }
    }
}
