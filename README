# Dungeon Settlers Modding

MelonLoader/Harmony mods for Dungeon Settlers (Steam, IL2CPP, Unity 6000.0.58f2).

## Background: Mono vs IL2CPP

Unity ships scripts either as Mono (real .NET IL in `Assembly-CSharp.dll`, fully decompilable) or
IL2CPP (C# compiled to native machine code in `GameAssembly.dll`, original IL discarded — not
recoverable by any decompiler). Dungeon Settlers is IL2CPP.

Two different "Assembly-CSharp.dll" files exist for an IL2CPP game and serve different purposes:

- `MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll` — the interop stub assembly. Real method
  signatures, but bodies are just native-call marshaling glue, not game logic. **This is what mod
  projects compile against.**
- `MelonLoader\Dependencies\Il2CppAssemblyGenerator\Cpp2IL\cpp2il_out\Assembly-CSharp.dll` — Cpp2IL's
  attempted reconstruction of actual logic from native disassembly. Best-effort and can fail
  entirely depending on the Unity/IL2CPP version (it does for this game — every method decompiles to
  a stub). Useful only for double-checking signatures, not logic.

Because method bodies aren't recoverable, mods here work by patching *around* the opaque native
logic with Harmony prefixes/postfixes, rather than by understanding it.

## Repo layout

Each mod lives in its own project folder, e.g. `DS.FreeReroll/` — free level-up statue rerolls after
the first prayer attempt.

## Building a mod

```
cd <ModProject>
dotnet build -c Release
```

Mod projects reference only files already present in the game/MelonLoader install (MelonLoader's own
assemblies, Harmony, and the Il2Cpp interop stubs) via local `HintPath`/`Reference` items — no NuGet
packages, no network access required to build.

## Installing a mod

Copy the built `.dll` from `bin\Release\` into:
```
<GameDir>\Mods\
```
Launch the game and check `<GameDir>\MelonLoader\Latest.log` for load errors and any log lines the
mod prints.

## Known gotchas

- **Generic IL2CPP interop struct return values can be unreliable to patch.** A postfix mutating
  `Il2CppSystem.ValueTuple<string,int,int>` fields compiled and applied without error, but the fields
  read back as garbage and the patch had no real effect — a marshaling issue specific to this
  Il2CppInterop build for generic types mixing reference- and value-type fields. Prefer patching a
  nearby method that uses only plain types (e.g. a single `int` parameter) over one returning a
  generic interop struct.
- **A patch applying without a Harmony exception doesn't mean it changed anything.** Add
  `MelonLogger.Msg(...)` inside prefixes/postfixes so `Latest.log` shows the patch actually fired with
  sane (non-garbage) values.
