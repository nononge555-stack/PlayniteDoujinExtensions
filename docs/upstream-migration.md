# Upstream Migration Plan

## Purpose

DLsite/FANZA integration will not be implemented from scratch unless necessary.

The primary upstream is `erri120/Playnite.Extensions`, an archived GPL-3.0 repository that already contains Playnite metadata providers for DLsite and FANZA, shared utilities, tests, manifests, and a generic `GameManagement` plugin.

This project will modernize and maintain the relevant upstream code while adding new doujin-specific save/archive functionality.

## Upstream Repository

- Repository: `erri120/Playnite.Extensions`
- Status: archived
- Default branch: `master`
- License: GNU GPL v3

Because code will be reused and modified, this repository must preserve the GPL-compatible licensing requirements and upstream attribution/copyright notices for imported files.

## Relevant Upstream Modules

### DLSiteMetadata

Existing upstream structure includes:

- `DLSiteMetadata.csproj`
- `DLSiteMetadataPlugin.cs`
- `DLSiteMetadataProvider.cs`
- `Scrapper.cs`
- `ScrapperResult.cs`
- `Settings.cs`
- `SettingsView.xaml`
- `SettingsView.xaml.cs`
- `extension.yaml`
- tests in `DLSiteMetadata.Test`

Existing functionality includes metadata lookup by DLsite URL/product ID, search fallback, configurable language, developer-role mapping, tags/categories/genres, images, and release date.

### FanzaMetadata

Existing upstream structure includes:

- `FanzaMetadata.csproj`
- `FanzaMetadataPlugin.cs`
- `FanzaMetadataProvider.cs`
- `Scrapper.cs`
- `ScrapperResult.cs`
- `Settings.cs`
- `SettingsView.xaml`
- `SettingsView.xaml.cs`
- `extension.yaml`
- tests in `FanzaMetadata.Test`

Existing functionality includes metadata lookup/search and mapping of title, developers, genres/tags, score, images, series, and release date.

### Extensions.Common

The upstream repository already factors shared extension code into `Extensions.Common`.

Before importing duplicated DLsite/FANZA logic, inspect this module and retain useful abstractions where they still fit current Playnite APIs.

### GameManagement

The upstream `GameManagement` Generic Plugin already implements storage statistics and deletion of an installation directory for games without an uninstaller.

This code is a useful reference for the **remove** half of `Archive & Remove`, but the safety model in this project is stricter:

1. Detect save data.
2. Back it up.
3. Verify the backup.
4. Persist the manifest.
5. Only then allow deletion of the game directory.

No upstream deletion code may bypass these requirements.

## Migration Strategy

### Step 1: Preserve Proven Structure

Start from the existing upstream architecture instead of generating equivalent DLsite/FANZA providers from zero.

Target layout:

```text
src/
|-- Extensions.Common/
|-- DLSiteMetadata/
|-- FanzaMetadata/
|-- Doujin.Core/
`-- DoujinTools/
```

Names may be adjusted later, but avoid renaming everything during the first import. A small diff from upstream makes behavioral regressions easier to identify.

### Step 2: Import With Attribution

For each imported file:

- preserve applicable copyright notices;
- document its upstream path and source commit where practical;
- do not remove GPL notices merely for stylistic cleanup;
- keep a record of substantial rewrites.

### Step 3: Make Upstream Build Again

Before adding new features:

- update project/SDK references for the currently supported Playnite 10 environment;
- update manifests only as required;
- fix compile errors;
- restore existing unit tests;
- package/load the extensions in Playnite.

Do not mix the initial modernization with major scraper redesign unless the old scraper no longer works.

### Step 4: Verify Live Metadata Behavior

For DLsite and FANZA separately, verify:

- URL/ID recognition;
- search;
- title;
- developer/circle/brand;
- tags/genres/features;
- release date;
- images;
- series/score where supported;
- error handling when HTML/site behavior changes.

Site scraping is expected to be the most fragile portion and should be isolated from Playnite mapping code.

### Step 5: Add Missing Library Features Separately

The upstream projects are primarily metadata providers. Purchase-library synchronization and re-download/install lifecycle support should be separate work.

Do not make metadata-provider modernization depend on purchase-history synchronization.

## New Code Owned by This Project

The primary new functionality is:

- engine detection;
- RPG Maker save location detection;
- save backup/restore;
- backup verification;
- backup history;
- Archive manifests;
- safe `Archive & Remove`;
- later store-library and reinstall integration.

These belong in `Doujin.Core` / `DoujinTools` and must remain usable even when the DLsite/FANZA integrations are broken or disabled.

## Non-Goals During Initial Migration

- rewriting all upstream code for style;
- combining DLsite and FANZA into one giant plugin;
- adding account scraping at the same time as metadata modernization;
- implementing download automation before metadata providers are stable;
- changing the save/archive safety rules to match the old GameManagement uninstaller.
