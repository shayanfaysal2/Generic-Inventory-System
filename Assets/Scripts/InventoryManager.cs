using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager: MonoBehaviour
{
    public static InventoryManager instance;

    public Item[] items;
    public int maxStackedItems;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    int selectedSlot = -1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        //toolbar selection
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeSelectedSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeSelectedSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeSelectedSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeSelectedSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeSelectedSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ChangeSelectedSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ChangeSelectedSlot(6);
        }
    }

    void ChangeSelectedSlot(int newSlot)
    {
        if (selectedSlot >= 0)
            inventorySlots[selectedSlot].Deselect();

        inventorySlots[newSlot].Select();
        selectedSlot = newSlot;
    }

    public bool AddItem(Item item)
    {
        //find slot containing same item with count lower than max
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            //if slot found
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        //find an empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            //if slot found
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    InventoryItem SpawnNewItem(Item item, InventorySlot slot)
    {
        //spawn new item in slot
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
        return inventoryItem;
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;

            //use the item
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }

            return item;
        }

        return null;
    }

    public void SaveInventory()
    {
        //save item id and count to playerprefs
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot)
            {
                PlayerPrefs.SetString("slot " + i, "item " + itemInSlot.item.id + "\ncount " + itemInSlot.count);
            }
        }

        print("Inventory saved!");
    }

    public void LoadInventory()
    {
        //load item id and count from playerprefs
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (PlayerPrefs.HasKey("slot " + i))
            {
                string str = PlayerPrefs.GetString("slot " + i);

                int item = -1;
                int itemCount = -1;

                //split the input string by newline character
                string[] lines = str.Split('\n');
                foreach (string line in lines)
                {
                    //split each line by space character
                    string[] parts = line.Split(' ');

                    //check if the line contains "item" or "count" and extract the value accordingly
                    if (parts.Length == 2)
                    {
                        if (parts[0].ToLower() == "item")
                        {
                            int.TryParse(parts[1], out item);
                        }
                        else if (parts[0].ToLower() == "count")
                        {
                            int.TryParse(parts[1], out itemCount);
                        }
                    }
                }

                //if valid
                if (item >= 0 && itemCount >= 0)
                {
                    //set in inventory
                    InventorySlot slot = inventorySlots[i];
                    if (slot.GetComponentInChildren<InventoryItem>() == null)
                    {
                        InventoryItem inventoryItem = SpawnNewItem(items[item], slot);
                        inventoryItem.count = itemCount;
                        inventoryItem.RefreshCount();

                        PlayerPrefs.DeleteKey("slot " + i);
                    }    
                }
            }
        }

        print("Inventory loaded!");
    }

    public void SortInventory()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            //if item found
            if (itemInSlot != null)
            {
                //find an empty slot
                int emptySlot = FindEmptySlot();

                if (emptySlot != -1 && emptySlot < i)
                {
                    //spawn the item in the empty slot
                    InventoryItem newItem = SpawnNewItem(itemInSlot.item, inventorySlots[emptySlot]);
                    newItem.count = itemInSlot.count;
                    newItem.RefreshCount();

                    //remove it from the previous slot
                    Destroy(itemInSlot.gameObject);
                }
            }
        }
    }

    int FindEmptySlot()
    {
        //iterate over slots and return the first empty slot found
        for (int j = 0; j < inventorySlots.Length; j++)
        {
            InventorySlot emptySlot = inventorySlots[j];
            InventoryItem itemInEmptySlot = emptySlot.GetComponentInChildren<InventoryItem>();

            if (itemInEmptySlot == null)
            {
                return j;
            }
        }

        return -1;
    }
}
