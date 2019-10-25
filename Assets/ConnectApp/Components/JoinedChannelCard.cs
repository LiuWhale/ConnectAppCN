using System.Collections.Generic;
using System.Linq;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class JoinedChannelCard : StatelessWidget {
        public JoinedChannelCard(
            ChannelView channel,
            string myUserId = null,
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(key: key) {
            this.channel = channel;
            this.myUserId = myUserId;
            this.onTap = onTap;
        }

        readonly ChannelView channel;
        readonly string myUserId;
        readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {

            string text;
            if (this.channel.lastMessage != null) {
                if (this.channel.lastMessage.deleted) {
                    text = "[此消息已被删除]";
                }
                else if (this.channel.lastMessage.type == ChannelMessageType.image) {
                    text = "[图片]";
                }
                else if (this.channel.lastMessage.type == ChannelMessageType.file) {
                    text = "[文件]";
                }
                else {
                    text = this.channel.lastMessage.content ?? "";
                }
                if (!string.IsNullOrEmpty(value: this.channel.lastMessage.author?.fullName) &&
                    !string.IsNullOrEmpty(value: text) &&
                    this.channel.lastMessage.author.id != this.myUserId) {
                    text = $"{this.channel.lastMessage.author?.fullName}: {text}";
                }
            }
            else {
                text = "";
            }

            Widget message = new RichText(
                text: new TextSpan(
                    this.channel.atMe || this.channel.atAll
                        ? "[有人@我] "
                        : "",
                    style: CTextStyle.PRegularError,
                    children: MessageUtils.messageWithMarkdownToTextSpans(
                        content: text,
                        mentions: this.channel.lastMessage?.mentions,
                        mentionEveryone: this.channel.lastMessage?.mentionEveryone ?? false,
                        onTap: null,
                        bodyStyle: CTextStyle.PRegularBody4,
                        linkStyle: CTextStyle.PRegularBody4
                    ).ToList()
                    
                ),
                overflow: TextOverflow.ellipsis,
                maxLines: 1
            );

            // Don't show the time if nonce is 0, i.e. the time is not loaded yet.
            // Otherwise, the time would be like 0001/01/01 8:00
            string timeString = this.channel.lastMessage?.nonce != 0
                ? this.channel.lastMessage?.time.DateTimeString() ?? ""
                : "";

            Widget titleLine = new Container(
                padding: EdgeInsets.only(16),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Expanded(
                            child: new Text(
                                data: this.channel.name,
                                style: CTextStyle.PLargeMedium,
                                maxLines: 1,
                                overflow: TextOverflow.ellipsis
                            )
                        ),
                        new Container(width: 16),
                        new Text(
                            data: timeString,
                            style: CTextStyle.PSmallBody4
                        )
                    }
                )
            );

            Widget icon = new Align(
                alignment: Alignment.centerRight,
                child: new NotificationDot(
                        this.channel.unread > 0
                            ? this.channel.mentioned > 0 ? $"{this.channel.mentioned}" : ""
                            : null
                    )
            );

            var messageIcon = new Row(
                children: new List<Widget> {
                    new Expanded(
                        child: new Container(
                            padding: EdgeInsets.symmetric(0, 16),
                            child: message
                        )
                    ),
                    icon
                }
            );

            return new GestureDetector(
                onTap: this.onTap,
                child: new Container(
                    color: this.channel.isTop ? CColors.PrimaryBlue.withOpacity(0.04f) : CColors.White,
                    height: 72,
                    padding: EdgeInsets.symmetric(12, 16),
                    child: new Row(
                        children: new List<Widget> {
                            new PlaceholderImage(
                                this.channel?.thumbnail ?? "",
                                48,
                                48,
                                4,
                                fit: BoxFit.cover,
                                true
                            ),
                            new Expanded(
                                child: new Column(
                                    children: new List<Widget> {
                                        titleLine,
                                        new Expanded(
                                            child: messageIcon
                                        )
                                    }
                                )
                            )
                        }
                    )
                )
            );
        }
    }
}