# Architecture

## Overview

本プロジェクトはPlaynite本体をforkせず、既存Playnite Extensionの近代化部分と、新規のセーブ/アーカイブ機能で構成する。

DLsite/FANZAのMetadata Providerはゼロから再実装せず、`erri120/Playnite.Extensions` のGPL-3.0実装をベースに現行環境へ移植する。

```text
Playnite
 |
 +-- DLSiteMetadata      upstreamベース
 |
 +-- FanzaMetadata       upstreamベース
 |
 +-- DoujinTools         新規 Generic Plugin
 |     |
 |     `-- Doujin.Core
 |
 `-- Extensions.Common   upstream共通コードを必要に応じて継承
```

## Architectural Rule

既存資産を使える場所では既存実装を優先し、独自実装はこのプロジェクト固有の価値に集中する。

新規実装の中心は以下。

- Engine Detection
- Save Detection
- Backup / Restore
- Backup Verification
- Backup History
- Archive Manifest
- Safe Archive & Remove

販売サイトのHTML取得・Metadata mappingなど、既にupstreamに存在する機能を理由なく書き直さない。

## Modules

### Extensions.Common

upstream `src/Extensions.Common` を調査し、DLsite/FANZA双方が依存する共通コードのうち現在も有効なものを移植する。

初回移植時は以下を優先する。

- 既存Providerが必要とする共通型
- HTTP / Playnite補助コード
- Metadata mappingに必要な共通処理

不要なupstreamモジュールまで一括で取り込まない。

### DLSiteMetadata

upstream `src/DLSiteMetadata` と `src/DLSiteMetadata.Test` をベースに近代化する。

既存構造には以下が含まれる。

```text
DLSiteMetadata/
|-- DLSiteMetadata.csproj
|-- DLSiteMetadataPlugin.cs
|-- DLSiteMetadataProvider.cs
|-- Scrapper.cs
|-- ScrapperResult.cs
|-- Settings.cs
|-- SettingsView.xaml
|-- SettingsView.xaml.cs
`-- extension.yaml
```

責務:

- DLsite URL / 商品ID認識
- 検索
- DLsite HTML/API等からのMetadata取得
- Playnite Metadataへの変換
- 言語/カテゴリ/ジャンル/開発者設定

初回移植では命名変更や全面リファクタリングより、現行Playnite上で既存機能を復旧することを優先する。

### FanzaMetadata

upstream `src/FanzaMetadata` と `src/FanzaMetadata.Test` をベースに近代化する。

既存構造には以下が含まれる。

```text
FanzaMetadata/
|-- FanzaMetadata.csproj
|-- FanzaMetadataPlugin.cs
|-- FanzaMetadataProvider.cs
|-- Scrapper.cs
|-- ScrapperResult.cs
|-- Settings.cs
|-- SettingsView.xaml
|-- SettingsView.xaml.cs
`-- extension.yaml
```

責務:

- FANZA URL認識
- 検索
- Metadata取得
- Playnite Metadataへの変換
- 設定

DLsiteと同様、まずupstream機能の復旧を優先する。

### Doujin.Core

本プロジェクトで新規に作る、Playniteに依存しない共通ロジック。

責務:

- ゲームエンジン判定
- セーブ位置判定
- バックアップ計画作成
- バックアップ/復元処理
- ハッシュ検証
- Archive manifest
- バックアップ世代管理

想定構成:

```text
Doujin.Core/
|-- Engines/
|-- Saves/
|-- Archives/
|-- Models/
`-- Storage/
```

### DoujinTools

本プロジェクトで新規に作るPlaynite Generic Plugin。

責務:

- Playniteイベントとの接続
- ゲーム右クリックメニュー
- 設定画面
- Engine Detection実行
- Backup / Restore操作
- Archive & Remove
- Playnite Game IDとCore側データの対応付け

## Upstream GameManagement

upstreamには `GameManagement` Generic Pluginが存在し、ゲームのInstallation Directoryを削除するUninstall処理を持つ。

このコードは以下の参考にできる。

- Playnite context menu integration
- Installation Directoryの扱い
- フォルダ削除
- storage information

ただし本プロジェクトでは削除の前提条件を強化する。

```text
Save Detection
   -> Backup
   -> Verify
   -> Persist Manifest
   -> Delete Installation Directory
```

**VerifyまたはManifest保存に失敗した場合、削除処理へ進んではならない。**

## Core Interfaces (Draft)

```text
IEngineDetector
ISaveLocator
ISaveBackupService
ISaveRestoreService
IBackupVerifier
IArchiveManifestStore
IFileSystem
```

インターフェース名は実装開始時に必要性を再評価する。設計書に存在するという理由だけで不要な抽象化を実装しない。

## Engine Detection

Engine Detectorはゲームディレクトリを受け取り、判定結果と確信度/根拠を返す想定。

```text
RpgMaker2000Detector
RpgMaker2003Detector
RpgMakerXpDetector
RpgMakerVxDetector
RpgMakerVxAceDetector
RpgMakerMvDetector
RpgMakerMzDetector
```

複数Detectorが一致する場合があるため、優先順位またはスコアリングを設ける。

## Save Location

Engine判定後、対応するSave Locatorを使う。

標準保存位置だけでなく、ユーザーがゲーム単位で保存場所を上書きできるようにする。

優先順位案:

1. ユーザー明示設定
2. 作品固有ルール
3. Engine標準ルール
4. Unknown / unmanaged

## Store / Save Independence

作品がDLsite由来かFANZA由来かローカル手動登録かにかかわらず、DoujinToolsは動作できること。

Store/Metadata Pluginが無効、サイト変更で壊れている、あるいはネットワークが利用できない場合でも、既存ゲームのセーブバックアップと復元は継続できることを必須条件とする。

## Library Synchronization Boundary

upstream DLsite/FANZA実装は主にMetadata Providerであり、購入済みライブラリ同期とは別問題として扱う。

以下はMetadata Provider復旧後の独立した機能として実装する。

- 購入履歴同期
- Store account/session handling
- Store IDとPlaynite Game IDの対応
- download / reinstall lifecycle

Metadata Providerを復旧させるために、購入履歴同期まで同時に完成させる必要はない。

## Licensing Boundary

upstreamからコードを移植するモジュールでは、GPL-3.0の条件と既存著作権表示を保持する。

移植時には可能な範囲で元パス・基準commitを記録する。

新規コードも、このリポジトリ全体のGPL互換配布方針に従う。

## Playnite Version Boundary

Playnite SDK依存コードをDLSiteMetadata/FanzaMetadata/DoujinTools側へ閉じ込める。

Doujin.Coreは可能な限りPlaynite SDKに依存させず、将来のPlaynite API変更時にもエンジン判定・セーブ管理・検証ロジックを再利用できることを目標とする。
