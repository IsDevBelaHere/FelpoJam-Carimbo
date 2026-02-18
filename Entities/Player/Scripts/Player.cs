using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -100.0f;
	public float speedMultiplier = 1;
	public static Player instance;
	public int direction = 1;
	public bool isInJumpKarimbo = false;
	public bool isExitingBoostKarimbo = false;
	public bool isEnteringStopKarimbo = false;
	public AnimatedSprite2D animatedSprite2D;

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
		await ToSignal(GetTree().CreateTimer(5f), SceneTreeTimer.SignalName.Timeout);
		speedMultiplier = 1f;
	}

	public override void _PhysicsProcess(double delta)
    {
		Vector2 velocity = Velocity;
		if (isExitingBoostKarimbo)
		{
			isExitingBoostKarimbo = false;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			BoostTimeout();
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
			if (velocity.Y > 0)
			{
				velocity.Y += JumpVelocity * 2;
			} else
			{
				velocity.Y += JumpVelocity;
			}
		}

		// Makes the character move right constantly, later this will be changed based on if its inverted or not (1 is right, -1 is left)

		velocity.X = direction * Speed * speedMultiplier;

		if (isEnteringStopKarimbo)
		{
			isEnteringStopKarimbo = false;
			velocity = new(0, 0);
		}
		
		Rotation = velocity.Angle();
		Velocity = velocity;
		MoveAndSlide();
    }
}
