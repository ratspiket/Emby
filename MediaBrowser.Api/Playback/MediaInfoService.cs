﻿using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Devices;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Dlna;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.Model.Session;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediaBrowser.Api.Playback
{
    [Route("/Items/{Id}/PlaybackInfo", "GET", Summary = "Gets live playback media info for an item")]
    public class GetPlaybackInfo : IReturn<PlaybackInfoResponse>
    {
        [ApiMember(Name = "Id", Description = "Item Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "GET")]
        public string Id { get; set; }

        [ApiMember(Name = "UserId", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "query", Verb = "GET")]
        public string UserId { get; set; }
    }

    [Route("/Items/{Id}/PlaybackInfo", "POST", Summary = "Gets live playback media info for an item")]
    public class GetPostedPlaybackInfo : PlaybackInfoRequest, IReturn<PlaybackInfoResponse>
    {
    }

    [Route("/LiveStreams/Open", "POST", Summary = "Opens a media source")]
    public class OpenMediaSource : LiveStreamRequest, IReturn<LiveStreamResponse>
    {
    }

    [Route("/LiveStreams/Close", "POST", Summary = "Closes a media source")]
    public class CloseMediaSource : IReturnVoid
    {
        [ApiMember(Name = "LiveStreamId", Description = "LiveStreamId", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "POST")]
        public string LiveStreamId { get; set; }
    }

    [Route("/Playback/BitrateTest", "GET")]
    public class GetBitrateTestBytes
    {
        [ApiMember(Name = "Size", Description = "Size", IsRequired = true, DataType = "int", ParameterType = "query", Verb = "GET")]
        public long Size { get; set; }

        public GetBitrateTestBytes()
        {
            // 100k
            Size = 102400;
        }
    }

    [Authenticated]
    public class MediaInfoService : BaseApiService
    {
        private readonly IMediaSourceManager _mediaSourceManager;
        private readonly IDeviceManager _deviceManager;
        private readonly ILibraryManager _libraryManager;
        private readonly IServerConfigurationManager _config;
        private readonly INetworkManager _networkManager;

        public MediaInfoService(IMediaSourceManager mediaSourceManager, IDeviceManager deviceManager, ILibraryManager libraryManager, IServerConfigurationManager config, INetworkManager networkManager)
        {
            _mediaSourceManager = mediaSourceManager;
            _deviceManager = deviceManager;
            _libraryManager = libraryManager;
            _config = config;
            _networkManager = networkManager;
        }

        public object Get(GetBitrateTestBytes request)
        {
            var bytes = new byte[request.Size];

            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0;
            }

            return ResultFactory.GetResult(bytes, "application/octet-stream");
        }

        public async Task<object> Get(GetPlaybackInfo request)
        {
            var result = await GetPlaybackInfo(request.Id, request.UserId, new[] { MediaType.Audio, MediaType.Video }).ConfigureAwait(false);
            return ToOptimizedResult(result);
        }

        public async Task<object> Post(OpenMediaSource request)
        {
            var authInfo = AuthorizationContext.GetAuthorizationInfo(Request);

            var result = await _mediaSourceManager.OpenLiveStream(request, false, CancellationToken.None).ConfigureAwait(false);

            var profile = request.DeviceProfile;
            if (profile == null)
            {
                var caps = _deviceManager.GetCapabilities(authInfo.DeviceId);
                if (caps != null)
                {
                    profile = caps.DeviceProfile;
                }
            }

            if (profile != null)
            {
                var item = _libraryManager.GetItemById(request.ItemId);

                SetDeviceSpecificData(item, result.MediaSource, profile, authInfo, request.MaxStreamingBitrate,
                    request.StartTimeTicks ?? 0, result.MediaSource.Id, request.AudioStreamIndex,
                    request.SubtitleStreamIndex, request.PlaySessionId);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(result.MediaSource.TranscodingUrl))
                {
                    result.MediaSource.TranscodingUrl += "&LiveStreamId=" + result.MediaSource.LiveStreamId;
                }
            }

            return ToOptimizedResult(result);
        }

        public void Post(CloseMediaSource request)
        {
            var task = _mediaSourceManager.CloseLiveStream(request.LiveStreamId, CancellationToken.None);
            Task.WaitAll(task);
        }

        public async Task<object> Post(GetPostedPlaybackInfo request)
        {
            var authInfo = AuthorizationContext.GetAuthorizationInfo(Request);

            var profile = request.DeviceProfile;

            if (profile == null)
            {
                var caps = _deviceManager.GetCapabilities(authInfo.DeviceId);
                if (caps != null)
                {
                    profile = caps.DeviceProfile;
                }
            }

            var info = await GetPlaybackInfo(request.Id, request.UserId, new[] { MediaType.Audio, MediaType.Video }, request.MediaSourceId, request.LiveStreamId).ConfigureAwait(false);

            if (profile != null)
            {
                var mediaSourceId = request.MediaSourceId;

                SetDeviceSpecificData(request.Id, info, profile, authInfo, request.MaxStreamingBitrate ?? profile.MaxStreamingBitrate, request.StartTimeTicks ?? 0, mediaSourceId, request.AudioStreamIndex, request.SubtitleStreamIndex);
            }

            return ToOptimizedResult(info);
        }

        private async Task<PlaybackInfoResponse> GetPlaybackInfo(string id, string userId, string[] supportedLiveMediaTypes, string mediaSourceId = null, string liveStreamId = null)
        {
            var result = new PlaybackInfoResponse();

            if (string.IsNullOrWhiteSpace(liveStreamId))
            {
                IEnumerable<MediaSourceInfo> mediaSources;
                try
                {
                    mediaSources = await _mediaSourceManager.GetPlayackMediaSources(id, userId, true, supportedLiveMediaTypes, CancellationToken.None).ConfigureAwait(false);
                }
                catch (PlaybackException ex)
                {
                    mediaSources = new List<MediaSourceInfo>();
                    result.ErrorCode = ex.ErrorCode;
                }

                result.MediaSources = mediaSources.ToList();

                if (!string.IsNullOrWhiteSpace(mediaSourceId))
                {
                    result.MediaSources = result.MediaSources
                        .Where(i => string.Equals(i.Id, mediaSourceId, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }
            else
            {
                var mediaSource = await _mediaSourceManager.GetLiveStream(liveStreamId, CancellationToken.None).ConfigureAwait(false);

                result.MediaSources = new List<MediaSourceInfo> { mediaSource };
            }

            if (result.MediaSources.Count == 0)
            {
                if (!result.ErrorCode.HasValue)
                {
                    result.ErrorCode = PlaybackErrorCode.NoCompatibleStream;
                }
            }
            else
            {
                result.PlaySessionId = Guid.NewGuid().ToString("N");
            }

            return result;
        }

        private void SetDeviceSpecificData(string itemId,
            PlaybackInfoResponse result,
            DeviceProfile profile,
            AuthorizationInfo auth,
            int? maxBitrate,
            long startTimeTicks,
            string mediaSourceId,
            int? audioStreamIndex,
            int? subtitleStreamIndex)
        {
            var item = _libraryManager.GetItemById(itemId);

            foreach (var mediaSource in result.MediaSources)
            {
                SetDeviceSpecificData(item, mediaSource, profile, auth, maxBitrate, startTimeTicks, mediaSourceId, audioStreamIndex, subtitleStreamIndex, result.PlaySessionId);
            }

            SortMediaSources(result);
        }

        private void SetDeviceSpecificData(BaseItem item,
            MediaSourceInfo mediaSource,
            DeviceProfile profile,
            AuthorizationInfo auth,
            int? maxBitrate,
            long startTimeTicks,
            string mediaSourceId,
            int? audioStreamIndex,
            int? subtitleStreamIndex,
            string playSessionId)
        {
            var streamBuilder = new StreamBuilder(Logger);

            var options = new VideoOptions
            {
                MediaSources = new List<MediaSourceInfo> { mediaSource },
                Context = EncodingContext.Streaming,
                DeviceId = auth.DeviceId,
                ItemId = item.Id.ToString("N"),
                Profile = profile
            };

            if (string.Equals(mediaSourceId, mediaSource.Id, StringComparison.OrdinalIgnoreCase))
            {
                options.MediaSourceId = mediaSourceId;
                options.AudioStreamIndex = audioStreamIndex;
                options.SubtitleStreamIndex = subtitleStreamIndex;
            }

            if (mediaSource.SupportsDirectPlay)
            {
                var supportsDirectStream = mediaSource.SupportsDirectStream;

                // Dummy this up to fool StreamBuilder
                mediaSource.SupportsDirectStream = true;
                options.MaxBitrate = maxBitrate;

                // The MediaSource supports direct stream, now test to see if the client supports it
                var streamInfo = string.Equals(item.MediaType, MediaType.Audio, StringComparison.OrdinalIgnoreCase) ?
                    streamBuilder.BuildAudioItem(options) :
                    streamBuilder.BuildVideoItem(options);

                if (streamInfo == null || !streamInfo.IsDirectStream)
                {
                    mediaSource.SupportsDirectPlay = false;
                }

                // Set this back to what it was
                mediaSource.SupportsDirectStream = supportsDirectStream;

                if (streamInfo != null)
                {
                    SetDeviceSpecificSubtitleInfo(streamInfo, mediaSource, auth.Token);
                }
            }

            if (mediaSource.SupportsDirectStream)
            {
                options.MaxBitrate = GetMaxBitrate(maxBitrate);

                // The MediaSource supports direct stream, now test to see if the client supports it
                var streamInfo = string.Equals(item.MediaType, MediaType.Audio, StringComparison.OrdinalIgnoreCase) ?
                    streamBuilder.BuildAudioItem(options) :
                    streamBuilder.BuildVideoItem(options);

                if (streamInfo == null || !streamInfo.IsDirectStream)
                {
                    mediaSource.SupportsDirectStream = false;
                }

                if (streamInfo != null)
                {
                    SetDeviceSpecificSubtitleInfo(streamInfo, mediaSource, auth.Token);
                }
            }

            if (mediaSource.SupportsTranscoding)
            {
                options.MaxBitrate = GetMaxBitrate(maxBitrate);

                // The MediaSource supports direct stream, now test to see if the client supports it
                var streamInfo = string.Equals(item.MediaType, MediaType.Audio, StringComparison.OrdinalIgnoreCase) ?
                    streamBuilder.BuildAudioItem(options) :
                    streamBuilder.BuildVideoItem(options);

                if (streamInfo != null)
                {
                    streamInfo.PlaySessionId = playSessionId;

                    if (streamInfo.PlayMethod == PlayMethod.Transcode)
                    {
                        streamInfo.StartPositionTicks = startTimeTicks;
                        mediaSource.TranscodingUrl = streamInfo.ToUrl("-", auth.Token).TrimStart('-');
                        mediaSource.TranscodingContainer = streamInfo.Container;
                        mediaSource.TranscodingSubProtocol = streamInfo.SubProtocol;
                    }

                    // Do this after the above so that StartPositionTicks is set
                    SetDeviceSpecificSubtitleInfo(streamInfo, mediaSource, auth.Token);
                }
            }
        }

        private int? GetMaxBitrate(int? clientMaxBitrate)
        {
            var maxBitrate = clientMaxBitrate;
            var remoteClientMaxBitrate = _config.Configuration.RemoteClientBitrateLimit;

            if (remoteClientMaxBitrate > 0)
            {
                var isInLocalNetwork = _networkManager.IsInLocalNetwork(Request.RemoteIp);

                Logger.Info("RemoteClientBitrateLimit: {0}, RemoteIp: {1}, IsInLocalNetwork: {2}", remoteClientMaxBitrate, Request.RemoteIp, isInLocalNetwork);
                if (!isInLocalNetwork)
                {
                    maxBitrate = Math.Min(maxBitrate ?? remoteClientMaxBitrate, remoteClientMaxBitrate);
                }
            }

            return maxBitrate;
        }

        private void SetDeviceSpecificSubtitleInfo(StreamInfo info, MediaSourceInfo mediaSource, string accessToken)
        {
            var profiles = info.GetSubtitleProfiles(false, "-", accessToken);
            mediaSource.DefaultSubtitleStreamIndex = info.SubtitleStreamIndex;

            foreach (var profile in profiles)
            {
                foreach (var stream in mediaSource.MediaStreams)
                {
                    if (stream.Type == MediaStreamType.Subtitle && stream.Index == profile.Index)
                    {
                        stream.DeliveryMethod = profile.DeliveryMethod;

                        if (profile.DeliveryMethod == SubtitleDeliveryMethod.External)
                        {
                            stream.DeliveryUrl = profile.Url.TrimStart('-');
                            stream.IsExternalUrl = profile.IsExternalUrl;
                        }
                    }
                }
            }
        }

        private void SortMediaSources(PlaybackInfoResponse result)
        {
            var originalList = result.MediaSources.ToList();

            result.MediaSources = result.MediaSources.OrderBy(i =>
            {
                // Nothing beats direct playing a file
                if (i.SupportsDirectPlay && i.Protocol == MediaProtocol.File)
                {
                    return 0;
                }

                return 1;

            }).ThenBy(i =>
            {
                // Let's assume direct streaming a file is just as desirable as direct playing a remote url
                if (i.SupportsDirectPlay || i.SupportsDirectStream)
                {
                    return 0;
                }

                return 1;

            }).ThenBy(i =>
            {
                switch (i.Protocol)
                {
                    case MediaProtocol.File:
                        return 0;
                    default:
                        return 1;
                }

            }).ThenBy(originalList.IndexOf)
            .ToList();
        }
    }
}
