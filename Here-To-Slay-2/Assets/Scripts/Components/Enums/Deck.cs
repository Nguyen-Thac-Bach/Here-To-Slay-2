namespace Components.Enums
{
    /// <summary>
    /// Possible places where a card can be
    /// </summary>
    public enum Deck
    {
        DrawDeck,
        DiscardDeck,
        MonsterDeck,
        AttackableMonsters,
        Player1Hand,
        Player1Field,
        Player1SlainMonsters,
        Player2Hand,
        Player2Field,
        Player2SlainMonsters,
        None
        //todo: add slain monsters for each player in view as well - 3 limit decks
    }

}

