using Godot;

public partial class PlayerController : CharacterBody2D
{
    [Export]
    public int Health = 100;

    [Export]
    public int MaxHealth = 100;

    [Export]
    private double InvulnerabilityCooldown = 3.0;

    private double CurrentInvulnerabilityDuration = 0.0;

    [Export]
    public int Speed { get; set; } = 400;

    [Signal]
    public delegate void HealthChangedEventHandler(int newValue);

    [Signal]
    public delegate void DiedEventHandler();

    private bool IsDead => Health <= 0;

    public override void _Ready()
    {
        Health = MaxHealth;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        CurrentInvulnerabilityDuration -= delta;
        if (CurrentInvulnerabilityDuration <= 0.0)
        {
            CurrentInvulnerabilityDuration = 0.0;
        }
    }

    public void GetInput()
    {
        Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down").Normalized();
        Velocity = inputDirection * Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsDead)
            return;

        GetInput();
        MoveAndSlide();
    }

    public void TakeDamage(int amount)
    {
        if (CurrentInvulnerabilityDuration > 0.0)
        {
            return; // still invincible
        }

        CurrentInvulnerabilityDuration = InvulnerabilityCooldown;
        
        Health -= amount;
        if (Health < 0)
        {
            Health = 0;
        }
        EmitSignal(SignalName.HealthChanged, Health);
        if (Health == 0)
        {
            EmitSignal(SignalName.Died);
        }
    }
}