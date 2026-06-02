using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoundEvents
{
    public const string ROUND_START = "roundStart";
    public const string SKILL_CHOSEN = "skillChosen";
}

public static class CardEvents
{
    public const string DRAW_CARD = "drawCard";
    public const string Destory_CARD = "clearCard";
    
}

public static class UIEvents {
    public const string DRAW_SKILL = "drawSkill";
    public const string PLACE_MOVE_CARD = "placeMoveCard";
    public const string PLAY_CARD = "playCard";
    public const string ALLOW_MOVING = "allowMoving";
    public const string END_MOVING = "endMoving";
    public const string ALLOW_SKILLING = "allowSkilling";
    public const string END_SKILLING = "endSkilling";
    public const string ALLOW_PLAYING = "allowPlaying";
    public const string END_PLAYING = "endPlaying";
}
