using System;
 using System.Collections.Generic;
 using System.Text;
 using Mogre;
 using Mogre.TutorialFramework;
using MOIS;

namespace Mogre.Tutorials
{

    public class MoveDemo : Tutorial
    {
        
        Environment env;
        protected static MOIS.Keyboard mEnvKeyboard;
       
        protected override void CreateScene()
        {
            // Create SceneManager
            mSceneMgr = mRoot.CreateSceneManager(SceneType.ST_EXTERIOR_CLOSE);
            env = new Environment(ref mSceneMgr);
            

        }

        protected override void CreateFrameListeners()
        {
            base.CreateFrameListeners();
            mRoot.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);


        }

        bool FrameStarted(FrameEvent evt)
        {
            env.moveCharacters(evt);
            return true;
            
        }
        protected override void InitializeInput()
        {
            base.InitializeInput();

            int windowHandle;
            mRenderWindow.GetCustomAttribute("WINDOW", out windowHandle);
            mInputMgr = MOIS.InputManager.CreateInputSystem((uint)windowHandle);

            mEnvKeyboard = (MOIS.Keyboard)mInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            mEnvKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(OnEnvKeyBuffPressed);
        }

        protected bool OnEnvKeyBuffPressed(MOIS.KeyEvent arg)
        {
            switch (arg.key)
            {
                case MOIS.KeyCode.KC_F:
                    env.removeNinja(ref mSceneMgr);
                    break;
                case MOIS.KeyCode.KC_R:
                    env.addNinja(ref mSceneMgr);
                    break;
                case MOIS.KeyCode.KC_H:
                    env.removeSpaceMarine(ref mSceneMgr);
                    break;
                case MOIS.KeyCode.KC_Y:
                    env.addSpaceMarine(ref mSceneMgr);
                    break;
                case MOIS.KeyCode.KC_Z:
                    Character.MWalkSpeed += 50;
                    break;
                case MOIS.KeyCode.KC_X:
                    Character.MWalkSpeed -= 50;
                    break;
            }

            return true;
        }

        protected override bool OnFrameRenderingQueued(FrameEvent evt)
        {
            base.OnFrameRenderingQueued(evt);
            
            mEnvKeyboard.Capture();
            Vector3 spotlightMove = Vector3.ZERO;
            
            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_I))
                spotlightMove.z -= 200;

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_K))
                spotlightMove.z += 200;

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_J))
                spotlightMove.x -= 200;

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_L))
                spotlightMove.x += 200;

            if (spotlightMove != Vector3.ZERO)
                env.spotLight.Translate(spotlightMove * evt.timeSinceLastFrame);
            
            return true;
        }

    }
}