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
            Name = "dsgdsg";
           
        }
        public LootItem Entity;
        public string Name;
        public Vector3 W2S;
        public Rectangle Rectangle;
        public void UpdateInformation()
        {
            if (Globals.GameWorld == null)
                return;
            Name = Entity.Item.Name.LocalizedShortName();
            W2S = Globals.WorldToScreen(Entity.transform.position);
            Rectangle = new Rectangle(W2S.x, W2S.y);

        }
    }
}
