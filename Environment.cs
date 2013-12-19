using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.TutorialFramework;

namespace Mogre.Tutorials
{
    class Environment
    {
        private List<Character> characters;
        private List<Stone> stones;
        private int MAX_X;
        private int MAX_Z;
        public SceneNode spotLight;
        //Function to get random number
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int rnd(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
        public Environment(ref SceneManager mSceneMgr)
        {
            mSceneMgr.AmbientLight = ColourValue.Black;
            mSceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;
            
            createGround(ref mSceneMgr);
            createLight(ref mSceneMgr);

            stones = new List<Stone>();
            stonesDistibution(ref mSceneMgr);

            characters = new List<Character>();
            
            characters.Add(new Ninja(ref mSceneMgr, new Vector3(750, 0, 750)));
            characters.Add(new SpaceMarine(ref mSceneMgr, new Vector3(120, 0, 120)));
        }

        private void createGround(ref SceneManager mSceneMgr)
        {
            Plane plane = new Plane(Vector3.UNIT_Y, 0);

            MAX_X = 1500;
            MAX_Z = 1500;
            MeshManager.Singleton.CreatePlane("ground",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
                MAX_X, MAX_Z, 20, 20, true, 1, 5, 5, Vector3.UNIT_Z);

            Entity groundEnt = mSceneMgr.CreateEntity("GroundEntity", "ground");
            mSceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);

            groundEnt.SetMaterialName("Examples/Rockwall");
            groundEnt.CastShadows = false;
        
        }

        private void createLight(ref SceneManager mSceneMgr)
        {
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

            Light spot = mSceneMgr.CreateLight("spotLight");
            spot.Type = Light.LightTypes.LT_SPOTLIGHT;
            spot.DiffuseColour = ColourValue.Blue;
            spot.SpecularColour = ColourValue.Blue;
            spot.Direction = new Vector3(-1, -1, 0);
            spot.Position = new Vector3(300, 300, 0);
            spot.SetSpotlightRange(new Degree(35), new Degree(50));
            spotLight = mSceneMgr.RootSceneNode.CreateChildSceneNode("spotlight");
            spotLight.AttachObject(spot);
            
            
        }

        private void createCastle(ref SceneManager mSceneMgr)
        { 
        }

        private void charactersDistribution(ref SceneManager mSceneMgr)
        {

        }

        private void stonesDistibution(ref SceneManager mSceneMgr)
        {
            //stones.Add(new Stone(ref mSceneMgr, new Vector3(600, 0, -500)));
            for (int i = (-MAX_X / 2) + 1; i < MAX_X / 2; i = i + 200)
            {
                for (int j = (-MAX_Z / 2) + 1; j < MAX_Z / 2; j = j + 200)
                {

                    stones.Add(new Stone(ref mSceneMgr, new Vector3(i, 0, j)));
                }
            }
        }

        public void moveCharacters(FrameEvent evt)
        {
            foreach (Character c in characters)
            {
                c.move(evt, this);
            }
        }

        public bool outOfGround(Vector3 characterPosition)
        {
            if (characterPosition.x > MAX_X / 2 || characterPosition.x > MAX_Z / 2 || characterPosition.x < -MAX_X / 2 || characterPosition.x < -MAX_Z / 2)
                return true;
            else
                return false;
        }

        public List<Character> lookCharacter(Character lookingCharacter)
        {
            
            List<Character> seenCharacter = new List<Character>();

            Vector3 targetDir;
            Vector3 lookingCharacterPosition = lookingCharacter.Node.Position;
            Vector3 lookDir = lookingCharacter.Node.Orientation * lookingCharacter.Forward;

            foreach (Character target in characters)
            {

                targetDir = target.Node.Position - lookingCharacterPosition;
                targetDir.Normalise();
                double vw = Math.Cos(lookingCharacter.ViewingAngle)*lookDir.Length*targetDir.Length;
                if (lookDir.DotProduct(targetDir) > vw)
                {
                    seenCharacter.Add(target);
                }
            }
            return seenCharacter;
        }
        public List<Stone> lookStone(Character lookingCharacter)
        {

            List<Stone> seenStone = new List<Stone>();

            Vector3 targetDir;
            Vector3 lookingCharacterPosition = lookingCharacter.Node.Position;
            Vector3 lookDir = lookingCharacter.Node.Orientation * lookingCharacter.Forward;

            foreach (Stone target in stones)
            {
                targetDir = target.Node.Position - lookingCharacterPosition;
                targetDir.Normalise();
                double vw = Math.Cos(lookingCharacter.ViewingAngle) * lookDir.Length * targetDir.Length;
                if (lookDir.DotProduct(targetDir) > vw)
                {
                    seenStone.Add(target);
                }
            }
            return seenStone;
        }

        private void removeCharacter(ref SceneManager mSceneMgr, ref Character c)
        {
            if (c != null)
            {
                characters.Remove(c);
                if (c.State == "stone")
                {
                    Node stone = c.Node.GetChild(0);
                    c.Node.RemoveChild(0);
                    c.Node.Parent.AddChild(stone);
                    stone.Position = c.Node.Position;

                }
                mSceneMgr.DestroySceneNode(c.Node);
                //c.die();
                c = null;
            }
        }

        public void addNinja(ref SceneManager mSceneMgr)
        {
            characters.Add(new Ninja(ref mSceneMgr, new Vector3(rnd(-MAX_X,MAX_X), 0, rnd(-MAX_Z,MAX_Z))));
        }

        public void removeNinja(ref SceneManager mSceneMgr) 
        {
            Character c= characters.Find(sh => sh is Ninja);
            removeCharacter(ref mSceneMgr, ref c);
        }

        public void addSpaceMarine(ref SceneManager mSceneMgr) 
        {
            characters.Add(new SpaceMarine(ref mSceneMgr, new Vector3(rnd(-MAX_X, MAX_X), 0, rnd(-MAX_Z, MAX_Z))));
        }

        public void removeSpaceMarine(ref SceneManager mSceneMgr)
        {
            Character c = characters.Find(sh => sh is SpaceMarine);
            removeCharacter(ref mSceneMgr,ref c);
        }

        public void removeStone(ref Stone s)
        {
            stones.Remove(s);
        }

        public void addStone(ref Stone s)
        {
            stones.Add(s);
        }
    }
}
