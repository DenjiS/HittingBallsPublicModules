using UnityEngine;

public class EffectArea : MonoBehaviour
{
    [SerializeField] private BaseScriptableEffect _effect;
    [SerializeField] private float _duration;

    private void Start()
    {
        Destroy(gameObject, _duration);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IEffectReceiver effectReceiver))
            effectReceiver.ApplyEffect(_effect);
    }
}
