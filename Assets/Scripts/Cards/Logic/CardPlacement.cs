using UnityEngine;

public class CardPlacement : MonoBehaviour
{
    public static CardPlacement Instance { get; private set; }
    [SerializeField] private Transform _playerPlayPosition;
    [SerializeField] private float spacing = 1.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Vector3 GetPlayerPlayPosition(int index)
    {
        float offsetMultiplier = index - 1;
        return _playerPlayPosition.position + (_playerPlayPosition.right * (spacing * offsetMultiplier));
    }
}