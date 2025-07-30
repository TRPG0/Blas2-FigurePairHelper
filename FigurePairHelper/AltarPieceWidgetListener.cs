using BlasII.ModdingAPI;
using Il2CppTGK.Game.Components.UI.Altarpiece;
using UnityEngine;

namespace FigurePairHelper
{
    public class AltarPieceWidgetListener : MonoBehaviour
    {
        public static AltarPieceWidgetListener Instance { get; private set; }

        public AltarPieceWidget widget;

        public static event OnAltarPieceModeChangedDelegate OnAltarPieceModeChanged;

        public delegate void OnAltarPieceModeChangedDelegate(AltarPieceWidget.Mode mode);

        private AltarPieceWidget.Mode previous;

        public void Awake()
        {
            if (Instance != null && Instance != this) return;
            Instance = this;
            widget = GetComponent<AltarPieceWidget>();
            ModLog.Info("Found AltarPieceWidget.");
        }

        public void Update()
        {
            if (previous != widget.CurrentMode)
            {
                if (OnAltarPieceModeChanged != null) OnAltarPieceModeChanged(widget.CurrentMode);
                previous = widget.CurrentMode;
            }
        }

        public void OnDestroy()
        {
            OnAltarPieceModeChanged = null;
        }
    }
}
