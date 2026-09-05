# Architecture

## Overview

本プロジェクトはPlaynite本体をforkせず、複数のPlaynite Extensionと共通Coreで構成する。

```text
Playnite
 |
 +-- DoujinTools (Generic Plugin)
 |     |
 |     `-- Doujin.Core
 |
 +-- DLsite (Library / Metadata Plugin)
 |
 `-- FANZA (Library / Metadata Plugin)
```

## Modules

### Doujin.Core

Playniteに依存しない共通ロジック。

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

Playnite Generic Plugin。

責務:

- Playniteイベントとの接続
- ゲーム右クリックメニュー
- 設定画面
- Engine Detection実行
- Backup / Restore操作
- Archive & Remove
- Playnite Game IDとCore側データの対応付け

### DLsite

DLsite固有のLibrary / Metadata Plugin。

CoreやDoujinToolsから独立させる。

### FANZA

FANZA/DMM固有のLibrary / Metadata Plugin。

DLsiteと同様、販売サイト固有処理をCoreへ持ち込まない。

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

## Store Independence

作品がDLsite由来かFANZA由来かローカル手動登録かにかかわらず、DoujinToolsは動作できること。

Store Pluginが無効・未ログイン・API変更で壊れていても、既存ゲームのセーブバックアップと復元は継続できることを必須条件とする。

## Playnite Version Boundary

Playnite SDK依存コードをDoujinTools/DLsite/FANZA側へ閉じ込める。

Playnite 10から将来バージョンへ移行する場合、Coreのエンジン判定・セーブ管理・検証ロジックを再利用できることを目標とする。
