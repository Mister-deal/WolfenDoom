using Godot;
using System;

public partial class Player : Enemy_base
{
	private Ui uiNode;
	private GlobalWeapon globalWeapon;
	private RayCast3D raycast;
	
	public const float Speed = 20.0f;
	public const float JumpVelocity = 4.5f;
	public const float TurnSpeed = 0.10f;
	
	private Node3D muzzlePoint;
	
	

	public override void _Ready()
	{
		AddToGroup("player");
	   uiNode = GetNodeOrNull<Ui>("ui");
	   globalWeapon = GetNode<GlobalWeapon>("/root/GlobalWeapon");
	 	raycast = GetNode<RayCast3D>("Camera3D/RayCast3D");
		muzzlePoint = GetNode<Node3D>("Camera3D/MuzzlePoint3D");

	if (uiNode == null)
	{
		GD.PrintErr("Le nœud 'ui' n'a pas été trouvé en tant qu'enfant de 'player'.");
	}
	if (globalWeapon == null)
		{
			GD.PrintErr("L'Autoload GlobalWeapon n'a pas été trouvé !");
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("shoot"))
		{
			GD.Print("Action d'attaque/de tir détectée !");

			if (uiNode != null)
			{
				uiNode.StartAttackAnimation();
			}
		}
		
		if (@event.IsActionPressed("cycle_weapon"))
		{
			if (globalWeapon != null)
			{
				globalWeapon.CycleNextWeapon();
			}
		}
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		if(Input.IsActionPressed("ui_left")){
			RotateY(TurnSpeed);
		};
		
		if(Input.IsActionPressed("ui_right")){
			RotateY(-TurnSpeed);
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
