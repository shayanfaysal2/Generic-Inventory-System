using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Item[] itemsToPickup;

    // Update is called once per frame
    void Update()
    {
        //open/close full inventory from Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(false);
            }
            else
            {
                inventoryPanel.SetActive(true);
            }
        }
    }

    public void PickupItem(int id)
    {
        //add item
        bool result = InventoryManager.instance.AddItem(itemsToPickup[id]);
        if (result)
            print("Item Added!");
        else
            print("Inventory Full!");
    }

    public void GetSelectedItem()
    {
        //get item
        Item recievedItem = InventoryManager.instance.GetSelectedItem(false);
        if (recievedItem != null)
        {
            print("Recieved item: " + recievedItem);
        }
        else
        {
            print("No item recieved!");
        }
    }

    public void UseSelectedItem()
    {
        //use item
        Item recievedItem = InventoryManager.instance.GetSelectedItem(true);
        if (recievedItem != null)
        {
            print("Used item: " + recievedItem);
        }
        else
        {
            print("No item used!");
        }
    }
}
