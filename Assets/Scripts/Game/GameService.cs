using UnityEngine;

public class GameService : MonoBehaviour
{
    public enum Innings
    {
        Batting,
        Bowling
    }

    private Innings currentInnings;

    private void Start()
    {
        currentInnings = Innings.Batting;   
    }

    public Innings GetCurrentInnings()
    { 
        return currentInnings; 
    }
    public void SetCurrentInnings(Innings currentInnings)
    {
        this.currentInnings = currentInnings;
    }

}
