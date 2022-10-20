namespace Fabulous.MauiControls.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.Maui
open Microsoft.Maui.Controls.Maps
open Microsoft.Maui.Devices.Sensors


type IMapPin =
    inherit Fabulous.Maui.IElement

module MapPin =
    let WidgetKey = Widgets.register<Pin> ()

    let PinType = Attributes.defineBindableWithEquality<PinType> Pin.TypeProperty

    let Location = Attributes.defineBindableWithEquality<Location> Pin.LocationProperty

    let Address = Attributes.defineBindableWithEquality<string> Pin.AddressProperty

    let Label = Attributes.defineBindableWithEquality<string> Pin.LabelProperty

    let MarkerClicked =
        Attributes.defineEvent<PinClickedEventArgs> "Pin_MarkerClicked" (fun target -> (target :?> Pin).MarkerClicked)

    let InfoWindowClicked =
        Attributes.defineEvent<PinClickedEventArgs> "Pin_InfoWindowClicked" (fun target ->
            (target :?> Pin).InfoWindowClicked)

[<AutoOpen>]
module MapPinBuilders =
    type Fabulous.Maui.View with

        /// Defines a Pin widget
        static member inline MapPin<'msg>(location: Location) =
            WidgetBuilder<'msg, IMapPin>(MapPin.WidgetKey, MapPin.Location.WithValue(location))

[<Extension>]
type MapPinModifiers =
    [<Extension>]
    static member inline address(this: WidgetBuilder<'msg, #IMapPin>, value: string) =
        this.AddScalar(MapPin.Address.WithValue(value))

    [<Extension>]
    static member inline label(this: WidgetBuilder<'msg, #IMapPin>, value: string) =
        this.AddScalar(MapPin.Label.WithValue(value))

    [<Extension>]
    static member inline pinType(this: WidgetBuilder<'msg, #IMapPin>, value: PinType) =
        this.AddScalar(MapPin.PinType.WithValue(value))

    [<Extension>]
    static member inline onMarkerClicked(this: WidgetBuilder<'msg, #IMapPin>, onMarkerClicked: bool -> 'msg) =
        this.AddScalar(MapPin.MarkerClicked.WithValue(fun args -> onMarkerClicked args.HideInfoWindow |> box))

    [<Extension>]
    static member inline onInfoWindowClicked(this: WidgetBuilder<'msg, #IMapPin>, onInfoWindowClicked: bool -> 'msg) =
        this.AddScalar(MapPin.InfoWindowClicked.WithValue(fun args -> onInfoWindowClicked args.HideInfoWindow |> box))

    /// <summary>Link a ViewRef to access the direct Pin control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMapPin>, value: ViewRef<Pin>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
