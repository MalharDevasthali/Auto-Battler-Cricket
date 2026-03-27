public class ShieldIconTooltip : TooltipBase
{
    protected override string GetMessage()
    {
        return "<b>Defence</b><sprite=2>:- Defence gets reduced when batsman faces a ball." +
            "<br>When Defence becomes less than 0, batsman gets out!";
    }
}