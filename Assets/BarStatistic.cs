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
    private int _statistics;

    public int Statistics { get => _statistics; set { _statistics = value; if (_statistics > maxStat) _statistics = maxStat; } }

    // Start is called before the first frame update
    protected void SpawnText(int damage)
    {
        var dmgText = Instantiate(_text);
        dmgText.transform.SetParent(_minionCanvas.transform);
        dmgText.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
        dmgText.transform.localRotation = Quaternion.identity;
        dmgText.text = damage.ToString();
        dmgText.color = _textColor;
        Destroy(dmgText.gameObject, dmgText.GetComponent<Animation>().clip.length);
    }

    protected void CalculateBar()
    {
        _statBar.fillAmount = (_statistics * 1.0f) / maxStat;
    }
    protected virtual void Start()
    {
        _statistics = maxStat;
        CalculateBar();
    }

    public virtual void Restart()
    {
        _statistics = maxStat;
        CalculateBar();
    }
}
