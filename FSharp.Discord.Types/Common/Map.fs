module Map

/// Map a map to an option where an empty map is None.
let toOption (m: Map<'T, 'U>) =
    match m with
    | m when m.Count = 0 -> None
    | m -> Some m
