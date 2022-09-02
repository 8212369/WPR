using WPR.WindowsCompability;
using WPR.Common;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class SignedInGamer : Gamer
    {
        private PlayerIndex _PlayerIndex;

        private static EventWaitHandle _NewSignedRequestSema;
        private static Thread? _SignedInvokeThread;
        private static event EventHandler<SignedInEventArgs> _SignedIn;


        private static void InitializeSignedInDelayThread()
        {
            _NewSignedRequestSema = new EventWaitHandle(false, EventResetMode.AutoReset);
            _SignedInvokeThread = new Thread(delegate () {
                _NewSignedRequestSema.WaitOne();

                Thread.Sleep(100);
                _SignedIn.Invoke(null, new SignedInEventArgs(_SignedInGamers[0]));
            });

            _SignedInvokeThread.Start();
        }

        public event EventHandler<EventArgs> AvatarChanged;

        public static event EventHandler<SignedInEventArgs> SignedIn
        {
            add
            {
                if (_SignedInvokeThread == null)
                {
                    InitializeSignedInDelayThread();
                }

                _NewSignedRequestSema.Set();
                _SignedIn += value;
            }
            remove
            {
                _SignedIn -= value;
            }
        }

        public static event EventHandler<SignedOutEventArgs> SignedOut;

        internal SignedInGamer()
        {
        }

        public IAsyncResult BeginGetAchievements(AsyncCallback? callback, Object? asyncState)
        {
            return Task.Run(async () =>
            {
                List<Achievement> achievementStored = await AchievementContext.Current!.Achievements!
                    .Where(x => x.OwnProductId == Application.Current.ProductId)
                    .ToListAsync();

                if (achievementStored.Count == 0)
                {
                    Task<AchievementCollection> collection = TrueAchievements.Scraper.QueryAchievements(Application.Current!.ProductId!);
                    if (collection.Result.Count != 0)
                    {
                        await AchievementContext.Current!.Achievements!.AddRangeAsync(collection.Result.ToArray());
                        await AchievementContext.Current!.SaveChangesAsync();
                    }

                    if (callback != null)
                    {
                        var compSource = new TaskCompletionSource<AchievementCollection>(asyncState);
                        compSource.SetResult(collection.Result);

                        callback(compSource.Task);
                    }

                    return collection.Result;
                }

                AchievementCollection coll = new AchievementCollection();
                foreach (Achievement achiQueried in achievementStored)
                {
                    coll.Add(achiQueried);
                }

                var completeSource = new TaskCompletionSource<AchievementCollection>(asyncState);
                completeSource.SetResult(coll);

                if (callback != null)
                {
                    callback(completeSource.Task);
                }

                return coll;
            });
        }

        public AchievementCollection EndGetAchievements(IAsyncResult result)
        {
            Task<AchievementCollection>? collectResult = result as Task<AchievementCollection>;
            return collectResult!.Result;
        }

        public AchievementCollection GetAchievements() => this.EndGetAchievements(this.BeginGetAchievements(null, null));

        public IAsyncResult BeginAwardAchievement(string achievementKey, AsyncCallback callback,
            object state)
        {
            return Task.Run(async () =>
            {
                List<Achievement> achievements = await AchievementContext.Current!.Achievements!
                    .Where(x => (x.OwnProductId == Application.Current.ProductId) && (x.Key == achievementKey))
                    .ToListAsync();

                if (achievements.Count > 1)
                {
                    Log.Warn(LogCategory.GamerServices, $"More then two achievements with key {achievementKey} exists!");
                }

                if (achievements.Count != 0)
                {
                    foreach (Achievement achievement in achievements)
                    {
                        if (achievement.IsEarned)
                        {
                            continue;
                        }

                        achievement.IsEarned = true;
                        achievement.EarnedOnline = true;
                        achievement.EarnedDateTime = DateTime.Now;
                    }

                    try
                    {
                        await NativeUI.NotificationManager.ShowNotification(new DesktopNotifications.Notification()
                        {
                            Title = Properties.Resources.AchievementUnlocked,
                            Body = $"{achievements[0].GamerScore}G - {achievements[0].Name}",
                            ImagePath = achievements[0]._IconPath
                        }, DateTime.Now + TimeSpan.FromSeconds(5));
                    } catch (Exception ex)
                    {
                        Log.Error(LogCategory.GamerServices, $"Fail to display Achievement notification with exception:\n {ex}");
                    }
                }

                await AchievementContext.Current!.SaveChangesAsync();

                TaskCompletionSource source = new TaskCompletionSource(state);
                source.SetResult();

                callback(source.Task);
                return source.Task;
            });
        }

        public void EndAwardAchievement(IAsyncResult result)
        {
        }

        public FriendCollection GetFriends()
        {
            throw new NotImplementedException();
        }

        public bool IsFriend(Gamer gamer)
        {
            throw new NotImplementedException();
        }
        public AvatarDescription Avatar
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public GameDefaults GameDefaults
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsGuest
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSignedInToLive
        {
            get
            {
                return true;
            }
        }

        public int PartySize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public PlayerIndex PlayerIndex
        {
            get => _PlayerIndex;
            set => _PlayerIndex = value;
        }

        public GamerPresence Presence
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public GamerPrivileges Privileges
        {
            get
            {
                throw new NotImplementedException();
            }
        }

    }
}
