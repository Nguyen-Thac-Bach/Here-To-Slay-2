
using Components.Enums;
using Unity.Collections;

namespace Components {
    public class BaseCard
    {
        #region Fields
        private int _cardId;
        private string _name;
        private string _description;
        private bool _isViewable;
        private Deck _deck;
        private int _cardPosition;

        #endregion

        #region Properties
        public int CardId { get => _cardId; }
        public string Name { get => _name; }
        //TODO: add image field
        public string Description { get => _description; }
        public bool IsViewable { get => _isViewable; }
        public Deck Deck { get => _deck; }
        /// <summary>
        /// Relevant for cards in the player's hand, field; attackable monsters
        /// </summary>
        public int CardPosition { get => _cardPosition; }

        #endregion

        #region Constructors
        public BaseCard(int cardId, string name, string description, bool isViewable, Deck deck)
        {
            _cardId = cardId;
            _name = name;
            _description = description;
            _isViewable = isViewable;
            _deck = deck;
            _cardPosition = -1;
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Make the card face up
        /// </summary>
        public void Show()
        {
            _isViewable = true;
        }
        /// <summary>
        /// Make the card face down
        /// </summary>
        public void Hide()
        {
            _isViewable = false;
        }
        /// <summary>
        /// Move the card to the specified deck
        /// </summary>
        /// <param name="deck"></param>
        public void SetDeck(Deck deck)
        {
            _deck = deck;
        }
        /// <summary>
        /// Moves the card to the specified position within the deck
        /// </summary>
        /// <param name="cardPosition">-1 if the deck it is in has no limits to respect</param>
        public void SetCardPosition(int cardPosition)
        {
            _cardPosition = cardPosition;
        }
        #endregion
    }
}

