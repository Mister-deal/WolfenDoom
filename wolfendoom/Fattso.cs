using Godot;
using System;

public partial class Fattso : Enemy_base
{
	[Export] public float attackDamage = 15.0f;
	[Export] public float hitStunTime = 1f;

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

		GD.Print("Fattso lance une attaque !");

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
		
		GD.Print("Le fattso a été vaincu !");

	}
	
	// Nouvelle méthode pour gérer les dégâts subis par le monstre
	public void TakeDamage()
	{
		if (dead) return;

		currentState = EnemyState.Hit;
		
		hitStunTimer.Start();
		
		// audioPlayer.Play("hit_sound");

	}
}
