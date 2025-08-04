using Godot;
using System;

public partial class Player : CharacterBody3D
{
	private Ui uiNode;
	
	public const float Speed = 8.0f;
	public const float JumpVelocity = 4.5f;
	public const float TurnSpeed = 0.05f;
	

	public override void _Ready()
	{
	   // Assurez-vous que le chemin est correct.
	   // Par exemple, "/root/world/ui".
	   uiNode = GetNodeOrNull<Ui>("ui");

	if (uiNode == null)
	{
		GD.PrintErr("Le nœud 'ui' n'a pas été trouvé en tant qu'enfant de 'player'.");
	}
	}
	
	public override void _Input(InputEvent @event)
	{
		// On vérifie si l'action "shoot" vient d'être pressée.
		if (Input.IsActionJustPressed("shoot"))
		{
			GD.Print("Action d'attaque/de tir détectée !");

			if (uiNode != null)
			{
				// On déclenche l'animation d'attaque/de tir.
				uiNode.StartAttackAnimation();
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
