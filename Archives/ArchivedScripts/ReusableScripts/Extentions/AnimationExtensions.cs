using System;
using System.Collections;

namespace UnityEngine
{
	
	public static class AnimationExtensions
	{
        public static AnimationClip RequireClip(this Animation argAnimation, AnimationClip argMasterClip,
            string argClipName, int argStart, int argEnd, bool argAddLoopFrame, WrapMode wrapMode, int layer, float speed)
        {
            AnimationState state = argAnimation[argClipName];
            if (state == null)
                argAnimation.AddClip(argMasterClip, argClipName, argStart, argEnd, argAddLoopFrame);

            state = argAnimation[argClipName];
            state.speed = 1;
            state.wrapMode = wrapMode;
            state.layer = layer;
            return state.clip;
        }

        public static void CrossFade(this Animation argAnimation,
            string argClipName, int argStart, int argEnd, bool argAddLoopFrame)
        {
            AnimationState state = argAnimation[argClipName];
            if (state != null)
            {
                string animationName = argAnimation.UniqueName(argClipName);
                argAnimation.AddClip(state.clip, animationName, 
                    argStart, argEnd, argAddLoopFrame);

                argAnimation.CrossFade(animationName);
            }
        }

	    public static AnimationClip PopClip(this Animation argAnimation, string argClipName)
		{
			AnimationClip clip = argAnimation.GetClip(argClipName);
			if(clip != null)
			{
				argAnimation.RemoveClip(clip);	
				return clip;
			}
			throw new NullReferenceException("Error: Clip Does Not Exist");
		}



        private static string UniqueName(this Animation argAnimation, string name)
        {
            var count = 0;
            while (argAnimation[name + count.ToString()] != null)
                count++;
            return name + count.ToString();
        }
	}	
}