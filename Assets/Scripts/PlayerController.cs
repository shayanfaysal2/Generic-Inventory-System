using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private void Start()
    {

    }

    void Update()
    {
        //simple player movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        transform.Translate(movement * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collecting item
        if (collision.CompareTag("Item"))
        {
            if (collision.TryGetComponent<CollectibleItem>(out CollectibleItem component))
            {
                if (InventoryManager.instance.AddItem(component.item))
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
