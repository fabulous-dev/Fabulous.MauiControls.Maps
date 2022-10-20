namespace Fabulous.MauiControls.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.Maui
open Microsoft.Maui.Controls.Maps
open Microsoft.Maui.Devices.Sensors
open Microsoft.Maui.Maps

type IMapCircle =
    inherit Fabulous.MauiControls.Maps.IMapElement

module MapCircle =
    let WidgetKey = Widgets.register<Circle> ()

    let Center = Attributes.defineBindableWithEquality<Location> Circle.CenterProperty

    let Radius = Attributes.defineBindableWithEquality<Distance> Circle.RadiusProperty

    let FillColor = Attributes.defineBindableAppThemeColor Circle.FillColorProperty

[<AutoOpen>]
module MapCircleBuilders =

    type Fabulous.Maui.View with

        static member inline MapCircle<'msg>(center: Location, radius: Distance) =
            WidgetBuilder<'msg, IMapCircle>(
                MapCircle.WidgetKey,
                MapCircle.Center.WithValue(center),
                MapCircle.Radius.WithValue(radius)
            )

[<Extension>]
type MapCircleModifiers =
    [<Extension>]
    static member inline fillColor(this: WidgetBuilder<'msg, #IMapCircle>, light: FabColor, ?dark: FabColor) =
        this.AddScalar(MapCircle.FillColor.WithValue(AppTheme.create light dark))

    /// <summary>Link a ViewRef to access the direct Circle control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMapCircle>, value: ViewRef<Circle>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
