using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class ArticleFocusListLoading : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.Separator2,
                child: new ListView(
                    children: new List<Widget> {
                        this._buildHead(),
                        this._buildContent(context),
                        this._buildContent(context),
                        this._buildContent(context),
                        this._buildContent(context)
                    })
            );
        }


        Widget _buildHead() {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(bottom: 48, left: 16, right: 16, top: 24),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            height: 14,
                            width: 64,
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 16),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: new List<Widget> {
                                    this._Avatar(),
                                    this._Avatar(),
                                    this._Avatar(),
                                    this._Avatar(),
                                    this._Avatar()
                                }
                            )
                        )
                    }
                )
            );
        }

        Widget _Avatar() {
            return new Container(
                width: 52,
                height: 52,
                decoration: new BoxDecoration(
                    color: new Color(0xFFF8F8F8),
                    borderRadius: BorderRadius.circular(26)
                )
            );
        }

        Widget _buildContent(BuildContext context) {
            return new Container(
                margin: EdgeInsets.only(top: 8),
                padding: EdgeInsets.only(16, 16, 16, 56),
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(bottom: 24),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: new List<Widget> {
                                    new Container(
                                        child: new Row(
                                            children: new List<Widget> {
                                                new Container(
                                                    margin: EdgeInsets.only(right: 8),
                                                    color: new Color(0xFFF8F8F8),
                                                    width: 38, height: 38),
                                                new Column(
                                                    children: new List<Widget> {
                                                        new Container(color: new Color(0xFFF8F8F8), width: 40,
                                                            height: 14,
                                                            margin: EdgeInsets.only(bottom: 12)),
                                                        new Container(color: new Color(0xFFF8F8F8), width: 40,
                                                            height: 6)
                                                    })
                                            })),
                                    new Container(color: new Color(0xFFF8F8F8), width: 40, height: 12)
                                })),
                        new Row(children: new List<Widget> {
                            new Expanded(child: new Container(height: 12, color: new Color(0xFFF8F8F8)))
                        }),
                        new Container(
                            padding: EdgeInsets.only(top: 16),
                            child: new Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Expanded(
                                        child: new Column(
                                            children: new List<Widget> {
                                                new Container(color: new Color(0xFFF8F8F8), height: 6,
                                                    margin: EdgeInsets.only(bottom: 16)),
                                                new Container(color: new Color(0xFFF8F8F8), height: 6)
                                            })),
                                    new Container(color: new Color(0xFFF8F8F8), width: 100, height: 66,
                                        margin: EdgeInsets.only(left: 16))
                                })),
                    }));
        }
    }
}