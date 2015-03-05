public class GameManager : Singleton<GameManager>
{
    #region Constructor & Variables

    protected GameManager() { }

    public CharacterState characterState = CharacterState.Sketch;

    #endregion

    #region Getters & Setters

    public CharacterState GetCharacterState()
    {
        return characterState;
    }

    public void SetCharacterState(CharacterState pCharacterState)
    {
        characterState = pCharacterState;
    }

    #endregion
}
