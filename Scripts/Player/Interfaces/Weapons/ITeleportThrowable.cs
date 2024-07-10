using UnityEngine;

namespace Player
{
    public interface ITeleportThrowable : IThrowable
    {
        public void SetPlayerTransform(Transform transform);
    }
}