# Source Layout

実装は既存Playnite拡張の移植・近代化から開始する。

```text
src/
|-- Extensions.Common/      upstreamから必要部分を移植
|-- DLSiteMetadata/         upstreamを現行環境へ近代化
|-- FanzaMetadata/          upstreamを現行環境へ近代化
|-- Doujin.Core/            本プロジェクト新規
|   |-- Engines/
|   |-- Saves/
|   |-- Archives/
|   |-- Models/
|   `-- Storage/
`-- DoujinTools/            本プロジェクト新規Generic Plugin
```

## Extensions.Common

Primary upstream: `erri120/Playnite.Extensions/src/Extensions.Common`。

DLsite/FANZA両Metadata Providerが必要とする共通コードのみを移植する。不要なupstreamプロジェクトを一括で取り込まない。

## DLSiteMetadata

Primary upstream: `erri120/Playnite.Extensions/src/DLSiteMetadata`。

既存Plugin / MetadataProvider / Scrapper / Settings / XAML / extension.yamlをベースに現行Playnite・現行DLsiteへ近代化する。

初回移植では大規模リネームを避ける。

## FanzaMetadata

Primary upstream: `erri120/Playnite.Extensions/src/FanzaMetadata`。

DLsiteと同様、既存実装をまず動作復旧してから追加機能を検討する。

## Doujin.Core

Playnite非依存のエンジン判定・セーブ管理・アーカイブロジック。

これは本プロジェクトの新規コードとしてPhase 2から実装する。

## DoujinTools

Playnite Generic Plugin。

担当予定:

- Engine Detection
- Save Backup / Restore
- Backup History
- Archive & Remove
- Playniteイベント連携

## Import Rule

upstreamコード移植時は以下を守る。

- GPL-3.0条件を保持する
- 既存著作権表示を削除しない
- upstream基準commitを記録する
- 元パスを追跡可能にする
- 動作復旧と全面リファクタリングを同時に行わない

詳細は `../docs/upstream-migration.md` を参照する。
