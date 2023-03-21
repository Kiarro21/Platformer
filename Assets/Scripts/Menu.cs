using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button[] Levels;
    public Text coinText;
    public Slider musicSlider, soundSlider;
    public Text musicText, soundText;
    public Sprite star, noStar;


    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("Level"))
            for (int i = 0; i < Levels.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("Level"))
                    Levels[i].interactable = true;
                else
                    Levels[i].interactable = false;
            }
        if (!PlayerPrefs.HasKey("hearthCount"))
            PlayerPrefs.SetInt("hearthCount", 0);
        if (!PlayerPrefs.HasKey("diamondCount"))
            PlayerPrefs.SetInt("diamondCount", 0);
        if (!PlayerPrefs.HasKey("sdiamondCount"))
            PlayerPrefs.SetInt("sdiamondCount", 0);

        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetInt("MusicVolume", 4);
        if (!PlayerPrefs.HasKey("SoundVolume"))
            PlayerPrefs.SetInt("SoundVolume", 7);
        for (int i = 1; i < 6; i++)
        {
            if (PlayerPrefs.HasKey("Stars" + i))
            {
                if (PlayerPrefs.GetInt("Stars" + i) == 1)
                {
                    Levels[i - 1].transform.GetChild(0).GetComponent<Image>().sprite = star;
                    Levels[i - 1].transform.GetChild(1).GetComponent<Image>().sprite = noStar;
                    Levels[i - 1].transform.GetChild(2).GetComponent<Image>().sprite = noStar;
                }
                else if (PlayerPrefs.GetInt("Stars" + i) == 2)
                {
                    Levels[i - 1].transform.GetChild(0).GetComponent<Image>().sprite = star;
                    Levels[i - 1].transform.GetChild(1).GetComponent<Image>().sprite = star;
                    Levels[i - 1].transform.GetChild(2).GetComponent<Image>().sprite = noStar;
                }
                else
                {
                    Levels[i - 1].transform.GetChild(0).GetComponent<Image>().sprite = star;
                    Levels[i - 1].transform.GetChild(1).GetComponent<Image>().sprite = star;
                    Levels[i - 1].transform.GetChild(2).GetComponent<Image>().sprite = star;
                }
            }
            else
            {
                Levels[i - 1].transform.GetChild(0).gameObject.SetActive(false);
                Levels[i - 1].transform.GetChild(1).gameObject.SetActive(false);
                Levels[i - 1].transform.GetChild(2).gameObject.SetActive(false);
            }
        }

        musicSlider.value = PlayerPrefs.GetInt("MusicVolume");
        soundSlider.value = PlayerPrefs.GetInt("SoundVolume");

    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetInt("MusicVolume", (int)musicSlider.value);
        PlayerPrefs.SetInt("SoundVolume", (int)soundSlider.value);
        musicText.text = musicSlider.value.ToString();
        soundText.text = soundSlider.value.ToString();
        if (PlayerPrefs.HasKey("Coins"))
            coinText.text = PlayerPrefs.GetInt("Coins").ToString();
        else
            coinText.text = "0";
    }

    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void DelKeys()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Buy_Health(int cost)
    {
        if (PlayerPrefs.GetInt("Coins") >= cost)
        {
            PlayerPrefs.SetInt("hearthCount", PlayerPrefs.GetInt("hearthCount") + 1);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - cost);
        }
    }
    public void Buy_Diamond(int cost)
    {
        if (PlayerPrefs.GetInt("Coins") >= cost)
        {
            PlayerPrefs.SetInt("diamondCount", PlayerPrefs.GetInt("diamondCount") + 1);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - cost);
        }
    }

    public void Buy_sDiamond(int cost)
    {
        if (PlayerPrefs.GetInt("Coins") >= cost)
        {
            PlayerPrefs.SetInt("sdiamondCount", PlayerPrefs.GetInt("sdiamondCount") + 1);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - cost);
        }
    }

}
