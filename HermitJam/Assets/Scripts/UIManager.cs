using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using  DG.Tweening;

public class UIManager : MonoBehaviour
{
    
    
    [SerializeField] TextMeshProUGUI _scoreUI;
    [SerializeField] TextMeshProUGUI _timeUI;
    [SerializeField] TextMeshProUGUI _scoreUIEndGame;
    [SerializeField] TextMeshProUGUI _timeUIEndGame;

    [SerializeField] private GameObject _endGameMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(float score, bool updateEndGameMenu = true)
    {
        if(_scoreUI) _scoreUI.SetText(score.ToString());
        if(_scoreUIEndGame && updateEndGameMenu) _scoreUIEndGame.SetText(score.ToString());
    }
    public void UpdateTime(int seconds, bool updateEndGameMenu = true)
    {
        var ts = new TimeSpan(0, 0, seconds);                
        string time = new DateTime(ts.Ticks).ToString(seconds >= 3600 ? "hh:mm:ss" : "mm:ss");
        if(_timeUI) _timeUI.SetText(time);
        if(_timeUIEndGame && updateEndGameMenu) _timeUIEndGame.SetText(time);
    }

    public void ShowEndMenu()
    {
        if(_endGameMenu == null) return;

        _endGameMenu.transform.localScale = new Vector3(0f, 0f, 1f);
        _endGameMenu.SetActive(true);
        _endGameMenu.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
    }
}
