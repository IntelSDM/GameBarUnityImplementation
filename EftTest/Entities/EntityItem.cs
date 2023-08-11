using EFT.Interactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using EftTest.Esp;
namespace EftTest.Entities
{
    class EntityItem
    {
        public EntityItem(LootItem obj)
        {
            Entity = obj;
           
        }
        public LootItem Entity;
        public string Name;
        public Vector3 W2S;
        public Loot Loot;
        public void UpdateInformation()
        {
            if (Globals.GameWorld == null)
                return;
            if (Entity == null)
                return;
            if (Entity.Item == null)
                return;
            Name = Entity.Item.LocalizedShortName();
            W2S = Globals.WorldToScreen(Entity.transform.position);
            Loot = new Loot((float)Math.Round(W2S.x,2), (float)Math.Round(W2S.y,2),Name);

        }
    }
}
