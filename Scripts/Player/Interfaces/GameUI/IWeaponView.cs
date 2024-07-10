using UnityEngine;

public interface IWeaponView
{
    public GameObject gameObject { get; }

    public int CurrentAmmo { get; set; }

    public void Initialize(int maxAmmo, int currentAmmo);

    public void RenderReload(float max, float current);

    public void ShowTotalAmmo(int totalAmmo);

    public void Clear();
}