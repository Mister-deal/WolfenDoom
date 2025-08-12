using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	[Export] public float speed = 15.0f;
	[Export] public float attackRange = 30.0f;
	[Export] public float sightRange = 40.0f;
	public bool dead = false;
	public bool isAttacking = false;
	
	protected CharacterBody3D player;
	protected AnimatedSprite3D animatedSprite;
	protected CollisionShape3D collisionShape;

	public override void _Ready()
	{
		player = GetTree().GetFirstNodeInGroup("player") as CharacterBody3D;
		AddToGroup("enemy");
		animatedSprite = GetNode<AnimatedSprite3D>("AnimatedSprite3D");
		collisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
		animatedSprite.AnimationFinished += OnAnimationFinished;
	}

	protected void OnAnimationFinished()
	{
		if (animatedSprite.Animation == "attack")
		{
			isAttacking = false;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (dead || player == null)
		{
			Velocity = Vector3.Zero;
			MoveAndSlide();
			return;
		}

		Vector3 direction = Vector3.Zero;
		float distToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);

		if (!isAttacking)
		{
			if (distToPlayer <= attackRange)
			{
				Attack();
			}
			else if (distToPlayer <= sightRange)
			{
				direction = (player.GlobalPosition - GlobalPosition).Normalized();
			}
		}
		
		Vector3 currentVelocity = Velocity;
		currentVelocity.X = direction.X * speed;
		currentVelocity.Z = direction.Z * speed;

		if (!IsOnFloor())
		{
			Vector3 gravity = (Vector3)ProjectSettings.GetSetting("physics/3d/default_gravity_vector") * (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
			currentVelocity += gravity * (float)delta;
		}
		
		Velocity = currentVelocity;
		MoveAndSlide();

		if (!isAttacking)
		{
			if (Velocity.Length() > 0.1f)
			{
				animatedSprite.Play("walk");
			}
			else
			{
				animatedSprite.Play("idle");
			}
		}
	}

	protected virtual void Attack()
	{
		if (isAttacking)
		{
			return;
		}
		isAttacking = true;
		animatedSprite.Play("attack");
	}

	public virtual void Die()
	{
		dead = true;
		animatedSprite.Play("death");
		collisionShape.Disabled = true;
	}
}
