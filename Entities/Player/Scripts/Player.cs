using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : CharacterBody2D
{
	public float Speed = 200.0f;
	public const float JumpVelocity = -55.0f;
	public float speedMultiplier = 0;
	[Export] public float sceneSpeedMultiplier;
	public static Player instance;
	public int direction = 1;
	public int isInJumpKarimbo = 0;
	public bool isExitingBoostKarimbo = false;
	public bool IsEnteringStopKarimbo = false;
	public AnimatedSprite2D animatedSprite2D;
	public int delay = 0;
	public bool frozen;

    public override void _Ready()
    {
        if (instance != this)
		{
			instance = this;
		} else
		{
			QueueFree();	
		}
		animatedSprite2D = GetChild<AnimatedSprite2D>(0);
		animatedSprite2D.Play("Running");
    }

	public async Task BoostTimeout()
	{
		await ToSignal(GetTree().CreateTimer(2f), SceneTreeTimer.SignalName.Timeout);
		if (speedMultiplier >= 1)
		{
			speedMultiplier -= sceneSpeedMultiplier/2;
		}
	}

	public async Task StopTimeout()
	{
		await ToSignal(GetTree().CreateTimer(2f), SceneTreeTimer.SignalName.Timeout);
		speedMultiplier = 1f;
	}

	public override void _PhysicsProcess(double delta)
    {
		if (frozen)
		{
			return;
		}
		Vector2 velocity = Velocity;
		if (isExitingBoostKarimbo)
		{
			isExitingBoostKarimbo = false;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			BoostTimeout();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		if (IsEnteringStopKarimbo)
		{
			IsEnteringStopKarimbo = false;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			StopTimeout();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (isInJumpKarimbo > 0)
		{
			velocity.Y += JumpVelocity;
		}
		// Makes the character move right constantly, later this will be changed based on if its inverted or not (1 is right, -1 is left)

		velocity.X = direction * Speed * speedMultiplier;

		
		animatedSprite2D.FlipH = direction < 0;
		
		Velocity = velocity;
		MoveAndSlide();
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);
			if (collision != null)
			{
				if (collision.GetCollider() is StaticBody2D )
				{
					StaticBody2D collider = collision.GetCollider() as StaticBody2D;
					if (collider.GetCollisionLayerValue(6))
					{
						if (collider.GetParent<KarimboTrigger>().carimboRes is CarimboSlime)
						{
							velocity = velocity.Bounce(collision.GetNormal());
							delay = 2;
						}
					}
					if (collider.GetCollisionLayerValue(7))
					{
						if (IsInsideTree())
						{
							GetTree().ReloadCurrentScene();
						}
					}
				}
			}
		}
		if (delay > 0)
		{
			Velocity = velocity;
			delay--;
		}
    }
}
