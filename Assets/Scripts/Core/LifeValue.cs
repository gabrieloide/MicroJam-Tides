using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewLifeValue", menuName = "LifeValue")]
public class LifeValue : ScriptableObject
{
    [SerializeField] private int value;
    public int Value => value;

    public Action OnValueChanged;

    public void Initialize(int initialValue)
    {
        value = initialValue;
        OnValueChanged?.Invoke();
    }

    public void ModifyValue(int amount)
    {
        value += amount;
        value = Mathf.Max(0, value);
        OnValueChanged?.Invoke();
    }
}