module Round.State

open Elmish
open Tournament.Pairing
open Tournament.Round
open Tournament.Tournament

type RoundModel =
    { Round: Round
      Form: Pairing option
      StandingsAcc: (string * int) list }

type RoundMsg =
    | Edit of Pairing option
    | SetPlayer1Score of int
    | SetPlayer2Score of int
    | ConfirmScore of Pairing
    | StartRound
    | FinishRound

let init num t =
    { Round = t.Rounds |> List.find (fun r -> r.Number = num)
      Form = None
      StandingsAcc =
        t.Rounds
        |> List.take num
        |> List.collect ((fun r -> r.Standings))
        |> List.groupBy (fun r -> fst r)
        |> List.map (fun r -> fst r, snd r |> List.sumBy (fun s -> snd s))
        |> List.sortBy (fun (_, score) -> -score) },
    Cmd.none

let update msg model =
    match msg with
    | Edit p when model.Round.Status = Ongoing -> { model with Form = p }, Cmd.none
    | SetPlayer1Score e -> { model with Form = Some { model.Form.Value with Player1Score = e } }, Cmd.none
    | SetPlayer2Score e -> { model with Form = Some { model.Form.Value with Player2Score = e } }, Cmd.none
    | ConfirmScore _ -> { model with Form = None }, Cmd.none
    | _ -> model, Cmd.none
