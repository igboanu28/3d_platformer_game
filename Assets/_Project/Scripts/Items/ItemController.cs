using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Platformer
{
    public class ItemController : MonoBehaviour
    {
        public Item Item;

        public void AddToInventory()
        {
            InventoryManager.Instance.Add(Item);
            Destroy(gameObject);
        }

    }

}
