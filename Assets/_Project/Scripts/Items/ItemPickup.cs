using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
namespace Platformer
{
    public class ItemPickup : MonoBehaviour 
    {
        public Item Item;

        void Pickup()
        {
            if (!InventoryManager.Instance.Items.Contains(Item))
            {
                InventoryManager.Instance.Add(Item);
                Destroy(gameObject);
            }
            else
            { 
                Debug.Log("Item already in inventory: " + Item.name);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup();
            }
        }
    }
}
