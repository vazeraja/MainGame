// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/GameInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""37734faa-64e4-48a4-901b-e0ff80b636be"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""103cf56a-d357-491a-8559-592da00641cc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""d603fc26-b424-4cff-a593-71ee48e3d0ef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""65a85ccc-c121-43d5-b6c9-83674e675ddc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""86a2519c-f018-4f88-a865-147dfedd5a3e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DashDirectionKeyboard"",
                    ""type"": ""Value"",
                    ""id"": ""1e2208d7-7a21-42ca-af73-7523cbc05a4c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad Left Stick"",
                    ""id"": ""bb70cc63-ec03-4bda-b1f6-edc72656ba83"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""39063a94-c3b3-4fd9-8dd6-f53600a700dd"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""49ff05d5-0d8a-4f79-bd99-fd41cdf0d7bd"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1fc52241-074a-41ac-8a81-2ce840b0290b"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""dfd22222-a8e3-4cdb-8f48-6cbffc53def7"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard WASD"",
                    ""id"": ""f8b893c4-85c2-492d-89ab-41f8d1520def"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""896f18e7-b01d-47a3-a018-7c2230b42022"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2247f5b8-f6a1-4a90-89f4-9846090bd18c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""204e90e1-b388-4636-9ba9-6f13df8ba19f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""611822fd-3a0d-49e1-9c2b-960f877aba76"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard Arrows"",
                    ""id"": ""b437a09d-8ca4-4996-a2f1-edf980623f6e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d0910d75-c140-4885-b9be-5bb8deae80e0"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""47380ccb-897f-4a93-bac4-3ea860b26105"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""42344a1d-be6c-4a9b-855c-bb33dd318651"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""13a640b3-5350-4478-8218-495187c01f40"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""265ab87e-ba29-4a65-8b54-518f12192e5d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""645ba428-0631-4e32-b9b0-eaac5d9ed36d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95428e26-6672-49d7-b72b-ccbfd5b4267d"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91ce58d8-88e1-4ade-945b-59c4f0939828"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08e210ce-12bd-4bf7-b50e-24be22fb3f0b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1b8ecce-183f-47e4-ba4e-4ff466341eb9"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""7d4d2bd5-ba26-4c74-b14b-a5e4c23bca58"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DashDirectionKeyboard"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3317c7af-d535-4f26-bed7-03e9eec8feab"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""DashDirectionKeyboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0753cb37-994e-4d36-8ccf-69771929a60b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""DashDirectionKeyboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d764ad38-9487-4b27-8f3e-da4b1ae111cb"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""DashDirectionKeyboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""da16ed23-611a-4397-ae55-df23ad90a6d7"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""DashDirectionKeyboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Dialogues"",
            ""id"": ""53768bd8-c94e-4805-abee-e6c15ca1fa64"",
            ""actions"": [
                {
                    ""name"": ""AdvanceDialogue"",
                    ""type"": ""Button"",
                    ""id"": ""9ace3f39-c79b-4bbc-ac18-26cafae1f479"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7fdec36e-9182-4c02-b692-3025d78c8431"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""AdvanceDialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08979c15-12c2-4736-962a-03eec0bff52f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79fef163-a828-41ee-a93a-f048723efc24"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""AdvanceDialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""DeveloperConsole"",
            ""id"": ""5a2d2c73-8dfb-4d86-b42e-779532e5bab5"",
            ""actions"": [
                {
                    ""name"": ""Open"",
                    ""type"": ""Button"",
                    ""id"": ""ab27ab1b-9a4b-46d2-98fb-1fb527355c0f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""6b1c22c4-96c4-4639-a165-f3775715dcd0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a420ce33-f651-4459-a57c-2e736e68102f"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Open"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f636d4be-bc70-489e-ad21-bf06367eb913"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardOrGamepad"",
            ""bindingGroup"": ""KeyboardOrGamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Attack = m_Gameplay.FindAction("Attack", throwIfNotFound: true);
        m_Gameplay_Dash = m_Gameplay.FindAction("Dash", throwIfNotFound: true);
        m_Gameplay_DashDirectionKeyboard = m_Gameplay.FindAction("DashDirectionKeyboard", throwIfNotFound: true);
        // Dialogues
        m_Dialogues = asset.FindActionMap("Dialogues", throwIfNotFound: true);
        m_Dialogues_AdvanceDialogue = m_Dialogues.FindAction("AdvanceDialogue", throwIfNotFound: true);
        // DeveloperConsole
        m_DeveloperConsole = asset.FindActionMap("DeveloperConsole", throwIfNotFound: true);
        m_DeveloperConsole_Open = m_DeveloperConsole.FindAction("Open", throwIfNotFound: true);
        m_DeveloperConsole_Enter = m_DeveloperConsole.FindAction("Enter", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Attack;
    private readonly InputAction m_Gameplay_Dash;
    private readonly InputAction m_Gameplay_DashDirectionKeyboard;
    public struct GameplayActions
    {
        private @GameInput m_Wrapper;
        public GameplayActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Attack => m_Wrapper.m_Gameplay_Attack;
        public InputAction @Dash => m_Wrapper.m_Gameplay_Dash;
        public InputAction @DashDirectionKeyboard => m_Wrapper.m_Gameplay_DashDirectionKeyboard;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Attack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Dash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @DashDirectionKeyboard.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDashDirectionKeyboard;
                @DashDirectionKeyboard.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDashDirectionKeyboard;
                @DashDirectionKeyboard.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDashDirectionKeyboard;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @DashDirectionKeyboard.started += instance.OnDashDirectionKeyboard;
                @DashDirectionKeyboard.performed += instance.OnDashDirectionKeyboard;
                @DashDirectionKeyboard.canceled += instance.OnDashDirectionKeyboard;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Dialogues
    private readonly InputActionMap m_Dialogues;
    private IDialoguesActions m_DialoguesActionsCallbackInterface;
    private readonly InputAction m_Dialogues_AdvanceDialogue;
    public struct DialoguesActions
    {
        private @GameInput m_Wrapper;
        public DialoguesActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @AdvanceDialogue => m_Wrapper.m_Dialogues_AdvanceDialogue;
        public InputActionMap Get() { return m_Wrapper.m_Dialogues; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DialoguesActions set) { return set.Get(); }
        public void SetCallbacks(IDialoguesActions instance)
        {
            if (m_Wrapper.m_DialoguesActionsCallbackInterface != null)
            {
                @AdvanceDialogue.started -= m_Wrapper.m_DialoguesActionsCallbackInterface.OnAdvanceDialogue;
                @AdvanceDialogue.performed -= m_Wrapper.m_DialoguesActionsCallbackInterface.OnAdvanceDialogue;
                @AdvanceDialogue.canceled -= m_Wrapper.m_DialoguesActionsCallbackInterface.OnAdvanceDialogue;
            }
            m_Wrapper.m_DialoguesActionsCallbackInterface = instance;
            if (instance != null)
            {
                @AdvanceDialogue.started += instance.OnAdvanceDialogue;
                @AdvanceDialogue.performed += instance.OnAdvanceDialogue;
                @AdvanceDialogue.canceled += instance.OnAdvanceDialogue;
            }
        }
    }
    public DialoguesActions @Dialogues => new DialoguesActions(this);

    // DeveloperConsole
    private readonly InputActionMap m_DeveloperConsole;
    private IDeveloperConsoleActions m_DeveloperConsoleActionsCallbackInterface;
    private readonly InputAction m_DeveloperConsole_Open;
    private readonly InputAction m_DeveloperConsole_Enter;
    public struct DeveloperConsoleActions
    {
        private @GameInput m_Wrapper;
        public DeveloperConsoleActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Open => m_Wrapper.m_DeveloperConsole_Open;
        public InputAction @Enter => m_Wrapper.m_DeveloperConsole_Enter;
        public InputActionMap Get() { return m_Wrapper.m_DeveloperConsole; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DeveloperConsoleActions set) { return set.Get(); }
        public void SetCallbacks(IDeveloperConsoleActions instance)
        {
            if (m_Wrapper.m_DeveloperConsoleActionsCallbackInterface != null)
            {
                @Open.started -= m_Wrapper.m_DeveloperConsoleActionsCallbackInterface.OnOpen;
                @Open.performed -= m_Wrapper.m_DeveloperConsoleActionsCallbackInterface.OnOpen;
                @Open.canceled -= m_Wrapper.m_DeveloperConsoleActionsCallbackInterface.OnOpen;
                @Enter.started -= m_Wrapper.m_DeveloperConsoleActionsCallbackInterface.OnEnter;
                @Enter.performed -= m_Wrapper.m_DeveloperConsoleActionsCallbackInterface.OnEnter;
                @Enter.canceled -= m_Wrapper.m_DeveloperConsoleActionsCallbackInterface.OnEnter;
            }
            m_Wrapper.m_DeveloperConsoleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Open.started += instance.OnOpen;
                @Open.performed += instance.OnOpen;
                @Open.canceled += instance.OnOpen;
                @Enter.started += instance.OnEnter;
                @Enter.performed += instance.OnEnter;
                @Enter.canceled += instance.OnEnter;
            }
        }
    }
    public DeveloperConsoleActions @DeveloperConsole => new DeveloperConsoleActions(this);
    private int m_KeyboardOrGamepadSchemeIndex = -1;
    public InputControlScheme KeyboardOrGamepadScheme
    {
        get
        {
            if (m_KeyboardOrGamepadSchemeIndex == -1) m_KeyboardOrGamepadSchemeIndex = asset.FindControlSchemeIndex("KeyboardOrGamepad");
            return asset.controlSchemes[m_KeyboardOrGamepadSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnDashDirectionKeyboard(InputAction.CallbackContext context);
    }
    public interface IDialoguesActions
    {
        void OnAdvanceDialogue(InputAction.CallbackContext context);
    }
    public interface IDeveloperConsoleActions
    {
        void OnOpen(InputAction.CallbackContext context);
        void OnEnter(InputAction.CallbackContext context);
    }
}
