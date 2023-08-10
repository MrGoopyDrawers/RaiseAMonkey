using MelonLoader;
using BTD_Mod_Helper;
using AdoptAndRaiseAMonkey;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using UnityEngine;
using Random = System.Random;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Audio;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using HarmonyLib;
using Il2Cpp;
using System;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using UnityEngine.Assertions;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Track;
using Il2CppAssets.Scripts.Models.Bloons;

[assembly: MelonInfo(typeof(AdoptAndRaiseAMonkey.AdoptAndRaiseAMonkey), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace AdoptAndRaiseAMonkey;

public class AdoptAndRaiseAMonkey : BloonsTD6Mod
{
    public static AdoptAndRaiseAMonkey mod;
    public int lifestealChance = 0;
    public int destroyChance = 0;
    public bool canAdopt = true;
    public bool hasDiminish = false;
    public bool hasLifesteal = false;
    public bool hasInfiniteTraining = false;
    public bool upgradeOpen = false;
    public int Food = 0;
    public int OrphanLevel = 0;
    public int trainingLevel = 0;
    public TowerModel adoptModel = null!;
    public Tower adopttower = null!;
    public override void OnApplicationStart()
    {
        mod = this;
        ModHelper.Msg<AdoptAndRaiseAMonkey>("AdoptAndRaiseAMonkey loaded!");
    }
    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        if (InGame.instance == null || InGame.instance.bridge == null) { return; }
        InGame game = InGame.instance;
        RectTransform rect = game.uiRect;
        if (tower.model.name.Contains("adopted"))
        {
            if (canAdopt == true)
            {
                adoptModel = tower.towerModel;
                adopttower = tower;
                canAdopt = false;
                upgradeOpen = true;
                tower.SetSellingBlocked(true);
                MenuUi.CreateUpgradeMenu(rect);
                ModHelper.Msg<AdoptAndRaiseAMonkey>("replaced tower");
            }
            else
            {
                tower.SellTower();
            }
        }
    }
    public override void OnWeaponFire(Weapon weapon)
    {
        if (InGame.instance == null || InGame.instance.bridge == null) { return; }
        InGame game = InGame.instance;
        if (weapon.model.name.Contains("orphanWeapon"))
        {
            if (hasLifesteal == true)
            {
                lifestealChance += 1;
                if (lifestealChance >= 20)
                {
                    lifestealChance = 0;
                    game.AddHealth(System.Math.Round(0.1 * Food));
                }
            }
        }
    }
    public override void OnBloonEmitted(Spawner spawner, BloonModel bloonModel, int round, int index, float startingDist, ref Bloon bloon)
    {
        if (InGame.instance == null || InGame.instance.bridge == null) { return; }
        InGame game = InGame.instance;
        if (hasDiminish == true)
        {
            game.AddCash(bloon.health + 5);
        }
    }
    public override void OnUpdate()
    {
        if (InGame.instance == null || InGame.instance.bridge == null) { return; }
        InGame game = InGame.instance;

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            RectTransform rect = game.uiRect;
            if (canAdopt == false)
            {
                if (upgradeOpen == false)
                {
                    MenuUi.CreateUpgradeMenu(rect);
                    upgradeOpen = true;
                }
                else
                {
                    MenuUi.instance.CloseMenu();
                    upgradeOpen = false;
                }
            }
        }
        if (canAdopt == false)
        {
            RectTransform rect = game.uiRect;
            if (OrphanLevel == 0)
            {
                if (Food >= 5)
                {
                    OrphanLevel = 1;
                    adoptModel.displayScale *= 1.1f;
                    MenuUi.instance.CloseMenu();
                    MenuUi.CreateUpgradeMenu(rect);
                }
            }
            if (OrphanLevel == 1)
            {
                if (Food >= 20)
                {
                    OrphanLevel = 2;
                    adoptModel.displayScale *= 1.1f;
                    MenuUi.instance.CloseMenu();
                    MenuUi.CreateUpgradeMenu(rect);
                }
            }
            if (OrphanLevel == 2)
            {
                if (Food >= 50)
                {
                    OrphanLevel = 3;
                    adoptModel.displayScale *= 1.1f;
                    MenuUi.instance.CloseMenu();
                    MenuUi.CreateUpgradeMenu(rect);
                }
            }
            if (OrphanLevel == 3)
            {
                if (Food >= 80)
                {
                    OrphanLevel = 4;
                    adoptModel.displayScale *= 1.1f;
                    MenuUi.instance.CloseMenu();
                    MenuUi.CreateUpgradeMenu(rect);
                }
            }
            if (OrphanLevel == 4)
            {
                if (Food >= 150)
                {
                    OrphanLevel = 5;
                    adoptModel.displayScale *= 1.1f;
                    MenuUi.instance.CloseMenu();
                    MenuUi.CreateUpgradeMenu(rect);
                }
            }
        }
    }
    public override void OnMatchStart()
    {
        canAdopt = true;
        Food = 0;
        OrphanLevel = 0;
        trainingLevel = 0;
        hasLifesteal = false;
        hasDiminish = false;
        hasInfiniteTraining = false;

    }
    [RegisterTypeInIl2Cpp(false)]
    public class MenuUi : MonoBehaviour
    {

        public static MenuUi instance;
        public ModHelperInputField input;
        public void GiveFood()
        {
            if (InGame.instance == null || InGame.instance.bridge == null) { return; }
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            if (AdoptAndRaiseAMonkey.mod.hasInfiniteTraining == true)
            {
                if (game.GetCash() >= 15000)
                {
                    game.AddCash(-15000);
                    AdoptAndRaiseAMonkey.mod.Food += 1;
                    var towerModel = AdoptAndRaiseAMonkey.mod.adoptModel;
                    var attackModel = towerModel.GetAttackModel();
                    attackModel.weapons[0].projectile.pierce += 15;
                    attackModel.weapons[0].rate *= 0.85f;
                    attackModel.weapons[0].projectile.GetDamageModel().damage += 650f;
                    Destroy(gameObject);
                    MenuUi.CreateUpgradeMenu(rect);
                    if (AdoptAndRaiseAMonkey.mod.lifestealChance >= 3)
                    {
                        AdoptAndRaiseAMonkey.mod.lifestealChance -= 1;
                    }
                }
            }
            else
            {
                if (game.GetCash() >= 100)
                {
                    game.AddCash(-100);
                    AdoptAndRaiseAMonkey.mod.Food += 1;
                    Destroy(gameObject);
                    MenuUi.CreateUpgradeMenu(rect);
                }
            }
        }
        public void PreliminaryTraining()
        {
            if (InGame.instance == null || InGame.instance.bridge == null) { return; }
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            if (game.GetCash() >= 1500)
            {
                game.AddCash(-1500);
                var towerModel = AdoptAndRaiseAMonkey.mod.adoptModel;
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce += 10;
                attackModel.weapons[0].rate *= 0.5f;
                attackModel.weapons[0].projectile.GetDamageModel().damage += 5f;
                AdoptAndRaiseAMonkey.mod.trainingLevel += 1;
                Destroy(gameObject);
                MenuUi.CreateUpgradeMenu(rect);
            }
        }
        public void IntermediateTraining()
        {
            if (InGame.instance == null || InGame.instance.bridge == null) { return; }
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            if (game.GetCash() >= 3000)
            {
                game.AddCash(-3000);
                var towerModel = AdoptAndRaiseAMonkey.mod.adoptModel;
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce += 5;
                attackModel.weapons[0].rate *= 0.5f;
                attackModel.weapons[0].projectile.GetDamageModel().damage += 3f;
                attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                AdoptAndRaiseAMonkey.mod.trainingLevel += 1;
                Destroy(gameObject);
                MenuUi.CreateUpgradeMenu(rect);
            }
        }
        public void AdvancedTraining()
        {
            if (InGame.instance == null || InGame.instance.bridge == null) { return; }
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            if (game.GetCash() >= 10000)
            {
                game.AddCash(-10000);
                var towerModel = AdoptAndRaiseAMonkey.mod.adoptModel;
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce += 25;
                attackModel.weapons[0].rate *= 0.4f;
                attackModel.weapons[0].projectile.GetDamageModel().damage += 25f;
                AdoptAndRaiseAMonkey.mod.trainingLevel += 1;
                Destroy(gameObject);
                MenuUi.CreateUpgradeMenu(rect);
            }
        }
        public void MegaTraining()
        {
            if (InGame.instance == null || InGame.instance.bridge == null) { return; }
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            if (game.GetCash() >= 50000)
            {
                game.AddCash(-50000);
                var towerModel = AdoptAndRaiseAMonkey.mod.adoptModel;
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce += 45;
                attackModel.weapons[0].rate *= 0.3f;
                attackModel.weapons[0].projectile.GetDamageModel().damage += 50f;
                AdoptAndRaiseAMonkey.mod.trainingLevel += 1;
                Destroy(gameObject);
                MenuUi.CreateUpgradeMenu(rect);
            }
        }
        public void MetaversalTraining()
        {
            if (InGame.instance == null || InGame.instance.bridge == null) { return; }
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            if (game.GetCash() >= 250000)
            {
                game.AddCash(-250000);
                var towerModel = AdoptAndRaiseAMonkey.mod.adoptModel;
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce += 34;
                attackModel.weapons[0].rate *= 0.087f;
                attackModel.weapons[0].projectile.GetDamageModel().damage += 2550f;
                AdoptAndRaiseAMonkey.mod.trainingLevel += 1;
                Destroy(gameObject);
                MenuUi.CreateUpgradeMenu(rect);
            }
        }
        public void UnlockLifesteal()
        {
            if (InGame.instance == null || InGame.instance.bridge == null) { return; }
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            if (game.GetCash() >= 30000)
            {
                game.AddCash(-30000);
                AdoptAndRaiseAMonkey.mod.hasLifesteal = true;
                Destroy(gameObject);
                MenuUi.CreateUpgradeMenu(rect);
            }
        }
        public void UnlockDiminsh()
        {
            if (InGame.instance == null || InGame.instance.bridge == null) { return; }
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            if (game.GetCash() >= 90000)
            {
                game.AddCash(-90000);
                AdoptAndRaiseAMonkey.mod.hasDiminish = true;
                Destroy(gameObject);
                MenuUi.CreateUpgradeMenu(rect);
            }
        }
        public void UnlockInfiniteTraining()
        {
            if (InGame.instance == null || InGame.instance.bridge == null) { return; }
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            if (game.GetCash() >= 150000)
            {
                game.AddCash(-150000);
                AdoptAndRaiseAMonkey.mod.hasInfiniteTraining = true;
                Destroy(gameObject);
                MenuUi.CreateUpgradeMenu(rect);
            }
        }

        public void CloseMenu()
        {
            AdoptAndRaiseAMonkey.mod.upgradeOpen = false;
            Destroy(gameObject);
        }
        public static void CreateUpgradeMenu(RectTransform rect)
        {
            AdoptAndRaiseAMonkey.mod.upgradeOpen = true;
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 500, 500, 1000, 2200, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
            MenuUi upgradeUi = panel.AddComponent<MenuUi>();
            instance = upgradeUi;
            if (AdoptAndRaiseAMonkey.mod.hasInfiniteTraining == true)
            {
                ModHelperText infoText = panel.AddText(new Info("Title_", 0, 700, 500, 500), "Give SUPER GOOD food ($15000)", 50);
            }
            else
            {
                ModHelperText infoText = panel.AddText(new Info("Title_", 0, 700, 500, 500), "Give food ($100)", 50);
            }
            ModHelperText TitleText = panel.AddText(new Info("Title_", 0, 1020, 800, 500), "Adopt Upgrade Panel", 90);
            ModHelperText infoText2 = panel.AddText(new Info("Title_", 0, 850, 500, 500), "More food = More abilities", 30);
            ModHelperText infoText3 = panel.AddText(new Info("Title_", 0, 900, 500, 500), "Food given: " + AdoptAndRaiseAMonkey.mod.Food.ToString(), 30);
            ModHelperText infoText4 = panel.AddText(new Info("Title_", 0, 800, 500, 500), "Stage: " + AdoptAndRaiseAMonkey.mod.OrphanLevel.ToString(), 30);
            ModHelperText infoText5 = panel.AddText(new Info("Title_", 0, 750, 500, 500), "Press the right bracket ] to toggle", 27);
            if (AdoptAndRaiseAMonkey.mod.trainingLevel == 0)
            {
                if (AdoptAndRaiseAMonkey.mod.OrphanLevel >= 1)
                {
                    ModHelperText trainText = panel.AddText(new Info("Title_", 0, 500, 500, 500), "Preliminary Training ($1500)", 50);
                    ModHelperButton submitButton = panel.AddButton(new Info("GiveFood_", 450, 500, 150), VanillaSprites.GreenBtn, new Action(() => upgradeUi.PreliminaryTraining()));
                }
            }
            if (AdoptAndRaiseAMonkey.mod.trainingLevel == 1)
            {
                if (AdoptAndRaiseAMonkey.mod.OrphanLevel >= 2)
                {
                    ModHelperText trainText = panel.AddText(new Info("Title_", 0, 500, 500, 500), "Intermediate Training ($3000)", 50);
                    ModHelperButton submitButton = panel.AddButton(new Info("GiveFood_", 450, 500, 150), VanillaSprites.GreenBtn, new Action(() => upgradeUi.IntermediateTraining()));
                }
            }
            if (AdoptAndRaiseAMonkey.mod.trainingLevel == 2)
            {
                if (AdoptAndRaiseAMonkey.mod.OrphanLevel >= 3)
                {
                    ModHelperText trainText = panel.AddText(new Info("Title_", 0, 500, 500, 500), "Advanced Training ($10000)", 50);
                    ModHelperButton submitButton = panel.AddButton(new Info("GiveFood_", 450, 500, 150), VanillaSprites.GreenBtn, new Action(() => upgradeUi.AdvancedTraining()));
                }
            }
            if (AdoptAndRaiseAMonkey.mod.trainingLevel == 3)
            {
                if (AdoptAndRaiseAMonkey.mod.OrphanLevel >= 4)
                {
                    ModHelperText trainText = panel.AddText(new Info("Title_", 0, 500, 500, 500), "Mega Training ($50000)", 50);
                    ModHelperButton submitButton = panel.AddButton(new Info("GiveFood_", 450, 500, 150), VanillaSprites.GreenBtn, new Action(() => upgradeUi.MegaTraining()));
                }
            }
            if (AdoptAndRaiseAMonkey.mod.trainingLevel == 4)
            {
                if (AdoptAndRaiseAMonkey.mod.OrphanLevel >= 5)
                {
                    ModHelperText trainText = panel.AddText(new Info("Title_", 0, 500, 500, 500), "Metaversal Training ($250000)", 50);
                    ModHelperButton submitButton = panel.AddButton(new Info("GiveFood_", 450, 500, 150), VanillaSprites.GreenBtn, new Action(() => upgradeUi.MetaversalTraining()));
                }
            }
            if (AdoptAndRaiseAMonkey.mod.OrphanLevel >= 3)
            {
                if (AdoptAndRaiseAMonkey.mod.hasLifesteal == false)
                {
                    ModHelperText trainText = panel.AddText(new Info("Title_", 0, 300, 500, 500), "Unlock Lifesteal Ability ($30000) [Gives lives when attacking]", 30);
                    ModHelperButton submitButton = panel.AddButton(new Info("GiveFood_", 450, 300, 150), VanillaSprites.GreenBtn, new Action(() => upgradeUi.UnlockLifesteal()));
                }
                else
                {
                    ModHelperText trainText = panel.AddText(new Info("Title_", 0, 300, 500, 500), "Lifesteal Ability [Unlocked]", 30);
                }
            }
            if (AdoptAndRaiseAMonkey.mod.OrphanLevel >= 4)
            {
                if (AdoptAndRaiseAMonkey.mod.hasDiminish == false)
                {
                    ModHelperText trainText = panel.AddText(new Info("Title_", 0, 100, 500, 500), "Unlock Syphon Ability ($90000) [Every bloon spawned rewards cash]", 30);
                    ModHelperButton submitButton = panel.AddButton(new Info("GiveFood_", 450, 100, 150), VanillaSprites.GreenBtn, new Action(() => upgradeUi.UnlockDiminsh()));
                }
                else
                {
                    ModHelperText trainText = panel.AddText(new Info("Title_", 0, 100, 500, 500), "Syphon Ability [Unlocked]", 30);
                }
            }
            if (AdoptAndRaiseAMonkey.mod.OrphanLevel >= 5)
            {
                if (AdoptAndRaiseAMonkey.mod.hasInfiniteTraining == false)
                {
                    ModHelperText trainText = panel.AddText(new Info("Title_", 0, -150, 500, 500), "Unlock Infinity Training ($150000) [Ability to upgrade stats further]", 30);
                    ModHelperButton submitButton = panel.AddButton(new Info("GiveFood_", 450, -150, 150), VanillaSprites.GreenBtn, new Action(() => upgradeUi.UnlockInfiniteTraining()));
                }
                else
                {
                    ModHelperText trainText = panel.AddText(new Info("Title_", 0, -150, 500, 500), "Infinity Training [Unlocked]", 30);
                }
            }
            ModHelperButton giveFoodButton = panel.AddButton(new Info("GiveFood", 450, 700, 150), VanillaSprites.GreenBtn, new Action(() => upgradeUi.GiveFood()));
        }
    }
    public class adopted : ModTower
    {


        public override TowerSet TowerSet => TowerSet.Primary;
        public override string BaseTower => "DartMonkey-002";
        public override int Cost => 0;
        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;
        public override string Description => "Raise this monkey and train them to fight bloons!";
        public override string DisplayName => "Orphan Monkey";
        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].name = "orphanWeapon";
        }
    }
}