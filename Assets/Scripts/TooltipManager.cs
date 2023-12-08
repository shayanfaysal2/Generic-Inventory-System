using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;

    public GameObject tooltip;
    public Text tooltipText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tooltip.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        if (tooltip.activeSelf)
            tooltip.transform.position = Input.mousePosition;
    }

    public void ShowTooltip(Item item)
    {
        //set the tooltip text and display it
        tooltipText.text =
            "Name: " + item.name + "\n" +
            "Description: " + item.description + "\n" +
            "Weight: " + item.weight + " kg \n" +
            "Value: " + item.value + " coins \n" +
            "Rarity: " + item.rarity;
        tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        //hide the tooltip
        tooltip.SetActive(false);
        tooltipText.text = string.Empty;
    }

}
