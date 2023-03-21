using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int hearthCount = 0, diamondCount = 0, sdiamondCount = 0, keyCount = 0;
    public Sprite[] numbers;
    public Image hearth_img, diamond_img, sdiamond_img, key_img;
    public Player player;

    private void Start()
    {
        if (PlayerPrefs.GetInt("hearthCount") > 0)
        {
            hearthCount = PlayerPrefs.GetInt("hearthCount");
            hearth_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hearthCount];
        }
        if (PlayerPrefs.GetInt("diamondCount") > 0)
        {
            diamondCount = PlayerPrefs.GetInt("diamondCount");
            diamond_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[diamondCount];
        }
        if (PlayerPrefs.GetInt("sdiamondCount") > 0)
        {
            sdiamondCount = PlayerPrefs.GetInt("sdiamondCount");
            sdiamond_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[sdiamondCount];
        }
    }

    public void Add_HP()
    {
        hearthCount++;
        hearth_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hearthCount];
    }

    public void Add_Diamond()
    {
        diamondCount++;
        diamond_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[diamondCount];
    }

    public void Add_sDiamond()
    {
        sdiamondCount++;
        sdiamond_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[sdiamondCount];
    }

    public void Add_Key()
    {
        keyCount++;
        key_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[keyCount];
    }

    public void Use_HP()
    {
        if (hearthCount > 0 && player.currentHP < 3)
        {
            hearthCount--;
            player.RecountHP(1);
            hearth_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hearthCount];
        }
    }

    public void Use_Diamond()
    {
        if (diamondCount > 0)
        {
            diamondCount--;
            player.diamondActivating();
            diamond_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[diamondCount];
        }
    }

    public void Use_sDiamond()
    {
        if (sdiamondCount > 0)
        {
            sdiamondCount--;
            player.sdiamondActivating();
            sdiamond_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[sdiamondCount];
        }
    }

    public void RecountItems()
    {
        PlayerPrefs.SetInt("hearthCount", hearthCount);
        PlayerPrefs.SetInt("diamondCount", diamondCount);
        PlayerPrefs.SetInt("sdiamondCount", sdiamondCount);
    }
}
