// GENERATED AUTOMATICALLY FROM 'Assets/_Game/Resources/InputSystem/GameplayControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameplayControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameplayControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameplayControls"",
    ""maps"": [
        {
            ""name"": ""Flying"",
            ""id"": ""a32f9448-6517-4e24-9aba-2a03f1ccf2d4"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""053ee7ab-5ba5-49e4-9b17-6d8124a6750c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DEBUG"",
                    ""type"": ""Button"",
                    ""id"": ""a1d5892c-47cc-4c1c-9236-828730e314d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""364efa5e-277a-422f-b4a6-7a95c2815cbd"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ced192d4-0fdd-4235-8e89-d512ab00971e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dde5e71f-23a9-44a9-bcdf-5759c395ad8b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9df7b3c1-564b-42f8-a81e-dfb60217470f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""263fac15-a947-44c3-ad68-8d0a81906c08"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Stick"",
                    ""id"": ""d8f96dd6-368c-42f6-8ed6-16998e085493"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""652687ad-c3ef-4a8c-ab43-b6b381f7480d"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""03fca6ad-b72e-46db-adb6-0a9de52f2ba1"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""197a216d-e213-46b8-b341-9fdf57252eea"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4387138c-0064-44e3-a371-aed47012f93f"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8fd71b38-a4f2-4f31-9002-9e7ad4322b36"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DEBUG"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Flying
        m_Flying = asset.FindActionMap("Flying", throwIfNotFound: true);
        m_Flying_Move = m_Flying.FindAction("Move", throwIfNotFound: true);
        m_Flying_DEBUG = m_Flying.FindAction("DEBUG", throwIfNotFound: true);
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

    // Flying
    private readonly InputActionMap m_Flying;
    private IFlyingActions m_FlyingActionsCallbackInterface;
    private readonly InputAction m_Flying_Move;
    private readonly InputAction m_Flying_DEBUG;
    public struct FlyingActions
    {
        private @GameplayControls m_Wrapper;
        public FlyingActions(@GameplayControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Flying_Move;
        public InputAction @DEBUG => m_Wrapper.m_Flying_DEBUG;
        public InputActionMap Get() { return m_Wrapper.m_Flying; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FlyingActions set) { return set.Get(); }
        public void SetCallbacks(IFlyingActions instance)
        {
            if (m_Wrapper.m_FlyingActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_FlyingActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_FlyingActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_FlyingActionsCallbackInterface.OnMove;
                @DEBUG.started -= m_Wrapper.m_FlyingActionsCallbackInterface.OnDEBUG;
                @DEBUG.performed -= m_Wrapper.m_FlyingActionsCallbackInterface.OnDEBUG;
                @DEBUG.canceled -= m_Wrapper.m_FlyingActionsCallbackInterface.OnDEBUG;
            }
            m_Wrapper.m_FlyingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @DEBUG.started += instance.OnDEBUG;
                @DEBUG.performed += instance.OnDEBUG;
                @DEBUG.canceled += instance.OnDEBUG;
            }
        }
    }
    public FlyingActions @Flying => new FlyingActions(this);
    public interface IFlyingActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnDEBUG(InputAction.CallbackContext context);
    }
}
