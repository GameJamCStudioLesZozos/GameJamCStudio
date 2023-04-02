using Godot;
using System;

public partial class ShakeCamera2D : Camera2D
{
    [Export] Node2D target; // Assign the node this camera will follow.

    const double Decay = 0.8;  // How quickly the shaking stops [0, 1].
    Vector2 MaxOffset = new Vector2(100, 75); // Maximum hor/ver shake in pixels.
    const float MaxRoll = 0.1f; // Maximum rotation in radians (use sparingly).

    double trauma = 0.0; // Current shake strength.
    const double TraumaPower = 2; // Trauma exponent. Use [2, 3].

    public override void _Ready()
    {
        GD.Randomize();
    }

    private void AddTrauma(double amount)
    {
        trauma = Math.Min(trauma + amount, 1.0);
    }

    public override void _PhysicsProcess(double _delta)
    {
        if (target != null)
            Position = target.Position;

        if (trauma > 0)
        {
            trauma = Math.Max(trauma - Decay * _delta, 0);
            Shake();
        }
    }

    private void Shake()
    {
        float amount = (float)Mathf.Pow(trauma, TraumaPower);
        Rotation = MaxRoll * amount * GD.RandRange(-1, 1);
        Offset = new Vector2(
            MaxOffset.X * amount * GD.RandRange(-1, 1),
            MaxOffset.Y * amount * GD.RandRange(-1, 1));
    }

    public void OnPlayerHit()
    {
        AddTrauma(0.5);
    }
}
