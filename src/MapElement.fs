namespace Fabulous.MauiControls.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.Maui
open Microsoft.Maui.Controls.Maps

type IMapElement =
    inherit Fabulous.Maui.IElement

module MapElement =
    let WidgetKey = Widgets.register<MapElement> ()

    let StrokeColor =
        Attributes.defineBindableAppThemeColor MapElement.StrokeColorProperty

    let StrokeWidth = Attributes.defineBindableFloat MapElement.StrokeWidthProperty


[<Extension>]
type MapElementModifiers =
    [<Extension>]
    static member inline strokeColor(this: WidgetBuilder<'msg, #IMapElement>, light: FabColor, ?dark: FabColor) =
        this.AddScalar(MapElement.StrokeColor.WithValue(AppTheme.create light dark))

    [<Extension>]
    static member inline strokeWidth(this: WidgetBuilder<'msg, #IMapElement>, value: float) =
        this.AddScalar(MapElement.StrokeWidth.WithValue(value))

    /// <summary>Link a ViewRef to access the direct MapElement control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMapElement>, value: ViewRef<MapElement>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
