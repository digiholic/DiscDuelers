using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "Character Data")]
public class CharacterData : ScriptableObject {
    public string characterName;

    public Texture discTex;
    public Texture characterPortrait;

    public int maxMoves;
    public int maxStrikes;
    public string effect;

    public DiscEvent startTurnEvent;
    public DiscEvent startRoundEvent;
    public DiscEvent endTurnEvent;
    public DiscEvent endRoundEvent;
    public DiscEvent onCrashEvent;
    public DiscEvent onFallEvent;
    public DiscEvent onGetStruckEvent;
    public DiscEvent onStrikeEvent;
}
