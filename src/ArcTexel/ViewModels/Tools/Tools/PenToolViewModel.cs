using Avalonia.Input;
using Drawie.Backend.Core.Vector;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.Extensions.CommonApi.UserPreferences.Settings.ArcTexel;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.Handlers.Tools;
using ArcTexel.Models.Input;
using Drawie.Numerics;
using ArcTexel.Models.Handlers.Toolbars;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Document.Blackboard;
using ArcTexel.ViewModels.Tools.ToolSettings.Settings;
using ArcTexel.ViewModels.Tools.ToolSettings.Toolbars;
using ArcTexel.Views.Blackboard;
using ArcTexel.Views.Overlays.BrushShapeOverlay;

namespace ArcTexel.ViewModels.Tools.Tools
{
    [Command.Tool(Key = Key.B)]
    internal class PenToolViewModel : BrushBasedToolViewModel, IPenToolHandler
    {
        public override string ToolNameLocalizationKey => "PEN_TOOL";

        public PenToolViewModel()
        {
            ViewModelMain.Current.ToolsSubViewModel.SelectedToolChanged += SelectedToolChanged;
        }

        public override LocalizedString Tooltip => new LocalizedString("PEN_TOOL_TOOLTIP", Shortcut);

        [Settings.Bool("PIXEL_PERFECT_SETTING", Notify = nameof(PixelPerfectChanged), ExposedByDefault = false)]
        public bool PixelPerfectEnabled => GetValue<bool>();

        public override string DefaultIcon => ArcPerfectIcons.Pen;

        public override void KeyChanged(bool ctrlIsDown, bool shiftIsDown, bool altIsDown, Key argsKey)
        {
            ActionDisplay = new LocalizedString("PEN_TOOL_ACTION_DISPLAY", Shortcut);
        }

        protected override Toolbar CreateToolbar()
        {
            return ToolbarFactory.Create<PenToolViewModel, BrushToolbar>(this);
        }

        protected override void SwitchToTool()
        {
            ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UsePenTool();
        }

        private void SelectedToolChanged(object sender, SelectedToolEventArgs e)
        {
            if (e.NewTool == this && PixelPerfectEnabled)
            {
                var toolbar = (BrushToolbar)Toolbar;
                var setting = toolbar.Settings.FirstOrDefault(x => x.Name == nameof(toolbar.ToolSize));
                if (setting is SizeSettingViewModel sizeSetting)
                {
                    sizeSetting.SetOverwriteValue(1d);
                }
            }
        }

        private void PixelPerfectChanged()
        {
            var toolbar = (BrushToolbar)Toolbar;
            var setting = toolbar.Settings.FirstOrDefault(x => x.Name == nameof(toolbar.ToolSize));

            if (setting is SizeSettingViewModel sizeSettingViewModel)
            {
                sizeSettingViewModel.IsEnabled = !PixelPerfectEnabled;

                if (PixelPerfectEnabled)
                {
                    sizeSettingViewModel.SetOverwriteValue(1d);
                }
                else
                {
                    sizeSettingViewModel.ResetOverwrite();
                }
            }
        }
    }
}
