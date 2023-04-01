using Godot;

public partial class PlayerController : CharacterBody2D
{
    [Export]
    public int Health = 100;

    [Export]
    public int MaxHealth = 100;

    [Export]
    public int Speed { get; set; } = 400;

    [Signal]
    public delegate void HealthChangedEventHandler(int newValue);

    [Signal]
    public delegate void DiedEventHandler();

    public override void _Ready()
    {
        Health = MaxHealth;
    }

    public void GetInput()
    {
        Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down").Normalized();
        Velocity = inputDirection * Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Health = 0;
            EmitSignal(SignalName.Died);
        }
        else
        {
            EmitSignal(SignalName.HealthChanged, Health);
        }
    }
}