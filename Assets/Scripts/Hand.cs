using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private Card _cardPrefab;
    [SerializeField]
    private RectTransform _startPosition;

    public RectTransform[] CardsPositions;
    public List<Card> Cards;

    public void StartGenerate()
    {
        StartCoroutine(GenerateCards());
    }

    private IEnumerator GenerateCards()
    {
        for (int i = 0; i < Random.Range(4, 7); i++)
        {
            Card card = Instantiate(_cardPrefab, _startPosition);
            card.CurrentPosition = Cards.Count;
            card.transform.SetParent(CardsPositions[Cards.Count]);
            Cards.Add(card);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void GenerateCard()
    {
        if (Cards.Count < 10)
        {
            Card card = Instantiate(_cardPrefab, _startPosition);
            card.CurrentPosition = Cards.Count;
            card.transform.SetParent(CardsPositions[Cards.Count]);
            Cards.Add(card);
        }
    }

    public void AddToHand(Card card)
    {
        if (Cards.Count < 10)
        {
            card.CurrentPosition = Cards.Count;
            card.transform.SetParent(CardsPositions[Cards.Count]);
            Cards.Add(card);
        }
    }

    public void ChangeProperty()
    {
        for(int i = 0; i < Cards.Count; i++)
        {
            Cards[i].Health = Random.Range(-2, 9);
            Cards[i].SetProperties();
        }
    }

    public void UpdateCards(int id)
    {
        Cards.RemoveAt(id);

        for(int i = 0; i < Cards.Count; i++)
        {
            if(Cards[i].CurrentPosition > id)
            {
                Cards[i].CurrentPosition--;
                Cards[i].transform.parent = CardsPositions[Cards[i].CurrentPosition];
            }
        }
    }
}
