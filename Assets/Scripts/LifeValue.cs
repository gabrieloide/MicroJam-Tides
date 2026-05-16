using UnityEngine;

[CreateAssetMenu(fileName = "NewLifeValue", menuName = "LifeValue")]
public class LifeValue: ScriptableObject{
    [SerializeField] private int value;
    public int Value { get => value; private set => value = value; }
}