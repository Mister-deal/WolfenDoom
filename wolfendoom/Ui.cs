using Godot;
using System;

public partial class Ui : CanvasLayer
{
	public int ammo = 10;
	public string currentWeapon = "gun";
	
	 private AnimatedSprite2D weaponSprite;
	
	//private Label ammoLabel;

	public override void _Ready()
{
	weaponSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");

	if (weaponSprite == null)
	{
		GD.PrintErr("weaponSprite introuvable !");
	}
	else
	{
		GD.Print("weaponSprite trouvé !");
		GD.Print("Animations disponibles : ");
		foreach (var name in weaponSprite.SpriteFrames.GetAnimationNames())
		{
			GD.Print("- " + name);
		}
	}

	weaponSprite.AnimationFinished += OnAnimationFinished;
}

	private bool isAttacking = false; // ajouté tout en haut de la classe

public void StartAttackAnimation()
{
	if (isAttacking)
	{
		GD.Print("Attaque déjà en cours");
		return;
	}

	if (currentWeapon == "knife")
	{
		isAttacking = true;
		weaponSprite.Play("stab");
	}
	else if (currentWeapon == "gun")
	{
		if (ammo > 0)
		{
			isAttacking = true;
			weaponSprite.Play("shoot");
			ammo -= 1;
			//UpdateAmmoDisplay();
		}
	}
}

private void OnAnimationFinished()
{
	GD.Print("Animation terminée !");
	isAttacking = false;

	if (currentWeapon == "knife")
	{
		weaponSprite.Play("knife_idle");
	}
	else if (currentWeapon == "gun")
	{
		weaponSprite.Play("gun_idle");
	}
}
/*
// Met à jour l'affichage des munitions sur l'UI.
	private void UpdateAmmoDisplay()
	{
		if (ammoLabel != null)
		{
			ammoLabel.Text = "AMMO: " + ammo.ToString();
		}
	}
*/

}
