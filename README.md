# Playnite Doujin Extensions

Playniteを同人ゲーム管理に特化させるための拡張群です。

## コンセプト

同人ゲームでは、購入したゲーム本体は販売サイトから再ダウンロードできることが多い一方、セーブデータは失うと再取得できません。

このプロジェクトでは、**「ゲーム本体は消しても、セーブデータとライブラリ情報は残す」**ことを基本思想とします。

Playniteが持つゲームライブラリ、検索、タグ、画像、プレイ時間、起動管理などを活用し、同人ゲーム特有の面倒を拡張機能で解決します。

## 開発方針の核

DLsite/FANZA連携はゼロから作り直しません。

既存のGPL-3.0プロジェクト [`erri120/Playnite.Extensions`](https://github.com/erri120/Playnite.Extensions) に含まれる `DLSiteMetadata`、`FanzaMetadata`、`Extensions.Common`、関連テストをベースに、現行Playnite・現行サイト仕様へ近代化して引き継ぐことを第一方針とします。

その上で、このプロジェクト独自の機能として以下を追加します。

- RPG Maker系ゲームのエンジン自動判定
- セーブデータの自動検出、バックアップ、復元
- ゲーム終了時の自動セーブバックアップ
- バックアップ成功を確認してからゲーム本体を削除する `Archive & Remove`
- 将来的な購入ライブラリ同期、再ダウンロード、再インストール、セーブ自動復元

つまり、**販売サイト対応は既存資産を活かし、セーブ保護・アーカイブ機能を新しく強化する**プロジェクトです。

## Upstream

主な移植元:

- `erri120/Playnite.Extensions`
- License: GNU GPL v3
- Status: Archived

既存実装には以下が含まれています。

### DLsite

- Metadata Provider
- DLsite URL / RJ等の商品ID認識
- 検索
- タイトル
- カテゴリ / ジャンル / タグ
- 開発者情報
- 発売日
- Icon / Cover / Background
- 言語・メタデータ割り当て設定
- テスト

### FANZA

- Metadata Provider
- FANZA URL認識 / 検索
- タイトル
- 開発者
- ジャンル / タグ
- Community Score
- Icon / Cover / Background
- Series
- Release Date
- 設定画面
- テスト

### GameManagement

元リポジトリには、インストールディレクトリ容量の集計と、インストーラを持たないゲームのフォルダ削除型Uninstallもあります。

この削除処理は `Archive & Remove` の参考にしますが、本プロジェクトでは安全要件を強化し、**セーブのバックアップ・検証が完了するまでゲーム本体の削除を許可しません。**

詳細は [`docs/upstream-migration.md`](docs/upstream-migration.md) を参照してください。

## 基本設計

販売サイトとゲームエンジン、セーブ管理を分離します。

```text
Playnite
|
+-- DLSiteMetadata       upstreamベースのMetadata Plugin
|
+-- FanzaMetadata        upstreamベースのMetadata Plugin
|
+-- DoujinTools          新規 Generic Plugin
|   +-- Engine Detection
|   +-- Save Management
|   +-- Archive & Remove
|   +-- Playnite integration
|
+-- Extensions.Common    upstream共通コードを必要に応じて継承
|
+-- Doujin.Core          Playnite非依存の新規コアロジック
```

`Doujin.Core` は可能な限りPlaynite SDKに依存させません。Playnite側のAPI変更や将来バージョン対応時に、エンジン判定・セーブ管理ロジックを再利用しやすくするためです。

## 想定リポジトリ構成

```text
PlayniteDoujinExtensions/
|-- src/
|   |-- Extensions.Common/     upstreamから必要部分を移植
|   |-- DLSiteMetadata/        upstreamを近代化
|   |-- FanzaMetadata/         upstreamを近代化
|   |-- Doujin.Core/           新規
|   |   |-- Engines/
|   |   |-- Saves/
|   |   |-- Archives/
|   |   |-- Models/
|   |   `-- Storage/
|   `-- DoujinTools/           新規
|-- tests/
|   |-- DLSiteMetadata.Test/
|   |-- FanzaMetadata.Test/
|   `-- Doujin.Core.Tests/
|-- docs/
|-- .github/workflows/
|-- AGENTS.md
`-- README.md
```

初回移植では不要な大規模リネームや全面書き直しを避け、upstreamとの差分を追いやすくします。

## 初期対応エンジン

セーブ管理の最初の対象はRPG Maker系です。

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

このプロジェクトの中心的な新規機能です。

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

### Phase 1 - Existing Extension Modernization

最初に既存資産を使える状態へ戻します。

- GPL-3.0 / attribution方針の確定
- `Extensions.Common` の必要部分を移植
- `DLSiteMetadata` とテストを移植
- `FanzaMetadata` とテストを移植
- 現行Playnite向けにSDK / manifest / buildを修正
- 現行DLsite/FANZAでメタデータ取得を動作確認
- 壊れているScraperのみ必要最小限修正
- Playniteでパッケージを読み込み実機確認

### Phase 2 - DoujinTools MVP

- `Doujin.Core`
- Playnite Generic Plugin
- RPG Makerエンジン判定
- セーブデータ検出
- 手動バックアップ
- 手動復元

### Phase 3 - Automatic Backup / Archive

- ゲーム終了時バックアップ
- 差分検出
- バックアップ世代管理
- ハッシュ検証
- Backup History
- `Archive & Remove`

### Phase 4 - Store Library Integration

Metadata Providerの復旧とは分離して実装します。

- DLsite購入作品同期
- FANZA購入作品同期
- ローカルインストールとの紐付け
- Store ID管理

### Phase 5 - Install Lifecycle

- 再取得支援
- 自動展開 / インストール
- セーブ自動復元
- RTP / 互換ランタイム管理

詳細は [`docs/roadmap.md`](docs/roadmap.md) を参照してください。

## ライセンス方針

既存 `erri120/Playnite.Extensions` のGPL-3.0コードを改変・再利用する方針のため、このプロジェクトもGPL-3.0互換の形で公開・配布します。

移植時には元コードの著作権表示・ライセンス条件を保持し、可能な範囲でupstreamの元パスや基準コミットを記録します。

## 開発原則

- Playnite本体はforkしない
- DLsite/FANZAは既存実装を優先して再利用する
- 初回移植で不要な全面リファクタリングをしない
- 販売サイト固有コードをDoujin.Coreへ入れない
- Playnite固有コードとCoreロジックを分離する
- セーブバックアップ成功前にはゲーム本体を削除しない
- セーブ形式の内容解析はMVPの対象外
- Unknown Engineでも通常のPlayniteゲームとして扱えること
- 新しいEngine Detector / Save Locatorを後から追加しやすくする
- 販売サイト連携が壊れてもセーブ管理機能は動作すること

## Status

Phase 1: 既存DLsite/FANZA Playnite拡張の移植・近代化準備中。
