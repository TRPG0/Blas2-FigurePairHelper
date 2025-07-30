using Il2CppInterop.Runtime.Injection;
using MelonLoader;

namespace FigurePairHelper;

internal class Main : MelonMod
{
    public static FigurePairHelper FigurePairHelper { get; private set; }

    public override void OnLateInitializeMelon()
    {
        FigurePairHelper = new FigurePairHelper();
        ClassInjector.RegisterTypeInIl2Cpp<AllFiguresWidgetListener>();
        ClassInjector.RegisterTypeInIl2Cpp<AltarPieceWidgetListener>();
        ClassInjector.RegisterTypeInIl2Cpp<PairsContainer>();
    }
}