using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private TextMeshProUGUI timeCountText;
    
    
    [SerializeField]private int levelNmbr;
    private int _killCount;

    public List<EnemyBase> Enemies = new List<EnemyBase>();
    
    private int _seconds;
    private int _fullseconds;
    private int _minutes;

    private void Awake()
    {
        Enemies = new List<EnemyBase>(FindObjectsOfType<EnemyBase>());
    }
    
    private void Start()
    {
        _killCount = 0;
        _seconds = 0;
        _fullseconds = 0;
        _minutes = 0;
        
        levelText.text = "Level: " + levelNmbr;
        killCountText.text = "Kills: " + _killCount;
        timeCountText.text = "Time: " + _minutes + ":" + _fullseconds+ _seconds;
        StartCoroutine(Counter());
    }

    public void AddKill()
    {
        _killCount++;
        killCountText.text = "Kills: " + _killCount;
    }
    

    private IEnumerator Counter()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _seconds += 1;
            if (_seconds == 10)
            {
                _seconds = 0;
                _fullseconds++;
                if (_fullseconds == 6)
                {
                    _fullseconds = 0;
                    _minutes++;
                }
            }
            timeCountText.text = "Time: " + _minutes + ":" + _fullseconds+ _seconds;

        }
    }
    
}
