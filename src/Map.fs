namespace Fabulous.MauiControls.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.Maui
open Fabulous.StackAllocatedCollections
open Fabulous.StackAllocatedCollections.StackList
open Microsoft.Maui.Devices.Sensors
open Microsoft.Maui.Maps
open Microsoft.Maui.Controls.Maps

type IMap =
    inherit Fabulous.Maui.IView

module Map =
    let WidgetKey = Widgets.register<Map> ()

    let MapType =
        Attributes.defineEnum<MapType> "Map_MapType" (fun _ newValueOpt node ->
            let map = node.Target :?> Map

            let value =
                match newValueOpt with
                | ValueNone -> MapType.Street
                | ValueSome v -> v

            map.MapType <- value)

    let IsShowingUser = Attributes.defineBindableBool Map.IsShowingUserProperty

    let IsTrafficEnabled = Attributes.defineBindableBool Map.IsTrafficEnabledProperty

    let IsScrollEnabled = Attributes.defineBindableBool Map.IsScrollEnabledProperty

    let IsZoomEnabled = Attributes.defineBindableBool Map.IsZoomEnabledProperty

    let RequestedRegion =
        Attributes.defineSimpleScalarWithEquality<MapSpan> "Map_RequestedRegion" (fun _ newValueOpt node ->
            let map = node.Target :?> Map

            match newValueOpt with
            | ValueNone -> ()
            | ValueSome mapSpan -> map.MoveToRegion(mapSpan))

    let Pins =
        Attributes.defineListWidgetCollection "Map_Pins" (fun target -> (target :?> Map).Pins)

    let MapElements =
        Attributes.defineListWidgetCollection "Map_MapElements" (fun target -> (target :?> Map).MapElements)

    let MapClicked =
        Attributes.defineEvent<MapClickedEventArgs> "Map_MapClicked" (fun target -> (target :?> Map).MapClicked)

[<AutoOpen>]
module MapBuilders =
    type Fabulous.Maui.View with

        /// Defines a Map widget
        static member inline Map<'msg>(?requestRegion: MapSpan) =
            match requestRegion with
            | Some mapSpan ->
                WidgetBuilder<'msg, IMap>(
                    Map.WidgetKey,
                    AttributesBundle(StackList.one (Map.RequestedRegion.WithValue(mapSpan)), ValueNone, ValueNone)
                )
            | None ->
                WidgetBuilder<'msg, IMap>(Map.WidgetKey, AttributesBundle(StackList.empty (), ValueNone, ValueNone))

        static member inline MapWithPins<'msg>(requestRegion: MapSpan) =
            CollectionBuilder<'msg, IMap, IMapPin>(
                Map.WidgetKey,
                Map.Pins,
                Map.RequestedRegion.WithValue(requestRegion)
            )

[<Extension>]
type MapModifiers =
    [<Extension>]
    static member inline isZoomEnabled(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.IsZoomEnabled.WithValue(value))

    [<Extension>]
    static member inline isScrollEnabled(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.IsScrollEnabled.WithValue(value))

    [<Extension>]
    static member inline mapType(this: WidgetBuilder<'msg, #IMap>, value: MapType) =
        this.AddScalar(Map.MapType.WithValue(value))

    [<Extension>]
    static member inline isShowingUser(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.IsShowingUser.WithValue(value))

    [<Extension>]
    static member inline isTrafficEnabled(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.IsTrafficEnabled.WithValue(value))

    [<Extension>]
    static member inline onMapClicked(this: WidgetBuilder<'msg, #IMap>, onMapClicked: Location -> 'msg) =
        this.AddScalar(Map.MapClicked.WithValue(fun args -> onMapClicked args.Location |> box))

    [<Extension>]
    static member inline mapElements<'msg, 'marker when 'marker :> IMap>(this: WidgetBuilder<'msg, 'marker>) =
        WidgetHelpers.buildAttributeCollection<'msg, 'marker, IMapElement> Map.MapElements this

    /// <summary>Link a ViewRef to access the direct Map control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMap>, value: ViewRef<Map>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))

[<Extension>]
type CollectionBuilderExtensions =
    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> Fabulous.MauiControls.Maps.IMapElement>
        (
            _: AttributeCollectionBuilder<'msg, 'marker, IMapElement>,
            x: WidgetBuilder<'msg, 'itemType>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }

    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> Fabulous.MauiControls.Maps.IMapElement>
        (
            _: AttributeCollectionBuilder<'msg, 'marker, IMapElement>,
            x: WidgetBuilder<'msg, Memo.Memoized<'itemType>>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }


    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> Fabulous.MauiControls.Maps.IMapPin>
        (
            _: CollectionBuilder<'msg, 'marker, IMapPin>,
            x: WidgetBuilder<'msg, Memo.Memoized<'itemType>>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }

    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> Fabulous.MauiControls.Maps.IMapPin>
        (
            _: CollectionBuilder<'msg, 'marker, IMapPin>,
            x: WidgetBuilder<'msg, 'itemType>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }
