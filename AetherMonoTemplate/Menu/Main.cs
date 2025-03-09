using BepInEx;
using HarmonyLib;
using PlayFab.ExperimentationModels;
using StupidTemplate.Classes;
using StupidTemplate.Notifications;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UIElements;
using static StupidTemplate.Menu.Buttons;
using static StupidTemplate.Settings;

namespace StupidTemplate.Menu
{
    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("LateUpdate", MethodType.Normal)]
    public class Main : MonoBehaviour
    {
        // Constant
        public static void Prefix()
        {
            // Initialize Menu
            try
            {
                bool toOpen = (!rightHanded && ControllerInputPoller.instance.leftControllerSecondaryButton) || (rightHanded && ControllerInputPoller.instance.rightControllerSecondaryButton);
                bool keyboardOpen = UnityInput.Current.GetKey(keyboardButton);

                if (menu == null)
                {
                    if (toOpen || keyboardOpen)
                    {
                        MenuCreator.CreateMenu();
                        RecenterMenu(rightHanded, keyboardOpen);
                        if (reference == null)
                        {
                            CreateReference(rightHanded);
                        }
                    }
                }
                else
                {
                    if ((toOpen || keyboardOpen))
                    {
                        RecenterMenu(rightHanded, keyboardOpen);
                    }
                    else
                    {
                        Rigidbody comp = menu.AddComponent(typeof(Rigidbody)) as Rigidbody;
                        if (rightHanded)
                        {
                            comp.velocity = GorillaLocomotion.Player.Instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0);
                        }
                        else
                        {
                            comp.velocity = GorillaLocomotion.Player.Instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0);
                        }

                        UnityEngine.Object.Destroy(menu, 2);
                        menu = null;

                        UnityEngine.Object.Destroy(reference);
                        reference = null;
                    }
                }
            }
            catch (Exception exc)
            {
                UnityEngine.Debug.LogError(string.Format("{0} // Error initializing at {1}: {2}", PluginInfo.Name, exc.StackTrace, exc.Message));
            }

            // Constant
            try
            {
                // Execute Enabled mods
                foreach (ButtonInfo[] buttonlist in buttons)
                {
                    foreach (ButtonInfo v in buttonlist)
                    {
                        if (v.enabled)
                        {
                            if (v.method != null)
                            {
                                try
                                {
                                    v.method.Invoke();
                                }
                                catch (Exception exc)
                                {
                                    UnityEngine.Debug.LogError(string.Format("{0} // Error with mod {1} at {2}: {3}", PluginInfo.Name, v.buttonText, exc.StackTrace, exc.Message));
                                }
                            }
                        }
                    }
                }
            } catch (Exception exc)
            {
                UnityEngine.Debug.LogError(string.Format("{0} // Error with executing mods at {1}: {2}", PluginInfo.Name, exc.StackTrace, exc.Message));
            }
        }
public class ButtonClickAnimation : MonoBehaviour
        {
            private Vector3 originalScale;
            private Material buttonMaterial;

            public Color deselectedColor = Color.white;
            public Color selectedColor = Color.blue;

            public static ButtonClickAnimation currentSelected;

            void Start()
            {
                originalScale = transform.localScale;
                buttonMaterial = GetComponent<Renderer>().material;
                buttonMaterial.color = deselectedColor;
            }

            void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.layer != gameObject.layer)
                    return; 

                if (other.gameObject == gameObject)
                    return;

                if (currentSelected != this)
                {
                    if (currentSelected != null)
                    {
                        currentSelected.Deselect();
                    }
                    currentSelected = this;
                    buttonMaterial.color = selectedColor;
                    StartCoroutine(AnimateButtonPress());
                }
            }

            public void Deselect()
            {
                buttonMaterial.color = deselectedColor;
            }

            IEnumerator AnimateButtonPress()
            {
                float animationDuration = 0.1f;
                Vector3 targetScale = originalScale * 0.9f;
                float timer = 0f;

                // Scale down.
                while (timer < animationDuration)
                {
                    transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / animationDuration);
                    timer += Time.deltaTime;
                    yield return null;
                }
                transform.localScale = targetScale;

                timer = 0f;
                while (timer < animationDuration)
                {
                    transform.localScale = Vector3.Lerp(targetScale, originalScale, timer / animationDuration);
                    timer += Time.deltaTime;
                    yield return null;
                }
                transform.localScale = originalScale;
            }
        }



        public static class MenuCreator
        {
    public static void CreateMenu()
            {



                menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
                UnityEngine.Object.Destroy(menu.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(menu.GetComponent<BoxCollider>());
                UnityEngine.Object.Destroy(menu.GetComponent<Renderer>());
                menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.3825f);

                // Menu Background
                menuBackground = GameObject.CreatePrimitive(PrimitiveType.Cube);
                UnityEngine.Object.Destroy(menuBackground.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(menuBackground.GetComponent<BoxCollider>());
                menuBackground.transform.parent = menu.transform;
                menuBackground.transform.rotation = Quaternion.identity;
                menuBackground.transform.localScale = menuSize;
                menuBackground.GetComponent<Renderer>().material.color = backgroundColor.colors[0].color;
                menuBackground.transform.position = new Vector3(0.05f, 0f, 0f);
                menuBackground.GetComponent<Renderer>().material.color = new Color(0.0706f, 0.0824f, 0.1098f);


                // Canvas
                canvasObject = new GameObject();
                canvasObject.transform.parent = menu.transform;
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
                canvasObject.AddComponent<GraphicRaycaster>();
                canvas.renderMode = RenderMode.WorldSpace;
                canvasScaler.dynamicPixelsPerUnit = 1000f;

                // Title and FPS
                Text text = new GameObject
                {
                    transform =
                    {
                        parent = canvasObject.transform
                    }
                }.AddComponent<Text>();
                text.font = currentFont;
                text.text = PluginInfo.Name;
                text.fontSize = 1;
                text.color = Color.white;
                text.supportRichText = true;
                text.fontStyle = FontStyle.Italic;
                text.alignment = TextAnchor.MiddleCenter;
                text.resizeTextForBestFit = false;
                text.resizeTextMinSize = 0;
                RectTransform component = text.GetComponent<RectTransform>();
                component.localPosition = Vector3.zero;
                component.sizeDelta = new Vector2(0.28f, 0.05f);
                component.position = new Vector3(0.06f, 0f, 0.167f);
                component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

                if (fpsCounter)
                {
                    fpsObject = new GameObject
                    {
                        transform =
                    {
                        parent = canvasObject.transform
                    }
                    }.AddComponent<Text>();
                    fpsObject.font = currentFont;
                    fpsObject.text = PluginInfo.Name;
                    fpsObject.color = Color.white;
                    fpsObject.fontSize = 5;
                    fpsObject.supportRichText = true;
                    fpsObject.fontStyle = FontStyle.Italic;
                    fpsObject.alignment = TextAnchor.MiddleCenter;
                    fpsObject.horizontalOverflow = UnityEngine.HorizontalWrapMode.Overflow;
                    fpsObject.resizeTextForBestFit = true;
                    fpsObject.resizeTextMinSize = 0;
                    RectTransform component2 = fpsObject.GetComponent<RectTransform>();
                    component2.localPosition = Vector3.zero;
                    component2.sizeDelta = new Vector2(1f, 0.02f);
                    component2.position = new Vector3(0.06f, 0f, 0.137f); //depth, width, height
                    component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

                    // Create button text
                    Text buttonTextObject = new GameObject
                    {
                        transform =
        {
            parent = canvasObject.transform
        }
                    }.AddComponent<Text>();
                    buttonTextObject.text = PluginInfo.Version;
                    buttonTextObject.font = currentFont;
                    buttonTextObject.color = Color.white;
                    buttonTextObject.fontSize = 1;
                    buttonTextObject.alignment = TextAnchor.MiddleCenter;
                    buttonTextObject.resizeTextForBestFit = true;
                    buttonTextObject.resizeTextMinSize = 0;

                    RectTransform textRect = buttonTextObject.GetComponent<RectTransform>();
                    textRect.localPosition = Vector3.zero;
                    textRect.sizeDelta = new Vector2(0.18f, 0.02f);
                    textRect.position = new Vector3(0.06f, -0.16f, 0.137f); //depth, width, height
                    textRect.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                }
            





                //second bg shit

                GameObject SBG = GameObject.CreatePrimitive(PrimitiveType.Cube);
                UnityEngine.Object.Destroy(SBG.GetComponent<Rigidbody>());
                SBG.GetComponent<BoxCollider>().isTrigger = true;
                SBG.transform.parent = menu.transform;
                SBG.transform.rotation = Quaternion.identity;
                SBG.transform.localScale = new Vector3(0.1f, 1.07f, 0.75f);
                SBG.transform.localPosition = new Vector3(0.53f, -0.266f, -0.077f); //depth, width, height
                SBG.GetComponent<Renderer>().material.color = buttonColors[0].colors[0].color;
                SBG.GetComponent<Renderer>().material.color = Color.black;





                Vector3 initialPosition = new Vector3(0.56f, 0.54f, 0.23f); // depth, width, height
                float verticalOffset = 0.1f;
                float verticalTextOffset = 0.0379f;


                void CreateSideButton(string buttonText, Vector3 position, int index, Color buttonColor, Color textColor, bool enableAnimation = true)
                {
                    GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    UnityEngine.Object.Destroy(button.GetComponent<Rigidbody>());
                    button.GetComponent<BoxCollider>().isTrigger = true;
                    button.transform.parent = menu.transform;
                    button.transform.rotation = Quaternion.identity;
                    button.transform.localScale = new Vector3(0.2f, 0.45f, 0.08f); // depth, width, height

                    // Assign the button to a dedicated layer so that only objects on this layer will trigger its animation.
                    button.layer = LayerMask.NameToLayer("SideButton");

                    button.transform.localPosition = position + new Vector3(0, 0, -verticalOffset * index);
                    button.GetComponent<Renderer>().material.color = buttonColor;
                    button.AddComponent<Classes.Button>().relatedText = buttonText;

                    // Only add the animation component if enabled.
                    if (enableAnimation)
                    {
                        button.AddComponent<ButtonClickAnimation>();
                    }

                    Text buttonTextObject = new GameObject
                    {
                        transform = { parent = canvasObject.transform }
                    }.AddComponent<Text>();

                    buttonTextObject.text = buttonText;
                    buttonTextObject.font = currentFont;
                    buttonTextObject.fontSize = 1;
                    buttonTextObject.color = textColor;
                    buttonTextObject.alignment = TextAnchor.MiddleCenter;
                    buttonTextObject.resizeTextForBestFit = true;
                    buttonTextObject.resizeTextMinSize = 0;

                    RectTransform textRect = buttonTextObject.GetComponent<RectTransform>();
                    textRect.sizeDelta = new Vector2(0.05f, 0.03f);
                    textRect.localPosition = new Vector3(0.068f, 0.157f, 0.088f - (verticalTextOffset * index));
                    textRect.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                }




                CreateSideButton("Advantages", initialPosition, 0, new Color(0.874f, 0.886f, 0.933f), Color.black, true);
                CreateSideButton("Movement", initialPosition, 1, new Color(0.874f, 0.886f, 0.933f), Color.black, true);
                CreateSideButton("Fun", initialPosition, 2, new Color(0.874f, 0.886f, 0.933f), Color.black, true);
                CreateSideButton("Miscellaneous", initialPosition, 3, new Color(0.874f, 0.886f, 0.933f), Color.black, true);
                CreateSideButton("Overpowered", initialPosition, 4, new Color(0.874f, 0.886f, 0.933f), Color.black, true);
                CreateSideButton("Visual", initialPosition, 5, new Color(0.874f, 0.886f, 0.933f), Color.black, true);
                CreateSideButton("Disconnect", initialPosition, 6, Color.red, Color.white, false);






                // Page Buttons
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if (!UnityInput.Current.GetKey(KeyCode.Q))
                {
                    gameObject.layer = 2;
                }
                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.parent = menu.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.2f, 0.25f, 0.07f);
                gameObject.transform.localPosition = new Vector3(0.58f, -0.05f, -0.38f); //deepth, width, height
                gameObject.GetComponent<Renderer>().material.color = buttonColors[0].colors[0].color;
                gameObject.AddComponent<Classes.Button>().relatedText = "PreviousPage";
                gameObject.GetComponent<Renderer>().material.color = new Color(0.874f, 0.886f, 0.933f);


                gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if (!UnityInput.Current.GetKey(KeyCode.Q))
                {
                    gameObject.layer = 2;
                }
                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.parent = menu.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.2f, 0.25f, 0.07f);
                gameObject.transform.localPosition = new Vector3(0.58f, -0.5f, -0.38f);  //deepth, width, height
                gameObject.GetComponent<Renderer>().material.color = buttonColors[0].colors[0].color;
                gameObject.AddComponent<Classes.Button>().relatedText = "NextPage";
                gameObject.GetComponent<Renderer>().material.color = new Color(0.874f, 0.886f, 0.933f);




                // Mod Buttons
                ButtonInfo[] activeButtons = buttons[buttonsType].Skip(pageNumber * buttonsPerPage).Take(buttonsPerPage).ToArray();
                for (int i = 0; i < activeButtons.Length; i++)
                {
                    CreateButton(i * 0.1f, activeButtons[i]);
                }
            }
        }


        public static void CreateButton(float offset, ButtonInfo method)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (!UnityInput.Current.GetKey(KeyCode.Q))
            {
                gameObject.layer = 2;
            }
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.2f, 0.7f, 0.08f);
            gameObject.transform.localPosition = new Vector3(0.58f, -0.27f, 0.23f - offset);
            gameObject.AddComponent<Classes.Button>().relatedText = method.buttonText;
            gameObject.GetComponent<Renderer>().material.color = new Color(0.874f, 0.886f, 0.933f);
            Text text = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.AddComponent<Text>();
            text.font = currentFont;
            text.text = method.buttonText;
            if (method.overlapText != null)
            {
                text.text = method.overlapText;
            }
            double doubleValue = 0.2;
            int intValue = (int)doubleValue;
            text.supportRichText = true;
            text.fontSize = intValue; // fontSize expects an integer value
            text.color = new Color(0f, 0f, 0f);
            text.alignment = TextAnchor.MiddleCenter;
            text.fontStyle = FontStyle.Normal;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(.05f, .03f);
            component.localPosition = new Vector3(0.069f, -0.075f, .088f - offset / 2.6f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

        }





        public static void RecreateMenu()
        {
            if (menu != null)
            {
                UnityEngine.Object.Destroy(menu);
                menu = null;

                MenuCreator.CreateMenu();
                RecenterMenu(rightHanded, UnityInput.Current.GetKey(keyboardButton));
            }
        }

        public static void RecenterMenu(bool isRightHanded, bool isKeyboardCondition)
        {
            if (!isKeyboardCondition)
            {
                if (!isRightHanded)
                {
                    menu.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    menu.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                }
                else
                {
                    menu.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    Vector3 rotation = GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles;
                    rotation += new Vector3(0f, 0f, 180f);
                    menu.transform.rotation = Quaternion.Euler(rotation);
                }
            }
            else
            {
                try
                {
                    TPC = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera").GetComponent<Camera>();
                }
                catch { }
                if (TPC != null)
                {
                    TPC.transform.position = new Vector3(-999f, -999f, -999f);
                    TPC.transform.rotation = Quaternion.identity;
                    GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    bg.transform.localScale = new Vector3(10f, 10f, 0.01f);
                    bg.transform.transform.position = TPC.transform.position + TPC.transform.forward;
                    bg.GetComponent<Renderer>().material.color = new Color32((byte)(backgroundColor.colors[0].color.r * 50), (byte)(backgroundColor.colors[0].color.g * 50), (byte)(backgroundColor.colors[0].color.b * 50), 255);
                    GameObject.Destroy(bg, Time.deltaTime);
                    menu.transform.parent = TPC.transform;
                    menu.transform.position = (TPC.transform.position + (Vector3.Scale(TPC.transform.forward, new Vector3(0.5f, 0.5f, 0.5f)))) + (Vector3.Scale(TPC.transform.up, new Vector3(-0.02f, -0.02f, -0.02f)));
                    Vector3 rot = TPC.transform.rotation.eulerAngles;
                    rot = new Vector3(rot.x - 90, rot.y + 90, rot.z);
                    menu.transform.rotation = Quaternion.Euler(rot);

                    if (reference != null)
                    {
                        if (Mouse.current.leftButton.isPressed)
                        {
                            Ray ray = TPC.ScreenPointToRay(Mouse.current.position.ReadValue());
                            RaycastHit hit;
                            bool worked = Physics.Raycast(ray, out hit, 100);
                            if (worked)
                            {
                                Classes.Button collide = hit.transform.gameObject.GetComponent<Classes.Button>();
                                if (collide != null)
                                {
                                    collide.OnTriggerEnter(buttonCollider);
                                }
                            }
                        }
                        else
                        {
                            reference.transform.position = new Vector3(999f, -999f, -999f);
                        }
                    }
                }
            }
        }

        public static void CreateReference(bool isRightHanded)
        {
            reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            if (isRightHanded)
            {
                reference.transform.parent = GorillaTagger.Instance.leftHandTransform;
            }
            else
            {
                reference.transform.parent = GorillaTagger.Instance.rightHandTransform;
            }
            reference.GetComponent<Renderer>().material.color = backgroundColor.colors[0].color;
            reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            buttonCollider = reference.GetComponent<SphereCollider>();

            ColorChanger colorChanger = reference.AddComponent<ColorChanger>();
            colorChanger.colorInfo = backgroundColor;
            colorChanger.Start();
        }

        public static void Toggle(string buttonText)
        {
            int lastPage = ((buttons[buttonsType].Length + buttonsPerPage - 1) / buttonsPerPage) - 1;
            if (buttonText == "PreviousPage")
            {
                pageNumber--;
                if (pageNumber < 0)
                {
                    pageNumber = lastPage;
                }
            } else
            {
                if (buttonText == "NextPage")
                {
                    pageNumber++;
                    if (pageNumber > lastPage)
                    {
                        pageNumber = 0;
                    }
                } else
                {
                    ButtonInfo target = GetIndex(buttonText);
                    if (target != null)
                    {
                        if (target.isTogglable)
                        {
                            target.enabled = !target.enabled;
                            if (target.enabled)
                            {
                                NotifiLib.SendNotification("<color=grey>[</color><color=green>ENABLE</color><color=grey>]</color> " + target.toolTip);
                                if (target.enableMethod != null)
                                {
                                    try { target.enableMethod.Invoke(); } catch { }
                                }
                            }
                            else
                            {
                                NotifiLib.SendNotification("<color=grey>[</color><color=red>DISABLE</color><color=grey>]</color> " + target.toolTip);
                                if (target.disableMethod != null)
                                {
                                    try { target.disableMethod.Invoke(); } catch { }
                                }
                            }
                        }
                        else
                        {
                            NotifiLib.SendNotification("<color=grey>[</color><color=green>ENABLE</color><color=grey>]</color> " + target.toolTip);
                            if (target.method != null)
                            {
                                try { target.method.Invoke(); } catch { }
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
            RecreateMenu();
        }

        public static GradientColorKey[] GetSolidGradient(Color color)
        {
            return new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) };
        }

        public static ButtonInfo GetIndex(string buttonText)
        {
            foreach (ButtonInfo[] buttons in Menu.Buttons.buttons)
            {
                foreach (ButtonInfo button in buttons)
                {
                    if (button.buttonText == buttonText)
                    {
                        return button;
                    }
                }
            }

            return null;
        }

        // Variables
            // Important
                // Objects
                    public static GameObject menu;
                    public static GameObject menuBackground;   
                    public static GameObject reference;
                    public static GameObject canvasObject;

                    public static SphereCollider buttonCollider;
                    public static Camera TPC;
                    public static Text fpsObject;

        // Data
            public static int pageNumber = 0;
            public static int buttonsType = 0;
    }
}
