using ExCSS;
using HtmlAgilityPack;
using WPR.Common;

using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Microsoft.Xna.Framework.GamerServices.TrueAchievements
{
    public class Scraper
    {
        public async static Task<AchievementCollection> QueryAchievements(string productId)
        {
            GameToKey researcher = new GameToKey();
            string? urlPathContent = researcher.GetURLFromProductId(productId);

            if (urlPathContent == null)
            {
                Log.Error(LogCategory.GamerServices, $"Failed to obtain TrueAchievements's URL for product {productId}");
                return new AchievementCollection();
            }

            string fullUrl = $"https://www.trueachievements.com/game/{urlPathContent}/achievements";
            var response = await WWWAccess.CallUrlForString(fullUrl);

            var document = new HtmlDocument();
            document.LoadHtml(response);

            // Get sms1 image path
            String sms1Path = null;
            var styles = document.DocumentNode.SelectNodes("//head/style");

            foreach (var style in styles)
            {
                var styleSheetParser = new StylesheetParser();
                var styleSheet = styleSheetParser.Parse(style.InnerText);

                var sms1Rule = styleSheet.StyleRules.Where(styleRule => styleRule.SelectorText == ".sms1");
                if (sms1Rule == null)
                {
                    continue;
                }

                String urlString = sms1Rule.First().Style.BackgroundImage;
                sms1Path = urlString.Substring(5, urlString.Length - 7);
                break;
            }

            if (sms1Path == null)
            {
                return new AchievementCollection();
            }

            byte[] imageData = await WWWAccess.CallUrlDownload(sms1Path);
            MemoryStream imageDataStream = new MemoryStream(imageData);

            var icons = document.DocumentNode.QuerySelectorAll(".ach-panels li");
            string currentStoreImages = Configuration.Current!.DataPath($"Database/Achievements/{productId}");
            List<String> imagePaths = ImageUtils.SplitAndSave(currentStoreImages,
                "achievement{0}.png", imageDataStream,
                icons.Count, 1);

            var collection = new AchievementCollection();
            Achievement[] achievementList = new Achievement[icons.Count];

            for (int i = 0; i < icons.Count; i++)
            {
                var achiKey = icons[i].QuerySelector("a .title");
                var achiScoreAndDesc = icons[i].QuerySelector("p");
                var test = achiScoreAndDesc.ChildAttributes("data-bf").First();
                var achiScore = Convert.ToInt32(achiScoreAndDesc.ChildAttributes("data-bf").First().Value.Split(' ')[0]);

                var keyFinal = researcher.GetAchievementKey(productId, achiKey.InnerText) ?? achiKey.InnerText;

                collection.Add(new Achievement()
                {
                    Key = keyFinal,
                    Name = achiKey.InnerText,
                    Description = achiScoreAndDesc.InnerText,
                    _IconPath = Path.GetRelativePath(Configuration.Current!.DataStorePath!, imagePaths[i]),
                    GamerScore = achiScore,
                    IsEarned = false,
                    HowToEarn = achiScoreAndDesc.InnerText,
                    DisplayBeforeEarned = true,
                    OwnProductId = productId
                });

                Log.Error(LogCategory.GamerServices, collection.Last().Key);
            }

            return collection; 
        }
    }
}
