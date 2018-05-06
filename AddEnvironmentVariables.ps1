$envVariables = @{
    TwitterConsumerKey = "";
    TwitterConsumerSecret = "";
    TwitterAccessToken = "";
    TwitterAccessTokenSecret = "";
    ConnectionString = "";
    YoutubeKey = "";
    TwitchClientId = ""
    DiscordToken = "";
    ImgurClientId = "";
    ImgurAlbumId = "";
    ThrottleLength = "";
    Prefix = "";

}
foreach ($pair in $envVariables.GetEnumerator())
{
     $pair | Select-Object -Property *
     [System.Environment]::SetEnvironmentVariable('@'+$pair.Key,$pair.Value, [System.EnvironmentVariableTarget]::User)
     
}
