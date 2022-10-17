namespace HelloMap

open Fabulous.Maui
open Fabulous.Maui.Maps

open type Fabulous.Maui.View

module App =
    let view () =
        Application(
            ContentPage(
                "HelloMap",
                Map()
            ).ignoreSafeArea()
        )

    let program =
        Program.stateless view
