using Drawie.Backend.Core.Text;
using ArcTexel.Models.Controllers;
using ArcTexel.Models.Handlers.Toolbars;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Tools.ToolSettings.Settings;

namespace ArcTexel.ViewModels.Tools.ToolSettings.Toolbars;

internal class TextToolbar : FillableShapeToolbar, ITextToolbar
{
    public FontFamilyName FontFamily
    {
        get
        {
            return GetSetting<FontFamilySettingViewModel>(nameof(FontFamily)).Value;
        }
        set
        {
            int index = Array.IndexOf(FontLibrary.AllFonts, value);
            if (index == -1)
            {
                try
                {
                    using Font font = Font.FromFontFamily(value);
                    if(font != null && FontLibrary.TryAddCustomFont(value))
                    {
                        index = Array.IndexOf(FontLibrary.AllFonts, value);
                    }

                    if (index == -1)
                    {
                        index = 0;
                    }
                }
                catch
                {
                    index = 0;
                }

            }

            GetSetting<FontFamilySettingViewModel>(nameof(FontFamily)).FontIndex = index;
        }
    }

    public double FontSize
    {
        get
        {
            return GetSetting<SizeSettingViewModel>(nameof(FontSize)).Value;
        }
        set
        {
            GetSetting<SizeSettingViewModel>(nameof(FontSize)).Value = value;
        }
    }

    public double Spacing
    {
        get
        {
            return GetSetting<SizeSettingViewModel>(nameof(Spacing)).Value;
        }
        set
        {
            GetSetting<SizeSettingViewModel>(nameof(Spacing)).Value = value;
        }
    }
    
    public bool ForceLowDpiRendering
    {
        get
        {
            return GetSetting<BoolSettingViewModel>(nameof(ForceLowDpiRendering)).Value;
        }
        set
        {
            GetSetting<BoolSettingViewModel>(nameof(ForceLowDpiRendering)).Value = value;
        }
    }

    public bool Bold
    {
        get
        {
            return GetSetting<BoolSettingViewModel>(nameof(Bold)).Value;
        }
        set
        {
            GetSetting<BoolSettingViewModel>(nameof(Bold)).Value = value;
        }
    }

    public bool Italic
    {
        get
        {
            return GetSetting<BoolSettingViewModel>(nameof(Italic)).Value;
        }
        set
        {
            GetSetting<BoolSettingViewModel>(nameof(Italic)).Value = value;
        }
    }

    public TextToolbar()
    {
        AddSetting(new FontFamilySettingViewModel(nameof(FontFamily), ""));
        FontFamily = FontLibrary.DefaultFontFamily;
        
        var sizeSetting =
            new SizeSettingViewModel(nameof(FontSize), "FONT_SIZE_LABEL", unit: new LocalizedString("UNIT_PT"))
            {
                Value = 12
            };
        AddSetting(sizeSetting);
        var spacingSetting =
            new SizeSettingViewModel(nameof(Spacing), unit: new LocalizedString("UNIT_PT"))
            {
                Tooltip = "SPACING_LABEL",
                Icon = ArcPerfectIcons.LineHeight
            };
        spacingSetting.Value = 12;

        sizeSetting.ValueChanged += (sender, args) =>
        {
            double delta = args.NewValue - args.OldValue;
            spacingSetting.Value += delta;
        };

        AddSetting(spacingSetting);

        AddSetting(new BoolSettingViewModel(nameof(Bold))
        {
            Icon = ArcPerfectIcons.Bold, Tooltip = "BOLD_TOOLTIP"
        });

        AddSetting(new BoolSettingViewModel(nameof(Italic))
        {
            Icon = ArcPerfectIcons.Italic, Tooltip = "ITALIC_TOOLTIP"
        });

        AddSetting(new BoolSettingViewModel(nameof(ForceLowDpiRendering), "__force_low_dpi_rendering")
        {
            IsExposed = false, Value = false
        });
    }

    public Font ConstructFont()
    {
        Font font = null;
        if (!string.IsNullOrEmpty(FontFamily.Name))
        {
            font = Font.FromFontFamily(FontFamily);
        }

        if (font is null)
        {
            font = Font.CreateDefault();
        }

        font.Size = (float)FontSize;
        font.Edging = AntiAliasing ? FontEdging.AntiAlias : FontEdging.Alias;
        font.Bold = Bold;
        font.Italic = Italic;

        return font;
    }
}
