using UnityEngine;
using UnityEngine.Events;

public class Health : IDamageableHealable, IDeathNotifier
{
    private readonly IHealthView _view;
    private readonly float _maxHealth;

    private float _currentHealth;

    public event UnityAction<IDeathNotifier> Died;

    public float MaxHealth => _maxHealth;

    public Health(IHealthView view, float maxHealth)
    {
        _view = view;
        _maxHealth = maxHealth;

        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            _currentHealth -= damage;

            _view.RenderHealth(Mathf.Clamp01(_currentHealth / _maxHealth));

            if (_currentHealth <= 0)
            {
                _view.Disable();
                Died?.Invoke(this);
            }
        }
    }

    public void TakeHeal(float health)
    {
        if (health > 0)
        {
            _currentHealth = Mathf.Min(_currentHealth + health, _maxHealth);

            _view.RenderHealth(Mathf.Clamp01(_currentHealth / _maxHealth));
        }
    }
}
