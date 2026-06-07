using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardEvents
{
    public const string PLAY_CARD = "Play_Card";
    public const string PLAY_CARD_REQUEST = "Play_Card_Request";
    public const string SHOW_CARD_RANGE = "Show_Card_Range";
    public const string CLEAR_CARD_RANGE = "Clear_Card_Range";
}

public static class UIEvents
{
    public const string DRAW_CARD = "Draw_Card";
}

public static class TurnEvents
{
    public const string END_TURN = "End_Turn";
}
