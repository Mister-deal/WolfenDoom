using Godot;
using System;

public partial class DemonSlave : Enemy_base
{
	[Export] public float attackDamage = 10.0f;
	[Export] public float hitStunTime = 0.5f;

	protected AudioStreamPlayer3D audioPlayer;
	private Timer hitStunTimer;
	
	public override void _Ready()
	{
		// Appelez toujours la méthode _Ready de la classe de base !
		base._Ready(); 
		
		// Initialisez les nœuds spécifiques au DemonSlave
		audioPlayer = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");

		hitStunTimer = new Timer();
		hitStunTimer.WaitTime = hitStunTime;
		hitStunTimer.OneShot = true;
		AddChild(hitStunTimer);
		hitStunTimer.Timeout += () => {
			currentState = EnemyState.Idle;
		};
	}

	protected override void Attack()
	{
		base.Attack();

		GD.Print("DemonSlave lance une attaque !");

		if (GlobalPosition.DistanceTo(player.GlobalPosition) <= attackRange)
		{
			// audioPlayer.Play("attack_sound");
			
			// if (player is PlayerController playerScript)
			// {
			//     playerScript.TakeDamage(attackDamage);
			// }
		}
	}

	public override void Die()
	{
		base.Die();
		
		GD.Print("Le DemonSlave a été vaincu !");

	}
	
	public void TakeDamage()
	{
		if (dead) return;

		currentState = EnemyState.Hit;
		
		hitStunTimer.Start();
		
		// audioPlayer.Play("hit_sound");

	}
}
