﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTxt = null;
    [SerializeField] private Button playBtn = null;
    [SerializeField] private Button menuBtn = null;
    [SerializeField] private Button playAgainBtn = null;
    [SerializeField] private GameObject pausePanel = null;
    [SerializeField] private GameObject previousLevel;
    [SerializeField] private GameObject nextLevel;

    private Animator scoreAnim;

    private void Awake()
    {
        scoreAnim = scoreTxt.GetComponent<Animator>();
    }

    public void Menu()
    {
        menuBtn.gameObject.SetActive(false);
        playAgainBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
        pausePanel.SetActive(false);
        scoreAnim.SetBool("Show", false);

        previousLevel.SetActive(true);
        nextLevel.SetActive(true);
    }

    public void Play()
    {
        playBtn.gameObject.SetActive(false);
        menuBtn.gameObject.SetActive(false);
        playAgainBtn.gameObject.SetActive(false);
        scoreAnim.SetBool("Show", true);
        scoreTxt.enabled = true;

        previousLevel.SetActive(false);
        nextLevel.SetActive(false);
    }

    public void UpdateScore(int score) => scoreTxt.text = score.ToString();

    public void ShowPausePanel() => pausePanel.SetActive(true);
    public void HidePausePanel() => pausePanel.SetActive(false);

    public void GameOver()
    {
        menuBtn.gameObject.SetActive(true);
        playAgainBtn.gameObject.SetActive(true);
    }

}
