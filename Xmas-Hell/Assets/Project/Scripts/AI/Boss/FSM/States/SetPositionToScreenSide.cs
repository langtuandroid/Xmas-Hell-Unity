using System.Collections.Generic;
using UnityEngine;

namespace BossBehaviourState
{
    public class SetPositionToScreenSide : BossStateMachineBehaviour
    {
        public bool LeftSide = true;
        public bool RightSide = true;
        public bool TopSide = true;
        public bool BottomSide = true;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            var gameAreaBounds = Boss.GameManager.GameArea.GetWorldRect();
            var newPosition = Boss.Position;

            List<EScreenSide> screenSides = new List<EScreenSide>();
            if (LeftSide) screenSides.Add(EScreenSide.Left);
            if (RightSide) screenSides.Add(EScreenSide.Right);
            if (TopSide) screenSides.Add(EScreenSide.Top);
            if (BottomSide) screenSides.Add(EScreenSide.Bottom);

            var randomSide = screenSides[Random.Range(0, screenSides.Count)];

            switch (randomSide)
            {
                case EScreenSide.Left:
                    newPosition.x = gameAreaBounds.xMin - Boss.Width;
                    break;
                case EScreenSide.Right:
                    newPosition.x = gameAreaBounds.xMax + Boss.Width;
                    break;
                case EScreenSide.Top:
                    newPosition.y = gameAreaBounds.yMax + Boss.Height;
                    break;
                case EScreenSide.Bottom:
                    newPosition.y = gameAreaBounds.yMin - Boss.Height;
                    break;
            }

            Boss.Position = newPosition;
        }
    }
}