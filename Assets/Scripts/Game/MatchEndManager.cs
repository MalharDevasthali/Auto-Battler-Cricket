using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MatchResult
{
    None,
    Won,
    Lost
}

public class MatchEndManager : MonoBehaviour
{
    private BattleController battleController;
    public MatchResult currentMatchResult;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI matchResultText;
    [SerializeField] private Button nextMatchButton;
    [SerializeField] private Button nextLeaugeButton;
    [SerializeField] private Button restartButton;


    public void Initialize(BattleController battleController)
    {
        this.battleController = battleController;
    }

    public void SetMatchResult(MatchResult result)
    {
        currentMatchResult = result;
    }

}
