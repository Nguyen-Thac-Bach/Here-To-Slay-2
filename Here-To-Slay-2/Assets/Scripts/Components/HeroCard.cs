using UnityEngine;

using Components.Enums;
namespace Components
{
    public class HeroCard : BaseCard
    {
        #region Fields
        private int _minRoll;
        private Effect[] _effects;
        private HeroClass _baseHeroClass;
        private HeroClass _currentHeroClass;
        private bool _isActivatable;
        //TODO: Add ItemCard when implemented
        //private ItemCard _attachedItem;
        #endregion
        #region Properties
        public int MinRoll { get => _minRoll; }
        public Effect[] Effects { get => _effects; }
        public HeroClass BaseHeroClass { get => _baseHeroClass; }
        public HeroClass CurrentHero { get => _currentHeroClass; }
        public bool IsActivatable { get => _isActivatable; }
        #endregion
        #region Constructors
        public HeroCard(int cardId,string name, string description,bool isViewable, int minRoll, Effect[] effects, HeroClass baseHeroClass, HeroClass currentHeroClass, bool isActivatable) : base(cardId, name, description, isViewable)
        {
            _minRoll = minRoll;
            _effects = effects;
            _baseHeroClass = baseHeroClass;
            _currentHeroClass = currentHeroClass;
            _isActivatable = isActivatable;
        }
    }
}

