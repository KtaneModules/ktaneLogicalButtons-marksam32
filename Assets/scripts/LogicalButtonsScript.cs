using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnitRandom = UnityEngine.Random;

public class LogicalButtonsScript : MonoBehaviour {

    // Module stuff:
    public KMBombModule Module;
    public KMAudio Audio;
    public KMBombInfo Info;

    // Selectables:
    public KMSelectable Btn1;
    public KMSelectable Btn2;
    public KMSelectable Btn3;
    public KMSelectable ScreenBtn;

    // Visual:
    public TextMesh Btn1Text;
    public TextMesh Btn2Text;
    public TextMesh Btn3Text;
    public TextMesh OperatorTxt;
    public MeshRenderer Btn1Renderer;
    public MeshRenderer Btn2Renderer;
    public MeshRenderer Btn3Renderer;
    public MeshRenderer[] StageLights;
    public Material[] materials;
    public Material LightOffMat;
    public Material LightOnMat;
    public Material BorderOffMat;
    public Material BorderOnMat;
    public MeshRenderer[] Borders;

    // Other:
    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private int stage;
    private bool isSolved = false;

    private LogicalButton[] buttons = new LogicalButton[3];

    private ILogicalGateOperator gateOperator;

    private LogicalButtonsHelper helper;

    private IList<int> solution;

    private int pressCount;

    string[] correct =
    {
        "GOOD",
        "NEAT",
        "NICE",
        "OKAY",
        "COOL",
        "YES"
    };

    //Color Blind:
    public TextMesh ColorBlindIndicatorTop;
    public TextMesh ColorBlindIndicatorLeft;
    public TextMesh ColorBlindIndicatorRight;


    void Activate()
    {
        this.stage = 1;
        this.InitLogic();
        this.InitButtons();
        ColorBlindIndicatorLeft.gameObject.SetActive(GetComponent<KMColorblindMode>().ColorblindModeActive);
        ColorBlindIndicatorRight.gameObject.SetActive(GetComponent<KMColorblindMode>().ColorblindModeActive);
        ColorBlindIndicatorTop.gameObject.SetActive(GetComponent<KMColorblindMode>().ColorblindModeActive);
    }

    // Use this for initialization
    void Start ()
    {
        _moduleId = _moduleIdCounter++;
        Module.OnActivate += Activate;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitButtons()
    {
        Btn1.OnInteract += delegate
        {
            if (this.isSolved)
            {
                Btn1.AddInteractionPunch();
                Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Btn1.transform);
                return false;
            }
            Btn1.AddInteractionPunch();
            
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Btn1.transform);
            if (!this.HasSolution)
            {
                this.HandleButtonPressWithNoSolution(this.buttons[0]);
            }
            else
            {
                this.CheckSolution(this.buttons[0]);
            }

            return false;
        };

        Btn2.OnInteract += delegate
        {
            if (this.isSolved)
            {
                Btn1.AddInteractionPunch();
                Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Btn1.transform);
                return false;
            }
            Btn2.AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Btn2.transform);
            if (!this.HasSolution)
            {
                this.HandleButtonPressWithNoSolution(this.buttons[1]);
            }
            else
            {
                this.CheckSolution(this.buttons[1]);
            }


            return false;
        };

        Btn3.OnInteract += delegate
        {
            if (this.isSolved)
            {
                Btn1.AddInteractionPunch();
                Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Btn1.transform);
                return false;
            }
            Btn3.AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Btn3.transform);
            if (!this.HasSolution)
            {
                this.HandleButtonPressWithNoSolution(this.buttons[2]);
            }
            else
            {
                this.CheckSolution(this.buttons[2]);
            }

            return false;
        };

        ScreenBtn.OnInteract += delegate
        {
            if (this.isSolved)
            {
                Btn1.AddInteractionPunch();
                Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Btn1.transform);
                return false;
            }
            ScreenBtn.AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, ScreenBtn.transform);
            if (this.HasSolution)
            {
                Debug.LogFormat("[Logical Buttons #{0}] Pressed the operator screen while had solution. Strike!", this._moduleId);
                Module.HandleStrike();
                this.InitLogic();
            }
            else
            {
                this.gateOperator = ChangeLogicalGateOperator(this.gateOperator);
                this.helper = new LogicalButtonsHelper(this.buttons, this.gateOperator);
                this.solution = this.helper.SolveOrder(this.stage);
                OperatorTxt.text = this.gateOperator.Name;
                Debug.LogFormat("[Logical Buttons #{0}] Pressing operator button, new answer is:", _moduleId);
                this.DebugMessage();
                
            }

            return false;
        };
    }

    private static ILogicalGateOperator ChangeLogicalGateOperator(ILogicalGateOperator currentOperator)
    {
        var newOperator = LogicalGateOperatorFactory.Create(Constants.GateStrings[UnitRandom.Range(0, 6)]);
        while (currentOperator.Name == newOperator.Name)
        {
            newOperator = LogicalGateOperatorFactory.Create(Constants.GateStrings[UnitRandom.Range(0, 6)]);
        }

        return newOperator;
    }

    private void HandleButtonPressWithNoSolution(LogicalButton button)
    {
        Debug.LogFormat("[Logical Buttons #{1}] Pressed incorrect button {0}, expected screen button. Strike!", button.Index + 1, this._moduleId);
        Module.HandleStrike();
        this.InitLogic();
    }

    private void InitLogic()
    {
        for (var i = 0; i < 3; i++)
        {
            this.buttons[i] = new LogicalButton(i, Constants.ButtonColors[UnitRandom.Range(0, 9)], Constants.WordStrings[UnitRandom.Range(0, 9)]);
        }

        //// Debuging purposes
        //this.buttons[0] = new LogicalButton(0, ButtonColor.Orange, Constants.ColorString);
        //this.buttons[1] = new LogicalButton(1, ButtonColor.Grey, Constants.LogicString);
        //this.buttons[2] = new LogicalButton(2, ButtonColor.Grey, Constants.LogicString);
        //this.gateOperator = LogicalGateOperatorFactory.Create(Constants.XorOperatorString);
        //this.stage = 3;

        this.pressCount = 0;
        this.gateOperator = LogicalGateOperatorFactory.Create(Constants.GateStrings[UnityEngine.Random.Range(0, 6)]);
        this.helper = new LogicalButtonsHelper(this.buttons, this.gateOperator);
        this.solution = this.helper.SolveOrder(this.stage);

        OperatorTxt.text = this.gateOperator.Name;

        Btn1Text.text = buttons[0].Label;
        Btn2Text.text = buttons[1].Label;
        Btn3Text.text = buttons[2].Label;

        Btn1Renderer.sharedMaterial = materials[Constants.ButtonColors.IndexOf(buttons[0].Color)];
        Btn2Renderer.sharedMaterial = materials[Constants.ButtonColors.IndexOf(buttons[1].Color)];
        Btn3Renderer.sharedMaterial = materials[Constants.ButtonColors.IndexOf(buttons[2].Color)];
        Debug.LogFormat("[Logical Buttons #{0}] STAGE: {1}", this._moduleId, this.stage);
        this.DebugMessage();

        ColorBlindIndicatorTop.text = buttons[0].Color.ToString();
        ColorBlindIndicatorLeft.text = buttons[1].Color.ToString();
        ColorBlindIndicatorRight.text = buttons[2].Color.ToString();
    }
    
    private bool HasSolution
    {
        get
        {
            return this.solution.Any();
        }
    }

    private void CheckSolution(LogicalButton button)
    {
        this.pressCount++;
        if (button.IsPressed || (button.Index + 1 != this.solution[this.pressCount - 1]))
        {
            Debug.LogFormat("[Logical Buttons #{2}] Pressed incorrect button {0}, expected button {1}. Strike!", button.Index + 1, this.solution[this.pressCount - 1], this._moduleId);
            this.InitLogic();
            Module.HandleStrike();
            return;
        }

        button.IsPressed = true;
        Borders[button.Index].material = BorderOnMat;
        Debug.LogFormat("[Logical Buttons #{0}] Pressed correct button {1}.", this._moduleId, button.Index + 1);
        if (this.pressCount == this.solution.Count)
        {
            StageLights[this.stage - 1].sharedMaterial = LightOnMat;
            if (this.stage == 3)
            {
                Debug.LogFormat("[Logical Buttons #{0}] All 3 stages passed. Module solved.", this._moduleId);
                this.isSolved = true;       
                Module.HandlePass();
                Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, Module.transform);
                Btn1Text.text = "";
                Btn2Text.text = "";
                Btn3Text.text = "";
                ColorBlindIndicatorTop.text = "Green";
                ColorBlindIndicatorLeft.text = "Green";
                ColorBlindIndicatorRight.text = "Green";
                Btn1Renderer.sharedMaterial = materials[2];
                Btn2Renderer.sharedMaterial = materials[2];
                Btn3Renderer.sharedMaterial = materials[2];
                for(int i = 0; i < Borders.Length; i++)
                {
                    Borders[i].material = BorderOffMat;
                }
                OperatorTxt.text = correct[UnitRandom.Range(0, correct.Length)];
            }
            else
            {
                Debug.LogFormat("[Logical Buttons #{0}] Completed stage {1}.", this._moduleId, this.stage);
                for (int i = 0; i < Borders.Length; i++)
                {
                    Borders[i].material = BorderOffMat;
                }
                this.stage++;
                Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, Module.transform);
                this.InitLogic();
            }
        }
    }

    private void DebugMessage()
    {
        foreach (var button in buttons)
        {
            Debug.LogFormat("[Logical Buttons #{0}] Button {1}: {2}", this._moduleId, button.Index + 1, this.helper.DebugStringButtons(button.Index, this.stage));
        }

        Debug.LogFormat("[Logical Buttons #{0}] Order gain: {1}.", this._moduleId, this.helper.DebugOrderString(this.stage));
    } 

    // Twitch plays:

    public string TwitchHelpMessage = "To press buttons, use !{0} press 1 2 3 or !{0} press 1 2 or !{0} press 1. To press the operator screen, use !{0} press operator. Enable colorblind mode using !{0} colorblind.";

    internal KMSelectable[] ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant().Trim();
        var pieces = command.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

        if (command.Equals("colorblind"))
        {
            ColorBlindIndicatorTop.gameObject.SetActive(true);
            ColorBlindIndicatorLeft.gameObject.SetActive(true);
            ColorBlindIndicatorRight.gameObject.SetActive(true);
            return new KMSelectable[0];
        }

        if (pieces[0] != "press" || pieces.Length < 2 || pieces.Length != pieces.Distinct().Count())
        {
            return null;
        }

        if ((pieces.Contains("operator") && pieces.Length > 2) || pieces.Length > 4)
        {
            return null;
        }
  
        var list = new List<KMSelectable>();
        for (int i = 1; i < pieces.Length; i++)
        {
            switch (pieces[i])
            {
                case "one":
                case "top":
                case "first":
                case "1":
                    list.Add(Btn1);
                    break;

                case "two":
                case "left":
                case "second":                
                case "2":
                    list.Add(Btn2);
                    break;

                case "three":
                case "right":
                case "third":    
                case "3":
                    list.Add(Btn3);
                    break;

                case "screen":
                case "op":
                case "gate":
                case "logic gate":
                case "operator":
                    list.Add(ScreenBtn);
                    break;

                default:
                    return null;
            }
        }
        return list.ToArray();
    }
}


