namespace Player
{
    public interface IThrowableSpawner
    {
        public bool TryGet(out IThrowable grenade);
    }
}
