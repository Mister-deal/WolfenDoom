using Godot;
using System;

public partial class Ui : CanvasLayer
{
	private GlobalWeapon globalWeapon;
	private AnimatedSprite2D weaponSprite;
	private bool isAttacking = false;

	public override void _Ready()
	{
		weaponSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		globalWeapon = GetNode<GlobalWeapon>("/root/GlobalWeapon");

		if (globalWeapon == null)
		{
			GD.PrintErr("L'Autoload GlobalWeapon n'a pas été trouvé !");
			return;
		}
		
		if (globalWeapon != null)
		{
			globalWeapon.WeaponChanged += OnWeaponChanged;
			
			OnWeaponChanged((int)globalWeapon.currentWeapon);
		}

		if (weaponSprite == null)
		{
			GD.PrintErr("weaponSprite introuvable !");
			return;
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
	
	public void StartAttackAnimation()
	{
		if (isAttacking)
		{
			GD.Print("Attaque déjà en cours");
			return;
		}

		switch (globalWeapon.currentWeapon)
		{
			case GlobalWeapon.WeaponType.Bat:
				isAttacking = true;
				weaponSprite.Play("smash");
				break;
				
			case GlobalWeapon.WeaponType.Gun:
				if (globalWeapon.weapons[globalWeapon.currentWeapon].Ammo > 0)
				{
					isAttacking = true;
					weaponSprite.Play("shoot");
					globalWeapon.weapons[globalWeapon.currentWeapon].Ammo -= 1;
				}
				else
				{
					GD.Print("plus de munitions");
				}
				break;
				
			case GlobalWeapon.WeaponType.Machinegun:
				if (globalWeapon.weapons[globalWeapon.currentWeapon].Ammo > 0)
				{
					isAttacking = true;
					weaponSprite.Play("machinegun_shoot");
					globalWeapon.weapons[globalWeapon.currentWeapon].Ammo -= 1;
				}
				else
				{
					GD.Print("plus de munitions");
				}
				break;
				
			case GlobalWeapon.WeaponType.Shotgun:
				if (globalWeapon.weapons[globalWeapon.currentWeapon].Ammo > 0)
				{
					isAttacking = true;
					weaponSprite.Play("shotgun_shoot");
					globalWeapon.weapons[globalWeapon.currentWeapon].Ammo -= 1;
				}
				else
				{
					GD.Print("plus de munitions");
				}
				break;
				
				case GlobalWeapon.WeaponType.Rocketlauncher:
				if (globalWeapon.weapons[globalWeapon.currentWeapon].Ammo > 0)
				{
					isAttacking = true;
					weaponSprite.Play("rocketlauncher_shoot");
					globalWeapon.weapons[globalWeapon.currentWeapon].Ammo -= 1;
				}
				else
				{
					GD.Print("plus de munitions");
				}
				break;
		}
	}

	private void OnAnimationFinished()
	{
		isAttacking = false;

		switch (globalWeapon.currentWeapon)
		{
			case GlobalWeapon.WeaponType.Bat:
				weaponSprite.Play("bat_idle");
				break;
				
			case GlobalWeapon.WeaponType.Gun:
				weaponSprite.Play("gun_idle");
				break;
				
			case GlobalWeapon.WeaponType.Machinegun:
				weaponSprite.Play("machinegun_idle");
				break;
				
			case GlobalWeapon.WeaponType.Shotgun:
				weaponSprite.Play("shotgun_idle");
				break;
				
			case GlobalWeapon.WeaponType.Rocketlauncher:
				weaponSprite.Play("rocketlauncher_idle");
				break;
		}
	}
	
	private void OnWeaponChanged(int newWeaponTypeInt)
	{
		GlobalWeapon.WeaponType newWeaponType = (GlobalWeapon.WeaponType)newWeaponTypeInt;
		switch (newWeaponType)
		{
			case GlobalWeapon.WeaponType.Bat:
				weaponSprite.Play("bat_idle");
				break;
			case GlobalWeapon.WeaponType.Gun:
				weaponSprite.Play("gun_idle");
				break;
			case GlobalWeapon.WeaponType.Machinegun:
				weaponSprite.Play("machinegun_idle");
				break;
			case GlobalWeapon.WeaponType.Shotgun:
				weaponSprite.Play("shotgun_idle");
				break;
			case GlobalWeapon.WeaponType.Rocketlauncher:
				weaponSprite.Play("rocketlauncher_idle");
				break;
		}
	}
}
