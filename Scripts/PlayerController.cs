using Godot;

public partial class PlayerController : CharacterBody2D
{
    [Export]
    public int Health = 100;

    [Export]
    public int MaxHealth = 100;

    [Export]
    private double InvulnerabilityCooldown = 3.0;

    [Export]
    private double TakeDamageCooldown = 0.25;

    private double CurrentInvulnerabilityDuration = 0.0;

    private double CurrentTakeDamageDuration = 0.0;

    [Export]
    public int Speed { get; set; } = 400;

    [Export]
    public int HitRecoilSpeed { get; set; } = 400;

    [Export]
    private AnimatedSprite2D animComponent;

    [Signal]
    public delegate void HealthChangedEventHandler(int newValue);

    [Signal]
    public delegate void DiedEventHandler();

    private bool IsDead => Health <= 0;
    private bool IsBeingHit => CurrentTakeDamageDuration > 0;
    private Vector2 HitDirection;
    private Vector2 lastNonZeroDirection;

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

        if (IsBeingHit)
        {
            CurrentTakeDamageDuration -= delta;
            if (CurrentTakeDamageDuration <= 0) // end of take damage cooldown
            {
                PlayIdle();
                CurrentTakeDamageDuration = 0;
            }
        }
    }

    public void GetInput()
    {
        if (IsBeingHit)
        {
            Velocity = HitDirection * HitRecoilSpeed;
            return;
        }

        Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down").Normalized();
        Velocity = inputDirection * Speed;

        if (inputDirection == Vector2.Zero)
        {
            PlayIdle();
        }
        else
        {
            PlayRunning();
            lastNonZeroDirection = inputDirection;
        }

        animComponent.FlipH = lastNonZeroDirection.X < 0;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsDead)
            return;

        GetInput();
        MoveAndSlide();
    }

    public void TakeDamage(int amount, Vector2 enemyPosition)
    {
        if (CurrentInvulnerabilityDuration > 0.0)
        {
            return; // still invincible
        }

        CurrentInvulnerabilityDuration = InvulnerabilityCooldown;
        CurrentTakeDamageDuration = TakeDamageCooldown;
        HitDirection = (Position - enemyPosition).Normalized();
        PlayHit();
        
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

    #region Animation
    private void PlayIdle() { PlayAnim("Idle"); }
    private void PlayRunning(){ PlayAnim("Running"); }
    private void PlayHit(){ PlayAnim("Hit"); }

    private void PlayAnim(string name)
    {
        animComponent.Play(name);
    }
    #endregion
}