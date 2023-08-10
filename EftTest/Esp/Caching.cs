using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFT;
using UnityEngine;
using System.Collections;
using Comfort.Common;

namespace EftTest.Esp
{
    class Caching : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Cache());
        }
        IEnumerator Cache()
        {
            for (; ; )
            {
                Globals.GameWorld = Singleton<GameWorld>.Instance;
                if (Globals.GameWorld == null)
                    yield return new WaitForSeconds(3f);
                Globals.MainCamera = Camera.main;

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
