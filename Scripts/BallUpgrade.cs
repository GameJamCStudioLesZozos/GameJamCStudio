using Godot;
using System;

/// <summary>
/// This is because Godot won't recognize the class for things like Signals,
/// if the class doesn't inherit from GodotObject.
/// So we wrap it to be able to still use the underlying class as freely as possible.
/// </summary>
public partial class BallUpgradeGD : GodotObject
{
    public BallUpgradeGD(BallUpgrade upgrade)
    {
        _upgrade = upgrade;
    }

    public BallUpgrade Get() => _upgrade;

    private readonly BallUpgrade _upgrade;
}

public class BallUpgrade : IEquatable<BallUpgrade>
{
    private const string ImagesRoot = "res://Sprites/UpgradeItems";

    public BallUpgrade(
        string description,
        Action<BallManager> effect,
        Predicate<BallManager> condition,
        string imageName)
    {
        Description = description;
        Effect = effect;
        Condition = condition;
        ImageName = imageName;
    }

    public string Description { get ; }
    public Action<BallManager> Effect { get; }
    public Predicate<BallManager> Condition { get; }
    public string ImageName { get; }

    public CompressedTexture2D Image
    {
        get
        {
            _image ??= GD.Load<CompressedTexture2D>($"{ImagesRoot}/{ImageName}.png");
            return _image;
        }
    }
    private CompressedTexture2D _image = null;

    public bool Equals(BallUpgrade other)
    {
        return Description == other.Description;
    }
}

public static class BallUpgrades
{
    public static BallUpgrade PickRandomUpgrade()
    {
        return Upgrades[GD.Randi() % Upgrades.Length];
    }

    public static readonly BallUpgrade[] Upgrades =
    {
        new(
            "Increase radius",
            (manager) => manager.IncreaseRadius(),
            (manager) => !manager.IsRadiusMax(),
            "radius_increase"
        ),
        new(
            "Decrease radius",
            (manager) => manager.DecreaseRadius(),
            (manager) => !manager.IsRadiusMin(),
            "radius_decrease"
        ),
        new(
            "Increase rotation speed",
            (manager) => manager.IncreaseRotationSpeed(),
            (manager) => !manager.IsRotationSpeedMax(),
            "rotation_increase"
        ),
        new(
            "Decrease rotation speed",
            (manager) => manager.DecreaseRotationSpeed(),
            (manager) => !manager.IsRotationSpeedMin(),
            "rotation_decrease"
        ),
        new(
            "Add 2 more balls",
            (manager) => manager.AddTwoMoreBalls(),
            (manager) => !manager.IsNumberOfBallsMax(),
            "ball_increase"
        ),
        new(
            "Remove 2 balls",
            (manager) => manager.RemoveTwoBalls(),
            (manager) => !manager.IsNumberOfBallsMin(),
            "ball_decrease"
        ),
        new(
            "Increase balls power",
            (manager) => manager.IncreaseBallsPower(),
            (manager) => true,
            "damage_increase"
        )
    };
}
