using System;
using Godot;

public partial class PlayerController : CharacterBody2D
{
    [Export] private int health = 100;
    public int Health
    {
        get => health;
        set
        {
            health = value;
            EmitSignal(SignalName.HealthChanged, health);
        }
    }
    [Export] private int maxHealth = 100;
    public int MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            EmitSignal(SignalName.MaxHealthChanged, maxHealth);
        }
    }
    public bool IsDead => health <= 0;

    [Export] private int xp = 0;
    public int XP
    {
        get => xp;
        set
        {
            xp = value;
            EmitSignal(SignalName.XPChanged, xp);
        }
    }
    [Export] private int level = 1;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            EmitSignal(SignalName.LevelChanged, level);
        }
    }
    [Export] private int xpToNextLevel = 100;
    public int XPToNextLevel
    {
        get => xpToNextLevel;
        set
        {
            xpToNextLevel = value;
            EmitSignal(SignalName.XPToNextLevelChanged, xpToNextLevel);
        }
    }
    [Export] private float XPToNextLevelScaling = 1.2f;
    [Export] private float MaxHealthScaling = 1.1f;

    [Export] private double InvulnerabilityCooldown = 3.0;
    [Export] private double TakeDamageCooldown = 0.25;
    private double CurrentInvulnerabilityDuration = 0.0;
    private double CurrentTakeDamageDuration = 0.0;
    
    [Export] private AudioStreamPlayer DamageSound;
    [Export] private AudioStreamPlayer DeathSound;
    [Export] private AudioStreamPlayer Music;

    private bool IsInvincible => CurrentInvulnerabilityDuration > 0;
    private bool IsBeingHit => CurrentTakeDamageDuration > 0;
    private Vector2 HitDirection;
    private Vector2 lastNonZeroDirection;

    [Export] public int Speed { get; set; } = 400;
    [Export] public int HitRecoilSpeed { get; set; } = 400;

    [Export] private CanvasItem spriteCanvasItemComponent;
    [Export] private AnimatedSprite2D animComponent;

    private int blinkingSign = 1;
    private float blinkingStep = 25/255f;
    private float alphaValue = 0;

    [Signal] public delegate void HealthChangedEventHandler(int newHealth);
    [Signal] public delegate void MaxHealthChangedEventHandler(int newMaxHealth);
    [Signal] public delegate void PlayerHitEventHandler();
    [Signal] public delegate void DiedEventHandler();
    [Signal] public delegate void XPChangedEventHandler(int newXP);
    [Signal] public delegate void XPToNextLevelChangedEventHandler(int newXPToNextLevel);
    [Signal] public delegate void LevelChangedEventHandler(int newLevel);


    public override void _Ready()
    {
        Health = MaxHealth;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (IsDead)
            return;

        if (IsInvincible)
        {
            // make player blink
            alphaValue += blinkingSign * blinkingStep;
            alphaValue = Math.Clamp(alphaValue, 0, 1);
            if (alphaValue == 0 || alphaValue == 1)
            {
                blinkingSign *= -1;
            }
            spriteCanvasItemComponent.SelfModulate = SetAlpha(spriteCanvasItemComponent.SelfModulate, alphaValue);

            CurrentInvulnerabilityDuration -= delta;
            if (CurrentInvulnerabilityDuration <= 0.0) // end of invincibility
            {
                CurrentInvulnerabilityDuration = 0.0;
                spriteCanvasItemComponent.SelfModulate = SetAlpha(spriteCanvasItemComponent.SelfModulate, 1);
            }
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
        if (IsDead || IsInvincible)
        {
            return; // omae wa mou shindeiru (nani???)
        }

        CurrentInvulnerabilityDuration = InvulnerabilityCooldown;
        CurrentTakeDamageDuration = TakeDamageCooldown;
        HitDirection = (Position - enemyPosition).Normalized();
        PlayHit();
        DamageSound.Play(0.36f);
        EmitSignal(SignalName.PlayerHit);
        
        Health = Math.Max(Health - amount, 0);
        if (Health == 0)
        {
            EmitSignal(SignalName.Died);
            PlayDie();
        }
    }

    public void OnFoeDied(int xpGained)
    {
        XP = Math.Min(XP + xpGained, XPToNextLevel);
        if (XP >= XPToNextLevel)
        {
            LevelUp();
        }
    }

    public int GetScore()
    {
        return Level * 100 + (int)((XP / (float)XPToNextLevel) * 100);
    }

    private void LevelUp()
    {
        ++Level;
        XP = 0;
        XPToNextLevel = (int)Math.Round(XPToNextLevel * XPToNextLevelScaling);
        MaxHealth  = (int)Math.Round(MaxHealth * MaxHealthScaling);
        Health = MaxHealth;
    }

    private Color SetAlpha(Color c, float alpha)
    {
        return new Color(c.R, c.G, c.B, alpha);
    }

    #region Animation
    private void PlayIdle() { PlayAnim("Idle"); }
    private void PlayRunning(){ PlayAnim("Running"); }
    private void PlayHit(){ PlayAnim("Hit"); }

    private void PlayDie()
    {
        PlayAnim("Die"); 
        Music.Stop();
        DeathSound.Play();
    }

    private void PlayAnim(string name)
    {
        animComponent.Play(name);
    }
    #endregion
}