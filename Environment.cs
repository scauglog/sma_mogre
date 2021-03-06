﻿using System;
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
        private static int MAX_X= 2000;
        private static int MAX_Z= 2000;
        private static int nbInitSpaceMarine=5;
        private static int nbInitNinja = 5;
        private static bool randomStone = true;
        private static int nbStone = 100;
        private static int stepStone = 500;
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
            for(int i=0;i<nbInitNinja;i++)
                characters.Add(new Ninja(ref mSceneMgr, new Vector3(rnd(-MAX_X / 2, MAX_X / 2), 0, rnd(-MAX_Z / 2, MAX_Z / 2))));
            for(int i=0;i<nbInitSpaceMarine;i++)
                characters.Add(new SpaceMarine(ref mSceneMgr, new Vector3(rnd(-MAX_X / 2, MAX_X / 2), 0, rnd(-MAX_Z / 2, MAX_Z / 2))));
        }

        private void createGround(ref SceneManager mSceneMgr)
        {
            Plane plane = new Plane(Vector3.UNIT_Y, 0);

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

        private void stonesDistibution(ref SceneManager mSceneMgr)
        {
            if (randomStone)
            {
                for (int i = 0; i < nbStone; i++)
                {
                    stones.Add(new Stone(ref mSceneMgr, new Vector3(rnd(-MAX_X / 2, MAX_X / 2), 0, rnd(-MAX_Z / 2, MAX_Z / 2))));
                }
            }
            else
            {
                for (int i = (-MAX_X / 2) + 1; i < MAX_X / 2; i = i + stepStone)
                {
                    for (int j = (-MAX_Z / 2) + 1; j < MAX_Z / 2; j = j + stepStone)
                    {
                        stones.Add(new Stone(ref mSceneMgr, new Vector3(i, 0, j)));
                    }
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
            if (characterPosition.x > MAX_X / 2 || characterPosition.z > MAX_Z / 2 || characterPosition.x < -MAX_X / 2 || characterPosition.z < -MAX_Z / 2)
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
                double dist = (target.Node.Position - lookingCharacter.Node.Position).Length;

                if (lookDir.DotProduct(targetDir) > vw && dist < lookingCharacter.MaxView)
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
                double dist = (target.Node.Position - lookingCharacter.Node.Position).Length;
                
                if (lookDir.DotProduct(targetDir) > vw && dist<lookingCharacter.MaxView && !target.IsCarried)
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
            characters.Add(new Ninja(ref mSceneMgr, new Vector3(rnd(-MAX_X/2,MAX_X/2), 0, rnd(-MAX_Z/2,MAX_Z/2))));
        }

        public void removeNinja(ref SceneManager mSceneMgr) 
        {
            Character c= characters.Find(sh => sh is Ninja);
            removeCharacter(ref mSceneMgr, ref c);
        }

        public void addSpaceMarine(ref SceneManager mSceneMgr) 
        {
            characters.Add(new SpaceMarine(ref mSceneMgr, new Vector3(rnd(-MAX_X/2, MAX_X/2), 0, rnd(-MAX_Z/2, MAX_Z/2))));
        }

        public void removeSpaceMarine(ref SceneManager mSceneMgr)
        {
            Character c = characters.Find(sh => sh is SpaceMarine);
            removeCharacter(ref mSceneMgr,ref c);
        }

        public void setCarriedStone(String stoneName)
        {
            foreach (Stone s in stones)
            {
                if (s.Name == stoneName)
                {
                    s.IsCarried = true;
                }
            }
        }

        public void setUncarriedStone(String stoneName)
        {
            foreach (Stone s in stones)
            {
                if (s.Name == stoneName)
                {
                    s.IsCarried = false;
                }
            }
        }
    }
}
