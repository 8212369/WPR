namespace WPR
{
    public enum ApplicationInstallError
    {
        None,
        MissingManifestFiles,
        InvalidManifestFiles,
        NotDecrypted,
        NotSupportedAppType,
        UnexpectedError,
        PatchFailed,
        ConvertFailed,
        Canceled
    }
}
