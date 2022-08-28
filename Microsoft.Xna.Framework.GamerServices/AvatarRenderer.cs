using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class AvatarRenderer : IDisposable
    {
        [CLSCompliant(false)]
        public const int BoneCount = 0x47;

        public AvatarRenderer(AvatarDescription avatarDescription)
            : this(avatarDescription, true)
        {
        }

        public AvatarRenderer(AvatarDescription avatarDescription, bool useLoadingEffect)
        {
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

        public void Draw(IList<Matrix> bones, AvatarExpression expression)
        {
            throw new NotImplementedException();
        }

        public Vector3 AmbientLightColor
        {
            get
        {
            throw new NotImplementedException();
        }
            set
        {
            throw new NotImplementedException();
        }
        }

        public ReadOnlyCollection<Matrix> BindPose
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsDisposed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsLoaded
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Vector3 LightColor
        {
            get
        {
            throw new NotImplementedException();
        }
            set
        {
            throw new NotImplementedException();
        }
        }

        public Vector3 LightDirection
        {
            get
        {
            throw new NotImplementedException();
        }
            set
        {
            throw new NotImplementedException();
        }
        }

        public ReadOnlyCollection<int> ParentBones
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Matrix Projection
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Matrix View
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Matrix World
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

    }
}
