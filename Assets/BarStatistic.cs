using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public abstract class BarStatistic : MonoBehaviour
{
    [SerializeField] protected int _maxStat = 100;
    [SerializeField] protected Image _statBar;
    [SerializeField] protected TextMeshProUGUI _text;
    [SerializeField] protected Canvas _minionCanvas;
    protected int _statistic;
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
        _statBar.fillAmount = (_statistic * 1.0f) / _maxStat;
    }
    protected virtual void Start()
    {
        Debug.Log("test1");
        _statistic = _maxStat;
        CalculateBar();
    }
}
