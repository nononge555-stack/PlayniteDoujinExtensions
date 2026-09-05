# Playnite Doujin Extensions

Playniteを同人ゲーム管理に特化させるための拡張群です。

## コンセプト

同人ゲームでは、購入したゲーム本体は販売サイトから再ダウンロードできることが多い一方、セーブデータは失うと再取得できません。

このプロジェクトでは、**「ゲーム本体は消しても、セーブデータとライブラリ情報は残す」**ことを基本思想とします。

Playniteが持つゲームライブラリ、検索、タグ、画像、プレイ時間、起動管理などを活用し、同人ゲーム特有の面倒を拡張機能で解決します。

## 目標

- RPG Maker系ゲームのエンジン自動判定
- セーブデータの自動検出、バックアップ、復元
- バックアップ成功を確認してからゲーム本体を削除する `Archive & Remove`
- ゲーム終了時の自動セーブバックアップ
- DLsite作品のライブラリ・メタデータ連携
- FANZA/DMM作品のライブラリ・メタデータ連携
- 将来的な再ダウンロード、再インストール、セーブ自動復元
- 将来的なWOLF RPG Editor、NScripter、KiriKiri、TyranoScript、Ren'Py等への対応

## 基本設計

販売サイトとゲームエンジン、セーブ管理を分離します。

```text
Playnite
|
+-- DoujinTools          Playnite Generic Plugin
|   +-- Engine Detection
|   +-- Save Management
|   +-- Archive & Remove
|   +-- Playnite integration
|
+-- DLsite               Library / Metadata Plugin
|
+-- FANZA                Library / Metadata Plugin
|
+-- Doujin.Core          Playnite非依存のコアロジック
```

`Doujin.Core` は可能な限りPlaynite SDKに依存させません。Playnite側のAPI変更や将来のPlaynite 11対応時に、エンジン判定・セーブ管理ロジックを再利用しやすくするためです。

## 想定リポジトリ構成

```text
PlayniteDoujinExtensions/
|-- src/
|   |-- Doujin.Core/
|   |   |-- Engines/
|   |   |-- Saves/
|   |   |-- Archives/
|   |   |-- Models/
|   |   `-- Storage/
|   |-- DoujinTools/
|   |-- DLsite/
|   `-- FANZA/
|-- tests/
|-- docs/
|-- .github/workflows/
|-- AGENTS.md
`-- README.md
```

## 初期対応エンジン

Phase 1ではRPG Maker系に絞ります。

- RPG Maker 2000
- RPG Maker 2003
- RPG Maker XP
- RPG Maker VX
- RPG Maker VX Ace
- RPG Maker MV
- RPG Maker MZ

ツクール以外のゲームは通常のPlayniteゲームとして利用でき、初期段階ではセーブ管理対象外とします。

## セーブ管理の思想

MVPではセーブファイルの中身を解析しません。

1. ゲームエンジンを判定する
2. そのエンジンの標準的なセーブ位置・パターンを使ってセーブを検出する
3. セーブを管理領域へコピーする
4. コピー結果とハッシュを検証する
5. manifestを保存する
6. すべて成功した場合のみゲーム本体の削除を許可する

ゲーム側のスクリプトやプラグインで保存先が変更される場合を考慮し、自動検出結果はユーザーが上書きできる設計にします。

## Archive & Remove

このプロジェクトの中心機能です。

```text
Engine Detection
      |
Save Detection
      |
Backup
      |
Verify backup
      |
Create manifest
      |
Only after success
      |
Remove game files
```

セーブバックアップが正常に確認できない状態では、ゲーム本体を削除しません。

## 開発ロードマップ

### Phase 1 - DoujinTools MVP

- Playnite Generic Pluginの土台
- RPG Makerエンジン判定
- セーブデータ検出
- 手動バックアップ
- 手動復元
- コアロジックのテスト

### Phase 2 - Archive / Automatic Backup

- ゲーム終了時バックアップ
- 差分検出
- バックアップ世代管理
- ハッシュ検証
- Archive & Remove

### Phase 3 - DLsite

- 既存Playnite DLsite拡張の調査
- Metadata Provider
- Library Import
- 購入作品同期

### Phase 4 - FANZA / DMM

- Metadata Provider
- Library Import
- 購入作品同期

### Phase 5 - Install Lifecycle

- 再ダウンロード
- 自動展開
- セーブ自動復元
- RTP / 互換ランタイム管理

詳細は `docs/` を参照してください。

## 開発方針

- Playnite本体はforkしない
- 販売サイト固有コードをCoreへ入れない
- Playnite固有コードとCoreロジックを分離する
- セーブバックアップ成功前にはゲーム本体を削除しない
- セーブ形式の内容解析はMVPの対象外
- Unknown Engineでも通常のPlayniteゲームとして扱えること
- 新しいEngine Detector / Save Locatorを後から追加しやすくする
- 販売サイト連携が壊れてもセーブ管理機能は動作すること

## Playnite開発メモ

Playnite 10系の.NETプラグインは `extension.yaml` を必要とし、現行の公式テンプレートは .NET Framework 4.6.2 (`net462`) とPlayniteSDK 6.x系を使用しています。

実装開始時はPlaynite Toolboxで生成される最新テンプレートを基準に、SDKバージョンとmanifest要件を確認してからプロジェクトファイルを固定します。

## Status

設計・初期構成作成中。