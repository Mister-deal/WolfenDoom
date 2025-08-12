using Godot;
using System;

public partial class Enemy_base : CharacterBody3D
{
	[Export] public float speed = 15.0f;
	[Export] public float attackRange = 30.0f;
	[Export] public float sightRange = 40.0f;
	public bool dead = false;
	public bool isAttacking = false;
	
	public enum EnemyState { Idle, Walk, Attack, Hit, Death }
	protected EnemyState currentState = EnemyState.Idle;

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
		if (animatedSprite.Animation.ToString().Contains("attack"))
	{
		isAttacking = false;
		currentState = EnemyState.Idle;
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

		// La logique d'état est gérée ici
		if (isAttacking)
		{
			currentState = EnemyState.Attack;
		}
		else if (distToPlayer <= attackRange)
		{
			currentState = EnemyState.Attack;
			Attack();
		}
		else if (distToPlayer <= sightRange)
		{
			currentState = EnemyState.Walk;
			direction = (player.GlobalPosition - GlobalPosition).Normalized();
		}
		else
		{
			currentState = EnemyState.Idle;
		}

		UpdateSpriteFacing(direction);

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
	}

	protected virtual void Attack()
	{
		if (isAttacking) return;
		isAttacking = true;
	}

	public virtual void Die()
	{
		dead = true;
		currentState = EnemyState.Death;
		UpdateSpriteFacing(Vector3.Zero);
		collisionShape.Disabled = true;
	}

	protected void UpdateSpriteFacing(Vector3 direction)
	{
		if (currentState == EnemyState.Death)
		{
			animatedSprite.Play("death_0_deg");
			return;
		}

		string baseAnimName = "";
		
		switch (currentState)
		{
			case EnemyState.Attack:
				baseAnimName = "attack";
				break;
			case EnemyState.Idle:
				baseAnimName = "idle";
				break;
			case EnemyState.Walk:
				baseAnimName = "walk";
				break;
			case EnemyState.Hit:
				baseAnimName = "hit";
				break;
			default:
				baseAnimName = "idle"; // Par défaut
				break;
		}

		// Calcule l'angle
		float angle = Mathf.RadToDeg(Vector3.Forward.SignedAngleTo(direction, Vector3.Up));
		string angleSuffix = GetAnimationSuffix(angle);
		bool flipH = GetFlipH(angle);
		
		string animationName = $"{baseAnimName}{angleSuffix}";
		animatedSprite.Play(animationName);
		animatedSprite.FlipH = flipH;
	}
	
	protected string GetAnimationSuffix(float angle)
	{
		if (angle >= -22.5f && angle < 22.5f) { return "_0_deg"; }
		if (angle >= 22.5f && angle < 67.5f) { return "_45_deg"; }
		if (angle >= 67.5f && angle < 112.5f) { return "_90_deg"; }
		if (angle >= 112.5f && angle < 157.5f) { return "_135_deg"; }
		if (angle >= 157.5f || angle < -157.5f) { return "_180_deg"; }
		if (angle >= -67.5f && angle < -22.5f) { return "_45_deg"; }
		if (angle >= -112.5f && angle < -67.5f) { return "_90_deg"; }
		if (angle >= -157.5f && angle < -112.5f) { return "_135_deg"; }
		
		return "_0_deg";
	}
	
	protected bool GetFlipH(float angle)
	{
		return angle < -22.5f && angle > -157.5f;
	}
	
	public void Hit()
	{
		currentState = EnemyState.Hit;
		
	}
}
