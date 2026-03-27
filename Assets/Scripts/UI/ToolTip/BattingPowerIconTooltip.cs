public class BattingPowerIconTooltip : TooltipBase
{
    protected override string GetMessage()
    {
        return "<b>Batting Power</b><sprite=0>:- Amount at which runs are scored per ball (Max. 6)\n" +
         "<b>Protip:</b> Get 50% chance to hit a <b><i>clean strike</i></b> after reaching Batting Power = 6\n" +
         "<b>Clean Strike:</b> You hit ball without reducing your defence at all!";
    }
}