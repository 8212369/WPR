using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.Tasks
{
    public class MarketplaceDetailTask
    {
        private MarketplaceContentType _contentType;

        public MarketplaceContentType ContentType
        {
            get => _contentType;
            set => _contentType = value;
        }

        public void Show()
        {

        }
    }
}
