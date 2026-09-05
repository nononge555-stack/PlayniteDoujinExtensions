# Upstream Attribution

This project reuses and modifies code from the archived open-source project [`erri120/Playnite.Extensions`](https://github.com/erri120/Playnite.Extensions).

## Primary upstream

- Repository: `erri120/Playnite.Extensions`
- License: GNU General Public License v3.0
- Baseline commit: `89195f1ae594e72d254a6daded9b560a12c35f89`
- Baseline commit message: `Merge pull request #99 from rxvincent/master - Update DLsite Scapper`
- Baseline date: 2022-07-08

The baseline commit is intentionally pinned so that imported files can be compared against a known source revision.

## Initially reused areas

The following upstream paths are candidates for direct migration and modernization:

- `src/Directory.Build.props`
- `src/Extensions.Common/`
- `src/DLSiteMetadata/`
- `src/DLSiteMetadata.Test/`
- `src/FanzaMetadata/`
- `src/FanzaMetadata.Test/`

`src/GameManagement/` is treated primarily as a reference for Playnite installation-directory handling and folder-based uninstall behavior. Any future `Archive & Remove` implementation must add save backup and verification before deletion.

## Modification policy

- Existing upstream copyright/license notices must not be removed.
- Upstream-derived code remains under GPL-3.0 terms.
- Major rewrites should be done after a working migration baseline exists, so the origin of behavior remains reviewable.
- When a migrated file is substantially changed, commit history and this baseline document remain the provenance record.
- Store-specific metadata code and new save/archive code should remain separable by module even though the repository as distributed follows GPL-3.0.

## Current Playnite compatibility target

At migration start (2026-09-05), the latest published PlayniteSDK package found is `6.16.0`, targeting .NET Framework 4.6.2 (`net462`).

The upstream baseline used PlayniteSDK `6.2.2`. Migration commits should therefore distinguish between:

1. importing upstream code;
2. updating build/SDK dependencies;
3. repairing site/API behavior.

This separation is deliberate and makes regressions easier to diagnose.
