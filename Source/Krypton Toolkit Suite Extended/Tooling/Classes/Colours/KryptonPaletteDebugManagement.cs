﻿using ComponentFactory.Krypton.Toolkit;
using Core.Settings.Classes;
using System.Drawing;

namespace Core.Classes.Colours
{
    public class KryptonPaletteDebugManagement
    {
        public KryptonPaletteDebugManagement()
        {

        }

        public static void PropagateHEXColourValues(KryptonListBox hexValues, bool useUppercase)
        {
            ColourSettingsManager colourSettingsManager = new ColourSettingsManager();

            if (useUppercase)
            {
                hexValues.Items.Add($"Base Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetBaseColour()) }");

                hexValues.Items.Add($"Darkest Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetDarkestColour()) }");

                hexValues.Items.Add($"Medium Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetMediumColour()) }");

                hexValues.Items.Add($"Light Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLightColour()) }");

                hexValues.Items.Add($"Lightest Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLightestColour()) }");

                hexValues.Items.Add($"Border Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetBorderColour()) }");

                hexValues.Items.Add($"Alternative Normal Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetAlternativeNormalTextColour()) }");

                hexValues.Items.Add($"Disabled Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetDisabledTextColour()) }");

                hexValues.Items.Add($"Normal Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetNormalTextColour()) }");

                hexValues.Items.Add($"Focused Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetFocusedTextColour()) }");

                hexValues.Items.Add($"Pressed Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetPressedTextColour()) }");

                hexValues.Items.Add($"Ribbon Tab Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetRibbonTabTextColour()) }");

                hexValues.Items.Add($"Disabled Control Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetDisabledColour()) }");

                hexValues.Items.Add($"Link Disabled Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLinkDisabledColour()) }");

                hexValues.Items.Add($"Link Focused Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLinkFocusedColour()) }");

                hexValues.Items.Add($"Link Normal Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLinkNormalColour()) }");

                hexValues.Items.Add($"Link Hover Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLinkHoverColour()) }");

                hexValues.Items.Add($"Link Visited Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLinkVisitedColour()) }");

                hexValues.Items.Add($"Menu Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetMenuTextColour()) }");

                hexValues.Items.Add($"Status Strip Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetStatusTextColour()) }");

                hexValues.Items.Add($"Custom Colour One Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomColourOne()) }");

                hexValues.Items.Add($"Custom Colour Two Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomColourTwo()) }");

                hexValues.Items.Add($"Custom Colour Three Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomColourThree()) }");

                hexValues.Items.Add($"Custom Colour Four Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomColourFour()) }");

                hexValues.Items.Add($"Custom Colour Five Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomColourFive()) }");

                hexValues.Items.Add($"Custom Text Colour One Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomTextColourOne()) }");

                hexValues.Items.Add($"Custom Text Colour Two Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomTextColourTwo()) }");

                hexValues.Items.Add($"Custom Text Colour Three Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomTextColourThree()) }");

                hexValues.Items.Add($"Custom Text Colour Four Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomTextColourFour()) }");

                hexValues.Items.Add($"Custom Text Colour Five Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomTextColourFive()) }");
            }
            else
            {
                hexValues.Items.Add($"Base Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetBaseColour()).ToUpper() }");

                hexValues.Items.Add($"Darkest Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetDarkestColour()).ToUpper() }");

                hexValues.Items.Add($"Medium Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetMediumColour()).ToUpper() }");

                hexValues.Items.Add($"Light Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLightColour()).ToUpper() }");

                hexValues.Items.Add($"Lightest Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLightestColour()).ToUpper() }");

                hexValues.Items.Add($"Border Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetBorderColour()).ToUpper() }");

                hexValues.Items.Add($"Alternative Normal Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetAlternativeNormalTextColour()).ToUpper() }");

                hexValues.Items.Add($"Disabled Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetDisabledTextColour()).ToUpper() }");

                hexValues.Items.Add($"Normal Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetNormalTextColour()).ToUpper() }");

                hexValues.Items.Add($"Focused Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetFocusedTextColour()).ToUpper() }");

                hexValues.Items.Add($"Pressed Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetPressedTextColour()).ToUpper() }");

                hexValues.Items.Add($"Ribbon Tab Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetRibbonTabTextColour()).ToUpper() }");

                hexValues.Items.Add($"Disabled Control Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetDisabledColour()).ToUpper() }");

                hexValues.Items.Add($"Link Disabled Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLinkDisabledColour()).ToUpper() }");

                hexValues.Items.Add($"Link Focused Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLinkFocusedColour()).ToUpper() }");

                hexValues.Items.Add($"Link Normal Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLinkNormalColour()).ToUpper() }");

                hexValues.Items.Add($"Link Hover Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLinkHoverColour()).ToUpper() }");

                hexValues.Items.Add($"Link Visited Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetLinkVisitedColour()).ToUpper() }");

                hexValues.Items.Add($"Menu Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetMenuTextColour()).ToUpper() }");

                hexValues.Items.Add($"Status Strip Text Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetStatusTextColour()).ToUpper() }");

                hexValues.Items.Add($"Custom Colour One Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomColourOne()).ToUpper() }");

                hexValues.Items.Add($"Custom Colour Two Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomColourTwo()).ToUpper() }");

                hexValues.Items.Add($"Custom Colour Three Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomColourThree()).ToUpper() }");

                hexValues.Items.Add($"Custom Colour Four Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomColourFour()).ToUpper() }");

                hexValues.Items.Add($"Custom Colour Five Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomColourFive()).ToUpper() }");

                hexValues.Items.Add($"Custom Text Colour One Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomTextColourOne()).ToUpper() }");

                hexValues.Items.Add($"Custom Text Colour Two Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomTextColourTwo()).ToUpper() }");

                hexValues.Items.Add($"Custom Text Colour Three Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomTextColourThree()).ToUpper() }");

                hexValues.Items.Add($"Custom Text Colour Four Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomTextColourFour()).ToUpper() }");

                hexValues.Items.Add($"Custom Text Colour Five Colour: { ColorTranslator.ToHtml(colourSettingsManager.GetCustomTextColourFive()).ToUpper() }");
            }
        }
    }
}