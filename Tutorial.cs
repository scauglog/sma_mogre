using Mogre;
using Mogre.TutorialFramework;
using System;



namespace Mogre.Tutorials
{
    class Tutorial : BaseApplication
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
            /*Entity ent = mSceneMgr.CreateEntity("Head", "ninja.mesh");
            ent.CastShadows = true;
            SceneNode node = mSceneMgr.RootSceneNode.CreateChildSceneNode("HeadNode");
            node.AttachObject(ent);
            node.Scale(0.5f,0.5f, 0.5f);
            node.Yaw(new Degree(-45));*/
            //node.Pitch(new Degree(-45));
            //node.Roll(new Degree(45));

            Ninja n= new Ninja(mSceneMgr,Vector3.ZERO);
            
            /*Entity ent3 = mSceneMgr.CreateEntity("Head3", "ninja.mesh");
            ent3.CastShadows = true;
            SceneNode node3 = mSceneMgr.RootSceneNode.CreateChildSceneNode("HeadNode3", new Vector3(100, 00, 0));
            node3.AttachObject(ent3);
            node3.Scale(0.5f, 0.5f, 0.5f);
            */
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
    }
}