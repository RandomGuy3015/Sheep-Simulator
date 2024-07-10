using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Test.DynamicContentManagement
{
    public class DynamicContentManager
    {
        private ContentManager mContentManager;
        public static DynamicContentManager Instance { get; private set; }
        private Dictionary<string, object> mLoadedContent;

        public DynamicContentManager(ContentManager contentManager)
        {
            this.mContentManager = contentManager;
            mLoadedContent = new Dictionary<string, object>();
            Instance = this;
        }

        public TAsset Load<TAsset>(string assetName) where TAsset : class
        {
            try
            {
                if (mLoadedContent.TryGetValue(assetName, out object asset))
                {
                    return asset as TAsset;
                }
                else
                {
                    TAsset newAsset = mContentManager.Load<TAsset>(assetName);
                    mLoadedContent.Add(assetName, newAsset);
                    return newAsset;
                }
            }
            catch (ContentLoadException ex)
            {
                // Log or handle the exception (e.g., print error message)
                Console.WriteLine($"Failed to load content '{assetName}': {ex.Message}");
                return null;
            }
        }
    }
}
