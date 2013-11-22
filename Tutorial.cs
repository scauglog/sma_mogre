using Mogre;
using Mogre.TutorialFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Mogre.Tutorials
{
    /*class Tutorial : BaseApplication
    {
        public static void Main()
        {
            new Tutorial().Go();
        }

        protected override void CreateScene()
        {
            //shadow
            mSceneMgr.AmbientLight = ColourValue.Black;
            mSceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;
            
            // Pitch is rotation around the x axis, 
            // yaw is around the y axis
            // roll is around the z axis.
            
            //mSceneMgr.AmbientLight = new ColourValue(1, 1, 1);
            ///Entity ent = mSceneMgr.CreateEntity("Head", "ninja.mesh");
            //ent.CastShadows = true;
            //SceneNode node = mSceneMgr.RootSceneNode.CreateChildSceneNode("HeadNode");
            //node.AttachObject(ent);
            //node.Scale(0.5f,0.5f, 0.5f);
            //node.Yaw(new Degree(-45));
            //node.Pitch(new Degree(-45));
            //node.Roll(new Degree(45));

            Ninja n= new Ninja(mSceneMgr,Vector3.ZERO);
            
            //Entity ent3 = mSceneMgr.CreateEntity("Head3", "ninja.mesh");
            //ent3.CastShadows = true;
            //SceneNode node3 = mSceneMgr.RootSceneNode.CreateChildSceneNode("HeadNode3", new Vector3(100, 00, 0));
            //node3.AttachObject(ent3);
            //node3.Scale(0.5f, 0.5f, 0.5f);
            
            //node2.Position += new Vector3(10, 0, 10);

            //create ground
            Plane plane = new Plane(Vector3.UNIT_Y, 0);

            MeshManager.Singleton.CreatePlane("ground",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
                1500, 1500, 20, 20, true, 1, 5, 5, Vector3.UNIT_Z);

            Entity groundEnt = mSceneMgr.CreateEntity("GroundEntity", "ground");
            mSceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);

            groundEnt.SetMaterialName("Examples/Rockwall");
            groundEnt.CastShadows = false;

            //ligth
            Light pointLight = mSceneMgr.CreateLight("pointLight");
            pointLight.Type = Light.LightTypes.LT_POINT;
            pointLight.Position = new Vector3(0, 150, 250);
            pointLight.DiffuseColour = ColourValue.Red;
            pointLight.SpecularColour = ColourValue.Red;

            Light directionalLight = mSceneMgr.CreateLight("directionalLight");
            directionalLight.Type = Light.LightTypes.LT_DIRECTIONAL;
            directionalLight.DiffuseColour = new ColourValue(.25f, .25f, 0);
            directionalLight.SpecularColour = new ColourValue(.25f, .25f, 0);
            directionalLight.Direction = new Vector3(0, -1, 1);

            Light spotLight = mSceneMgr.CreateLight("spotLight");
            spotLight.Type = Light.LightTypes.LT_SPOTLIGHT;
            spotLight.DiffuseColour = ColourValue.Blue;
            spotLight.SpecularColour = ColourValue.Blue;
            spotLight.Direction = new Vector3(-1, -1, 0);
            spotLight.Position = new Vector3(300, 300, 0);

            spotLight.SetSpotlightRange(new Degree(35), new Degree(50));
        }

        protected override void CreateCamera()
        {
            mCamera    = mSceneMgr.CreateCamera("PlayerCam");
            mCameraMan = new CameraMan(mCamera);
            mCamera.Position = new Vector3(0, 10, 500);
            mCamera.LookAt(Vector3.ZERO);
            //mCamera.Yaw(new Degree(45));
            mCamera.NearClipDistance = 100;
            mCamera.FarClipDistance = 500;
            mCameraMan = new CameraMan(mCamera);

            

        }
        protected override void CreateViewports()
        {
            Viewport viewport = mWindow.AddViewport(mCamera);
            viewport.BackgroundColour = ColourValue.Blue;
            mCamera.AspectRatio = (float)viewport.ActualWidth / viewport.ActualHeight;
        }
        public void createNinja()
        {
            Entity ent2 = mSceneMgr.CreateEntity("Head2", "robot.mesh");
            ent2.CastShadows = true;
            SceneNode node2 = mSceneMgr.RootSceneNode.CreateChildSceneNode("HeadNode2", new Vector3(0, 0, 100));
            node2.AttachObject(ent2);
        }
    }*/
    public class Tutorial
    {
        protected static Root mRoot;
        protected static RenderWindow mRenderWindow;
        protected static float mTimer = 50;
        protected static SceneManager mSceneMgr;
        protected static Camera mCamera;
        protected static CameraMan mCameraMan;
        
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

        /* tuto create scene
        protected static void CreateScene()
        {
            SceneManager sceneMgr = mRoot.CreateSceneManager(SceneType.ST_GENERIC);
            Camera camera = sceneMgr.CreateCamera("Camera");
            camera.Position = new Vector3(0, 0, 150);
            camera.LookAt(Vector3.ZERO);
            mRenderWindow.AddViewport(camera);

            Entity ogreHead = sceneMgr.CreateEntity("Head", "ogrehead.mesh");
            SceneNode headNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            headNode.AttachObject(ogreHead);

            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            Light l = sceneMgr.CreateLight("MainLight");
            l.Position = new Vector3(20, 80, 50);
        }*/

        protected virtual void CreateScene()
        {
            mSceneMgr = mRoot.CreateSceneManager(SceneType.ST_GENERIC);

            Environment env = new Environment(ref mSceneMgr);
            
            //mSceneMgr.AmbientLight = ColourValue.Black;
            //mSceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;

            //// Pitch is rotation around the x axis, 
            //// yaw is around the y axis
            //// roll is around the z axis.

            ////mSceneMgr.AmbientLight = new ColourValue(1, 1, 1);
            /////Entity ent = mSceneMgr.CreateEntity("Head", "ninja.mesh");
            ////ent.CastShadows = true;
            ////SceneNode node = mSceneMgr.RootSceneNode.CreateChildSceneNode("HeadNode");
            ////node.AttachObject(ent);
            ////node.Scale(0.5f,0.5f, 0.5f);
            ////node.Yaw(new Degree(-45));
            ////node.Pitch(new Degree(-45));
            ////node.Roll(new Degree(45));

            //Ninja n = new Ninja(ref mSceneMgr, Vector3.ZERO);

            ////Entity ent3 = mSceneMgr.CreateEntity("Head3", "ninja.mesh");
            ////ent3.CastShadows = true;
            ////SceneNode node3 = mSceneMgr.RootSceneNode.CreateChildSceneNode("HeadNode3", new Vector3(100, 00, 0));
            ////node3.AttachObject(ent3);
            ////node3.Scale(0.5f, 0.5f, 0.5f);

            ////node2.Position += new Vector3(10, 0, 10);

            ////create ground
            //Plane plane = new Plane(Vector3.UNIT_Y, 0);

            //MeshManager.Singleton.CreatePlane("ground",
            //    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
            //    1500, 1500, 20, 20, true, 1, 5, 5, Vector3.UNIT_Z);

            //Entity groundEnt = mSceneMgr.CreateEntity("GroundEntity", "ground");
            //mSceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);

            //groundEnt.SetMaterialName("Examples/Rockwall");
            //groundEnt.CastShadows = false;

            ////ligth
            //Light pointLight = mSceneMgr.CreateLight("pointLight");
            //pointLight.Type = Light.LightTypes.LT_POINT;
            //pointLight.Position = new Vector3(0, 150, 250);
            //pointLight.DiffuseColour = ColourValue.Red;
            //pointLight.SpecularColour = ColourValue.Red;

            //Light directionalLight = mSceneMgr.CreateLight("directionalLight");
            //directionalLight.Type = Light.LightTypes.LT_DIRECTIONAL;
            //directionalLight.DiffuseColour = new ColourValue(.25f, .25f, 0);
            //directionalLight.SpecularColour = new ColourValue(.25f, .25f, 0);
            //directionalLight.Direction = new Vector3(0, -1, 1);

            //Light spotLight = mSceneMgr.CreateLight("spotLight");
            //spotLight.Type = Light.LightTypes.LT_SPOTLIGHT;
            //spotLight.DiffuseColour = ColourValue.Blue;
            //spotLight.SpecularColour = ColourValue.Blue;
            //spotLight.Direction = new Vector3(-1, -1, 0);
            //spotLight.Position = new Vector3(300, 300, 0);

            //spotLight.SetSpotlightRange(new Degree(35), new Degree(50));
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