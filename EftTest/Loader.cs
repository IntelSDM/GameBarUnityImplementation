using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EftTest.Helpers;
using System.Net.WebSockets;
namespace EftTest
{
    public static class Loader
    {
        public static void Load()
        {


            DumbHook FileAvaliability = new DumbHook();

            FileAvaliability.Init(
                   typeof(FilesChecker.CheckResultExtension).GetMethod(
                       "Succeed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static),
                   typeof(Globals).GetMethod(
                       "Succeed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                   );
            FileAvaliability.Hook();
            DumbHook FileFailed = new DumbHook();

            FileFailed.Init(
                   typeof(FilesChecker.CheckResultExtension).GetMethod(
                       "Failed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static),
                   typeof(Globals).GetMethod(
                       "Failed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                   );
            FileFailed.Hook();




            HackObject.AddComponent<Globals>();
            HackObject.AddComponent<Esp.Caching>();
            HackObject.AddComponent<Esp.Items>();
            GameObject.DontDestroyOnLoad(HackObject);
            
        }
    

        private static GameObject HackObject = new GameObject();
    }
}
