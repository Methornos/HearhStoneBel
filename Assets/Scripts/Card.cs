using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform _selfRect;

    private RectTransform _selectedCard;

    private string _path;
    private bool _isSelected = false;
    private bool _isTaken = false;
    private bool _onTable = false;

    private CardsKeeper _keeper;
    private Hand Hand;

    public string Name;
    public int Attack;
    public int Health;
    public string Description;

    public Image CardImage;
    public Text CardName;
    public Text CardAttack;
    public Text CardHealth;
    public Text CardDescription;

    [Space]
    public int CurrentPosition;

    private void Start()
    {
        _keeper = GameObject.Find("CardsKeeper").GetComponent<CardsKeeper>();
        Hand = GameObject.FindWithTag("Hand").GetComponent<Hand>();

        _selectedCard = GameObject.Find("SelectedCard").GetComponent<RectTransform>();

        _selfRect = GetComponent<RectTransform>();

        int randomCard = Random.Range(0, _keeper.CardsVariants.Length);

        CardImage.sprite = _keeper.Sprites[randomCard];

        Name = _keeper.CardsVariants[randomCard].Name;
        Attack = _keeper.CardsVariants[randomCard].Attack;
        Health = _keeper.CardsVariants[randomCard].Health;
        Description = _keeper.CardsVariants[randomCard].Description;

        SetProperties();
    }

    private void Update()
    {
        if (Health <= 0)
        {
            Hand.UpdateCards(CurrentPosition);
            Destroy(gameObject);
        }

        if (!_onTable)
        {
            if (!_isTaken)
            {
                if (!_isSelected)
                {
                    _selfRect.localPosition = Vector2.Lerp(_selfRect.position, _selfRect.parent.position, 0.1f);
                    _selfRect.localEulerAngles = Vector3.Lerp(_selfRect.localEulerAngles, _selfRect.parent.localEulerAngles, 1f);
                    _selfRect.localScale = Vector3.Lerp(_selfRect.localScale, new Vector3(1, 1, 1), 0.5f);
                }
                else
                {
                    _selfRect.localEulerAngles = Vector3.Lerp(_selfRect.localEulerAngles, _selectedCard.localEulerAngles, 0.8f);
                    _selfRect.localScale = Vector3.Lerp(_selfRect.localScale, _selectedCard.localScale, 0.7f);
                }
            }
        }
    }

    public void SetProperties()
    {
        CardName.text = Name;
        CardAttack.text = Attack.ToString();
        CardHealth.text = Health.ToString();
        CardDescription.text = Description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_onTable)
        {
            Vector3 currentCard = Hand.CardsPositions[CurrentPosition].position;
            _selectedCard.position = new Vector3(currentCard.x, currentCard.y + 100, currentCard.z);
            _selfRect.parent = _selectedCard;
            _isSelected = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_onTable)
        {
            _selfRect.parent = Hand.CardsPositions[CurrentPosition];
            _isSelected = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Table.OpenTable();
        _isTaken = true;
        GetComponent<Image>().enabled = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            List<GameObject> hovered = eventData.hovered;
            for (int i = 0; i < hovered.Count; i++)
            {
                if (hovered[i].transform.tag == "Table")
                {
                    Hand.UpdateCards(CurrentPosition);
                    _selfRect.parent = hovered[i].transform;
                    _onTable = true;
                }
            }
        }
        _isTaken = false;
        GetComponent<Image>().enabled = false;

    }

    public void OnDrag(PointerEventData eventData) => _selfRect.localPosition += (Vector3)eventData.delta;
}
