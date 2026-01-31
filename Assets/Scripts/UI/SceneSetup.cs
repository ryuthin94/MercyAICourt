using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MercyAICourt.UI
{
    /// <summary>
    /// Editor script to set up the Mercy AI Court scene.
    /// Run from Unity Editor: Tools > Mercy AI Court > Setup Scene
    /// </summary>
    public class SceneSetup : MonoBehaviour
    {
        #if UNITY_EDITOR
        [MenuItem("Tools/Mercy AI Court/Setup Scene")]
        public static void SetupScene()
        {
            // Create GameManager
            GameObject gameManagerObj = new GameObject("GameManager");
            gameManagerObj.AddComponent<Managers.GameManager>();

            // Create Canvas
            GameObject canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            CanvasScaler scaler = canvasObj.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            // Create Background
            GameObject backgroundObj = new GameObject("Background");
            backgroundObj.transform.SetParent(canvasObj.transform, false);
            Image bgImage = backgroundObj.AddComponent<Image>();
            bgImage.color = Color.black;
            RectTransform bgRect = backgroundObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;

            // Create Main Menu Panel
            GameObject mainMenuPanel = CreatePanel(canvasObj, "MainMenuPanel");
            CreateButton(mainMenuPanel, "StartButton", "START INTERROGATION", new Vector2(0, -100));
            CreateButton(mainMenuPanel, "QuitButton", "QUIT", new Vector2(0, -200));
            CreateTitleText(mainMenuPanel, "MenuTitle", "MERCY AI COURT SYSTEM", new Vector2(0, 200));

            // Create Interrogation Panel
            GameObject interrogationPanel = CreatePanel(canvasObj, "InterrogationPanel");
            interrogationPanel.SetActive(false);

            // Title
            GameObject titleObj = CreateTitleText(interrogationPanel, "Title", "MERCY AI COURT SYSTEM", new Vector2(0, 450));

            // Timer (Left side)
            GameObject timerObj = CreateLargeText(interrogationPanel, "Timer", "90:00", new Vector2(-400, 200));
            GameObject timerLabelObj = CreateSmallText(interrogationPanel, "TimerLabel", "TIME REMAINING", new Vector2(-400, 250));

            // Guilt Percentage (Right side)
            GameObject guiltTextObj = CreateLargeText(interrogationPanel, "GuiltPercentage", "98%", new Vector2(400, 200));
            guiltTextObj.GetComponent<TextMeshProUGUI>().color = Color.red;
            GameObject guiltLabelObj = CreateSmallText(interrogationPanel, "GuiltLabel", "GUILT LEVEL", new Vector2(400, 250));

            // Guilt Meter (Bottom)
            GameObject guiltMeterObj = CreateGuiltMeter(interrogationPanel, "GuiltMeter", new Vector2(0, -400));

            // Create Victory Panel
            GameObject victoryPanel = CreatePanel(canvasObj, "VictoryPanel");
            victoryPanel.SetActive(false);
            CreateLargeText(victoryPanel, "VictoryText", "VERDICT: NOT GUILTY", new Vector2(0, 100));
            CreateButton(victoryPanel, "VictoryRestartButton", "RESTART", new Vector2(-150, -100));
            CreateButton(victoryPanel, "VictoryMenuButton", "MAIN MENU", new Vector2(150, -100));

            // Create Game Over Panel
            GameObject gameOverPanel = CreatePanel(canvasObj, "GameOverPanel");
            gameOverPanel.SetActive(false);
            CreateLargeText(gameOverPanel, "GameOverText", "VERDICT: GUILTY\nExecution initiated", new Vector2(0, 100));
            CreateButton(gameOverPanel, "GameOverRestartButton", "RESTART", new Vector2(-150, -100));
            CreateButton(gameOverPanel, "GameOverMenuButton", "MAIN MENU", new Vector2(150, -100));

            // Create Testing Panel
            GameObject testingPanel = CreateTestingPanel(canvasObj);

            // Create UIManager
            GameObject uiManagerObj = new GameObject("UIManager");
            uiManagerObj.transform.SetParent(canvasObj.transform, false);
            UIManager uiManager = uiManagerObj.AddComponent<UIManager>();

            // Assign references using reflection or serialization
            SerializedObject serializedUI = new SerializedObject(uiManager);
            serializedUI.FindProperty("titleText").objectReferenceValue = titleObj.GetComponent<TextMeshProUGUI>();
            serializedUI.FindProperty("timerText").objectReferenceValue = timerObj.GetComponent<TextMeshProUGUI>();
            serializedUI.FindProperty("guiltPercentageText").objectReferenceValue = guiltTextObj.GetComponent<TextMeshProUGUI>();
            serializedUI.FindProperty("guiltMeterFill").objectReferenceValue = guiltMeterObj.transform.Find("Fill").GetComponent<Image>();
            serializedUI.FindProperty("mainMenuPanel").objectReferenceValue = mainMenuPanel;
            serializedUI.FindProperty("interrogationPanel").objectReferenceValue = interrogationPanel;
            serializedUI.FindProperty("victoryPanel").objectReferenceValue = victoryPanel;
            serializedUI.FindProperty("gameOverPanel").objectReferenceValue = gameOverPanel;
            serializedUI.FindProperty("victoryText").objectReferenceValue = victoryPanel.transform.Find("VictoryText").GetComponent<TextMeshProUGUI>();
            serializedUI.FindProperty("gameOverText").objectReferenceValue = gameOverPanel.transform.Find("GameOverText").GetComponent<TextMeshProUGUI>();
            serializedUI.ApplyModifiedProperties();

            // Connect buttons
            ConnectButton(mainMenuPanel, "StartButton", uiManager, "OnStartGameButton");
            ConnectButton(mainMenuPanel, "QuitButton", uiManager, "OnQuitButton");
            ConnectButton(victoryPanel, "VictoryRestartButton", uiManager, "OnRestartButton");
            ConnectButton(victoryPanel, "VictoryMenuButton", uiManager, "OnMainMenuButton");
            ConnectButton(gameOverPanel, "GameOverRestartButton", uiManager, "OnRestartButton");
            ConnectButton(gameOverPanel, "GameOverMenuButton", uiManager, "OnMainMenuButton");

            // Create Camera
            GameObject cameraObj = new GameObject("Main Camera");
            Camera camera = cameraObj.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            camera.orthographic = true;
            camera.orthographicSize = 5;
            cameraObj.tag = "MainCamera";

            Debug.Log("Scene setup complete!");
        }

        private static GameObject CreatePanel(GameObject parent, string name)
        {
            GameObject panel = new GameObject(name);
            panel.transform.SetParent(parent.transform, false);
            RectTransform rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            return panel;
        }

        private static GameObject CreateTitleText(GameObject parent, string name, string text, Vector2 position)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent.transform, false);
            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = 60;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            
            RectTransform rect = textObj.GetComponent<RectTransform>();
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(1200, 100);
            
            return textObj;
        }

        private static GameObject CreateLargeText(GameObject parent, string name, string text, Vector2 position)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent.transform, false);
            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = 80;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            
            RectTransform rect = textObj.GetComponent<RectTransform>();
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(400, 120);
            
            return textObj;
        }

        private static GameObject CreateSmallText(GameObject parent, string name, string text, Vector2 position)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent.transform, false);
            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = 24;
            tmp.color = new Color(0.7f, 0.7f, 0.7f);
            tmp.alignment = TextAlignmentOptions.Center;
            
            RectTransform rect = textObj.GetComponent<RectTransform>();
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(400, 40);
            
            return textObj;
        }

        private static GameObject CreateButton(GameObject parent, string name, string text, Vector2 position)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent.transform, false);
            
            RectTransform rect = buttonObj.AddComponent<RectTransform>();
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(300, 60);
            
            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f);
            
            Button button = buttonObj.AddComponent<Button>();
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = 24;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            return buttonObj;
        }

        private static GameObject CreateGuiltMeter(GameObject parent, string name, Vector2 position)
        {
            GameObject meterObj = new GameObject(name);
            meterObj.transform.SetParent(parent.transform, false);
            RectTransform meterRect = meterObj.AddComponent<RectTransform>();
            meterRect.anchoredPosition = position;
            meterRect.sizeDelta = new Vector2(800, 40);

            // Background
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(meterObj.transform, false);
            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(0.2f, 0.2f, 0.2f);
            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;

            // Fill
            GameObject fillObj = new GameObject("Fill");
            fillObj.transform.SetParent(meterObj.transform, false);
            Image fillImage = fillObj.AddComponent<Image>();
            fillImage.color = Color.red;
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillAmount = 0.98f;
            RectTransform fillRect = fillObj.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.sizeDelta = Vector2.zero;

            // Label
            GameObject labelObj = new GameObject("Label");
            labelObj.transform.SetParent(meterObj.transform, false);
            TextMeshProUGUI labelTmp = labelObj.AddComponent<TextMeshProUGUI>();
            labelTmp.text = "GUILT METER";
            labelTmp.fontSize = 20;
            labelTmp.color = new Color(0.7f, 0.7f, 0.7f);
            labelTmp.alignment = TextAlignmentOptions.Center;
            RectTransform labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchoredPosition = new Vector2(0, -35);
            labelRect.sizeDelta = new Vector2(800, 30);

            return meterObj;
        }

        private static GameObject CreateTestingPanel(GameObject parent)
        {
            GameObject panelObj = new GameObject("TestingPanel");
            panelObj.transform.SetParent(parent.transform, false);
            
            RectTransform panelRect = panelObj.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0, 1);
            panelRect.anchorMax = new Vector2(0, 1);
            panelRect.pivot = new Vector2(0, 1);
            panelRect.anchoredPosition = new Vector2(10, -10);
            panelRect.sizeDelta = new Vector2(300, 500);
            
            Image panelImage = panelObj.AddComponent<Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
            
            TestingPanel testingPanel = panelObj.AddComponent<TestingPanel>();
            
            // Create status text
            GameObject statusTextObj = new GameObject("StatusText");
            statusTextObj.transform.SetParent(panelObj.transform, false);
            TextMeshProUGUI statusTmp = statusTextObj.AddComponent<TextMeshProUGUI>();
            statusTmp.fontSize = 16;
            statusTmp.color = Color.white;
            statusTmp.alignment = TextAlignmentOptions.TopLeft;
            RectTransform statusRect = statusTextObj.GetComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0, 0);
            statusRect.anchorMax = new Vector2(1, 1);
            statusRect.offsetMin = new Vector2(10, 10);
            statusRect.offsetMax = new Vector2(-10, -10);
            
            // Create buttons
            CreateTestButton(panelObj, "Guilt +", new Vector2(80, -60), testingPanel, "OnIncreaseGuilt");
            CreateTestButton(panelObj, "Guilt -", new Vector2(220, -60), testingPanel, "OnDecreaseGuilt");
            CreateTestButton(panelObj, "Time +", new Vector2(80, -120), testingPanel, "OnAddTime");
            CreateTestButton(panelObj, "Time -", new Vector2(220, -120), testingPanel, "OnSubtractTime");
            CreateTestButton(panelObj, "Win", new Vector2(80, -180), testingPanel, "OnSkipToWin");
            CreateTestButton(panelObj, "Lose", new Vector2(220, -180), testingPanel, "OnSkipToLose");
            CreateTestButton(panelObj, "Reset", new Vector2(150, -240), testingPanel, "OnResetGame");
            CreateTestButton(panelObj, "Pause", new Vector2(150, -300), testingPanel, "OnTogglePause");
            
            // Add title
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(panelObj.transform, false);
            TextMeshProUGUI titleTmp = titleObj.AddComponent<TextMeshProUGUI>();
            titleTmp.text = "TESTING PANEL (T)";
            titleTmp.fontSize = 18;
            titleTmp.color = Color.yellow;
            titleTmp.alignment = TextAlignmentOptions.Center;
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchoredPosition = new Vector2(150, -20);
            titleRect.sizeDelta = new Vector2(280, 30);
            
            // Assign panel reference
            SerializedObject serializedPanel = new SerializedObject(testingPanel);
            serializedPanel.FindProperty("panelObject").objectReferenceValue = panelObj;
            serializedPanel.FindProperty("statusText").objectReferenceValue = statusTmp;
            serializedPanel.ApplyModifiedProperties();
            
            panelObj.SetActive(false);
            
            return panelObj;
        }

        private static void CreateTestButton(GameObject parent, string text, Vector2 position, TestingPanel panel, string methodName)
        {
            GameObject buttonObj = new GameObject(text + "Button");
            buttonObj.transform.SetParent(parent.transform, false);
            
            RectTransform rect = buttonObj.AddComponent<RectTransform>();
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(120, 40);
            
            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.3f, 0.3f, 0.3f);
            
            Button button = buttonObj.AddComponent<Button>();
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = 16;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            // Connect button
            UnityEngine.Events.UnityAction action = () => panel.GetType().GetMethod(methodName).Invoke(panel, null);
            button.onClick.AddListener(action);
        }

        private static void ConnectButton(GameObject panel, string buttonName, UIManager uiManager, string methodName)
        {
            Transform buttonTransform = panel.transform.Find(buttonName);
            if (buttonTransform != null)
            {
                Button button = buttonTransform.GetComponent<Button>();
                if (button != null)
                {
                    UnityEngine.Events.UnityAction action = () => uiManager.GetType().GetMethod(methodName).Invoke(uiManager, null);
                    button.onClick.AddListener(action);
                }
            }
        }
        #endif
    }
}
