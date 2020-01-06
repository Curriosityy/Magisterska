using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Minion : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private Image _healthBar;
    [SerializeField] private TextMeshProUGUI _dmgText;
    [SerializeField] private Canvas _minionCanvas;
    private int _health;
    public void DealDamage(int damagage)
    {
        var oldHp = _health;
        _health -= damagage;
        CalculateHealthBar();
        SpawnText(damagage);
        if (_health <= 0)
        {
            Die();
        }
        Debug.Log(string.Format("{0} Took {1} DMG, HP Before {2} HP now {3}", gameObject.name, damagage, oldHp, _health));
    }

    private void SpawnText(int damage)
    {
        var dmgText = Instantiate(_dmgText);
        dmgText.transform.SetParent(_minionCanvas.transform);
        dmgText.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
        dmgText.text = damage.ToString();
        Destroy(dmgText.gameObject, dmgText.GetComponent<Animation>().clip.length);
    }

    private void Die()
    {
        //throw new NotImplementedException();
    }

    private void CalculateHealthBar()
    {
        _healthBar.fillAmount = (_health * 1.0f) / _maxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;
        CalculateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
