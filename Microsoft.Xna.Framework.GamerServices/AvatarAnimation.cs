using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class AvatarAnimation : IDisposable
    {

        private Matrix[] avatarBones = new Matrix[0x47];
        private ReadOnlyCollection<Matrix> boneTransforms;
        private AvatarExpression currentExpression = new AvatarExpression();
        private TimeSpan currentPosition;
        private bool isDisposed;
        private TimeSpan length;

        public AvatarAnimation(AvatarAnimationPreset animationPreset)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }

        public void Update(TimeSpan elapsedAnimationTime, bool loop)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<Matrix> BoneTransforms
        {
            get
            {
                return this.boneTransforms;
            }
        }

        public TimeSpan CurrentPosition
        {
            get
            {
                return this.currentPosition;
            }
            set
            {
                this.currentPosition = value;
                this.Update(TimeSpan.Zero, false);
            }
        }

        public AvatarExpression Expression
        {
            get
            {
                return this.currentExpression;
            }
        }

        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
        }

        public TimeSpan Length
        {
            get
            {
                return this.length;
            }
        }

    }
}
