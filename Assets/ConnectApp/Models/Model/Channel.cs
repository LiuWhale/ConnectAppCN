using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Models.Api;
using ConnectApp.Utils;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Channel {
        public string id;
        public string groupId;
        public string thumbnail;
        public string imageUrl;
        public string name;
        public string topic;
        public int memberCount;
        public bool isMute;
        public bool live;
        public ChannelMessage lastMessage;
    }

    [Serializable]
    public class ChannelMessage {
        public string id;
        public string type;
        public string nonce;
        public string channelId;
        public string content;
        public User author;
        public List<Attachment> attachments;
        public bool mentionEveryone;
        public List<User> mentions;
        public bool starred;
        public List<string> replyMessageIds;
        public List<string> lowerMessageIds;
        public List<User> replyUsers;
        public List<User> lowerUsers;
        public List<Reaction> reactions;
        public List<Embed> embeds;
        public bool pending;
        public string deletedTime;
    }

    [Serializable]
    public class Attachment {
        public string id;
        public string url;
        public string filename;
        public string contentType;
        public int width;
        public int height;
        public int size;
        public int duration;
    }

    [Serializable]
    public class Embed {
        public string embedType;
        public EmbedData embedData;
    }

    [Serializable]
    public class EmbedData {
        public string description;
        public string image;
        public string name;
        public string title;
        public string url;
    }

    [Serializable]
    public class ChannelMember {
        public string id;
        public string channelId;
        public User user;
        public string role;
        public string presenceStatus = null;
        public bool isBanned;
        public bool kicked;
        public bool left;
        public bool guideFinished;
        public bool showTerms;

        public static ChannelMember fromSocketResponseChannelMemberChangeData(
            SocketResponseChannelMemberChangeData data) {
            return new ChannelMember {
                id = data.id,
                channelId = data.channelId,
                user = data.user,
                role = data.role,
                presenceStatus = null,
                isBanned = data.isBanned,
                kicked = data.kicked,
                left = data.left,
                guideFinished = data.guideFinished,
                showTerms = data.showTerms
            };
        }

        public void updateFromSocketResponseChannelMemberChangeData(
            SocketResponseChannelMemberChangeData data) {
            this.id = data.id ?? this.id;
            this.channelId = data.channelId ?? this.channelId;
            this.user = data.user ?? this.user;
            this.role = data.role ?? this.role;
            this.isBanned = data.isBanned;
            this.kicked = data.kicked;
            this.left = data.left;
            this.guideFinished = data.guideFinished;
            this.showTerms = data.showTerms;
        }
    }

    public class ChannelView {
        public string id;
        public string groupId;
        public string thumbnail;
        public string name;
        public string topic;
        public int memberCount;
        public bool isMute;
        public bool live;
        public string lastMessageId;
        public ChannelMessageView lastMessage;
        public List<string> messageIds;
        public List<string> oldMessageIds;
        public List<string> newMessageIds;
        public int unread = 0;
        public int mentioned = 0;
        public bool isTop = false;
        public bool joined = false;
        public bool joinLoading = false;
        public bool atMe = false;
        public bool atAll = false;
        public bool sendingMessage = false;
        public bool sentMessageFailed = false;
        public bool sentMessageSuccess = false;
        public bool hasMore = true;
        public bool hasMoreNew = true;
        public List<string> memberIds;
        public Dictionary<string, ChannelMember> membersDict;
        public int memberOffset;
        public bool atBottom = true;

        public static ChannelView fromChannel(Channel channel) {
            return new ChannelView {
                atAll = channel?.lastMessage?.mentionEveryone ?? false,
                memberIds = new List<string>(),
                membersDict = new Dictionary<string, ChannelMember>(),
                id = channel?.id,
                groupId = channel?.groupId,
                thumbnail = channel?.thumbnail,
                name = channel?.name,
                topic = channel?.topic,
                memberCount = channel?.memberCount ?? 0,
                isMute = channel?.isMute ?? false,
                live = channel?.live ?? false,
                lastMessageId = channel?.lastMessage?.id,
                lastMessage = ChannelMessageView.fromChannelMessage(channel?.lastMessage),
                messageIds = new List<string>(),
                oldMessageIds = new List<string>(),
                newMessageIds = new List<string>()
            };
        }

        public void updateFromChannel(Channel channel) {
            this.atAll = this.atAll || (channel?.lastMessage?.mentionEveryone ?? false);
            this.id = channel?.id ?? this.id;
            this.groupId = channel?.groupId ?? this.groupId;
            this.thumbnail = channel?.thumbnail ?? this.thumbnail;
            this.name = channel?.name ?? this.name;
            this.topic = channel?.topic ?? this.topic;
            this.memberCount = channel?.memberCount ?? this.memberCount;
            this.isMute = channel?.isMute ?? this.isMute;
            this.live = channel?.live ?? this.live;
            this.lastMessage = channel?.lastMessage == null
                ? this.lastMessage
                : ChannelMessageView.fromChannelMessage(channel.lastMessage);
            this.lastMessageId = channel?.lastMessage?.id ?? this.lastMessageId;
        }

        public static ChannelView fromNormalChannelLite(NormalChannelLite channel) {
            return new ChannelView {
                atAll = false,
                memberIds = new List<string>(),
                membersDict = new Dictionary<string, ChannelMember>(),
                id = channel?.id,
                groupId = channel?.groupId,
                thumbnail = channel?.thumbnail,
                name = channel?.name,
                topic = channel?.topic,
                memberCount = channel?.memberCount ?? 0,
                isMute = channel?.isMute ?? false,
                live = channel?.live ?? false,
                lastMessageId = channel?.lastMessageId,
                messageIds = new List<string>(),
                oldMessageIds = new List<string>(),
                newMessageIds = new List<string>()
            };
        }

        public void updateFromNormalChannelLite(NormalChannelLite channel) {
            this.id = channel?.id ?? this.id;
            this.groupId = channel?.groupId ?? this.groupId;
            this.thumbnail = channel?.thumbnail ?? this.thumbnail;
            this.name = channel?.name ?? this.name;
            this.topic = channel?.topic ?? this.topic;
            this.memberCount = channel?.memberCount ?? this.memberCount;
            this.isMute = channel?.isMute ?? this.isMute;
            this.live = channel?.live ?? this.live;
            this.lastMessageId = channel?.lastMessageId ?? this.lastMessageId;
        }

        public void handleUnreadMessage(ChannelMessageView message, string userId) {
            if (message.mentionEveryone || message.mentions.Any(user => user.id == userId)) {
                this.mentioned += 1;
                this.atMe = true;
            }
            this.atAll = this.atAll || message.mentionEveryone;
            this.unread += 1;
        }

        public void clearUnread() {
            this.unread = 0;
            this.mentioned = 0;
            this.atAll = false;
            this.atMe = false;
        }

        public ChannelMember getMember(string userId) {
            if (this.membersDict.TryGetValue(userId, out var member)) {
                return member;
            }

            return null;
        }

        public void updateMessageUser(MessageUser user, bool addUserIfNotExist = true) {
            if (this.membersDict.TryGetValue(user.id, out var member)) {
                member.user.id = user.id;
                member.user.username = user.username;
                member.user.fullName = user.fullname;
                member.user.avatar = user.avatar;
                member.user.title = user.title;
                member.user.coverImage = user.coverImage;
                member.user.followCount = user.followCount;
                member.presenceStatus = user.presenceStatus;
                return;
            }

            if (addUserIfNotExist) {
                this.membersDict[user.id] = new ChannelMember {
                    user = new User {
                        id = user.id,
                        username = user.username,
                        fullName = user.fullname,
                        avatar = user.avatar,
                        title = user.title,
                        coverImage = user.coverImage,
                        followCount = user.followCount
                    },
                    id = user.id,
                    presenceStatus = user.presenceStatus
                };
            }
        }
    }

    public enum ChannelMessageType {
        text,
        image,
        file,
        embed
    }

    public class ChannelMessageView {
        public string id;
        public long nonce;
        public string channelId;
        public User author;
        public DateTime time;
        public ChannelMessageType type = ChannelMessageType.text;
        public string content;
        public long fileSize = 0;
        public int width;
        public int height;
        public List<Attachment> attachments;
        public bool mentionEveryone = false;
        public List<User> mentions;
        public bool starred = false;
        public List<string> replyMessageIds;
        public List<string> lowerMessageIds;
        public List<User> replyUsers;
        public List<User> lowerUsers;
        public bool pending = false;
        public bool deleted = false;
        public List<Reaction> reactions;
        public List<Embed> embeds;

        public bool shouldSkip() {
            return this.deleted || (this.type == ChannelMessageType.text && string.IsNullOrEmpty(this.content));
        }

        static ChannelMessageType getType(
            string content,
            List<Attachment> attachments = null,
            List<Embed> embeds = null) {
            return content != null || (attachments?.Count ?? 0) == 0
                ? (embeds?.Count ?? 0) == 0 ? ChannelMessageType.text : ChannelMessageType.embed
                : attachments[0].contentType.StartsWith("image")
                    ? ChannelMessageType.image
                    : ChannelMessageType.file;
        }

        static long getNonce(string nonce) {
            return string.IsNullOrEmpty(nonce) ? 0 : Convert.ToInt64(nonce, 16);
        }

        static string getContent(string content, List<Attachment> attachments = null, List<Embed> embeds = null) {
            switch (getType(content, attachments, embeds)) {
                case ChannelMessageType.text:
                case ChannelMessageType.embed:
                    return content ?? "";
                case ChannelMessageType.image:
                    return attachments[0].url;
                case ChannelMessageType.file:
                    return attachments[0].filename;
            }
            return "";
        }

        static int getFileSize(string content, List<Attachment> attachments = null, List<Embed> embeds = null) {
            return getType(content, attachments, embeds) == ChannelMessageType.file ? attachments[0].size : 0;
        }

        static int getImageWidth(string content, List<Attachment> attachments = null, List<Embed> embeds = null) {
            return getType(content, attachments, embeds) == ChannelMessageType.image ? attachments[0].width : 0;
        }

        static int getImageHeight(string content, List<Attachment> attachments = null, List<Embed> embeds = null) {
            return getType(content, attachments, embeds) == ChannelMessageType.image ? attachments[0].height : 0;
        }

        public static ChannelMessageView fromPushMessage(SocketResponseMessageData message) {
            return message == null
                ? new ChannelMessageView()
                : new ChannelMessageView {
                    id = message.id,
                    nonce = getNonce(message.nonce),
                    channelId = message.channelId,
                    author = message.author,
                    content = getContent(message.content, message.attachments, message.embeds),
                    fileSize = getFileSize(message.content, message.attachments, message.embeds),
                    width = getImageWidth(message.content, message.attachments, message.embeds),
                    height = getImageHeight(message.content, message.attachments, message.embeds),
                    time = DateConvert.DateTimeFromNonce(message.id),
                    attachments = message.attachments,
                    type = getType(message.content, message.attachments, message.embeds),
                    mentionEveryone = message.mentionEveryone,
                    mentions = message.mentions,
                    starred = message.starred,
                    replyMessageIds = message.replyMessageIds,
                    lowerMessageIds = message.lowerMessageIds,
                    replyUsers = message.replyUsers,
                    lowerUsers = message.lowerUsers,
                    pending = message.pending,
                    deleted = message.deletedTime != null,
                    embeds = message.embeds,
                    reactions = message.reactions
                };
        }

        public static ChannelMessageView fromChannelMessageLite(ChannelMessageLite message) {
            return message == null
                ? new ChannelMessageView()
                : new ChannelMessageView {
                    id = message.id,
                    nonce = getNonce(message.nonce),
                    channelId = message.channelId,
                    author = new User {id = message.author.id},
                    content = getContent(message.content, message.attachments),
                    fileSize = getFileSize(message.content, message.attachments),
                    width = getImageWidth(message.content, message.attachments),
                    height = getImageHeight(message.content, message.attachments),
                    time = DateConvert.DateTimeFromNonce(message.id),
                    attachments = message.attachments,
                    type = getType(message.content, message.attachments),
                    mentionEveryone = message.mentionEveryone,
                    mentions = message.mentions?.Select(user => new User {id = user.id}).ToList()
                };
        }

        public static ChannelMessageView fromChannelMessage(ChannelMessage message) {
            return message == null
                ? new ChannelMessageView()
                : new ChannelMessageView {
                    id = message.id,
                    nonce = getNonce(message.nonce),
                    channelId = message.channelId,
                    author = message.author,
                    content = getContent(message.content, message.attachments, message.embeds),
                    fileSize = getFileSize(message.content, message.attachments, message.embeds),
                    width = getImageWidth(message.content, message.attachments, message.embeds),
                    height = getImageHeight(message.content, message.attachments, message.embeds),
                    time = DateConvert.DateTimeFromNonce(message.id),
                    attachments = message.attachments,
                    type = getType(message.content, message.attachments, message.embeds),
                    mentionEveryone = message.mentionEveryone,
                    mentions = message.mentions,
                    starred = message.starred,
                    replyMessageIds = message.replyMessageIds,
                    lowerMessageIds = message.lowerMessageIds,
                    replyUsers = message.replyUsers,
                    lowerUsers = message.lowerUsers,
                    pending = message.pending,
                    deleted = message.deletedTime != null,
                    embeds = message.embeds,
                    reactions = message.reactions
                };
        }
    }
}