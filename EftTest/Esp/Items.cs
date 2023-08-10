using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFT;
using UnityEngine;
using System.Collections;
using EFT.Interactive;
using System.IO.MemoryMappedFiles;
using EftTest.Entities;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace EftTest.Esp
{
    class Loot
    {
        public Loot(float x, float y, string name)
        {
            Type = "Loot";
            X = x;
            Y = y;
            Name = name;
        }
        public string Type;
        public float X;
        public float Y;
        public string Name;

    }
    class Items : MonoBehaviour
    {
        void Start()
        {
          
            StartCoroutine(CacheItems());
            StartCoroutine(UpdateItems());
        }
      
        IEnumerator CacheItems()
        {
            for (; ; )
            {
               
                if (Globals.GameWorld == null)
                    yield return new WaitForSeconds(5f);
                Globals.CorpseList.Clear();
                Globals.LootList.Clear();
            


                if (Globals.GameWorld != null)
                {
                    System.IO.File.WriteAllText(Globals.GameWorld.LootItems.Count.ToString(), "");
                    for (int i = 0; i < Globals.GameWorld.LootItems.Count; i++)
                    {
                        LootItem item = Globals.GameWorld.LootItems.GetByIndex(i);
                        if (item == null)
                            continue;
                        if (!(item.Item.TemplateId == "55d7217a4bdc2d86028b456d".Localized()))
                        {
                            Entities.EntityItem item1 = new Entities.EntityItem(item);
                            Globals.LootList.Add(item1);
                            
                        }


                    }
                }

                yield return new WaitForSeconds(1.5f);

            }
        }
        List<Loot> LootList = new List<Loot>();
        IEnumerator UpdateItems()
        {
            for (; ; )
            {
                   if (Globals.GameWorld == null)
                   yield return new WaitForSeconds(5f);

                LootList = new List<Loot>();
                //      System.IO.File.WriteAllText(Globals.LootList.Count.ToString(), "");
                foreach (Entities.EntityItem entity in Globals.LootList)
                {
                    if (entity.Entity == null)
                        continue;
                    if (entity.Entity.Item == null)
                        continue;
                    entity.UpdateInformation();
                    if (Globals.IsScreenPointVisible(entity.W2S))
                        LootList.Add(entity.Loot);
                }
                if (!Globals.SocketsSetUp)
                    yield return new WaitForSeconds(5f);
                string json = JsonConvert.SerializeObject(LootList, Formatting.None);
                SocketServer.TCPClient.SendText(json);
               // System.IO.File.WriteAllText("networked", "");
                yield return new WaitForEndOfFrame();

            }
        }

    
    }
}
