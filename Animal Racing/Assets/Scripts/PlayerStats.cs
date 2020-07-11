using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public struct PlayerStats
{
    private string _name;
    private float _stamina; //Increases max stamina
    private float _style; //Gain more income for winning
    private float _constitution; //Decreases effectiveness of getting pushed
    private float _speed; //Overall move speed for race
    private float _acceleration; //Increases speedup on race start and after falling over
    private float _poise; //Decreases chance of ragdolling

    public void SetName(string name) => _name = name;
    public string GetName () => _name;

    public PlayerStats(string name, float stamina, float style, float constitution, float speed, float acceleration, float poise)
    {
        _name = name;
        _stamina = stamina;
        _style = style;
        _constitution = constitution;
        _speed = speed;
        _acceleration = acceleration;
        _poise = poise;
    }

    public PlayerStats(string name)
    {
        _name = name;
        _stamina = Random.Range(0, 100);
        _style = Random.Range(0, 100);
        _constitution = Random.Range(0, 100);
        _speed = Random.Range(0, 100);
        _acceleration = Random.Range(0, 100);
        _poise = Random.Range(0, 100);
    }

    public static PlayerStats Breed(PlayerStats p1, PlayerStats p2)
    {
        PlayerStats p3 = new PlayerStats(PlayerName.GetRandomName());
        p3._stamina = (p1._stamina + p2._stamina + p3._stamina) / 3f;
        p3._style = (p1._style + p2._style + p3._style) / 3f;
        p3._constitution = (p1._constitution + p2._constitution + p3._constitution) / 3f;
        p3._speed = (p1._speed + p2._speed + p3._speed) / 3f;
        p3._acceleration = (p1._acceleration + p2._acceleration + p3._acceleration) / 3f;
        p3._poise = (p1._poise + p2._poise + p3._poise) / 3f;
        return p3;
    }
}
