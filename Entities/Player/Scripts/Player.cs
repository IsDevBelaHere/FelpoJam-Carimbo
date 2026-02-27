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
	public bool isInJumpKarimbo = false;
	public bool isExitingBoostKarimbo = false;
	public bool IsEnteringStopKarimbo = false;
	public AnimatedSprite2D animatedSprite2D;

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
			speedMultiplier -= 0.5f;
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
		if (isInJumpKarimbo)
		{
			velocity.Y += JumpVelocity;
		}
		// Makes the character move right constantly, later this will be changed based on if its inverted or not (1 is right, -1 is left)

		velocity.X = direction * Speed * speedMultiplier;

		
		animatedSprite2D.FlipH = direction < 0;
		KinematicCollision2D collision = MoveAndCollide(velocity * (float)delta, true);
		if (collision != null)
		{
			if (collision.GetCollider() is StaticBody2D )
			{
				StaticBody2D collider = collision.GetCollider() as StaticBody2D;
				if (collider.GetCollisionLayerValue(6))
				{
					if (collider.GetParent<KarimboTrigger>().carimboRes is CarimboSlime)
					{
						velocity -= 3 * GetGravity() * (float)delta;
						velocity = velocity.Bounce(collision.GetNormal());
					}
				}
			}
		}
		Velocity = velocity;
		MoveAndSlide();
    }
}
