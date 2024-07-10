using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RandomArray<T>
{
    [SerializeField] private T[] _array;

    private int _currentIndex;

    public RandomArray() { }

    public RandomArray(IEnumerable<T> values)
    {
        _array = values.ToArray();
    }

    public T this[int index]
    {
        get => _array[index];
    }

    public T GetNewRandom()
    {
        if (_array.Length == 0)
            throw new System.Exception("random array empty");

        int index;

        do if (_array.Length >= 2)
            {
                index = Random.Range(0, _array.Length);
            }
            else
            {
                index = 0;
                break;
            }
        while (index == _currentIndex);

        _currentIndex = index;

        return _array[index];
    }
}
