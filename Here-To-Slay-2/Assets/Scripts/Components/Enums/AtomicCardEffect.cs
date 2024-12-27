using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Enums
{
    /// <summary>
    /// These are the smallest possible effects that a card can have.
    /// </summary>
    /// <remarks>Categorized in comments based on 3 criteria: 
    /// needsChoosingCard (by user), needsChoosingDestination, actorRelevant</remarks>
    /// TODO: Implement AtomicCardCondition enum
    public enum AtomicCardEffect
    {
        //actorRelevant
        Draw,
        //needsChoosingCard
        Recall,
        //needsChoosingCard, actorRelevant
        Discard,
        PlaySpell,
        PlayHero,
        PlayModifier,
        PlayChallenge,
        Destroy,
        StealHero,
        Sacrifice,
        Slay,
        Give,
        StealFromHand,
        //needsChoosingCard, needsChoosingDestination, actorRelevant
        PlayItem
        
        //Restore - if ever implemented, it would dig for a card in the discard pile and put it back in the hand
        
        
    }
}
