using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using HarmonyLib;
using System.Collections.Generic;
using Il2CppTGK.Game.Components.UI.Altarpiece;
using MelonLoader;
using System;
using System.Collections;
using UnityEngine;
using Il2CppTGK.Inventory;
using Il2CppTGK.Game.Components.UI.Inventory;
using UnityEngine.UI;

namespace FigurePairHelper;

public class FigurePairHelper : BlasIIMod
{
    internal FigurePairHelper() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    public static AltarpieceLogic altarpieceLogic;

    public static Sprite categoryEmpty;
    public static Sprite categoryErudition;
    public static Sprite categoryFaith;
    public static Sprite categoryPilgrimage;
    public static Sprite categoryPunishment;

    public static readonly Dictionary<string, FigureCategory> figureCategories = new Dictionary<string, FigureCategory>()
    {
        ["FG01"] = FigureCategory.Faith,
        ["FG11"] = FigureCategory.Faith,
        ["FG16"] = FigureCategory.Faith,
        ["FG22"] = FigureCategory.Faith,
        ["FG29"] = FigureCategory.Faith,
        ["FG45"] = FigureCategory.Faith,
        ["FG44"] = FigureCategory.Faith,
        ["FG104"] = FigureCategory.Faith,
        ["FG113"] = FigureCategory.Faith,
        ["FG31"] = FigureCategory.Faith,
        ["FG02"] = FigureCategory.Pilgrimage,
        ["FG13"] = FigureCategory.Pilgrimage,
        ["FG14"] = FigureCategory.Pilgrimage,
        ["FG15"] = FigureCategory.Pilgrimage,
        ["FG36"] = FigureCategory.Pilgrimage,
        ["FG103"] = FigureCategory.Pilgrimage,
        ["FG109"] = FigureCategory.Pilgrimage,
        ["FG105"] = FigureCategory.Pilgrimage,
        ["FG111"] = FigureCategory.Pilgrimage,
        ["FG32"] = FigureCategory.Pilgrimage,
        ["FG03"] = FigureCategory.Punishment,
        ["FG12"] = FigureCategory.Punishment,
        ["FG17"] = FigureCategory.Punishment,
        ["FG21"] = FigureCategory.Punishment,
        ["FG23"] = FigureCategory.Punishment,
        ["FG27"] = FigureCategory.Punishment,
        ["FG28"] = FigureCategory.Punishment,
        ["FG107"] = FigureCategory.Punishment,
        ["FG106"] = FigureCategory.Punishment,
        ["FG33"] = FigureCategory.Punishment,
        ["FG04"] = FigureCategory.Erudition,
        ["FG05"] = FigureCategory.Erudition,
        ["FG06"] = FigureCategory.Erudition,
        ["FG07"] = FigureCategory.Erudition,
        ["FG08"] = FigureCategory.Erudition,
        ["FG09"] = FigureCategory.Erudition,
        ["FG10"] = FigureCategory.Erudition,
        ["FG26"] = FigureCategory.Erudition,
        ["FG101"] = FigureCategory.Erudition,
        ["FG30"] = FigureCategory.Erudition,
        ["FG19"] = FigureCategory.Empty,
        ["FG18"] = FigureCategory.Empty,
        ["FG25"] = FigureCategory.Empty,
        ["FG20"] = FigureCategory.Empty,
        ["FG102"] = FigureCategory.Empty,
        ["FG108"] = FigureCategory.Empty,
        ["FG110"] = FigureCategory.Empty,
        ["FG112"] = FigureCategory.Empty
    };

    public static readonly Dictionary<string, string[]> figurePairs = new Dictionary<string, string[]>()
    {
        // The Anointed One
        ["FG01"] = ["FG04", "FG06", "FG19"],

        // The Veteran One
        ["FG02"] = ["FG05", "FG07", "FG19"],

        // The Punished One
        ["FG03"] = ["FG05", "FG07", "FG19"],

        // The Purified One
        ["FG04"] = ["FG01"],

        // The Tempest
        ["FG05"] = ["FG02", "FG14", "FG03"],

        // The Alchemist
        ["FG06"] = ["FG01", "FG14", "FG15", "FG23"],

        // The Guide
        ["FG07"] = ["FG02", "FG03"],

        // The Woman of the Stolen Face
        ["FG09"] = ["FG18"],

        // The Selfless Father
        ["FG10"] = ["FG11"],

        // The Thurifer
        ["FG11"] = ["FG10"],

        // The Demented One
        ["FG12"] = ["FG15"],

        // The Pilgrim
        ["FG13"] = ["FG21"],

        // The Pillager
        ["FG14"] = ["FG05", "FG06"],

        // The Partisan
        ["FG15"] = ["FG12", "FG06"],

        // The Ecstatic Novice
        ["FG16"] = ["FG17", "FG21"],

        // Viridiana
        ["FG17"] = ["FG16"],

        // The Flagellant
        ["FG18"] = ["FG09"],

        // The Confessor
        ["FG19"] = ["FG01", "FG02", "FG03"],

        // Castula
        ["FG20"] = ["FG23"],

        // Nacimiento
        ["FG21"] = ["FG16", "FG22", "FG13"],

        // The Scribe
        ["FG22"] = ["FG21"],

        // Trifon
        ["FG23"] = ["FG06", "FG20"],

        // Tirso
        ["FG27"] = ["FG102"],

        // Cobijada Mayor
        ["FG45"] = ["FG107"],

        // The Mercy
        ["FG102"] = ["FG27"],

        // The Family
        ["FG107"] = ["FG45"],
    };

    protected override void OnInitialize()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
    }

    public static void SetupAltarpieceLogic(AltarpieceLogic apl)
    {
        if (altarpieceLogic != null)
        {
            CheckSlotsSelect();
            return;
        }

        altarpieceLogic = apl;
        apl.altarPieceWidget.gameObject.AddComponent<AltarPieceWidgetListener>();
        AltarPieceWidgetListener.OnAltarPieceModeChanged += AltarPieceModeChanged;

        apl.altarPieceWidget.allFiguresWidget.OnTabChanged.AddListener((Action)CheckAllFiguresSelect);
        apl.altarPieceWidget.allFiguresWidget.gameObject.AddComponent<AllFiguresWidgetListener>();
        AllFiguresWidgetListener.OnAllFiguresElementChanged += OnAllFiguresSelectChanged;

        AltarPieceSlotsWidget slotsWidget = apl.altarPieceWidget.slotsWidget;
        slotsWidget.OnElementChanged.AddListener((Action)CheckSlotsSelect);
        slotsWidget.OnTabChange.AddListener((Action)OnSlotsTabChanged);
        slotsWidget.OnAcceptPressedEvent.AddListener((Action)CheckAllFiguresSelect);

        if (PairsContainer.Instance == null) CreatePairsContainer();
    }

    public static void AltarPieceModeChanged(AltarPieceWidget.Mode mode)
    {
        if (mode == AltarPieceWidget.Mode.Slots) CheckSlotsSelect();
        //else if (mode == AltarPieceWidget.Mode.Edition) CheckAllFiguresSelect();
    }

    public static void OnSlotsTabChanged()
    {
        if (altarpieceLogic.altarPieceWidget.slotsWidget.CurrentMode == AltarPieceSlotsWidget.Modes.Slots) PairsContainer.Instance.Hide();
        else MelonCoroutines.Start(OnSlotsSelectChanged());
    }

    public static void CheckSlotsSelect()
    {
        if (altarpieceLogic.altarPieceWidget.slotsWidget.CurrentMode == AltarPieceSlotsWidget.Modes.Slots) MelonCoroutines.Start(OnSlotsSelectChanged());
            
    }

    public static IEnumerator OnSlotsSelectChanged()
    {
        yield return null; // wait 1 frame

        if (altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlotItem)
        {
            if (figurePairs.ContainsKey(altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlotItem.name))
            {
                string figureName = altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlotItem.name;

                /*
                ModLog.Info($"Selected: {altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlot.name} | " +
                    $"{figureName.PadRight(5)} {altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlotItem.id.caption}");
                foreach (string figure in figurePairs[figureName])
                {
                    ModLog.Info($"-> Pair: {figure.PadRight(5)} {AssetStorage.Figures[figure].caption} {(AssetStorage.PlayerInventory.HasItem(AssetStorage.Figures[figure]) ? "(unlocked)" : "(not unlocked)")}");
                }
                */

                FigureItemID fg1 = null;
                FigureItemID fg2 = null;
                FigureItemID fg3 = null;
                FigureItemID fg4 = null;

                switch (figurePairs[figureName].Length)
                {
                    case 1:
                        fg1 = AssetStorage.Figures[figurePairs[figureName][0]];
                        PairsContainer.Instance.SetTextAndIcons(altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlot.name, fg1);
                        break;
                    case 2:
                        fg1 = AssetStorage.Figures[figurePairs[figureName][0]];
                        fg2 = AssetStorage.Figures[figurePairs[figureName][1]];
                        PairsContainer.Instance.SetTextAndIcons(altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlot.name, fg1, fg2);
                        break;
                    case 3:
                        fg1 = AssetStorage.Figures[figurePairs[figureName][0]];
                        fg2 = AssetStorage.Figures[figurePairs[figureName][1]];
                        fg3 = AssetStorage.Figures[figurePairs[figureName][2]];
                        PairsContainer.Instance.SetTextAndIcons(altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlot.name, fg1, fg2, fg3);
                        break;
                    case 4:
                        fg1 = AssetStorage.Figures[figurePairs[figureName][0]];
                        fg2 = AssetStorage.Figures[figurePairs[figureName][1]];
                        fg3 = AssetStorage.Figures[figurePairs[figureName][2]];
                        fg4 = AssetStorage.Figures[figurePairs[figureName][3]];
                        PairsContainer.Instance.SetTextAndIcons(altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlot.name, fg1, fg2, fg3, fg4);
                        break;
                    default: break;
                }
            }
            else
            {
                /*
                ModLog.Info($"Selected: {altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlot.name} | " +
                    $"{altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlotItem.name.PadRight(5)} {altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlotItem.id.caption}");
                ModLog.Info("-> No pairs");
                */
                PairsContainer.Instance.SetTextAndIcons(altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlot.name, null);
            }
        }
        else
        {
            /*
            ModLog.Info($"Selected: {altarpieceLogic.altarPieceWidget.slotsWidget.SelectedSlot.name} | null");
            ModLog.Info("-> Slot is empty");
            */
            PairsContainer.Instance.Hide();
        }
    }

    public static void CheckAllFiguresSelect()
    {
        OnAllFiguresSelectChanged(altarpieceLogic.altarPieceWidget.allFiguresWidget.SelectedFiguresGridItem, altarpieceLogic.altarPieceWidget.allFiguresWidget.SelectedFiguresItem);
    }

    public static void OnAllFiguresSelectChanged(AltarPieceGridItem grid, InventoryFigureItem figureItem)
    {
        if (figureItem)
        {
            if (figurePairs.ContainsKey(figureItem.name))
            {
                /*
                ModLog.Info($"Selected: {grid.name} | {figureItem.name.PadRight(5)} {figureItem.id.caption}");
                foreach (string figure in figurePairs[figureItem.name])
                {
                    ModLog.Info($"-> Pair: {figure.PadRight(5)} {AssetStorage.Figures[figure].caption} {(AssetStorage.PlayerInventory.HasItem(AssetStorage.Figures[figure]) ? "(unlocked)" : "(not unlocked)")}");
                }
                */

                FigureItemID fg1 = null;
                FigureItemID fg2 = null;
                FigureItemID fg3 = null;
                FigureItemID fg4 = null;

                switch (figurePairs[figureItem.name].Length)
                {
                    case 1:
                        fg1 = AssetStorage.Figures[figurePairs[figureItem.name][0]];
                        PairsContainer.Instance.SetTextAndIcons(grid.name, fg1);
                        break;
                    case 2:
                        fg1 = AssetStorage.Figures[figurePairs[figureItem.name][0]];
                        fg2 = AssetStorage.Figures[figurePairs[figureItem.name][1]];
                        PairsContainer.Instance.SetTextAndIcons(grid.name, fg1, fg2);
                        break;
                    case 3:
                        fg1 = AssetStorage.Figures[figurePairs[figureItem.name][0]];
                        fg2 = AssetStorage.Figures[figurePairs[figureItem.name][1]];
                        fg3 = AssetStorage.Figures[figurePairs[figureItem.name][2]];
                        PairsContainer.Instance.SetTextAndIcons(grid.name, fg1, fg2, fg3);
                        break;
                    case 4:
                        fg1 = AssetStorage.Figures[figurePairs[figureItem.name][0]];
                        fg2 = AssetStorage.Figures[figurePairs[figureItem.name][1]];
                        fg3 = AssetStorage.Figures[figurePairs[figureItem.name][2]];
                        fg4 = AssetStorage.Figures[figurePairs[figureItem.name][3]];
                        PairsContainer.Instance.SetTextAndIcons(grid.name, fg1, fg2, fg3, fg4);
                        break;
                    default: break;
                }
            }
            else
            {
                /*
                ModLog.Info($"Selected: {grid.name} | {figureItem.name.PadRight(5)} {figureItem.id.caption}");
                ModLog.Info("-> No pairs");
                */
                PairsContainer.Instance.SetTextAndIcons(grid.name, null);
            }
        }
        else
        {
            /*
            ModLog.Info($"Selected: {grid.name} | null");
            ModLog.Info("-> Slot is empty");
            */
            PairsContainer.Instance.Hide();
        }
    }

    public static void FindCategorySprites()
    {
        Transform dots = altarpieceLogic.altarPieceWidget.allFiguresWidget.gameObject.transform.Find("Selector/PageSelector/Dots/elements/");

        categoryFaith = dots.Find("dot0/dotBig/").GetComponent<Image>().sprite;
        categoryPilgrimage = dots.Find("dot1/dotBig/").GetComponent<Image>().sprite;
        categoryPunishment = dots.Find("dot2/dotBig/").GetComponent<Image>().sprite;
        categoryErudition = dots.Find("dot3/dotBig/").GetComponent<Image>().sprite;
        categoryEmpty = dots.Find("dot4/dotBig/").GetComponent<Image>().sprite;
    }

    public static Sprite GetFigureCategorySprite(FigureCategory category)
    {
        if (category == FigureCategory.Faith)
        {
            if (categoryFaith == null) FindCategorySprites();
            return categoryFaith;
        }
        else if (category == FigureCategory.Pilgrimage)
        {
            if (categoryPilgrimage == null) FindCategorySprites();
            return categoryPilgrimage;
        }
        else if (category == FigureCategory.Punishment)
        {
            if (categoryPunishment == null) FindCategorySprites();
            return categoryPunishment;
        }
        else if (category == FigureCategory.Erudition)
        {
            if (categoryErudition == null) FindCategorySprites();
            return categoryErudition;
        }
        else if (category == FigureCategory.Empty)
        {
            if (categoryEmpty == null) FindCategorySprites();
            return categoryEmpty;
        }
        return null;
    }

    public static void CreatePairsContainer()
    {
        GameObject.Instantiate(FigurePairHelper.altarpieceLogic.altarPieceWidget.lore.transform.Find("Background/Content/").gameObject, FigurePairHelper.altarpieceLogic.altarPieceWidget.lore.transform.parent).AddComponent<PairsContainer>().Setup();
    }

    public static List<string> SearchAssetKeys(string contains)
    {
        List<string> keys = new List<string>();
        foreach (AssetBundle bundle in (IEnumerable<AssetBundle>)AssetBundle.GetAllLoadedAssetBundles())
        {
            foreach (string key in bundle.GetAllAssetNames())
            {
                if (key.Contains(contains)) keys.Add(key);
            }
        }
        return keys;
    }
}

public enum FigureCategory
{
    Faith,
    Pilgrimage,
    Punishment,
    Erudition,
    Empty
}

[HarmonyPatch(typeof(AltarpieceLogic), "OnShow")]
public class AltarpieceLogic_OnShow_Patch
{
    public static void Postfix(AltarpieceLogic __instance)
    {
        ModLog.Info("Altarpiece menu opened.");
        FigurePairHelper.SetupAltarpieceLogic(__instance);
    }
}
