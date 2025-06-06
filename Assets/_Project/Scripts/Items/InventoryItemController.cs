﻿using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class InventoryItemController : MonoBehaviour 
    {
        Item item;

        public Button RemoveButton;
        public void RemoveItem()
        { 
            InventoryManager.Instance.Remove(item);

            Destroy(gameObject);
        }

        public void AddItem(Item newItem)
        { 
            item = newItem;

        }
    }
}
