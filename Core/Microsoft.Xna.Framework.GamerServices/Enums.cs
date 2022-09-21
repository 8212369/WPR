namespace Microsoft.Xna.Framework.GamerServices
{
    using System;
    using Microsoft.Xna.Framework.GamerServices;

    public enum AvatarAnimationPreset
    {
        Stand0,
        Stand1,
        Stand2,
        Stand3,
        Stand4,
        Stand5,
        Stand6,
        Stand7,
        Clap,
        Wave,
        Celebrate,
        FemaleIdleCheckNails,
        FemaleIdleLookAround,
        FemaleIdleShiftWeight,
        FemaleIdleFixShoe,
        FemaleAngry,
        FemaleConfused,
        FemaleLaugh,
        FemaleCry,
        FemaleShocked,
        FemaleYawn,
        MaleIdleLookAround,
        MaleIdleStretch,
        MaleIdleShiftWeight,
        MaleIdleCheckHand,
        MaleAngry,
        MaleConfused,
        MaleLaugh,
        MaleCry,
        MaleSurprised,
        MaleYawn
    }

    public enum AvatarBodyType
    {
        Female,
        Male
    }

    public enum AvatarBone
    {
        AnkleLeft = 11,
        AnkleRight = 15,
        BackLower = 1,
        BackUpper = 5,
        CollarLeft = 12,
        CollarRight = 0x10,
        ElbowLeft = 0x19,
        ElbowRight = 0x1c,
        FingerIndex2Left = 0x33,
        FingerIndex2Right = 0x38,
        FingerIndex3Left = 0x3d,
        FingerIndex3Right = 0x42,
        FingerIndexLeft = 0x25,
        FingerIndexRight = 0x2c,
        FingerMiddle2Left = 0x34,
        FingerMiddle2Right = 0x39,
        FingerMiddle3Left = 0x3e,
        FingerMiddle3Right = 0x43,
        FingerMiddleLeft = 0x26,
        FingerMiddleRight = 0x2d,
        FingerRing2Left = 0x35,
        FingerRing2Right = 0x3a,
        FingerRing3Left = 0x3f,
        FingerRing3Right = 0x44,
        FingerRingLeft = 0x27,
        FingerRingRight = 0x2e,
        FingerSmall2Left = 0x36,
        FingerSmall2Right = 0x3b,
        FingerSmall3Left = 0x40,
        FingerSmall3Right = 0x45,
        FingerSmallLeft = 40,
        FingerSmallRight = 0x2f,
        FingerThumb2Left = 0x37,
        FingerThumb2Right = 60,
        FingerThumb3Left = 0x41,
        FingerThumb3Right = 70,
        FingerThumbLeft = 0x2b,
        FingerThumbRight = 50,
        Head = 0x13,
        HipLeft = 2,
        HipRight = 3,
        KneeLeft = 6,
        KneeRight = 8,
        Neck = 14,
        PropLeft = 0x29,
        PropRight = 0x30,
        Root = 0,
        ShoulderLeft = 20,
        ShoulderRight = 0x16,
        SpecialLeft = 0x2a,
        SpecialRight = 0x31,
        ToeLeft = 0x15,
        ToeRight = 0x17,
        WristLeft = 0x21,
        WristRight = 0x24
    }

    public enum AvatarEye
    {
        Neutral,
        Sad,
        Angry,
        Confused,
        Laughing,
        Shocked,
        Happy,
        Yawning,
        Sleeping,
        LookUp,
        LookDown,
        LookLeft,
        LookRight,
        Blink
    }

    public enum AvatarEyebrow
    {
        Neutral,
        Sad,
        Angry,
        Confused,
        Raised
    }

    public enum AvatarMouth
    {
        Neutral,
        Sad,
        Angry,
        Confused,
        Laughing,
        Shocked,
        Happy,
        PhoneticO,
        PhoneticAi,
        PhoneticEe,
        PhoneticFv,
        PhoneticW,
        PhoneticL,
        PhoneticDth
    }

    public enum ControllerSensitivity
    {
        Low = 0,
        Medium = 1,
        High = 2,
    }

    [Flags]
    internal enum FriendState
    {
        FriendHasVoice = 0x20,
        FriendIsAway = 8,
        FriendIsBusy = 0x10,
        FriendIsJoinable = 4,
        FriendIsOnline = 1,
        FriendIsPlaying = 2,
        FriendRequestReceivedFrom = 0x40,
        FriendRequestSentTo = 0x80,
        InviteAccepted = 0x400,
        InviteReceivedFrom = 0x100,
        InviteRejected = 0x800,
        InviteSentTo = 0x200
    }

    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard
    }

    public enum GamerPresenceMode
    {
        None,
        SinglePlayer,
        Multiplayer,
        LocalCoOp,
        LocalVersus,
        OnlineCoOp,
        OnlineVersus,
        VersusComputer,
        Stage,
        Level,
        CoOpStage,
        CoOpLevel,
        ArcadeMode,
        CampaignMode,
        ChallengeMode,
        ExplorationMode,
        PracticeMode,
        PuzzleMode,
        ScenarioMode,
        StoryMode,
        SurvivalMode,
        TutorialMode,
        DifficultyEasy,
        DifficultyMedium,
        DifficultyHard,
        DifficultyExtreme,
        Score,
        VersusScore,
        Winning,
        Losing,
        ScoreIsTied,
        Outnumbered,
        OnARoll,
        InCombat,
        BattlingBoss,
        TimeAttack,
        TryingForRecord,
        FreePlay,
        WastingTime,
        StuckOnAHardBit,
        NearlyFinished,
        LookingForGames,
        WaitingForPlayers,
        WaitingInLobby,
        SettingUpMatch,
        PlayingWithFriends,
        AtMenu,
        StartingGame,
        Paused,
        GameOver,
        WonTheGame,
        ConfiguringSettings,
        CustomizingPlayer,
        EditingLevel,
        InGameStore,
        WatchingCutscene,
        WatchingCredits,
        PlayingMinigame,
        FoundSecret,
        CornflowerBlue
    }

    public enum GamerPrivilegeSetting
    {
        Blocked,
        FriendsOnly,
        Everyone
    }
    public enum GamerZone
    {
        Unknown,
        Recreation,
        Pro,
        Family,
        Underground
    }

    public enum RacingCameraAngle
    {
        Back,
        Front,
        Inside
    }
    public enum MessageBoxIcon
    {
        None,
        Error,
        Warning,
        Alert
    }

    public enum NotificationPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        CenterLeft,
        Center,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

}
