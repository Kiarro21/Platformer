using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    int Invent = 0;
    public Player player;
    public Text coinsText, timeText;
    public Image[] hearts;
    public Sprite isLife, noLife;
    public GameObject PauseScreen, WinScreen, LoseScreen, InvertoryButtons, Invertory;
    float timer = 0f;
    public TimeWork KindTime;
    public float countDown;
    public SoudnEffector soundEffector;
    public AudioSource musicSource, soundSource;
    int allCoins;

    private void Start()
    {
        musicSource.volume = (float)PlayerPrefs.GetInt("MusicVolume") / 10;
        soundSource.volume = (float)PlayerPrefs.GetInt("SoundVolume") / 10;
        if ((int)KindTime == 2)
            timer = countDown;
        allCoins = GameObject.FindGameObjectsWithTag("coin").Length;
    }

    public void Update()
    {
        coinsText.text = player.GetCoins().ToString();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (player.GetHP() > i)
                hearts[i].sprite = isLife;
            else
                hearts[i].sprite = noLife;
        }

        if ((int)KindTime == 1)
        {
            timer += Time.deltaTime;
            timeText.text = timer.ToString("F2").Replace(",", ":");
        }
        else if ((int)KindTime == 2)
        {
            timer -= Time.deltaTime;
            timeText.text = timer.ToString("F2").Replace(",", ":");
            if (timer <= 0)
                Lose();
        }
        else
        {
            timeText.gameObject.SetActive(false);
        }
    }

    public void PauseOn()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        PauseScreen.SetActive(true);
    }

    public void PauseOff()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        PauseScreen.SetActive(false);
    }

    public void Win()
    {
        soundEffector.PlayWinSound();
        Time.timeScale = 0f;
        player.enabled = false;
        WinScreen.SetActive(true);

        if (!PlayerPrefs.HasKey("Level") || PlayerPrefs.GetInt("Level") < SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        }

        if (PlayerPrefs.HasKey("Coins"))
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + player.GetCoins());
        else
            PlayerPrefs.SetInt("Coins", player.GetCoins());
        Invertory.SetActive(false);

        if ((player.GetCoins() / allCoins) < 0.33f && !PlayerPrefs.HasKey("Stars" + SceneManager.GetActiveScene().buildIndex))
            PlayerPrefs.SetInt("Stars" + SceneManager.GetActiveScene().buildIndex, 1);
        else if ((player.GetCoins() / allCoins) >= 0.33f && (player.GetCoins() / allCoins) < 0.66f && (!PlayerPrefs.HasKey("Stars" + SceneManager.GetActiveScene().buildIndex) || PlayerPrefs.GetInt("Stars" + SceneManager.GetActiveScene().buildIndex) < 2))
            PlayerPrefs.SetInt("Stars" + SceneManager.GetActiveScene().buildIndex, 2);
        else if ((player.GetCoins() / allCoins) > 0.66f && (!PlayerPrefs.HasKey("Stars" + SceneManager.GetActiveScene().buildIndex) || PlayerPrefs.GetInt("Stars" + SceneManager.GetActiveScene().buildIndex) < 3))
            PlayerPrefs.SetInt("Stars" + SceneManager.GetActiveScene().buildIndex, 3);


        GetComponent<Inventory>().RecountItems();
    }

    public void Lose()
    {
        soundEffector.PlayLoseSound();
        Time.timeScale = 0f;
        player.enabled = false;
        Invertory.SetActive(false);
        LoseScreen.SetActive(true);
    }

    public void LevelMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        player.enabled = true;
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        player.enabled = true;
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
        player.enabled = true;
    }

    public void Inventory()
    {
        switch(Invent)
        {
            case 0:
                Invertory.SetActive(true);
                Invent = 1;
                break;
            case 1:
                Invertory.SetActive(false);
                Invent = 0;
                break;
        }
    }
}

public enum TimeWork
{
    None,
    StopWatch,
    Timer
}
