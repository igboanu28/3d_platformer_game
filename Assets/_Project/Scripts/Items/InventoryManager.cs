using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace Platformer
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        public List<Item> Items = new List<Item>();

        public Transform ItemContent;
        public GameObject InventoryItem;

        public Toggle EnableDelete;

        public InventoryItemController[] InventoryItems;

        private void Awake()
        {
            Instance = this;
        }

        public void Add(Item item)
        {
            Items.Add(item);
            Debug.Log($"Item '{item.itemName}' added to inventory!");
        }

        public void Remove(Item item)
        {
            Items.Remove(item);
            Debug.Log($"Item '{item.itemName}' removed from inventory!");

        }

        public void ListItems()
        {
            // i call it here to clear the content before adding new items to the inventory ui
            CleanContent(); 
            foreach (var item in Items)
            {
                GameObject obj = Instantiate(InventoryItem, ItemContent);
                var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
                var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
                var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();

                itemName.text = item.itemName;
                itemIcon.sprite = item.icon;

                if (EnableDelete.isOn)
                    removeButton.gameObject.SetActive(true);
            }

            SetInventoryItems();
        }

        //this method will help clear items from the inventoy because the ListItems method will keep adding items to the inventory
        private void CleanContent()
        {
            foreach (Transform child in ItemContent)
            {
                Destroy(child.gameObject);
            }
        }

        //this method will be called when the player closes the inventory
        public void CloseInventory()
        {
            CleanContent();
            // Hide inventory panel or perform other closing operations
        }

        public void EnableItemsRemove()
        {
            if (EnableDelete.isOn)
            {
                foreach (Transform item in ItemContent)
                { 
                    item.Find("RemoveButton").gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (Transform item in ItemContent)
                {
                    item.Find("RemoveButton").gameObject.SetActive(false);
                }
            }
        }

        public void SetInventoryItems()
        { 
            InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();

            for (int i = 0; i < Items.Count; i++)
            { 
                InventoryItems[i].AddItem(Items[i]);
            }
        }

        public void CollectItemForQuest(string objective, int quantity)
        {
            foreach (var quest in QuestManager.Instance.CurrentQuestList)
            {
                if (quest.questObjective == objective)
                {
                    quest.questObjectiveCount += quantity;

                    if (quest.IsComplete)
                    {
                        QuestManager.Instance.CompleteQuest(quest); // Complete quest
                    }
                }
            }
        }

    }
}
