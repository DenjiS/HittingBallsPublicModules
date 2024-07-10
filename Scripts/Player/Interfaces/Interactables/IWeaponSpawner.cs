namespace Player
{
    public interface IWeaponSpawner
    {
        public bool TryGet(out IWeapon spawnedWeapon);
    }
}