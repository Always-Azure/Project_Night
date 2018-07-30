using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// State 관련
public enum STATE_BOX { CLOSE, OPEN };
public enum STATE_ENERMY { SPAWNING, ALIVE, ATTACK, DEAD};
public enum STATE_LIGHT { OFF, ON};
public enum STATE_BATTERY { EMPTY, USE};
public enum STATE_PLAYER {ALIVE, SAFETY, DEAD };
public enum STATE_STREETLIGHT { BT100, BT50, BT25, BT10, BT0}

// Type 관련
public enum TYPE_ATTACK { ENERMY, ENVIRONMENT}
public enum TYPE_ENERMY { BAT, RABBIT }
public enum DRAWTYPE_BOX { OPEN, INSERT, CHANGE, REMOVE, CLOSE};

public class Enum{
}