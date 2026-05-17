using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Code.Scripts.Audio;

public class CardPlayer : MonoBehaviour
{
    public static CardPlayer Instance { get; private set; }
    [SerializeField] private LifeValue playerLifeValue;

    public static System.Action OnPlayerDeath;

    private List<Card> hand = new List<Card>();
    private List<Card> playedCardsThisTurnList = new List<Card>();
    private List<GameObject> played3DCards = new List<GameObject>();
    private int cardsPlayedThisTurn;
    [SerializeField] private Transform handPosition;
    [SerializeField] private GameObject cardUiPrefab;
    [SerializeField] private GameObject card3dPrefab;
    [SerializeField] private Material attackMaterial;
    [SerializeField] private Material heavyAttackMaterial;
    [SerializeField] private Material shieldMaterial;
    [SerializeField] private Material heavyShieldMaterial;
    [SerializeField] private Material drawMaterial;
    [SerializeField] private Material cycleMaterial;
    [SerializeField] private Material sacrificeMaterial;

    public int GetHandCount() => hand.Count;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (playerLifeValue != null)
        {
            playerLifeValue.Initialize(100);
        }
    }

    public void TakeDamage(int amount)
    {
        if (playerLifeValue != null)
        {
            playerLifeValue.ModifyValue(-amount);
            Debug.Log($"Player took {amount} damage. Current health: {playerLifeValue.Value}");
            
            if (FloatingTextManager.Instance != null)
            {
                FloatingTextManager.Instance.Show(transform.position + Vector3.up * 0.5f, $"-{amount}", Color.red);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("SFX_Damage_Player");
            }

            if (playerLifeValue.Value <= 0)
            {
                Debug.Log("Player Dead!");
                OnPlayerDeath?.Invoke();
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX("SFX_Defeat");
                    AudioManager.Instance.StopMusic();
                }

                if (GameNotificationManager.Instance != null)
                {
                    GameNotificationManager.Instance.ShowDefeat();
                }
            }
            
            // Camera Shake feedback for taking damage
            if (Camera.main != null)
            {
                Camera.main.transform.DOShakePosition(0.5f, new Vector3(0.3f, 0.3f, 0), 20, 90, false, true);
            }
        }
    }

    private void OnEnable()
    {
        Deck.OnInitializeDeck += DrawCards;
    }

    private void OnDisable()
    {
        Deck.OnInitializeDeck -= DrawCards;
    }

    public void DrawCards(int drawAmount)
    {
        StartCoroutine(DrawCardsRoutine(drawAmount));
    }

    private System.Collections.IEnumerator DrawCardsRoutine(int drawAmount)
    {
        for (int i = 0; i < drawAmount; i++)
        {
            if (Deck.Instance.DrawStack.Count == 0)
            {
                Deck.Instance.RefillDeckFromDiscard();
            }

            if (Deck.Instance.DrawStack.Count > 0)
            {
                Card newCard = Deck.Instance.DrawStack.Pop();
                hand.Add(newCard);
                CreateCardInUi(newCard.data.Sprite, newCard);

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX("SFX_Card_Draw");
                }
            }
            else
            {
                Debug.Log("Deck is empty and no cards to refill");
                break;
            }

            // Small delay to make drawing feel extremely premium and space SFX perfectly
            yield return new WaitForSeconds(0.12f);
        }
    }

    public void DiscardHand()
    {
        foreach (Transform child in handPosition)
        {
            Destroy(child.gameObject);
        }

        foreach (var card in hand)
        {
            Deck.Instance.DiscardCard(card);
        }
        hand.Clear();
    }

    public void PlayCard(CardDisplay display, Card card)
    {
        if (hand.Contains(card) && cardsPlayedThisTurn < 3)
        {
            hand.Remove(card);
            Destroy(display.gameObject);

            Vector3 pos = CardPlacement.Instance.GetPlayerPlayPosition(cardsPlayedThisTurn);
            GameObject card3D = Instantiate(card3dPrefab, pos + Vector3.up * 2f, Quaternion.identity);
            
            MeshRenderer[] renderers = card3D.GetComponentsInChildren<MeshRenderer>(true);
            foreach (var r in renderers)
            {
                if (r.gameObject != card3D)
                {
                    if (card.data is AttackCardData)
                        r.material = card.data.isHeavy ? heavyAttackMaterial : attackMaterial;
                    else if (card.data is ShieldCardData)
                        r.material = card.data.isHeavy ? heavyShieldMaterial : shieldMaterial;
                    else if (card.data is DrawCardData)
                        r.material = drawMaterial;
                    else if (card.data is CycleCardData)
                        r.material = cycleMaterial;
                    else if (card.data is SacrificeCardData)
                        r.material = sacrificeMaterial;
                }
            }

            card3D.transform.DOMove(pos, 0.4f).SetEase(Ease.OutBounce);
            card3D.transform.localScale = Vector3.zero;
            card3D.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);

            playedCardsThisTurnList.Add(card);
            played3DCards.Add(card3D);
            cardsPlayedThisTurn++;

            if (AudioManager.Instance != null)
            {
                if (card.data is AttackCardData)
                {
                    AudioManager.Instance.PlaySFX("SFX_Card_Play_Attack");
                }
                else if (card.data is ShieldCardData)
                {
                    AudioManager.Instance.PlaySFX("SFX_Card_Play_Shield");
                }
                else
                {
                    AudioManager.Instance.PlaySFX("SFX_Card_Play_Utility");
                }
            }

            Debug.Log($"Card {card.data.CardName} placed on board.");
        }
        else if (cardsPlayedThisTurn >= 3)
        {
            Debug.Log("Limit of 3 cards per turn reached.");
        }
    }

    public System.Collections.IEnumerator ResolvePlayedCardEffectsRoutine()
    {
        Debug.Log("Resolving played card effects sequentially...");
        
        // Create a copy list to iterate safely
        List<Card> tempCards = new List<Card>(playedCardsThisTurnList);
        
        for (int i = 0; i < tempCards.Count; i++)
        {
            Card card = tempCards[i];
            
            // Visual feedback: Make the corresponding 3D board card jump and scale slightly when it activates!
            if (i < played3DCards.Count && played3DCards[i] != null)
            {
                GameObject card3D = played3DCards[i];
                card3D.transform.DOKill();
                card3D.transform.DOPunchPosition(Vector3.up * 0.4f, 0.4f, 5, 1);
                card3D.transform.DOPunchScale(Vector3.one * 0.12f, 0.4f, 5, 1);
            }

            card.Play();
            Deck.Instance.DiscardCard(card);

            // Wait 0.8 seconds between card activations to appreciate float texts, stats, and SFX individually
            yield return new WaitForSeconds(0.8f);
        }

        playedCardsThisTurnList.Clear();
        cardsPlayedThisTurn = 0;
    }

    public void ClearPlayedCardsVisuals()
    {
        foreach (var obj in played3DCards)
        {
            obj.transform.DOShakeScale(0.3f, 0.5f).OnComplete(() => {
                obj.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => {
                    Destroy(obj);
                });
            });
        }

        played3DCards.Clear();
    }

    public int GetShieldCardsCount()
    {
        int count = 0;
        foreach (var card in hand)
        {
            if (card.data is ShieldCardData)
            {
                count++;
            }
        }

        return count;
    }

    private void Update()
    {
        // Use the new Input System API since Legacy Input is disabled in Player Settings
        if (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.rightButton.wasPressedThisFrame)
        {
            UndoLastPlayedCard();
        }
    }

    public void UndoLastPlayedCard()
    {
        // Only allow undoing during the Player's active turn
        if (TurnManager.Instance != null && TurnManager.Instance.GetTurn() != 0)
        {
            Debug.Log("[Undo] Cannot undo cards during enemy or cleanup turns!");
            return;
        }

        if (playedCardsThisTurnList.Count == 0 || played3DCards.Count == 0)
        {
            Debug.Log("[Undo] No cards played this turn to undo!");
            return;
        }

        // Get last played card data and remove from queue
        int lastIndex = playedCardsThisTurnList.Count - 1;
        Card lastCard = playedCardsThisTurnList[lastIndex];
        playedCardsThisTurnList.RemoveAt(lastIndex);

        // Get last played 3D card model on board and remove from queue
        int last3DIndex = played3DCards.Count - 1;
        GameObject last3D = played3DCards[last3DIndex];
        played3DCards.RemoveAt(last3DIndex);

        // Satisfying shrink animation before destroying the 3D board card
        if (last3D != null)
        {
            last3D.transform.DOKill();
            last3D.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => {
                Destroy(last3D);
            });
        }

        // Add back to data hand list
        hand.Add(lastCard);

        // Recreate the UI visual card in your hand
        CreateCardInUi(lastCard.data.Sprite, lastCard);

        // Decrement our played cards slot counter
        cardsPlayedThisTurn--;

        // Play a nice physical draw feedback sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("SFX_Card_Draw");
        }

        Debug.Log($"[Undo] Returned {lastCard.data.CardName} to hand. Played slot reset to: {cardsPlayedThisTurn}");
    }

    private void CreateCardInUi(Sprite sprite, Card card)
    {
        var c = Instantiate(cardUiPrefab, handPosition);
        c.transform.SetParent(handPosition);
        c.transform.localScale = Vector3.one;
        c.transform.localPosition = Vector3.zero;
        c.GetComponent<CardDisplay>().Setup(sprite, card);
    }
}