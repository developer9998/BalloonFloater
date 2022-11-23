using ComputerInterface;
using ComputerInterface.ViewLib;
using System.Text;
using UnityEngine;

namespace BalloonFloater.ComputerInterface
{
    public class BFView : ComputerView
    {
        private readonly UISelectionHandler _selectionHandler;
        internal string[] flight = new string[2] { "Default", "Enhanced" };
        internal string[] flightModes = new string[2] { "For a raw experience", "All around movement" };
        internal string valueColour = "2982D6";

        public BFView()
        {
            _selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            _selectionHandler.MaxIdx = 2;
            _selectionHandler.OnSelected += OnEntrySelected;
            _selectionHandler.ConfigureSelectionIndicator($"<color=#{valueColour}>></color> ", "", "  ", "");
        }

        // This is called when you view is opened
        public override void OnShow(object[] args)
        {
            base.OnShow(args);
            // changing the Text property will fire an PropertyChanged event
            // which lets the computer know the text has changed and update it
            Redraw();
        }

        public void Redraw()
        {
            StringBuilder str = new StringBuilder();

            str.BeginCenter().MakeBar('-', SCREEN_WIDTH, 0, "FFFFFF10").AppendLine();
            str.AppendClr("BalloonFloater", "2971C6").AppendLine();
            str.Append("A gameplay mod by <color=#2979D0>dev9998</color>").AppendLine();
            str.MakeBar('-', SCREEN_WIDTH, 0, "FFFFFF10").AppendLines(2).EndAlign();

            str.Append(_selectionHandler.GetIndicatedText(0, "Flight Mode: ")).AppendClr(flight[Plugin.Instance.data.movementMode], valueColour).AppendLine();
            str.Append(" ").Append(flightModes[Plugin.Instance.data.movementMode]).AppendLines(2);

            str.Append(_selectionHandler.GetIndicatedText(1, "Player Gain: ")).AppendClr(Plugin.Instance.data.playerGain.ToString(), valueColour).AppendLine();
            str.Append(" Player Maximum Gain: ").AppendClr(Plugin.Instance.data.playerMaxGain.ToString(), valueColour).AppendLines(2);

            str.Append(_selectionHandler.GetIndicatedText(2, "Despawn Time: ")).AppendClr(Plugin.Instance.data.destroyTime.ToString(), valueColour).AppendLine();

            Text = str.ToString();
        }

        private void OnEntrySelected(int index)
        {

        }

        private void OnEntryAdjusted(int index, bool increase)
        {
            float offset = increase ? 1 : -1;
            switch (index)
            {
                case 0:
                    Plugin.Instance.data.movementMode = Mathf.Clamp(Plugin.Instance.data.movementMode + (int)offset, 0, 1);
                    Plugin.Instance.recovery.SetData(Plugin.Instance.data);
                    break;
                case 1:
                    offset = increase ? 0.5f : -0.5f;
                    Plugin.Instance.data.playerGain = Mathf.Clamp(Plugin.Instance.data.playerGain + offset, 0.5f, 3);
                    Plugin.Instance.data.playerMaxGain = Plugin.Instance.data.playerGain + 1.5f;
                    Plugin.Instance.data.balloonGain = Plugin.Instance.data.playerGain + 1f;
                    Plugin.Instance.recovery.SetData(Plugin.Instance.data);
                    break;
                case 2:
                    offset = increase ? 0.25f : -0.25f;
                    Plugin.Instance.data.destroyTime = Mathf.Clamp(Plugin.Instance.data.destroyTime + offset, 0.25f, 1f);
                    Plugin.Instance.recovery.SetData(Plugin.Instance.data);
                    break;
            }
        }

        // you can do something on keypresses by overriding "OnKeyPressed"
        // it get's an EKeyboardKey passed as a parameter which wraps the old character string
        public override void OnKeyPressed(EKeyboardKey key)
        {
            if (_selectionHandler.HandleKeypress(key))
            {
                Redraw();
                return;
            }

            if (key == EKeyboardKey.Left || key == EKeyboardKey.Right)
            {
                OnEntryAdjusted(_selectionHandler.CurrentSelectionIndex, key == EKeyboardKey.Right);
                Redraw();
            }

            switch (key)
            {
                case EKeyboardKey.Back:
                    // "ReturnToMainMenu" will basically switch to the main menu again
                    ReturnToMainMenu();
                    break;
            }
        }
    }
}