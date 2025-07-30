using BlasII.ModdingAPI.Assets;
using Il2Cpp;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Components.UI.Inventory;
using Il2CppTGK.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FigurePairHelper
{
    public class PairsContainer : MonoBehaviour
    {
        public static PairsContainer Instance { get; private set; }

        public GameObject content;

        public GameObject figure1;
        public UIPixelTextWithShadow figure1Text;
        public Image figure1Image;

        public GameObject figure2;
        public UIPixelTextWithShadow figure2Text;
        public Image figure2Image;

        public GameObject figure3;
        public UIPixelTextWithShadow figure3Text;
        public Image figure3Image;

        public GameObject figure4;
        public UIPixelTextWithShadow figure4Text;
        public Image figure4Image;

        private const int xPosSlots = -336;
        private const int yPosSlots = 0;

        private const int xPosAllFigures = -425;
        private const int yPosAllFigures = -80;

        private const int xSize = 740;
        private const int ySize1 = 140;
        private const int ySize2 = 210;
        private const int ySize3 = 290;
        private const int ySize4 = 360;

        public static readonly Color textGold = new Color(1, 0.898f, 0.447f);
        public static readonly Color textRed = new Color(0.678f, 0.098f, 0.098f);

        private static readonly Dictionary<string, bool> isSlotsInUpperHalf = new Dictionary<string, bool>()
        {
            ["slot0"] = true,
            ["slot1"] = true,
            ["slot2"] = true,
            ["slot3"] = true,
            ["slot4"] = false,
            ["slot5"] = false,
            ["slot6"] = false,
            ["slot7"] = false
        };

        private static readonly Dictionary<string, bool> isAllFiguresInUpperHalf = new Dictionary<string, bool>()
        {
            ["Grid_0x0"] = true,
            ["Grid_1x0"] = true,
            ["Grid_2x0"] = true,
            ["Grid_3x0"] = true,
            ["Grid_4x0"] = true,
            ["Grid_0x1"] = false,
            ["Grid_1x1"] = false,
            ["Grid_2x1"] = false,
            ["Grid_3x1"] = false,
            ["Grid_4x1"] = false
        };

        public void Awake()
        {
            if (Instance != null && Instance != this) return;
            Instance = this;
        }

        public void Setup()
        {
            content = gameObject;
            content.name = "FigurePairsContainer";
            Component.Destroy(content.GetComponent<MonoBehaviourCallbacks>());
            GameObject.Destroy(content.transform.Find("Button/").gameObject);
            content.transform.localPosition = new Vector3(xPosSlots, yPosSlots, 0);
            content.GetComponent<Image>().color = new Color(1, 1, 1, 0.6f);

            Transform loreInfo = content.transform.Find("LoreInfo/");
            GameObject.Destroy(loreInfo.Find("Description/").gameObject);
            Component.Destroy(loreInfo.Find("Caption/Image/").GetComponent<Image>());
            Component.Destroy(loreInfo.GetComponent<InventoryInfo>());
            loreInfo.name = "FiguresList";
            loreInfo.transform.localPosition = new Vector3(-50, 90, 0);
            figure1 = loreInfo.Find("Caption/").gameObject;
            figure1.name = "Figure1";
            figure1.GetComponent<LayoutElement>().minHeight = 55;
            figure1Text = figure1.transform.Find("Caption/").GetComponent<UIPixelTextWithShadow>();
            figure1Image = figure1.transform.Find("Image/item/").GetComponent<Image>();
            figure1Image.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            figure1Image.gameObject.AddComponent<Shadow>().effectColor = new Color(0, 0, 0, 1);
            figure1Image.gameObject.GetComponent<Shadow>().effectDistance = new Vector2(0, -10);

            figure2 = GameObject.Instantiate(figure1, figure1.transform.parent);
            figure2.name = "Figure2";
            figure2Text = figure2.transform.Find("Caption/").GetComponent<UIPixelTextWithShadow>();
            figure2Image = figure2.transform.Find("Image/item/").GetComponent<Image>();
            figure2Image.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            figure3 = GameObject.Instantiate(figure1, figure1.transform.parent);
            figure3.name = "Figure3";
            figure3Text = figure3.transform.Find("Caption/").GetComponent<UIPixelTextWithShadow>();
            figure3Image = figure3.transform.Find("Image/item/").GetComponent<Image>();
            figure3Image.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            figure4 = GameObject.Instantiate(figure1, figure1.transform.parent);
            figure4.name = "Figure4";
            figure4Text = figure4.transform.Find("Caption/").GetComponent<UIPixelTextWithShadow>();
            figure4Image = figure4.transform.Find("Image/item/").GetComponent<Image>();
            figure4Image.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            gameObject.AddComponent<Shadow>().effectDistance = new Vector2(0, -8);
            //gameObject.SetActive(false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetPosAndSize(string slot, int y)
        {
            RectTransform rt = GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(xSize, y);

            if (isSlotsInUpperHalf.ContainsKey(slot))
            {
                gameObject.transform.localPosition = new Vector2(xPosSlots, yPosSlots);
                if (isSlotsInUpperHalf[slot]) rt.pivot = new Vector2(0.5f, 1);
                else rt.pivot = new Vector2(0.5f, 0);
            }
            else if (isAllFiguresInUpperHalf.ContainsKey(slot))
            {
                gameObject.transform.localPosition = new Vector2(xPosAllFigures, yPosAllFigures);
                if (isAllFiguresInUpperHalf[slot]) rt.pivot = new Vector2(0.5f, 1);
                else rt.pivot = new Vector2(0.5f, 0);
            }
        }

        public void SetTextAndIcons(string slot, FigureItemID fg1)
        {
            SetPosAndSize(slot, ySize1);
            gameObject.SetActive(true);

            if (fg1 == null)
            {
                figure1.SetActive(true);
                figure1Image.sprite = FigurePairHelper.GetFigureCategorySprite(FigureCategory.Empty);
                figure1Text.normalText.color = Color.gray;
                figure1Text.SetText(Main.FigurePairHelper.LocalizationHandler.Localize("nopairs"));

                figure2.SetActive(false);
                figure3.SetActive(false);
                figure4.SetActive(false);
            }
            else
            {
                figure1.SetActive(true);
                figure1Image.sprite = FigurePairHelper.GetFigureCategorySprite(FigurePairHelper.figureCategories[fg1.name]);
                if (AssetStorage.PlayerInventory.HasItem(fg1)) figure1Text.normalText.color = textGold;
                else figure1Text.normalText.color = textRed;
                figure1Text.SetText(fg1.caption);

                figure2.SetActive(false);
                figure3.SetActive(false);
                figure4.SetActive(false);
            }
        }

        public void SetTextAndIcons(string slot, FigureItemID fg1, FigureItemID fg2)
        {
            SetPosAndSize(slot, ySize2);
            gameObject.SetActive(true);

            figure1.SetActive(true);
            figure1Image.sprite = FigurePairHelper.GetFigureCategorySprite(FigurePairHelper.figureCategories[fg1.name]);
            if (AssetStorage.PlayerInventory.HasItem(fg1)) figure1Text.normalText.color = textGold;
            else figure1Text.normalText.color = textRed;
            figure1Text.SetText(fg1.caption);

            figure2.SetActive(true);
            figure2Image.sprite = FigurePairHelper.GetFigureCategorySprite(FigurePairHelper.figureCategories[fg2.name]);
            if (AssetStorage.PlayerInventory.HasItem(fg2)) figure2Text.normalText.color = textGold;
            else figure2Text.normalText.color = textRed;
            figure2Text.SetText(fg2.caption);

            figure3.SetActive(false);
            figure4.SetActive(false);
        }

        public void SetTextAndIcons(string slot, FigureItemID fg1, FigureItemID fg2, FigureItemID fg3)
        {
            SetPosAndSize(slot, ySize3);
            gameObject.SetActive(true);

            figure1.SetActive(true);
            figure1Image.sprite = FigurePairHelper.GetFigureCategorySprite(FigurePairHelper.figureCategories[fg1.name]);
            if (AssetStorage.PlayerInventory.HasItem(fg1)) figure1Text.normalText.color = textGold;
            else figure1Text.normalText.color = textRed;
            figure1Text.SetText(fg1.caption);

            figure2.SetActive(true);
            figure2Image.sprite = FigurePairHelper.GetFigureCategorySprite(FigurePairHelper.figureCategories[fg2.name]);
            if (AssetStorage.PlayerInventory.HasItem(fg2)) figure2Text.normalText.color = textGold;
            else figure2Text.normalText.color = textRed;
            figure2Text.SetText(fg2.caption);

            figure3.SetActive(true);
            figure3Image.sprite = FigurePairHelper.GetFigureCategorySprite(FigurePairHelper.figureCategories[fg3.name]);
            if (AssetStorage.PlayerInventory.HasItem(fg3)) figure3Text.normalText.color = textGold;
            else figure3Text.normalText.color = textRed;
            figure3Text.SetText(fg3.caption);

            figure4.SetActive(false);
        }

        public void SetTextAndIcons(string slot, FigureItemID fg1, FigureItemID fg2, FigureItemID fg3, FigureItemID fg4)
        {
            SetPosAndSize(slot, ySize4);
            gameObject.SetActive(true);

            figure1.SetActive(true);
            figure1Image.sprite = FigurePairHelper.GetFigureCategorySprite(FigurePairHelper.figureCategories[fg1.name]);
            if (AssetStorage.PlayerInventory.HasItem(fg1)) figure1Text.normalText.color = textGold;
            else figure1Text.normalText.color = textRed;
            figure1Text.SetText(fg1.caption);

            figure2.SetActive(true);
            figure2Image.sprite = FigurePairHelper.GetFigureCategorySprite(FigurePairHelper.figureCategories[fg2.name]);
            if (AssetStorage.PlayerInventory.HasItem(fg2)) figure2Text.normalText.color = textGold;
            else figure2Text.normalText.color = textRed;
            figure2Text.SetText(fg2.caption);

            figure3.SetActive(true);
            figure3Image.sprite = FigurePairHelper.GetFigureCategorySprite(FigurePairHelper.figureCategories[fg3.name]);
            if (AssetStorage.PlayerInventory.HasItem(fg3)) figure3Text.normalText.color = textGold;
            else figure3Text.normalText.color = textRed;
            figure3Text.SetText(fg3.caption);

            figure4.SetActive(true);
            figure4Image.sprite = FigurePairHelper.GetFigureCategorySprite(FigurePairHelper.figureCategories[fg4.name]);
            if (AssetStorage.PlayerInventory.HasItem(fg4)) figure4Text.normalText.color = textGold;
            else figure4Text.normalText.color = textRed;
            figure4Text.SetText(fg4.caption);
        }
    }
}
