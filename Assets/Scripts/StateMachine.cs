using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private enum State
    {
        IdleNormal,
        IdleBattle,
        MoveForwardNormal,
        MoveLeftBattle,
        MoveRightBattle,
        MoveForwardBattle,
        MoveBackwardBattle,
        JumpNormal,
        JumpSpin,
        AttackRightToLeft,
        AttackLeftToRight,
        AttackStab,
        AttackSpin,
        GetDizzy,
        GetHit,
        DefendStance,
        DefendHit,
        Die,
        DieStay,
        Revive,
        LevelUp,
        Victory,
    }

    private State state;

    private void Start()
    {
        state = State.IdleNormal;
    }

    private void Update()
    {
        switch (state)
        {
            case State.IdleNormal:
                break;
            case State.IdleBattle:
                break;
            case State.MoveForwardNormal:
                break;
            case State.MoveLeftBattle:
                break;
            case State.MoveRightBattle:
                break;
            case State.MoveForwardBattle:
                break;
            case State.MoveBackwardBattle:
                break;
            case State.JumpNormal:
                break;
            case State.JumpSpin:
                break;
            case State.AttackRightToLeft:
                break;
            case State.AttackLeftToRight:
                break;
            case State.AttackStab:
                break;
            case State.AttackSpin:
                break;
            case State.GetDizzy:
                break;
            case State.GetHit:
                break;
            case State.DefendStance:
                break;
            case State.DefendHit:
                break;
            case State.Die:
                break;
            case State.DieStay:
                break;
            case State.Revive:
                break;
            case State.LevelUp:
                break;
            case State.Victory:
                break;
        }
    }
}
