module TournamentTracker.Test

open Fable.Jester
open TournamentTracker

Jest.describe (
    "TournamentTracker tests",
    fun () ->
        Jest.test (
            "can create tournament",
            (fun () ->
                let tournament = createTournament 1

                Jest
                    .expect(tournament)
                    .toEqual ("{\"rounds\":[{\"number\":1,\"pairings\":[],\"status\":\"Pregame\"}],\"players\":[]}"))
        )

        Jest.test (
            "can add players",
            (fun () ->
                let tournament =
                    createTournament 1
                    |> addPlayers [| "Ossi"; "Aku" |]

                Jest
                    .expect(tournament)
                    .toEqual (
                        "{\"rounds\":[{\"number\":1,\"pairings\":[],\"status\":\"Pregame\"}],\"players\":[\"Aku\",\"Ossi\"]}"
                    )

                let withMorePlayers = tournament |> addPlayers [| "Veikka" |]

                Jest
                    .expect(withMorePlayers)
                    .toEqual (
                        "{\"rounds\":[{\"number\":1,\"pairings\":[],\"status\":\"Pregame\"}],\"players\":[\"Aku\",\"Ossi\",\"Veikka\"]}"
                    ))
        )

        Jest.test (
            "can pair",
            (fun () ->
                let tournament =
                    createTournament 1
                    |> addPlayers [| "Ossi"; "Aku" |]
                    |> pair "Swiss"

                Jest
                    .expect(tournament)
                    .toEqual (
                        "{\"rounds\":[{\"number\":1,\"pairings\":[{\"number\":0,\"player1\":\"Aku\",\"player2\":\"Ossi\",\"player1Score\":0,\"player2Score\":0}],\"status\":\"Pregame\"}],\"players\":[\"Aku\",\"Ossi\"]}"
                    ))
        )

        Jest.test (
            "can start round",
            (fun () ->
                let tournament =
                    createTournament 1
                    |> addPlayers [| "Ossi"; "Aku" |]
                    |> pair "Swiss"
                    |> startRound

                Jest
                    .expect(tournament)
                    .toEqual (
                        "{\"rounds\":[{\"number\":1,\"pairings\":[{\"number\":0,\"player1\":\"Aku\",\"player2\":\"Ossi\",\"player1Score\":0,\"player2Score\":0}],\"status\":\"Ongoing\"}],\"players\":[\"Aku\",\"Ossi\"]}"
                    ))
        )

        Jest.test (
            "can score",
            (fun () ->
                let tournament =
                    createTournament 1
                    |> addPlayers [| "Ossi"; "Aku" |]
                    |> pair "Swiss"
                    |> startRound
                    |> score 0 (10, 10)

                Jest
                    .expect(tournament)
                    .toEqual (
                        "{\"rounds\":[{\"number\":1,\"pairings\":[{\"number\":0,\"player1\":\"Aku\",\"player2\":\"Ossi\",\"player1Score\":10,\"player2Score\":10}],\"status\":\"Ongoing\"}],\"players\":[\"Aku\",\"Ossi\"]}"
                    ))
        )

        Jest.test (
            "can finish round",
            (fun () ->
                let tournament =
                    createTournament 1
                    |> addPlayers [| "Ossi"; "Aku" |]
                    |> pair "Swiss"
                    |> startRound
                    |> score 0 (20, 0)
                    |> finishRound


                Jest
                    .expect(tournament)
                    .toEqual (
                        "{\"rounds\":[{\"number\":1,\"pairings\":[{\"number\":0,\"player1\":\"Aku\",\"player2\":\"Ossi\",\"player1Score\":20,\"player2Score\":0}],\"status\":\"Finished\"}],\"players\":[\"Aku\",\"Ossi\"]}"
                    ))
        )

        Jest.test (
            "can swap two players",
            (fun () ->
                let tournament =
                    createTournament 1
                    |> addPlayers [| "Aku"
                                     "Juha"
                                     "Ossi"
                                     "Veikka" |]
                    |> pair "Swiss"
                    |> swap "Aku" "Ossi"

                Jest
                    .expect(tournament)
                    .toEqual (
                        "{\"rounds\":[{\"number\":1,\"pairings\":[{\"number\":0,\"player1\":\"Ossi\",\"player2\":\"Juha\",\"player1Score\":0,\"player2Score\":0},{\"number\":1,\"player1\":\"Aku\",\"player2\":\"Veikka\",\"player1Score\":0,\"player2Score\":0}],\"status\":\"Pregame\"}],\"players\":[\"Aku\",\"Juha\",\"Ossi\",\"Veikka\"]}"
                    ))
        )

        Jest.test (
            "can display standings",
            (fun () ->
                let standings =
                    createTournament 1
                    |> addPlayers [| "Aku"
                                     "Juha"
                                     "Ossi"
                                     "Veikka" |]
                    |> pair "Swiss"
                    |> startRound
                    |> score 0 (11, 9)
                    |> score 1 (5, 15)
                    |> finishRound
                    |> standings

                Jest
                    .expect(standings)
                    .toEqual ("[[\"Veikka\",15],[\"Aku\",11],[\"Juha\",9],[\"Ossi\",5]]"))
        )

        Jest.test (
            "can display pairings",
            (fun () ->
                let pairings =
                    createTournament 1
                    |> addPlayers [| "Aku"
                                     "Juha"
                                     "Ossi"
                                     "Veikka" |]
                    |> pair "Swiss"
                    |> pairings

                Jest
                    .expect(pairings)
                    .toEqual (
                        "[{\"number\":0,\"player1\":\"Aku\",\"player2\":\"Juha\",\"player1Score\":0,\"player2Score\":0},{\"number\":1,\"player1\":\"Ossi\",\"player2\":\"Veikka\",\"player1Score\":0,\"player2Score\":0}]"
                    ))
        )

        Jest.test (
            "createTournament2",
            (fun () ->
                let foo = createTournament2 1

                Jest
                    .expect(foo)
                    .toEqual (
                        {| Players = [||]
                           Rounds =
                            [| {| Number = 1
                                  Pairings = [||]
                                  Status = "Pregame" |} |] |}
                    ))
        )
)
