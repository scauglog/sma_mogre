using Mogre;
using Mogre.TutorialFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Mogre.Tutorials
{
    public class Tutorial
    {
        protected static Root mRoot;
        protected static RenderWindow mRenderWindow;
        protected static float mTimer = 50;
        protected static SceneManager mSceneMgr;
        protected static Camera mCamera;
        protected static CameraMan mCameraMan;

        protected static MOIS.InputManager mInputMgr;
        protected static MOIS.Keyboard mKeyboard;
        protected static MOIS.Mouse mMouse;
        
        public static void Main()
        {
            new MoveDemo().Go();
            
        }

        public void Go() 
        {
            try
            {
                CreateRoot();
                DefineResources();
                CreateRenderSystem();
                CreateRenderWindow();
                InitializeResources();
                InitializeInput();
                CreateScene();
                CreateCamera();
                CreateViewports();
                CreateFrameListeners();
                EnterRenderLoop();
            }
            catch (OperationCanceledException) { }
        }
        
        protected void CreateRoot()
        {
            mRoot = new Root();
        }

        protected void DefineResources()
        {
            ConfigFile cf = new ConfigFile();
            cf.Load("resources.cfg", "\t:=", true);

            var section = cf.GetSectionIterator();
            while (section.MoveNext())
            {
                foreach (var line in section.Current)
                {
                    ResourceGroupManager.Singleton.AddResourceLocation(
                        line.Value, line.Key, section.CurrentKey);
                }
            }
        }

        protected void CreateRenderSystem()
        {
            if (!mRoot.ShowConfigDialog())
                throw new OperationCanceledException();

            //RenderSystem renderSystem = mRoot.GetRenderSystemByName("Direct3D9 Rendering Subsystem");
            //renderSystem.SetConfigOption("Full Screen", "No");
            //renderSystem.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
            //mRoot.RenderSystem = renderSystem;
        }

        protected void CreateRenderWindow()
        {
            mRenderWindow = mRoot.Initialise(true, "Main Ogre Window");
        }

        protected void InitializeResources()
        {
            TextureManager.Singleton.DefaultNumMipmaps = 5;
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
        }

        protected void InitializeInput()
        {
            int windowHandle;
            mRenderWindow.GetCustomAttribute("WINDOW", out windowHandle);
            mInputMgr = MOIS.InputManager.CreateInputSystem((uint)windowHandle);

            mKeyboard = (MOIS.Keyboard)mInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, false);
            mMouse = (MOIS.Mouse)mInputMgr.CreateInputObject(MOIS.Type.OISMouse, false);
        }


        protected virtual void CreateScene()
        {
            mSceneMgr = mRoot.CreateSceneManager(SceneType.ST_GENERIC);

            Environment env = new Environment(ref mSceneMgr);
            
            
        }

        protected void CreateCamera()
        {
            mCamera = mSceneMgr.CreateCamera("PlayerCam");
            mCamera.Position = new Vector3(50, 200, 1000);
            mCamera.LookAt(Vector3.ZERO);
            //mCamera.Yaw(new Degree(45));
            //mCamera.NearClipDistance = 100;
            //mCamera.FarClipDistance = 500;
            mCameraMan = new CameraMan(mCamera);



        }
        protected void CreateViewports()
        {
            Viewport viewport = mRenderWindow.AddViewport(mCamera);
            viewport.BackgroundColour = ColourValue.Blue;
            mCamera.AspectRatio = (float)viewport.ActualWidth / viewport.ActualHeight;
        }

        protected virtual void CreateFrameListeners()
        {
            mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(OnFrameRenderingQueued);
            mRoot.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
        }

        static bool OnFrameRenderingQueued(FrameEvent evt)
        {
            mKeyboard.Capture();
            mMouse.Capture();

            Vector3 cameraMove = Vector3.ZERO;

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_W))
                cameraMove += mCamera.Direction;

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_S))
                cameraMove -= mCamera.Direction;

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_A))
                cameraMove -= mCamera.Right;

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_D))
                cameraMove += mCamera.Right;

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_Q))
                cameraMove += mCamera.Up;

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_E))
                cameraMove -= mCamera.Up;


            cameraMove.Normalise();
            cameraMove *= 150; // Natural speed is 150 units/sec.
            
            if (cameraMove != Vector3.ZERO)
                mCamera.Move(cameraMove * evt.timeSinceLastFrame);

            mTimer -= evt.timeSinceLastFrame;
            return (mTimer > 0);
        }

        protected void EnterRenderLoop()
        {
            mRoot.StartRendering();
        }

        bool FrameStarted(FrameEvent evt)
        {
            
            return true;
        }

        
    }
}