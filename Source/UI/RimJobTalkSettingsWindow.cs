using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimJobTalk.UI
{
    /// <summary>
    /// Settings window for RimJobTalk.
    /// Allows players to edit prompt templates.
    /// </summary>
    public static class RimJobTalkSettingsUI
    {
        private static Vector2 scrollPosition = Vector2.zero;
        private static int selectedTab = 0;
        
        private static string[] GetTabNames()
        {
            return new[]
            {
                "RimJobTalk_Tab_Normal".Translate().ToString(),
                "RimJobTalk_Tab_Rape".Translate().ToString(),
                "RimJobTalk_Tab_Whoring".Translate().ToString(),
                "RimJobTalk_Tab_Bestiality".Translate().ToString(),
                "RimJobTalk_Tab_Necrophilia".Translate().ToString(),
                "RimJobTalk_Tab_Solo".Translate().ToString(),
                "RimJobTalk_Tab_Context".Translate().ToString(),
                "RimJobTalk_Tab_Help".Translate().ToString()
            };
        }

        /// <summary>
        /// Draw the settings window content
        /// </summary>
        public static void DoSettingsWindowContents(Rect inRect, RimJobTalkSettings settings)
        {
            // Layout
            float tabHeight = 35f;
            string[] tabNames = GetTabNames();
            float tabWidth = inRect.width / tabNames.Length;
            
            // Draw tabs
            Rect tabRect = new Rect(inRect.x, inRect.y, inRect.width, tabHeight);
            DrawTabs(tabRect, tabWidth, tabNames);
            
            // Content area
            Rect contentRect = new Rect(inRect.x, inRect.y + tabHeight + 10f, inRect.width, inRect.height - tabHeight - 50f);
            
            // Draw content based on selected tab
            Widgets.BeginScrollView(contentRect, ref scrollPosition, new Rect(0, 0, contentRect.width - 20f, GetContentHeight(selectedTab, settings)));
            
            float curY = 0f;
            switch (selectedTab)
            {
                case 0: // Normal
                    DrawPromptEditor(ref curY, contentRect.width - 20f,
                        "RimJobTalk_Prompt_Normal_Title".Translate(),
                        "RimJobTalk_Prompt_Normal_Desc".Translate(),
                        ref settings.PromptNormal, RimJobTalkSettings.DefaultPromptNormal,
                        "RimJobTalk_Variables_Standard".Translate());
                    break;
                    
                case 1: // Rape
                    DrawPromptEditor(ref curY, contentRect.width - 20f,
                        "RimJobTalk_Prompt_Rape_Title".Translate(),
                        "RimJobTalk_Prompt_Rape_Desc".Translate(),
                        ref settings.PromptRape, RimJobTalkSettings.DefaultPromptRape,
                        "RimJobTalk_Variables_Standard".Translate());
                    break;
                    
                case 2: // Whoring
                    DrawPromptEditor(ref curY, contentRect.width - 20f,
                        "RimJobTalk_Prompt_Whoring_Title".Translate(),
                        "RimJobTalk_Prompt_Whoring_Desc".Translate(),
                        ref settings.PromptWhoring, RimJobTalkSettings.DefaultPromptWhoring,
                        "RimJobTalk_Variables_Standard".Translate());
                    break;
                    
                case 3: // Bestiality
                    DrawPromptEditor(ref curY, contentRect.width - 20f,
                        "RimJobTalk_Prompt_Bestiality_Title".Translate(),
                        "RimJobTalk_Prompt_Bestiality_Desc".Translate(),
                        ref settings.PromptBestiality, RimJobTalkSettings.DefaultPromptBestiality,
                        "RimJobTalk_Variables_Bestiality".Translate());
                    break;
                    
                case 4: // Necrophilia
                    DrawPromptEditor(ref curY, contentRect.width - 20f,
                        "RimJobTalk_Prompt_Necrophilia_Title".Translate(),
                        "RimJobTalk_Prompt_Necrophilia_Desc".Translate(),
                        ref settings.PromptNecrophilia, RimJobTalkSettings.DefaultPromptNecrophilia,
                        "RimJobTalk_Variables_Necrophilia".Translate());
                    break;
                    
                case 5: // Solo
                    DrawPromptEditor(ref curY, contentRect.width - 20f,
                        "RimJobTalk_Prompt_Solo_Title".Translate(),
                        "RimJobTalk_Prompt_Solo_Desc".Translate(),
                        ref settings.PromptSolo, RimJobTalkSettings.DefaultPromptSolo,
                        "RimJobTalk_Variables_Solo".Translate());
                    break;
                    
                case 6: // Context
                    DrawContextEditors(ref curY, contentRect.width - 20f, settings);
                    break;
                    
                case 7: // Help
                    DrawHelpContent(ref curY, contentRect.width - 20f);
                    break;
            }
            
            Widgets.EndScrollView();
            
            // Bottom buttons
            Rect bottomRect = new Rect(inRect.x, inRect.yMax - 35f, inRect.width, 35f);
            DrawBottomButtons(bottomRect, settings);
        }

        private static void DrawTabs(Rect tabRect, float tabWidth, string[] tabNames)
        {
            for (int i = 0; i < tabNames.Length; i++)
            {
                Rect tabButtonRect = new Rect(tabRect.x + i * tabWidth, tabRect.y, tabWidth - 2f, tabRect.height);
                bool selected = selectedTab == i;
                
                if (selected)
                {
                    Widgets.DrawHighlightSelected(tabButtonRect);
                }
                
                if (Widgets.ButtonText(tabButtonRect, tabNames[i], true, true, selected ? Color.yellow : Color.white))
                {
                    selectedTab = i;
                    scrollPosition = Vector2.zero;
                }
            }
        }

        private static void DrawPromptEditor(ref float curY, float width, TaggedString title, TaggedString description,
            ref string promptValue, string defaultValue, TaggedString variableHint)
        {
            // Title
            Text.Font = GameFont.Medium;
            Rect titleRect = new Rect(0, curY, width, 30f);
            Widgets.Label(titleRect, title);
            curY += 35f;
            
            // Description
            Text.Font = GameFont.Small;
            GUI.color = Color.gray;
            Rect descRect = new Rect(0, curY, width, 20f);
            Widgets.Label(descRect, description);
            curY += 25f;
            
            // Variable hint
            GUI.color = new Color(0.5f, 0.8f, 1f);
            Rect hintRect = new Rect(0, curY, width, 20f);
            Widgets.Label(hintRect, variableHint);
            curY += 25f;
            GUI.color = Color.white;
            
            // Text area for prompt
            float textAreaHeight = 150f;
            Rect textAreaRect = new Rect(0, curY, width, textAreaHeight);
            promptValue = Widgets.TextArea(textAreaRect, promptValue);
            curY += textAreaHeight + 10f;
            
            // Reset button
            Rect resetRect = new Rect(0, curY, 120f, 25f);
            if (Widgets.ButtonText(resetRect, "RimJobTalk_Button_ResetToDefault".Translate()))
            {
                promptValue = defaultValue;
            }
            curY += 35f;
            
            // Separator
            curY += 10f;
            Widgets.DrawLineHorizontal(0, curY, width);
            curY += 15f;
        }

        private static void DrawContextEditors(ref float curY, float width, RimJobTalkSettings settings)
        {
            // Loving context
            DrawSmallPromptEditor(ref curY, width,
                "RimJobTalk_Context_Loving_Title".Translate(),
                "RimJobTalk_Context_Loving_Desc".Translate(),
                ref settings.ContextLoving, RimJobTalkSettings.DefaultContextLoving);
            
            // Tailjob context
            DrawSmallPromptEditor(ref curY, width,
                "RimJobTalk_Context_Tailjob_Title".Translate(),
                "RimJobTalk_Context_Tailjob_Desc".Translate(),
                ref settings.ContextTailjob, RimJobTalkSettings.DefaultContextTailjob);
            
            // Tail flavor - speaker
            DrawSmallPromptEditor(ref curY, width,
                "RimJobTalk_Context_TailSpeaker_Title".Translate(),
                "RimJobTalk_Context_TailSpeaker_Desc".Translate(),
                ref settings.TailFlavorSpeaker, RimJobTalkSettings.DefaultTailFlavorSpeaker);
            
            // Tail flavor - target
            DrawSmallPromptEditor(ref curY, width,
                "RimJobTalk_Context_TailTarget_Title".Translate(),
                "RimJobTalk_Context_TailTarget_Desc".Translate(),
                ref settings.TailFlavorTarget, RimJobTalkSettings.DefaultTailFlavorTarget);
        }

        private static void DrawSmallPromptEditor(ref float curY, float width, TaggedString title, TaggedString hint,
            ref string value, string defaultValue)
        {
            // Title
            Text.Font = GameFont.Small;
            GUI.color = Color.white;
            Rect titleRect = new Rect(0, curY, width, 22f);
            Widgets.Label(titleRect, title);
            curY += 24f;
            
            // Hint
            GUI.color = Color.gray;
            Rect hintRect = new Rect(0, curY, width, 18f);
            Widgets.Label(hintRect, hint);
            curY += 20f;
            GUI.color = Color.white;
            
            // Text field (single line for small prompts)
            float textHeight = 60f;
            Rect textRect = new Rect(0, curY, width - 100f, textHeight);
            value = Widgets.TextArea(textRect, value);
            
            // Reset button
            Rect resetRect = new Rect(width - 95f, curY, 90f, 25f);
            if (Widgets.ButtonText(resetRect, "RimJobTalk_Button_Reset".Translate()))
            {
                value = defaultValue;
            }
            
            curY += textHeight + 15f;
        }

        private static void DrawHelpContent(ref float curY, float width)
        {
            Text.Font = GameFont.Medium;
            Rect titleRect = new Rect(0, curY, width, 30f);
            Widgets.Label(titleRect, "RimJobTalk_Help_AvailableVariables".Translate());
            curY += 35f;
            
            Text.Font = GameFont.Small;
            
            // Prompt variables section
            GUI.color = Color.cyan;
            Widgets.Label(new Rect(0, curY, width, 22f), "RimJobTalk_Help_PromptVariables".Translate());
            curY += 25f;
            GUI.color = Color.white;
            
            foreach (var kvp in RimJobTalkSettings.GetVariableHelp())
            {
                Rect varRect = new Rect(0, curY, 120f, 22f);
                Rect descRect = new Rect(125f, curY, width - 130f, 22f);
                
                GUI.color = new Color(1f, 0.9f, 0.5f);
                Widgets.Label(varRect, kvp.Key);
                GUI.color = Color.white;
                Widgets.Label(descRect, kvp.Value);
                curY += 24f;
            }
            
            curY += 20f;
            
            // Scriban variables section
            GUI.color = Color.cyan;
            Widgets.Label(new Rect(0, curY, width, 22f), "RimJobTalk_Help_ScribanVariables".Translate());
            curY += 25f;
            GUI.color = Color.white;
            
            GUI.color = Color.gray;
            Widgets.Label(new Rect(0, curY, width, 40f), "RimJobTalk_Help_ScribanNote".Translate());
            curY += 45f;
            GUI.color = Color.white;
            
            foreach (var kvp in RimJobTalkSettings.GetScribanVariableHelp())
            {
                Rect varRect = new Rect(0, curY, 180f, 22f);
                Rect descRect = new Rect(185f, curY, width - 190f, 22f);
                
                GUI.color = new Color(0.5f, 1f, 0.5f);
                Widgets.Label(varRect, kvp.Key);
                GUI.color = Color.white;
                Widgets.Label(descRect, kvp.Value);
                curY += 24f;
            }
            
            curY += 20f;
            
            // Usage tips
            GUI.color = Color.cyan;
            Widgets.Label(new Rect(0, curY, width, 22f), "RimJobTalk_Help_Tips".Translate());
            curY += 25f;
            GUI.color = Color.white;
            
            string[] tips = new[]
            {
                "RimJobTalk_Tip_Speaker".Translate().ToString(),
                "RimJobTalk_Tip_Target".Translate().ToString(),
                "RimJobTalk_Tip_SexType".Translate().ToString(),
                "RimJobTalk_Tip_TailFlavor".Translate().ToString(),
                "RimJobTalk_Tip_AIDescriptive".Translate().ToString(),
                "RimJobTalk_Tip_Scriban".Translate().ToString()
            };
            
            foreach (string tip in tips)
            {
                Widgets.Label(new Rect(0, curY, width, 22f), tip);
                curY += 24f;
            }
        }

        private static void DrawBottomButtons(Rect rect, RimJobTalkSettings settings)
        {
            float buttonWidth = 150f;
            
            // Reset All button
            Rect resetAllRect = new Rect(rect.x, rect.y, buttonWidth, 30f);
            if (Widgets.ButtonText(resetAllRect, "RimJobTalk_Button_ResetAll".Translate()))
            {
                Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                    "RimJobTalk_Confirm_ResetAll".Translate(),
                    () => settings.ResetToDefaults(),
                    destructive: true
                ));
            }
        }

        private static float GetContentHeight(int tab, RimJobTalkSettings settings)
        {
            switch (tab)
            {
                case 0: case 1: case 2: case 3: case 4: case 5:
                    return 300f;
                case 6: // Context
                    return 500f;
                case 7: // Help
                    return 800f;
                default:
                    return 400f;
            }
        }
    }
}