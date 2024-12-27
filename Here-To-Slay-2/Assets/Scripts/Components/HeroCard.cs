using UnityEngine;
using System.Collections.Generic;
using Components.Enums;
namespace Components
{
    public class HeroCard : BaseCard
    {
        #region Fields
        private int _minRoll;
        private List<Effect> _effects;
        private HeroClass _baseHeroClass;
        private HeroClass _currentHeroClass;
        /// <summary>
        /// Tracks if the card has been activated. Normally only once per turn
        /// </summary>
        private bool _isActivatable;
        //TODO: Add ItemCard when implemented
        //private ItemCard _attachedItem;
        #endregion
        #region Properties
        public int MinRoll { get => _minRoll; }
        public List<Effect> Effects { get => _effects; }
        public HeroClass BaseHeroClass { get => _baseHeroClass; }
        public HeroClass CurrentHero { get => _currentHeroClass; }
        public bool IsActivatable { get => _isActivatable; }
        #endregion
        #region Constructors
        public HeroCard(int cardId,string name, string description,bool isViewable,Deck deck, int minRoll, List<Effect> effects, HeroClass baseHeroClass, HeroClass currentHeroClass, bool isActivatable) : base(cardId, name, description, isViewable, deck)
        {
            _minRoll = minRoll;
            _effects = effects;
            _baseHeroClass = baseHeroClass;
            _currentHeroClass = currentHeroClass;
            _isActivatable = isActivatable;
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Activates the card. Normally called by GameManager when the card is played
        /// </summary>
        /// <remarks>
        /// GameManager will read the effects of the card and apply them to the game state
        /// </remarks>
        public void Activate()
        {
            //TODO: Implement GameManager
            _isActivatable = false;
        }
        /// <summary>
        /// Resets the card to be activatable again. Normally called at the start of a turn
        /// </summary>
        public void Reset()
        {
            _isActivatable = true;
        }
        public void ChangeHeroClass(HeroClass newHeroClass)
        {
            _currentHeroClass = newHeroClass;
        }
        public void ResetHeroClass()
        {
            _currentHeroClass = _baseHeroClass;
        }
        //public void AttachItem(ItemCard item)
        //public void DetachItem()
        #endregion
    }
}

