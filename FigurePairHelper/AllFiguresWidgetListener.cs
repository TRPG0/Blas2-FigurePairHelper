using BlasII.ModdingAPI;
using Il2CppTGK.Game.Components.UI.Altarpiece;
using Il2CppTGK.Game.Components.UI.Inventory;
using Il2CppTGK.Inventory;
using UnityEngine;

namespace FigurePairHelper
{
    public class AllFiguresWidgetListener : MonoBehaviour
    {
        public static AllFiguresWidgetListener Instance { get; private set; }

        public AltarPieceAllFiguresWidget widget;

        public static event OnAllFiguresElementChangedDelegate OnAllFiguresElementChanged;

        public delegate void OnAllFiguresElementChangedDelegate(AltarPieceGridItem grid, InventoryFigureItem figure);

        private InventoryFigureItem previous;

        public void Awake()
        {
            if (Instance != null && Instance != this) return;
            Instance = this;
            widget = GetComponent<AltarPieceAllFiguresWidget>();
            ModLog.Info("Found AltarPieceAllFiguresWidget.");
        }

        public void Update()
        {
            if (previous != widget.SelectedFiguresItem)
            {
                if (OnAllFiguresElementChanged != null) OnAllFiguresElementChanged(widget.SelectedFiguresGridItem, widget.SelectedFiguresItem);
                previous = widget.SelectedFiguresItem;
            }
        }

        public void OnDestroy()
        {
            OnAllFiguresElementChanged = null;
        }
    }
}
