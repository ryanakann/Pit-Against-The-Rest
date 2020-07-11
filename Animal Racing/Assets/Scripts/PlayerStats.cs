using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerStats : MonoBehaviour
{
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    
    public float Stamina
    {
        get => _stamina;
        set => _stamina = value;
    }

    public float Style
    {
        get => _style;
        set => _style = value;
    }

    public float Constitution
    {
        get => _constitution;
        set => _constitution = value;
    }

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public float Acceleration
    {
        get => _acceleration;
        set => _acceleration = value;
    }

    public float Poise
    {
        get => _poise;
        set => _poise = value;
    }

    public Color Color
    {
        get => _color;
        set => _color = value;
    }
    
    private string _name;
    private float _stamina; //Increases max stamina
    private float _style; //Gain more income for winning
    private float _constitution; //Decreases effectiveness of getting pushed
    private float _speed; //Overall move speed for race
    private float _acceleration; //Increases speedup on race start and after falling over
    private float _poise; //Decreases chance of ragdolling

    private Color _color;

    private void Awake()
    {
        // RandomizePlayerStats();
    }

    public void CopyPlayerStats(PlayerStats stats)
    {
        Name = stats.Name;
        Stamina = stats.Stamina;
        Style = stats.Style;
        Constitution = stats.Constitution;
        Speed = stats.Speed;
        Acceleration = stats.Acceleration;
        Poise = stats.Poise;
        Color = stats.Color;
    }

    public void RandomizePlayerStats()
    {
        Name = PlayerName.GetRandomName();
        gameObject.name = Name;
        Stamina = Random.Range(0, 100);
        Style = Random.Range(0, 100);
        Constitution = Random.Range(0, 100);
        Speed = Random.Range(0, 100);
        Acceleration = Random.Range(0, 100);
        Poise = Random.Range(0, 100);
        Color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
    
}
