using UnityEngine;

public interface IPhysics 
{
    public Transform BaseTransform { get; }
    public Rigidbody BaseRigidbody { get; }
}