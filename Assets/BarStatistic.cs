using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public abstract class BarStatistic : MonoBehaviour
{
    [SerializeField] protected int maxStat = 100;
    [SerializeField] private Image _statBar;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Canvas _minionCanvas;
    [SerializeField] private Color _textColor;
    protected int statistic;
    // Start is called before the first frame update
    protected void SpawnText(int damage)
    {
        var dmgText = Instantiate(_text);
        dmgText.transform.SetParent(_minionCanvas.transform);
        dmgText.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
        dmgText.text = damage.ToString();

        Destroy(dmgText.gameObject, dmgText.GetComponent<Animation>().clip.length);
    }

    protected void CalculateBar()
    {
        _statBar.fillAmount = (statistic * 1.0f) / maxStat;
    }
    protected virtual void Start()
    {
        _text.color = _textColor;
        statistic = maxStat;
        CalculateBar();
        
    }
}
